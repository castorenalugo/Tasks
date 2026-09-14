using Microsoft.AspNetCore.Mvc;
using Tasks.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace Tasks.Api.Controllers;

public class UserController(TasksDbContext _dbContext) : ControllerBase
{
    [HttpGet("/users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _dbContext.Users.ToListAsync();
        return Ok(users);
    }
}