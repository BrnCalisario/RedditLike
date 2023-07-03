namespace Reddit.Repositories;

using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model;

public class CommentRepository : IRepository<Comment>
{
    private RedditContext ctx;

    public CommentRepository(RedditContext ctx)
        => this.ctx = ctx;

    public async Task Add(Comment obj)
    {
        this.ctx.Comments.Add(obj);
        await this.ctx.SaveChangesAsync();
    }

    public async Task Delete(Comment obj)
    {
        this.ctx.Comments.Remove(obj);
        await this.ctx.SaveChangesAsync();
    }

    public async Task<List<Comment>> Filter(Expression<Func<Comment, bool>> exp)
    {
        var query = ctx.Comments.Where(exp);
        return await query.ToListAsync();
    }

    public async Task<Comment> Find(int id)
    {
        var comment = await 
            ctx.Comments
                .Include(c => c.Post)
                .Include(c => c.AuthorId)
                .FirstAsync(c => c.Id == id);
        return comment;
    }

    public async Task Save()
    {
        await ctx.SaveChangesAsync();
    }

    public async Task Update(Comment obj)
    {
        this.ctx.Comments.Update(obj);
        await ctx.SaveChangesAsync();
    }
}