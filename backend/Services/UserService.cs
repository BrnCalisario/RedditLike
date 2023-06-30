using Reddit.Model;
using Reddit.Repositories;
using Security.Jwt;

public interface IUserService
{
    Task<User> ValidateUserToken(Jwt jwt);
}

public class UserService : IUserService
{
    private IJwtService jwtService;
    private IUserRepository userRepository;

    public UserService(IJwtService jwtService, IUserRepository userRepository)
    {
        this.jwtService = jwtService;
        this.userRepository = userRepository;
    }

    public async Task<User> ValidateUserToken(Jwt jwt)
    {
        User user = null;

        var token = jwtService.Validate<UserToken>(jwt.Value);

        if(!token.Authenticated)
            throw new InvalidDataException();

        user = await userRepository.Find(token.UserID);
    
        return user;
    }
}