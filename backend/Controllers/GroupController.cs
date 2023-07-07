using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Security.Jwt;

namespace Reddit.Controllers;

using Model;
using Repositories;
using DTO;
using Services;

[ApiController]
[EnableCors("MainPolicy")]
[Route("group")]
public class GroupController : ControllerBase
{
    [HttpPost("by-name")]
    public async Task<ActionResult<GroupDTO>> GetGroup(
        [FromBody] CreateGroupDTO groupData,
        [FromServices] IGroupRepository groupRepository,
        [FromServices] IUserService userService
    )
    {
        User user;
        try
        {
            user = await userService.ValidateUserToken(new Jwt { Value = groupData.Jwt });
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        if (user is null)
            return NotFound("User not found");

        var query = await groupRepository.Filter(g => g.Name == groupData.Name);

        Group group = query.FirstOrDefault();

        if(group is null)   
            return NotFound("Group not found");

        GroupDTO result = new GroupDTO {
            Id = group.Id,
            Name = group.Name,
            Description = group.Description,
            OwnerID = group.OwnerId,
            ImageId = group.Image,
            UserQuantity = await groupRepository.GetUserQuantity(group), 
            isMember = await groupRepository.IsMember(user, group),
        };

        if(result.isMember)
            result.UserRole = await groupRepository.GetRoleName(user, group);

        return Ok(result);
    }


    [HttpPost("list")]
    public async Task<ActionResult<List<Group>>> ListGroups(
    [FromServices] IGroupRepository groupRepository,
    [FromServices] IUserService userService,
    [FromBody] Jwt jwt
    )
    {
        User user;
        try
        {
            user = await userService.ValidateUserToken(jwt);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        if (user is null)
            return NotFound();

        var allGroups = await groupRepository.Filter(g => true);

        var result = new List<GroupDTO>();
        foreach (var g in allGroups)
        {
            result.Add(new GroupDTO
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                ImageId = g.Image,
                isMember = await groupRepository.IsMember(user, g),
                UserQuantity = await groupRepository.GetUserQuantity(g),
            });
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Post(
        [FromServices] IGroupRepository groupRepo,
        [FromServices] IUserService userService,
        [FromBody] CreateGroupDTO groupData
    )
    {
        User user;
        try
        {
            user = await userService.ValidateUserToken(new Jwt { Value = groupData.Jwt });
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        if (user is null)
            return NotFound();

        var duplicates = await groupRepo.Filter(g => g.Name == groupData.Name.ToLower());

        if (duplicates.Count() > 0)
            return BadRequest("Group already exists");

        if(groupData.Name.Split(" ").Length > 1) 
            return BadRequest("Group name must not contain spaces");

        Group group = new Group()
        {
            OwnerId = user.Id,
            Name = groupData.Name.ToLower(),
            Description = groupData.Description,
            CreationDate = DateTime.Now,
        };

        await groupRepo.Add(group);

        var query = await groupRepo.Filter(g => g.Name == group.Name);
        int groupId = query.First().Id;

        return Ok(groupId);
    }

    [HttpPut]
    public async Task<ActionResult> Update(
        [FromBody] GroupDTO groupData,
        [FromServices] IGroupRepository groupRepository,
        [FromServices] IUserService userService
    )
    {
        Group group = await groupRepository.Find(groupData.Id);

        if (group is null)
            return NotFound("Grupo não encontrado");

        User user;
        try
        {
            user = await userService.ValidateUserToken(new Jwt { Value = groupData.Jwt });
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        if (user is null)
            return NotFound();

        if (group.Description != groupData.Description && !string.IsNullOrWhiteSpace(groupData.Description))
            group.Description = groupData.Description;

        await groupRepository.Update(group);

        return Ok();
    }

    [HttpDelete("remove")]
    public async Task<ActionResult> Delete(
        [FromBody] GroupDTO groupData,
        [FromServices] IGroupRepository groupRepository,
        [FromServices] IUserService userService
    )
    {
        Group group = await groupRepository.Find(groupData.Id);

        if (group is null)
            return NotFound();

        User user;
        try
        {
            user = await userService.ValidateUserToken(new Jwt { Value = groupData.Jwt });
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        if (user is null)
            return NotFound();

        bool canDrop = await groupRepository.HasPermission(user, group, PermissionEnum.DropGroup);

        if (!canDrop || group.OwnerId != user.Id)
            return BadRequest();

        await groupRepository.Delete(group);

        return Ok();
    }

    [HttpPost("enter")]
    public async Task<ActionResult> AddMember(
        [FromBody] MemberDTO memberData,
        [FromServices] IGroupRepository groupRepository,
        [FromServices] IUserService userService
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

        bool isMember = await groupRepository.IsMember(user, group);

        if(isMember)
            return BadRequest("Already a member");

        await groupRepository.AddMember(group, user);

        return Ok();
    }

    [HttpPost("exit")]
    public async Task<ActionResult> ExitGroup(
        [FromBody] MemberDTO memberData,
        [FromServices] IGroupRepository groupRepository,
        [FromServices] IUserService userService
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

        if (group.OwnerId == user.Id)
            return BadRequest("Owner can't quit");

        await groupRepository.RemoveMember(group, user);

        return Ok();
    }

    [HttpPost("remove-member")]
    public async Task<ActionResult> RemoveMember(
        [FromBody] MemberDTO memberData,
        [FromServices] IGroupRepository groupRepository,
        [FromServices] IUserService userService,
        [FromServices] IUserRepository userRepository
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


        bool canBan = await groupRepository.HasPermission(user, group, PermissionEnum.Ban);

        if (!canBan)
            return BadRequest("Don't have permission");

        var target = await userRepository.Find(memberData.UserId);

        if (target is null)
            return NotFound("User not found");

        await groupRepository.RemoveMember(group, target);

        return Ok();
    }


    [HttpPost("group-members")]
    public async Task<ActionResult<List<MemberItemDTO>>> GetGroupMembers(
        [FromBody] CreateGroupDTO memberData,
        [FromServices] IGroupRepository groupRepository,
        [FromServices] IUserService userService,
        [FromServices] IUserRepository userRepository
    )
    {
        Group group = await groupRepository.Find(memberData.Id);

        if (group is null)
            return NotFound("Group");

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

        var userList = await groupRepository.GetGroupMembers(group);

        return Ok(userList);
    }

}
