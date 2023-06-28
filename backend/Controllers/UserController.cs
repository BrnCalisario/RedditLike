using Microsoft.AspNetCore.Mvc;
using Security.Jwt;
using Microsoft.AspNetCore.Cors;

namespace Reddit.Controllers;

using Model;
using Repositories;
using DTO;
using Services;

[ApiController]
[EnableCors("MainPolicy")]
[Route("user")]
public class UserController : ControllerBase
{

    [HttpGet("{id}")]
    public async Task<ActionResult<UserData>> Get(
        [FromServices] IUserRepository userRepository,
        [FromServices] IGroupRepository groupRepository,
        int id
    ) {
        var userList = await userRepository.Filter(u => u.Id == id);

        if(userList.Count == 0) 
            return BadRequest("Invalid ID");

        var user = userList.First();

        UserData u = new UserData()
        {
            Username = user.Username,
            Email = user.Email,
            ProfilePicture = user.ProfilePicture,
            Posts = user.Posts,
        };

        u.Groups = await groupRepository.GetUserGroups(user);


        return Ok(u);
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAll(
        [FromServices] IUserRepository userRepository
    )
    {
        var query = await userRepository.Filter(u => true);
        return query;
    }

    [HttpPost("register")]
    public async Task<ActionResult<int>> Register(
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

        return Ok(u.Id);

    }

    [HttpPost("validate")]
    public async Task<ActionResult<UserJwt>> ValidateJwt(
        [FromServices] IJwtService jwtService,
        [FromBody] Jwt jwt
    )
    {
        if(jwt.Value == "" || jwt.Value is null)
        {
            return Ok(new UserJwt { Authenticated = false });
        }

        try {
            var result = jwtService.Validate<UserJwt>(jwt.Value);
            return Ok(result);
        } catch(Exception e){
            return Ok(new UserJwt { Authenticated = false });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResult>> Login(
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
            string token = jwtService.GetToken<UserJwt>(new UserJwt { UserID = target.Id, Authenticated = true });

            result.Jwt = token;
            result.Success = true;
            return Ok(result);
        }

        result.Success = false;
        return Ok(result);
    }
}