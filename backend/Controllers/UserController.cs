using Microsoft.AspNetCore.Mvc;
using Security.Jwt;

namespace Reddit.Controllers;

using Model;
using Repositories;
using DTO;

using Microsoft.AspNetCore.Cors;
using Reddit.Services;

[ApiController]
[EnableCors("MainPolicy")]
[Route("users")]
public class UserController : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAll(
        [FromServices] IUserRepository userRepository
    )
    {
        var query = await userRepository.Filter(u => true);
        return query;
    }

    [HttpPost("/register")]
    public async Task<ActionResult> Register(
        [FromServices] IUserRepository userRep,
        [FromServices] IPasswordHasher psh,
        [FromBody] UserRegister userData)
    {

        var query = await userRep.Filter(u => u.Username == userData.Username || u.Email == userData.Email);

        if(query.Count() > 0)
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
            ProfilePicture = null,
            BirthDate = userData.Birthdate,
        };

        await userRep.Add(u);

        return Ok();

    }

    [HttpPost("/login")]
    public async Task<ActionResult<LoginResult>> Login(
        [FromBody] UserLogin loginData,
        [FromServices] IPasswordHasher psh,
        [FromServices] IUserRepository userRep,
        [FromServices] IJwtService jwtService
    )
    {   
        var result = new LoginResult();

        var userList = await userRep.Filter(u => u.Email == loginData.Email);

        if (userList.Count() == 0)
            return BadRequest(result);
        

        User target = userList.First();

        if (psh.Validate(loginData.Password, target.Password, target.Salt))
        {
            string token = jwtService.GetToken<JwtToken>(new JwtToken { UserID = target.Id, Logged = true });

            return Ok();
        }

        result.Success = false;

        return BadRequest(result);
        
    }
}