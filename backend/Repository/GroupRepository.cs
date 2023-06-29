using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Reddit.Repositories;

using System;
using System.Collections.Generic;
using Model;


public interface IGroupRepository : IRepository<Group>
{
    Task AddMember(Group group, User user);
    Task RemoveMember(Group group, User user);
    Task<List<Group>> GetUserGroups(User user);
    int GetUserQuantity(Group group);
}


public class GroupRepository : IGroupRepository
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

    public async Task Update(Group obj)
    {
        ctx.Groups.Update(obj);
        await ctx.SaveChangesAsync();
    }

    public Task<List<Group>> Filter(Expression<Func<Group, bool>> exp)
    {
        var query = ctx.Groups
            .Include(g => g.Owner)
            .Where(exp);
        return query.ToListAsync();
    }


    public async Task Save()
    {
        await this.ctx.SaveChangesAsync();
    }

    public async Task AddMember(Group group, User user)
    {
        UserGroup ug = new UserGroup();
        
        ug.UserId = user.Id;
        ug.GroupId = group.Id;

        await ctx.UserGroups.AddAsync(ug);
        await ctx.SaveChangesAsync();
    }

    public async Task RemoveMember(Group group, User user)
    {
        var target = ctx.UserGroups.FirstOrDefault(ug => ug.UserId == user.Id && ug.GroupId == group.Id);
        ctx.UserGroups.Remove(target);
        await ctx.SaveChangesAsync();
    }

    public async Task<List<Group>> GetUserGroups(User user)
    {
        var query = ctx.UserGroups
            .Where(ug => ug.UserId == user.Id)
            .Select(ug => ug.Group);

        return await query.ToListAsync();
    }

    public int GetUserQuantity(Group group)
    {
        int count = ctx.UserGroups.Count(ug => ug.GroupId == group.Id);
        return count;
    }
}