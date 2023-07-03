namespace Reddit.Repositories;

using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model;

public class UpvoteRepository : IRepository<Upvote>
{
    private RedditContext ctx;

    public UpvoteRepository(RedditContext ctx)
        => this.ctx = ctx;


    public async Task Add(Upvote obj)
    {
        this.ctx.Upvotes.Add(obj);
        await this.ctx.SaveChangesAsync();
    }

    public async Task Delete(Upvote obj)
    {
        this.ctx.Upvotes.Remove(obj);
        await this.ctx.SaveChangesAsync();
    }

    public async Task<List<Upvote>> Filter(Expression<Func<Upvote, bool>> exp)
    {
        var query = ctx.Upvotes.Where(exp);
        return await query.ToListAsync();
    }

    public async Task<Upvote> Find(int id)
    {
        var vote = await ctx.Upvotes.FindAsync(id);
        return vote;
    }

    public async Task Save()
    {
        await ctx.SaveChangesAsync();
    }

    public async Task Update(Upvote obj)
    {
        this.ctx.Upvotes.Update(obj);
        await ctx.SaveChangesAsync();
    }
}