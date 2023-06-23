using Microsoft.AspNetCore.Mvc;

namespace Reddit.Controllers;

using Model;
using Repositories;


using Microsoft.AspNetCore.Cors;


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
        if(int.TryParse(code, out var id))
        {
            var query = await repo.Filter(im => im.Id == id);
            var img = query.FirstOrDefault();

            if(img != null)
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
        
        if(files is null || files.Count == 0)
            return BadRequest();
        
        var file = Request.Form.Files[0];

        if(file.Length < 1)
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
}