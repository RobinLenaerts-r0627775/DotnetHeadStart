namespace DotnetHeadStart.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TemplateController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello World");
    }

    [HttpGet("/test")]
    public IActionResult Test()
    {
        throw new Exception("Test Exception");
        return Ok("Test");
    }
}
