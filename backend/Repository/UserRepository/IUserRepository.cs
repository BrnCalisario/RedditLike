namespace Reddit.Repositories;

using Model;

public interface IUserRepository : IRepository<User>
{
    Task<bool> userNameExists(string username);
    Task<bool> emailExists(string email);
}