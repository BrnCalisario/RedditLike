namespace Reddit.Repositories;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Model;

public interface IPostRepository : IRepository<Post>
{
    Task Vote(Upvote vote);
    Task UndoVote(User user, Post post);
    Task AddComment(Comment comment);
    Task RemoveComment(Comment comment);
    Task<int> GetLikeCount(Post post);
    Task<bool> HasVoted(User user, Post post);
    Task<PostVote> GetPostVote(User user, Post post);
}


public class PostRepository : IPostRepository
{
    private RedditContext ctx;

    public PostRepository(RedditContext ctx)
        => this.ctx = ctx;

    public async Task Add(Post obj)
    {
        this.ctx.Posts.Add(obj);
        await this.ctx.SaveChangesAsync();
    }

    public async Task Delete(Post obj)
    {
        var votesToRemove = this.ctx.Upvotes.Where(v => v.PostId == obj.Id);
        ctx.Upvotes.RemoveRange(votesToRemove.ToList());

        var commentsToRemove = this.ctx.Comments.Where(c => c.PostId == obj.Id);
        ctx.Comments.RemoveRange(commentsToRemove.ToList());

        this.ctx.Posts.Remove(obj);
        await this.ctx.SaveChangesAsync();

    }

    public async Task<List<Post>> Filter(Expression<Func<Post, bool>> exp)
    {
        var query = ctx.Posts.Include(p => p.Author).Where(exp);
        return await query.ToListAsync();
    }

    public async Task Save()
    {
        await this.ctx.SaveChangesAsync();
    }

    public async Task Update(Post obj)
    {
        this.ctx.Posts.Update(obj);
        await this.ctx.SaveChangesAsync();
    }

    public async Task Vote(Upvote vote)
    {
        this.ctx.Upvotes.Add(vote);
        await this.ctx.SaveChangesAsync();
    }

    public async Task UndoVote(User user, Post post)
    {
        Upvote vote = this.ctx.Upvotes.FirstOrDefault(v => v.PostId == post.Id && v.UserId == user.Id);

        if(vote is null)
            return;

        this.ctx.Upvotes.Remove(vote);
        await this.ctx.SaveChangesAsync();
    }

    public async Task AddComment(Comment comment)
    {
        await this.ctx.Comments.AddAsync(comment);
        await this.ctx.SaveChangesAsync();
    }

    public async Task RemoveComment(Comment comment)
    {
        this.ctx.Comments.Remove(comment);
        await this.ctx.SaveChangesAsync();
    }

    public async Task<Post> Find(int id)
    {
        var post = await this.ctx.Posts.Include(p => p.Author).Where(p => p.Id == id).FirstAsync();
        return post;
    }

    public async Task<int> GetLikeCount(Post post)
    {
        var likes = await this.ctx.Upvotes
            .Where(up => up.PostId == post.Id)
            .SumAsync(up => (up.Value ?? false) ? 1 : -1);

        return likes;
    }

    public async Task<bool> HasVoted(User user, Post post)
    {
        bool hasVoted = await this.ctx.Upvotes.AnyAsync(up => up.PostId == post.Id && up.UserId == user.Id);
        return hasVoted;
    }

    public async Task<PostVote> GetPostVote(User user, Post post)
    {
        bool? value = await this.ctx.Upvotes
            .Where(up => up.PostId == post.Id && up.UserId == user.Id)
            .Select(up => up.Value)
            .FirstOrDefaultAsync();

        if (value is null)
            return PostVote.None;

        if (value ?? false)
            return PostVote.UpVote;
        else
            return PostVote.DownVote;
    }
}

public enum PostVote
{
    None,
    UpVote,
    DownVote
}