using Microsoft.AspNetCore.Mvc;

namespace WeatherBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var response = new
        {
            Message = "Hello, world!",
            Time = DateTime.UtcNow
        };
        return Ok(response);
    }
}