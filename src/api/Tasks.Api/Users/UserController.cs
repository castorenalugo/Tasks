using Microsoft.AspNetCore.Mvc;
using Tasks.Api.Exceptions;

namespace Tasks.Api.Users;

public class UserController(UserService _service) : ControllerBase
{
    [HttpPost("/users")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var record = await _service.CreateUser(request);
        return Created("/users/" + record.Id, record);
    }

    [HttpPost("/login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var token = await _service.Authenticate(request.Email, request.Password);
            return Ok(new { Token = token });
        }
        catch (ValidationEx ex) when (ex.Message == "Invalid credentials.")
        {
            return Unauthorized();
        }
        catch (NotFoundEx ex) when (ex.Message == "Invalid credentials.")
        {
            return Unauthorized();
        }
    }
}