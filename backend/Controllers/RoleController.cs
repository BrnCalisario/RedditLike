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
    
    public RoleController(IUserService userService)
    {
        this.userService = userService;
    }

    private async Task<User> ValidateJwt(string jwt)
    {
        User user = null;
        try
        {
            user = await this.userService.ValidateUserToken(new Jwt { Value = jwt });
        }
        catch (Exception ex)
        {
            return user;
        }

        return user;
    }

    [HttpPost]
    public async Task<ActionResult> AddRole(
        [FromBody] RoleDTO roleData,
        [FromServices] IGroupRepository groupRepository,
        [FromServices] IRoleRepository roleRepository
    )
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

        await roleRepository.InsertRole(role, roleData.PermissionsSet);

        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> UpdateRole(
        [FromBody] RoleDTO roleData,
        [FromServices] IGroupRepository groupRepository,
        [FromServices] IRoleRepository roleRepository
    )
    {
        if (roleData.Id == 0)
            return BadRequest();

        User user = await this.ValidateJwt(roleData.Jwt);

        if (user is null)
            return NotFound("Usuário não encontrado");

        Role role = await roleRepository.Find(roleData.Id);

        await roleRepository.UpdateRole(role, roleData.PermissionsSet);

        return Ok();
    }

    [HttpPost("remove")]
    public async Task<ActionResult> DeleteRole(
        [FromBody] RoleDTO roleData,
        [FromServices] IGroupRepository groupRepository,
        [FromServices] IRoleRepository roleRepository
    )
    {
        if (roleData.Id == 0)
            return BadRequest();

        User user = await this.ValidateJwt(roleData.Jwt);

        if (user is null)
            return NotFound();

        Role role = await roleRepository.Find(roleData.Id);

        if (role is null)
            return NotFound("Not found role");

        Group group = role.Group;

        bool canManage = await groupRepository.HasPermission(user, group, PermissionEnum.ManageRole);

        if (!canManage)
            return BadRequest();

        await roleRepository.DeleteRole(role);

        return Ok();
    }

    [HttpPost("promote-member")]
    public async Task<ActionResult> PromoteRole(
    [FromBody] MemberRoleDTO memberData,
    [FromServices] IGroupRepository groupRepository,
    [FromServices] IUserRepository userRepository,
    [FromServices] IRoleRepository roleRepository
)
    {
        Group group = await groupRepository.Find(memberData.GroupId);

        if (group is null)
            return BadRequest();

        User user = await this.ValidateJwt(memberData.Jwt);

        if (user is null)
            return NotFound();

        var role = await roleRepository.Find(memberData.RoleId);

        if (role is null)
            return NotFound("Role not found");

        var targetUser = await userRepository.Find(memberData.MemberId);

        if(targetUser is null)
            return NotFound("User not found");

        await groupRepository.PromoteMember(group, targetUser, role);

        return Ok();
    }


    [HttpPost("group-roles")]
    public async Task<ActionResult<List<RoleDTO>>> GetGroupRole(
        [FromBody] GroupDTO groupData,
        [FromServices] IGroupRepository groupRepository,
        [FromServices] IRoleRepository roleRepository
    )
    {
        User user = await this.ValidateJwt(groupData.Jwt);

        if(user is null)
            return NotFound("User");

        var query = await groupRepository.Filter(g => g.Name == groupData.Name);

        Group group = query.FirstOrDefault();

        if(group is null)
            return NotFound("Group");

        var queryDefaultRoles = await roleRepository.Filter(r => r.GroupId == null || r.GroupId == group.Id);

        List<RoleDTO> result = new List<RoleDTO>();

        foreach(var role in queryDefaultRoles)
        {
            result.Add(new RoleDTO {
                Id = role.Id,
                Name = role.Name,
                PermissionsSet = await groupRepository.GetPermissions(role)
            });
        }

        return result;
    }

}