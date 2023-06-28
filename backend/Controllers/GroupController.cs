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

        var owner = await userRepo.Find(group.OwnerId);

        group.Owner = owner;

        return group;
    }


    [HttpGet]
    public async Task<ActionResult<Group>> GetAll(
        [FromServices] IGroupRepository groupRepo
    )
    {
        var groups = await groupRepo.Filter(u => true);
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

        Group group = new Group() {
            OwnerId = user.Id,
            Name = groupDTO.Name,
            Description = groupDTO.Description,
            CreationDate = DateTime.Now,
        };

        await groupRepo.Add(group);

        group.Owner = user;

        return Ok();
    }
}