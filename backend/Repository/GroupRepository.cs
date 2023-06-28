using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Reddit.Repositories;

using System;
using System.Collections.Generic;
using Model;

public class GroupRepository : IRepository<Group>
{
    private RedditContext ctx;

    public GroupRepository(RedditContext ctx)
        => this.ctx = ctx;

    public async Task Add(Group obj)
    {
        await ctx.Groups.AddAsync(obj);
        await ctx.SaveChangesAsync();
    }

    public async Task Delete(Group obj)
    {
        ctx.Groups.Remove(obj);
        await ctx.SaveChangesAsync();
    }

    public Task<List<Group>> Filter(Expression<Func<Group, bool>> exp)
    {
        var query = ctx.Groups.Where(exp);
        return query.ToListAsync();
    }

    public async Task Save()
    {
        await this.ctx.SaveChangesAsync();
    }

    public async Task Update(Group obj)
    {   
        ctx.Groups.Update(obj);
        await ctx.SaveChangesAsync();
    }

}