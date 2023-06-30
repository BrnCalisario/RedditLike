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
}