using System.Linq.Expressions;

namespace Reddit.Repositories;

using Microsoft.EntityFrameworkCore;
using Model;

public interface IRoleRepository
{
    Task InsertRole(Role role, List<int> permissions);
    Task DeleteRole(Role role);
    Task UpdateRole(Role role, List<int> permissions);
    Task<List<Role>> Filter(Expression<Func<Role, bool>> exp);
    Task<Role> Find(int id);
}

public class RoleRepository : IRoleRepository
{
    private RedditContext ctx;

    public RoleRepository(RedditContext ctx)
        => this.ctx = ctx;

    public async Task<List<Role>> Filter(Expression<Func<Role, bool>> exp)
    {
        var query = this.ctx.Roles.Where(exp);
        return await query.ToListAsync();
    }

    public async Task<Role> Find(int id)
    {
        var role = await this.ctx.Roles.Include(r => r.Group).FirstAsync(r => r.Id == id);
        return role;
    }

    public async Task InsertRole(Role role, List<int> permissions)
    {
        await this.ctx.Roles.AddAsync(role);

        foreach (var permission in permissions)
        {
            await this.ctx.RolePermissions.AddAsync(new RolePermission()
            {
                RoleId = role.Id,
                PermissionId = permission,
            });
        }

        await this.ctx.SaveChangesAsync();
    }

    public async Task DeleteRole(Role role)
    {
        var rolesToRemove = await this.ctx.RolePermissions.Where(rl => rl.RoleId == role.Id).ToListAsync();
        this.ctx.RolePermissions.RemoveRange(rolesToRemove);

        this.ctx.Roles.Remove(role);

        await this.ctx.SaveChangesAsync();
    }


    public async Task UpdateRole(Role role, List<int> permissions)
    {
        this.ctx.Roles.Update(role);

        var rolesToRemove = await this.ctx.RolePermissions.Where(rl => rl.RoleId == role.Id).ToListAsync();
        this.ctx.RolePermissions.RemoveRange(rolesToRemove);

        foreach (var permission in permissions)
        {
            await this.ctx.RolePermissions.AddAsync(new RolePermission() {
                RoleId = role.Id,
                PermissionId = permission
            });
        }

        await this.ctx.SaveChangesAsync();
    }
}