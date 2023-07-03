namespace Reddit.Repositories;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Model;

public interface IPostRepository : IRepository<Post>
{
    Task Vote(Upvote vote);   
    Task UndoVote(int voteId);
    Task AddComment(Comment comment);
    Task RemoveComment(Comment comment);
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
        var query = ctx.Posts.Where(exp);
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

    public async Task UndoVote(int voteID)
    {
        Upvote vote = this.ctx.Upvotes.First(v => v.Id ==voteID);
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
        var post = await this.ctx.Posts.FindAsync(id);
        return post;
    }
}