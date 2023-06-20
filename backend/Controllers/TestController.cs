using Microsoft.AspNetCore.Mvc;

namespace Reddit.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public ActionResult<string> Get()
        => Ok("Funcionando");

}
