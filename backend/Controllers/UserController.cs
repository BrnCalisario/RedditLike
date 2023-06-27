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

    [HttpPost("/validate")]
    public async Task<ActionResult<bool>> ValidateJwt(
        [FromServices] IJwtService jwtService,
        [FromBody] string jwt
    )
    {
        try {
            var result = jwtService.Validate<UserJwt>(jwt);
            return Ok(true);
        } catch(Exception e){
            return BadRequest(e.Message);
        }
    }


    [HttpPost("/login")]
    public async Task<ActionResult<bool>> Login(
        [FromBody] UserLogin loginData,
        [FromServices] IPasswordHasher psh,
        [FromServices] IUserRepository userRep,
        [FromServices] IJwtService jwtService
    )
    {   
        var result = new LoginResult();

        var userList = await userRep.Filter(u => u.Email == loginData.Email);

        result.UserExists = userList.Count() > 0;
        if (!result.UserExists)
        {
            return Ok(result);
        }
        
        User target = userList.First();

        if (psh.Validate(loginData.Password, target.Password, target.Salt))
        {
            string token = jwtService.GetToken<UserJwt>(new UserJwt { UserID = target.Id });

            result.Jwt = token;
            result.Success = true;
            return Ok(result);
        }

        result.Success = false;
        return Ok(result);
    }
}