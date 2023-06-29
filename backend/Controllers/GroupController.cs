using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

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
    [HttpGet("{id}")]
    public async Task<ActionResult<Group>> GetSingle(
        [FromServices] IGroupRepository groupRepo,
        [FromServices] IUserRepository userRepo,
        int id
    )
    {
        var groupList = await groupRepo.Filter(g => g.Id == id);

        if(groupList.Count == 0)
            return BadRequest("");

        var group = groupList.First();

        group.Owner.Groups = null;

        // var owner = await userRepo.Find(group.OwnerId);

        // group.Owner = owner;

        return group;
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
    public async Task<ActionResult> Post(
        [FromServices] IGroupRepository groupRepo,
        [FromServices] IUserRepository userRepo,
        [FromBody] GroupDTO groupDTO
    ) 
    {
        var user = await userRepo.Find(groupDTO.OwnerID);

        if(user is null)
            return BadRequest("Usuário inválido");

        var duplicates = await groupRepo.Filter(g => g.Name == groupDTO.Name.ToLower());

        if(duplicates.Count() > 0)
            return BadRequest("Group already exists"); 

        Group group = new Group() {
            OwnerId = user.Id,
            Name = groupDTO.Name.ToLower(),
            Description = groupDTO.Description,
            CreationDate = DateTime.Now,
        };

        await groupRepo.Add(group);

        group.Owner = user;

        return Ok();
    }
}