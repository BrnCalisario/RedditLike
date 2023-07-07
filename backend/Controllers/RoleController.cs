using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Reddit.Controllers;

using Model;
using Repositories;
using DTO;
using Services;

[ApiController]
[EnableCors("MainPolicy")]
[Route("role")]
public class RoleController : ControllerBase
{
    private IUserService userService;
    private IRoleRepository roleRepository;
    private IGroupRepository groupRepository;
    private IUserRepository userRepository;

    public RoleController(
        [FromServices] IUserService userService,
        [FromServices] IRoleRepository roleRepository,
        [FromServices] IGroupRepository groupRepository,
        [FromServices] IUserRepository userRepository
    )
    {
        this.userService = userService;
        this.roleRepository = roleRepository;
        this.groupRepository = groupRepository;
        this.userRepository = userRepository;
    }

    private async Task<User> ValidateJwt(string jwt)
    {
        User user = new User();
        try
        {
            user = await this.userService.ValidateUserToken(new Jwt { Value = jwt });
        }
        catch
        {
            return user;
        }

        return user;
    }

    [HttpPost]
    public async Task<ActionResult> AddRole([FromBody] RoleDTO roleData)
    {
        User user = await this.ValidateJwt(roleData.Jwt);

        if (user is null)
            return NotFound("Usuário não encontrado");

        Group group = await groupRepository.Find(roleData.GroupId);

        if (group is null)
            return NotFound("Grupo não encontrado");

        Role role = new Role
        {
            Name = roleData.Name,
            GroupId = roleData.GroupId
        };

        await this.roleRepository.InsertRole(role, roleData.PermissionsSet);

        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> UpdateRole([FromBody] RoleDTO roleData)
    {
        if (roleData.Id == 0)
            return BadRequest();

        User user = await this.ValidateJwt(roleData.Jwt);

        if (user is null)
            return NotFound("Usuário não encontrado");

        Role role = await this.roleRepository.Find(roleData.Id);

        await this.roleRepository.UpdateRole(role, roleData.PermissionsSet);

        return Ok();
    }

    [HttpPost("remove")]
    public async Task<ActionResult> DeleteRole([FromBody] RoleDTO roleData)
    {
        if (roleData.Id == 0)
            return BadRequest();

        User user = await this.ValidateJwt(roleData.Jwt);

        if (user is null)
            return NotFound();

        Role role = await this.roleRepository.Find(roleData.Id);

        if (role is null)
            return NotFound("Not found role");

        Group group = role.Group;

        bool canManage = await this.groupRepository.HasPermission(user, group, PermissionEnum.ManageRole);

        if (!canManage)
            return BadRequest();

        await this.roleRepository.DeleteRole(role);

        return Ok();
    }

    [HttpPost("promote-member")]
    public async Task<ActionResult> PromoteRole( [FromBody] MemberRoleDTO memberData )
    {
        Group group = await this.groupRepository.Find(memberData.GroupId);

        if (group is null)
            return BadRequest();

        User user = await this.ValidateJwt(memberData.Jwt);

        if (user is null)
            return NotFound();

        var role = await this.roleRepository.Find(memberData.RoleId);

        if (role is null)
            return NotFound("Role not found");

        var targetUser = await this.userRepository.Find(memberData.UserId);

        if (targetUser is null)
            return NotFound("Target user not found");

        await this.groupRepository.PromoteMember(group, targetUser, role);

        return Ok();
    }


    [HttpPost("group-roles")]
    public async Task<ActionResult<List<RoleDTO>>> GetGroupRole( [FromBody] GroupDTO groupData )
    {
        User user = await this.ValidateJwt(groupData.Jwt);

        if (user is null)
            return NotFound("User");

        Group group = await this.groupRepository.FindByName(groupData.Name);

        if (group is null)
            return NotFound("Group");

        var queryDefaultRoles = await this.roleRepository.Filter(r => r.GroupId == null || r.GroupId == group.Id);

        List<RoleDTO> result = new List<RoleDTO>();

        foreach (var role in queryDefaultRoles)
        {
            result.Add(new RoleDTO
            {
                Id = role.Id,
                Name = role.Name,
                PermissionsSet = await this.groupRepository.GetRolePermissions(role)
            });
        }

        return result;
    }


    [HttpPost("permission-list")]
    public async Task<ActionResult<List<int>>> GetGroupPermissions( [FromBody] GroupDTO groupData )
    {
        User user = await this.ValidateJwt(groupData.Jwt);

        if (user is null)
            return NotFound("User");

        Group group = await this.groupRepository.Find(groupData.Id);

        if (group is null)
            return NotFound("Group");

        var permissions = await this.groupRepository.GetUserPermissions(user, group);

        return Ok(permissions);
    }
}