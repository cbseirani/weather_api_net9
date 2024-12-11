using Microsoft.AspNetCore.Mvc;
using WeatherBackend.Services;

namespace WeatherBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService) : ControllerBase
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