using Tasks.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace Tasks.Api.Users;

public class UserRepository(TasksDbContext _dbContext)
{
    public async Task<User?> GetUserByEmail(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task CreateUser(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
    }
}