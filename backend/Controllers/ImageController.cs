using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Security.Jwt;

namespace Reddit.Controllers;

using Model;
using Repositories;
using DTO;
using Services;


[ApiController]
[Route("img")]
[EnableCors("MainPolicy")]
public class ImageController : Controller
{
    [HttpGet("{code}")]
    public async Task<ActionResult> Get(
        string code,
        [FromServices] IRepository<ImageDatum> repo
    )
    {
        if (int.TryParse(code, out var id))
        {
            var query = await repo.Filter(im => im.Id == id);
            var img = query.FirstOrDefault();

            if (img is null)
                return NotFound();

            return File(img.Photo, "image/jpeg");
        }

        return BadRequest("Code needs to be an intenger");
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    public async Task<ActionResult<string>> Post(
        [FromServices] IRepository<ImageDatum> repo
    )
    {
        var files = Request.Form.Files;

        if (files is null || files.Count == 0)
            return BadRequest();


        var file = Request.Form.Files[0];

        if (file.Length < 1)
            return BadRequest();

        using MemoryStream ms = new MemoryStream();

        await file.CopyToAsync(ms);
        var data = ms.GetBuffer();

        var img = new ImageDatum();
        img.Photo = data;
        await repo.Add(img);

        var code = img.Id.ToString();
        return Ok(code);
    }

    [HttpPost("add-avatar")]
    public async Task<ActionResult> AddAvatar(
        [FromServices] IImageService imageService,
        [FromServices] IUserService userService,
        [FromServices] IUserRepository userRepository
    )
    {
        var jwt = Request.Form["jwt"].ToString();
        
        User user;
        try
        {
            user = await userService.ValidateUserToken(new Jwt {Value = jwt});
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        if(user is null)
            return NotFound();

        var files = Request.Form.Files;

        if(files is null || files.Count == 0)
            return BadRequest();

        var file = Request.Form.Files[0];

        if(file.Length < 1)
            return BadRequest();

        var imageId = await imageService.SaveImg(file);
        
        user.ProfilePicture = imageId;
        await userRepository.Update(user);

        Console.WriteLine(user.ProfilePicture);

        return Ok();
    }

}