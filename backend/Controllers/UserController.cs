using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Security.PasswordHasher;

namespace Reddit.Controllers;

using Model;
using Repositories;
using DTO;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    [HttpPost("/")]
    public async Task<ActionResult<User>> RegisterUser(
        [FromServices] IRepository<User> userRep,
        [FromServices] IPasswordHasher psh,
        [FromServices] ISaltProvider slp,
        [FromBody] UserRegister user)
    {
        var duplicate = await userRep.Filter(u => u.Username == user.Username);
        
        if(duplicate.Count > 0)
            return BadRequest();

        User _user = new User();

        _user.Username = user.Username;
        _user.Email = user.Email;
        _user.Salt = slp.ProvideSalt();
        _user.Password = psh.Hash(user.Password, _user.Salt);
        _user.BirthDate = user.Birthdate;
        _user.ProfilePicture = null;

        userRep.Add(_user);

        return Ok(_user);
    }

    [HttpPost("/login")]
    public async Task<ActionResult<bool>> Login(
        [FromBody] UserLogin user,
        [FromServices] IPasswordHasher psh,
        [FromServices] IRepository<User> userTable
    )
    {
        var userExists = await userTable.Filter(u => u.Username == user.Username);
        if(userExists.Count == 0)
            return BadRequest("Usuário não existe");

        var userFromTable = userExists.First();
        
        if(psh.Validate(user.Password, userFromTable.Salt, userFromTable.Password))
            return Ok(true);
        else
            return BadRequest("");
    }
}