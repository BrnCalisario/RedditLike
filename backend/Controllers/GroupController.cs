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
public class GroupController : Controller
{
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
        var userGroups = await groupRepository.GetUserGroups(user);


        // Precisando deixar o Get User Quantity Assincrono
        var result = new List<GroupDTO>();
        foreach (var g in allGroups)
        {
            result.Add(new GroupDTO
            {
                Name = g.Name,
                Description = g.Description,
                ImageId = g.Image,
                isMember = userGroups.Any(ug => g.Id == ug.Id),
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

    [HttpDelete]
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

        if(!canDrop || group.OwnerId != user.Id)
            return BadRequest();

        await groupRepository.Delete(group);

        return Ok();
    }



    [HttpPost("addImage")]
    public async Task<ActionResult> AddImage(
        [FromServices] IGroupRepository groupRepository,
        [FromServices] IImageService imageService,
        [FromServices] IUserService userService
    )
    {
        var jwt = Request.Form["jwt"].ToString();

        User user;
        Group group;
        try
        {
            int groupId;
            if (!int.TryParse(Request.Form["groupId"].ToString(), out groupId))
                return BadRequest();

            System.Console.WriteLine(groupId);

            var query = await groupRepository.Filter(g => g.Id == groupId);

            group = query.FirstOrDefault();

            if (group is null)
                return NotFound();


            user = await userService.ValidateUserToken(new Jwt { Value = jwt });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        if (user is null)
            return NotFound();

        var files = Request.Form.Files;

        if (files is null || files.Count == 0)
            return BadRequest();

        var file = Request.Form.Files[0];

        if (file.Length < 1)
            return BadRequest();

        int imageId = await imageService.SaveImg(file);

        group.Image = imageId;
        await groupRepository.Update(group);

        return Ok();
    }



}

// [HttpGet("{id}")]
// public async Task<ActionResult<Group>> GetSingle(
//     [FromServices] IGroupRepository groupRepo,
//     [FromServices] IUserRepository userRepo,
//     int id
// )
// {
//     var group = await groupRepo.Find(id);

//     if (group is null)
//         return NotFound();

//     group.Owner.Groups = null;

//     // var owner = await userRepo.Find(group.OwnerId);
//     // group.Owner = owner;

//     return group;
// }

// [HttpPost("{groupName}")]
// public async Task<ActionResult<GroupDTO>> GetGroup(
//     [FromServices] IGroupRepository groupRepository,
//     [FromServices] IUserService userService,
//     [FromBody] Jwt jwt,
//     string groupName
// )
// {
//     User user;
//     try
//     {
//         user = await userService.ValidateUserToken(jwt);
//     }
//     catch (Exception ex)
//     {
//         return BadRequest(ex.Message);
//     }

//     if (user is null)
//         return NotFound("Usuário não encontrado");

//     var query = await groupRepository.Filter(g => g.Name == groupName);
//     var group = query.FirstOrDefault();

//     if (group is null)
//         return NotFound("Grupo não encontrado");

//     var queryUserGroups = await groupRepository.GetUserGroups(user);

//     bool isMember = queryUserGroups.Any(g => g.Id == group.Id);

//     GroupDTO result = new GroupDTO()
//     {
//         Name = group.Name,
//         OwnerID = group.OwnerId,
//         Description = group.Description,
//         isMember = isMember,
//         UserQuantity = await groupRepository.GetUserQuantity(group),
//         ImageId = group.Image,
//         Posts = new List<PostDTO>()
//     };

//     return Ok(result);
// }

// [HttpGet]
// public async Task<ActionResult<Group>> GetAll(
//     [FromServices] IGroupRepository groupRepo
// )
// {
//     var groups = await groupRepo.Filter(u => true);

//     groups.ForEach(g => g.Owner.Groups = null);

//     return Ok(groups);
// }
