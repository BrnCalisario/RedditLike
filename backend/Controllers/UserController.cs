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
        [FromServices] IUserRepository userRep,
        [FromServices] IPasswordHasher psh,
        [FromServices] ISaltProvider slp,
        [FromBody] UserRegister registerData)
    {
        if(await userRep.userNameExists(registerData.Username) 
            || await userRep.emailExists(registerData.Email))
            return BadRequest("Usuário já existe");
    
    
        User user = new User();

        user.Username = registerData.Username;
        user.Email = registerData.Email;

        user.Salt = slp.ProvideSalt();
        user.Password = psh.Hash(registerData.Password, user.Salt);
        
        user.BirthDate = registerData.Birthdate;
        user.ProfilePicture = null;

        userRep.Add(user);

        return Ok(user);
    }

    [HttpPost("/login")]
    public async Task<ActionResult<bool>> Login(
        [FromBody] UserLogin loginData,
        [FromServices] IPasswordHasher psh,
        [FromServices] UserRepository userRep
    )
    {
        var userList = await userRep.Filter(u => u.Username == loginData.Username);

        if(userList.Count() == 0)
            return BadRequest("Usuário inválido");

        User target = userList.First();

        if(psh.Validate(loginData.Password, target.Salt, target.Password))
            return Ok("Logado");

        return BadRequest("Senha inválida");
    }
}