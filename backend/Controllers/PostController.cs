using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Reddit.Controllers;

using Model;
using Repositories;
using DTO;
using Services;

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
        [FromServices] IUserService userService
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

    [HttpPost("vote")]
    public async Task<ActionResult> LikePost(
        [FromBody] VoteDTO voteData,
        [FromServices] IPostRepository postRepository,
        [FromServices] IUserService userService
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

    [HttpPost("undo")]
    public async Task<ActionResult> UnlikePost(
        [FromBody] VoteDTO voteData,
        [FromServices] IPostRepository postRepository,
        [FromServices] IUserService userService
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


    [HttpPost("comment")]
    public async Task<ActionResult> Comment(
        [FromBody] CommentDTO commentData,
        [FromServices] IPostRepository postRepository,
        [FromServices] IUserService userService
    )
    {
        if (commentData.Content.Length < 1)
            return BadRequest("Conteúdo necessário");

        User user;
        try
        {
            user = await userService.ValidateUserToken(new Jwt { Value = commentData.Jwt });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        Comment c = new Comment()
        {
            AuthorId = user.Id,
            PostId = commentData.PostID,
            Content = commentData.Content,
        };

        await postRepository.AddComment(c);

        return Ok();
    }


    [HttpDelete("delete-comment")]
    public async Task<ActionResult> DeleteComment(
        [FromBody] CommentDTO commentData,
        [FromServices] IGroupRepository groupRepository,
        [FromServices] IPostRepository postRepository,
        [FromServices] IRepository<Comment> commentRepository,
        [FromServices] IUserService userService
    )
    {
        User user;
        try
        {
            user = await userService.ValidateUserToken(new Jwt { Value = commentData.Jwt });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        if (user is null)
            return BadRequest("Invalid user");


        var comment = await commentRepository.Find(commentData.Id);

        Group group = comment.Post.Group;
        Console.WriteLine(group);

        bool canRemove = await groupRepository.HasPermission(user, group, PermissionEnum.Delete);

        if (!canRemove && comment.AuthorId != user.Id)
            return StatusCode(405);

        await commentRepository.Delete(comment);

        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> UpdatePost(
        [FromBody] CreatePostDTO postData,
        [FromServices] IPostRepository postRepository,
        [FromServices] IUserService userService
    )
    {
        Post post = await postRepository.Find(postData.Id);

        if (post is null)
            return NotFound();

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
            return BadRequest("Invalid user");

        if (post.Title != postData.Title)
            post.Title = postData.Title;

        if (post.Content != postData.Content)
            post.Content = postData.Content;

        await postRepository.Update(post);

        return Ok();
    }

    [HttpPost("remove")]
    public async Task<ActionResult> Delete(
        [FromBody] CreatePostDTO postData,
        [FromServices] IPostRepository postRepository,
        [FromServices] IGroupRepository groupRepository,
        [FromServices] IUserService userService
    )
    {
        Post post = await postRepository.Find(postData.Id);

        if (post is null)
            return NotFound();

        User user;
        try
        {
            user = await userService.ValidateUserToken(new Jwt { Value = postData.Jwt });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        if (post.Group is null)
            post.Group = await groupRepository.Find(postData.GroupID);


        bool canDelete = await groupRepository.HasPermission(user, post.Group, PermissionEnum.Delete);

        if (!canDelete && post.AuthorId != user.Id)
            return BadRequest();

        await postRepository.Delete(post);

        return Ok();
    }


    [HttpPost("main-feed")]
    public async Task<ActionResult<List<FeedPostDTO>>> GetMainFeed(
        [FromBody] Jwt jwt,
        [FromServices] IUserService userService,
        [FromServices] IPostRepository postRepository,
        [FromServices] IGroupRepository groupRepository
    )
    {
        User user;
        try
        {
            user = await userService.ValidateUserToken(jwt);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        if (user is null)
            return NotFound();

        List<FeedPostDTO> feedPosts = new List<FeedPostDTO>();

        var userGroups = await groupRepository.GetUserGroups(user);

        if (userGroups.Count() == 0)
            return Ok(feedPosts);

        foreach (var group in userGroups)
        {
            var posts = await postRepository.Filter(p => p.GroupId == group.Id);

            foreach (var post in posts)
            {
                FeedPostDTO fp = new FeedPostDTO
                {
                    Id = post.Id,
                    Title = post.Title,
                    Content = post.Content,
                    PostDate = post.PostDate,
                    AuthorName = post.Author.Username,
                    GroupName = group.Name,
                    LikeCount = await postRepository.GetLikeCount(post),
                };

                feedPosts.Add(fp);
            }
        }

        return Ok(feedPosts);
    }

    [HttpPost("group-feed")]
    public async Task<ActionResult<List<FeedPostDTO>>> GetGroupFeed(
        [FromBody] CreateGroupDTO groupData,
        [FromServices] IUserService userService,
        [FromServices] IPostRepository postRepository,
        [FromServices] IGroupRepository groupRepository
    )
    {
        User user;
        try
        {
            user = await userService.ValidateUserToken(new Jwt { Value = groupData.Jwt });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        if (user is null)
            return NotFound("User not found");

        List<FeedPostDTO> feedPosts = new List<FeedPostDTO>();

        Group group = await groupRepository.Find(groupData.Id);

        if(group is null)
            return NotFound("Group not found");

        var groupPosts = await postRepository.Filter(p => p.GroupId == group.Id);

        foreach (var post in groupPosts)
        {
            FeedPostDTO fp = new FeedPostDTO
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                PostDate = post.PostDate,
                AuthorName = post.Author.Username,
                GroupName = group.Name,
                LikeCount = await postRepository.GetLikeCount(post),
            };

            feedPosts.Add(fp);
        }

        return Ok(feedPosts);
    }
}