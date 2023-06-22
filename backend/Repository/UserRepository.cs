using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Reddit.Repositories;
using Model;

public class UserRepository : IRepository<User>
{
    private RedditContext ctx;

    public UserRepository(RedditContext ctx)
        => this.ctx = ctx;

    public async void Add(User obj)
    {
        await ctx.Users.AddAsync(obj);
        await ctx.SaveChangesAsync();
    }

    public async void Delete(User obj)
    {
        ctx.Users.Remove(obj);
        await ctx.SaveChangesAsync();
    }

    public async Task<List<User>> Filter(Expression<Func<User, bool>> exp)
    {
        var query = ctx.Users.Where(exp);
        return await query.ToListAsync();
    }

    public async void Update(User obj)
    {
        ctx.Users.Update(obj);
        await ctx.SaveChangesAsync();
    }
}