using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Reddit.Repositories;
using Model;


public class PostRepository : IRepository<Post>
{
    private RedditContext ctx;

    public PostRepository(RedditContext ctx)
        => this.ctx = ctx;

    public async void Add(Post obj)
    {
        await ctx.Posts.AddAsync(obj);
        await ctx.SaveChangesAsync();
    }

    public async void Delete(Post obj)
    {
        ctx.Posts.Remove(obj);
        await ctx.SaveChangesAsync();
    }

    public async Task<List<Post>> Filter(Expression<Func<Post, bool>> exp)
    {
        var query = ctx.Posts.Where(exp);
        return await query.ToListAsync();
    }

    public async void Update(Post obj)
    {
        ctx.Posts.Update(obj);
        await ctx.SaveChangesAsync();
    }
}