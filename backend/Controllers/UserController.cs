using Microsoft.AspNetCore.Mvc;
using Security.PasswordHasher;

namespace Reddit.Controllers;

using Model;
using Repositories;
using DTO;
using Microsoft.AspNetCore.Cors;

[ApiController]

[Route("users")]
public class UserController : ControllerBase
{

    [HttpGet]
    [EnableCors("MainPolicy")]
    public async Task<ActionResult<List<User>>> GetAll(
        [FromServices] IUserRepository userRepository
    )
    {
        var query = await userRepository.Filter(u => true);
        return query;
    }

    [HttpPost("/register")]
    [EnableCors("MainPolicy")]
    public async Task<ActionResult> Register(
        [FromServices] IUserRepository userRep,
        [FromServices] IPasswordHasher psh,
        [FromBody] UserRegister userData)
    {
        if (await userRep.userNameExists(userData.Username)

            || await userRep.emailExists(userData.Email))
            return BadRequest();

        byte[] hashPassword;
        string salt;

        (hashPassword, salt) = psh.GetHashAndSalt(userData.Password);

        User u = new User()
        {
            Username = userData.Username,
            Email = userData.Email,
            Password = hashPassword,
            Salt = salt,
            BirthDate = userData.Birthdate,
            ProfilePicture = null
        };

        await userRep.Add(u);

        return Ok();

    }

    [HttpPost("/login")]
    [EnableCors("MainPolicy")]
    public async Task<ActionResult> Login(
        [FromBody] UserLogin loginData,
        [FromServices] IPasswordHasher psh,
        [FromServices] IUserRepository userRep
    )
    {
        var userList = await userRep.Filter(u => u.Email == loginData.Email);

        if (userList.Count() == 0)
            return BadRequest();

        User target = userList.First();

        if (psh.Validate(loginData.Password, target.Salt, target.Password))
            return Ok();

        return BadRequest();
    }
}