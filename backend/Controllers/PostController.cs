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
[Route("post")]
public class PostController : Controller
{

    [HttpPost]
    public async Task<ActionResult> Post(
        [FromBody] CreatePostDTO postData,
        [FromServices] IPostRepository postRepository,
        [FromServices] IGroupRepository groupRepository,
        [FromServices] UserService userService
    )
    {
        User user;
        try
        {
            user = await userService.ValidateUserToken(new Jwt { Value = postData.Jwt });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        if (user is null)
            return NotFound();

        var groupQuery = await groupRepository.Filter(g => g.Id == postData.GroupID);
        var group = groupQuery.FirstOrDefault();

        if (group is null)
            return BadRequest();

        Post post = new Post
        {
            Title = postData.Title,
            Content = postData.Content,
            GroupId = postData.GroupID,
            AuthorId = user.Id,
            IndexedImage = null,
        };

        await postRepository.Add(post);

        return Ok();
    }


    [HttpPost]
    public async Task<ActionResult> LikePost(
        [FromBody] VoteDTO voteData,
        [FromServices] IPostRepository postRepository,
        [FromServices] UserService userService
    )
    {
        User user;
        try
        {
            user = await userService.ValidateUserToken(new Jwt { Value = voteData.Jwt });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        Upvote vote = new Upvote()
        {
            UserId = user.Id,
            PostId = voteData.PostId,
            Value = voteData.Value
        };

        await postRepository.Vote(vote);

        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult> UnlikePost(
        [FromBody] VoteDTO voteData,
        [FromServices] IPostRepository postRepository,
        [FromServices] UserService userService
    )
    {
        User user;
        try
        {
            user = await userService.ValidateUserToken(new Jwt { Value = voteData.Jwt });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        await postRepository.UndoVote(voteData.PostId);

        return Ok();
    }

}