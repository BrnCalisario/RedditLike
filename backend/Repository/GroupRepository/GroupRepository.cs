using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Reddit.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using Model;


public interface IGroupRepository : IRepository<Group>
{
    Task AddMember(Group group, User user);
    Task RemoveMember(Group group, User user);
    Task<List<Group>> GetUserGroups(User user);
    Task<int> GetUserQuantity(Group group);
    // Task<List<PermissionEnum>> GetPermissionEnum(User user, Group group);
    Task<bool> HasPermission(User user, Group group, PermissionEnum permission);
    Task PromoteMember(Group group, User user, Role role);
    Task DemoteMember(Group group, User user);
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
        ug.RoleId = 1;

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

    public async Task<int> GetUserQuantity(Group group)
    {
        int count = await ctx.UserGroups.CountAsync(ug => ug.GroupId == group.Id);
        return count;
    }

    public async Task<List<PermissionEnum>> GetPermissionEnum(User user, Group group)
    {
        var role = this.ctx.UserGroups.First(ug => ug.GroupId == group.Id && ug.UserId == user.Id).RoleId;

        var perms = this.ctx.RolePermissions
            .Where(rp => rp.RoleId == role)
            .Select(r => (PermissionEnum)r.Permission.Id)
            .ToListAsync();

        return await perms;
    }

    public async Task<Group> Find(int id)
    {
        var group = await ctx.Groups.FindAsync(id);
        return group;
    }

    public async Task<bool> HasPermission(User user, Group group, PermissionEnum permission)
    {
        var role = this.ctx.UserGroups.First(ug => ug.GroupId == group.Id && ug.UserId == user.Id).RoleId;

        var perms = await this.ctx.RolePermissions
            .Where(rp => rp.RoleId == role)
            .Select(r => (PermissionEnum)r.Permission.Id)
            .ToListAsync();

        var hasPerm = perms.Contains(permission);

        return hasPerm;
    }

    public async Task PromoteMember(Group group, User user, Role role)
    {
        var userGroup = await this.ctx.UserGroups.FirstAsync(ug => ug.UserId == user.Id && ug.GroupId == group.Id);

        userGroup.RoleId = role.Id;

        this.ctx.UserGroups.Update(userGroup);
        await this.ctx.SaveChangesAsync();
    }

    public async Task DemoteMember(Group group, User user)
    {
        var userGroup = await this.ctx.UserGroups.FirstAsync(ug => ug.UserId == user.Id && ug.GroupId == group.Id);

        userGroup.RoleId = 1;

        this.ctx.UserGroups.Update(userGroup);
        await this.ctx.SaveChangesAsync();
    }
}