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
    [HttpGet]
    public async Task<ActionResult<Group>> Get(
        [FromServices] IRepository<Group> groupRepo
    )
    {
        var groups = await groupRepo.Filter(u => true);
        return Ok(groups);
    }


    [HttpPost]
    public async Task<ActionResult> Post(
        [FromServices] IRepository<Group> groupRepo,
        [FromServices] IUserRepository userRepo,
        [FromBody] GroupDTO groupDTO
    ) 
    {
        var user = await userRepo.Find(groupDTO.OwnerID);

        if(user is null)
            return BadRequest();

        Group group = new Group() {
            OwnerId = user.Id,
            Name = groupDTO.Name,
            Description = groupDTO.Description,
        };

        await groupRepo.Add(group);

        group.Owner = user;

        return Ok();
    }
}