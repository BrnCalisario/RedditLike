using Microsoft.AspNetCore.Mvc;


namespace Reddit.Controllers;

using DTO;
using Services;
using Model;

public class RedditController : ControllerBase
{
    private IUserService userService;

    public RedditController([FromServices] IUserService userService)
        => this.userService = userService;

    protected virtual async Task<User> ValidateJwt(string jwt)
    {
        User user = new User();
        try
        {
            user = await this.userService.ValidateUserToken(new Jwt { Value = jwt });
        }
        catch
        {
            return user;
        }

        return user;
    }
}