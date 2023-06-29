using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Reddit.Controllers;

using Model;
using Repositories;
using DTO;
using Services;
using Security.Jwt;

[ApiController]
[EnableCors("MainPolicy")]
[Route("group")]
public class GroupController : Controller
{
    [HttpGet("{id}")]
    public async Task<ActionResult<Group>> GetSingle(
        [FromServices] IGroupRepository groupRepo,
        [FromServices] IUserRepository userRepo,
        int id
    )
    {
        var groupList = await groupRepo.Filter(g => g.Id == id);

        if (groupList.Count == 0)
            return BadRequest("");

        var group = groupList.First();

        group.Owner.Groups = null;

        // var owner = await userRepo.Find(group.OwnerId);

        // group.Owner = owner;

        return group;
    }

    [HttpPost("list")]
    public async Task<ActionResult<List<Group>>> ListGroups(
    [FromServices] IGroupRepository groupRepository,
    [FromServices] IUserRepository userRepository,
    [FromServices] IJwtService jwtService,
    [FromBody] Jwt jwt
)
    {
        User user;
        try
        {
            var token = jwtService.Validate<UserToken>(jwt.Value);

            if (!token.Authenticated)
                return Unauthorized();

            user = await userRepository.Find(token.UserID);
        }
        catch
        {
            return BadRequest();
        }

        if (user is null)
            return NotFound();

        var allGroups = await groupRepository.Filter(g => true);
        var userGroups = await groupRepository.GetUserGroups(user);


        // Precisando deixar o Get User Quantity Assincrono
        var result = allGroups
            .Select(g => new GroupDTO
            {
                Name = g.Name,
                Description = g.Description,
                ImageId = g.Image,
                UserParticipates = userGroups.Any(ug => g.Id == ug.Id),
                UserQuantity = groupRepository.GetUserQuantity(g),
            });


        return Ok(result.ToList());
    }


    [HttpGet]
    public async Task<ActionResult<Group>> GetAll(
        [FromServices] IGroupRepository groupRepo
    )
    {
        var groups = await groupRepo.Filter(u => true);

        groups.ForEach(g => g.Owner.Groups = null);

        return Ok(groups);
    }


    [HttpPost]
    public async Task<ActionResult<int>> Post(
        [FromServices] IGroupRepository groupRepo,
        [FromServices] IUserRepository userRepo,
        [FromBody] GroupDTO groupDTO
    )
    {
        var user = await userRepo.Find(groupDTO.OwnerID);

        if (user is null)
            return BadRequest("Usuário inválido");

        var duplicates = await groupRepo.Filter(g => g.Name == groupDTO.Name.ToLower());

        if (duplicates.Count() > 0)
            return BadRequest("Group already exists");

        Group group = new Group()
        {
            OwnerId = user.Id,
            Name = groupDTO.Name.ToLower(),
            Description = groupDTO.Description,
            CreationDate = DateTime.Now,
        };

        await groupRepo.Add(group);

        System.Console.WriteLine(group.Id);
        return Ok(group.Id);
    }

    [HttpPost("addImage/{groupId}")]
    public async Task<ActionResult> AddImage(
        [FromServices] IGroupRepository groupRepo,
        [FromServices] IRepository<ImageDatum> imageRepo,
        int groupId
    )
    {
        var query = await groupRepo.Filter(g => g.Id == groupId);

        var group = query.FirstOrDefault();

        if(group is null)
            return NotFound("Group not found");

        var files = Request.Form.Files;

        if (files is null || files.Count == 0)
            return BadRequest();

        var file = Request.Form.Files[0];

        if (file.Length < 1)
            return BadRequest();

        using MemoryStream ms = new MemoryStream();

        await file.CopyToAsync(ms);
        var data = ms.GetBuffer();

        var img = new ImageDatum();
        img.Photo = data;
        await imageRepo.Add(img);

        group.Image = img.Id;
        await groupRepo.Save();

        return Ok();
    }
}