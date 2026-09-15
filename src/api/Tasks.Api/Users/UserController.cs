using Microsoft.AspNetCore.Mvc;

namespace Tasks.Api.Users;

public class UserController(UserService _service) : ControllerBase
{
    [HttpPost("/users")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var record = await _service.CreateUser(request);
        return Ok(record);
    }
}