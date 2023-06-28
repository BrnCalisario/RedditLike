using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Reddit.Repositories;
using Model;

public class UserRepository : IUserRepository
{
    private RedditContext ctx;

    public UserRepository(RedditContext ctx)
        => this.ctx = ctx;

    public async Task Add(User obj)
    {
        await ctx.Users.AddAsync(obj);
        await ctx.SaveChangesAsync();
    }

    public async Task Delete(User obj)
    {
        ctx.Users.Remove(obj);
        await ctx.SaveChangesAsync();
    }

    public async Task Update(User obj)
    {   
        ctx.Users.Update(obj);
        await ctx.SaveChangesAsync();
    }

    public async Task<List<User>> Filter(Expression<Func<User, bool>> exp)
    {
        var query = ctx.Users.Where(exp);
        return await query.ToListAsync();
    }

    public async Task<bool> userNameExists(string username)
        => await ctx.Users.AnyAsync(u => u.Username == username);
        
    public async Task<bool> emailExists(string email)
        => await ctx.Users.AnyAsync(u => u.Email == email);

    public async Task Save()
    {
        await ctx.SaveChangesAsync();
    }

    public async Task<User> Find(int id)
    {
        var user = await ctx.Users.FindAsync(id);
        return user;
    }
}