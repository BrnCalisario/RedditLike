
using System;
using System.Linq.Expressions;

namespace Reddit.Repositories;

using Model;

public class SQLRepository<T> : IRepository<T>
{
    private RedditContext entity;

    // private DbSet table => typeof(T) switch
    // {
    //     typeof(Group) => entity.Groups
    // }


    public SQLRepository(RedditContext service)
        => this.entity = service;

    public async void Add(T obj)
    {
        entity.Add(obj);
        await entity.SaveChangesAsync();
    }   

    public async void Delete(T obj)
    {
        entity.Remove(obj);
        await entity.SaveChangesAsync();
    }

    public async void Update(T obj)
    {
        entity.Update(obj);
        await entity.SaveChangesAsync();
    }

    public async Task<List<T>> Filter(Expression<Func<T, bool>> exp)
    {
        throw new NotImplementedException();
    }

}