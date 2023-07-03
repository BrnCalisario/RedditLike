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
    [HttpPost]
    public async Task<ActionResult> AddRole(
        [FromBody] RoleDTO roleData,
        [FromServices] IGroupRepository groupRepository,
        [FromServices] IRoleRepository roleRepository,
        [FromServices] IUserService userService
    )
    {
        User user;
        try
        {
            user = await userService.ValidateUserToken(new Jwt { Value = roleData.Jwt });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

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
        [FromServices] IRoleRepository roleRepository,
        [FromServices] IUserService userService
    )
    {
        if (roleData.Id == 0)
            return BadRequest();

        User user;
        try
        {
            user = await userService.ValidateUserToken(new Jwt { Value = roleData.Jwt });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        if (user is null)
            return NotFound();

        Role role = await roleRepository.Find(roleData.Id);

        await roleRepository.UpdateRole(role, roleData.PermissionsSet);

        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteRole(
        [FromBody] RoleDTO roleData,
        [FromServices] IGroupRepository groupRepository,
        [FromServices] IRoleRepository roleRepository,
        [FromServices] IUserService userService
    )
    {
        if (roleData.Id == 0)
            return BadRequest();

        User user;
        try
        {
            user = await userService.ValidateUserToken(new Jwt { Value = roleData.Jwt });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

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
    [FromServices] IUserService userService,
    [FromServices] IRepository<Role> roleRepository
)
    {
        Group group = await groupRepository.Find(memberData.GroupId);

        if (group is null)
            return BadRequest();

        User user;
        try
        {
            user = await userService.ValidateUserToken(new Jwt { Value = memberData.Jwt });
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        if (user is null)
            return NotFound();

        var role = await roleRepository.Find(memberData.RoleId);

        if (role is null)
            return NotFound("Role not found");

        await groupRepository.PromoteMember(group, user, role);

        return Ok();
    }

}