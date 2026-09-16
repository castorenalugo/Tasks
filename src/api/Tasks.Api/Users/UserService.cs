using Tasks.Api.Exceptions;
using Tasks.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace Tasks.Api.Users;

public class UserService(TasksDbContext _dbContext)
{
    public async Task<UserResponse> CreateUser(CreateUserRequest request)
    {
        var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if(existingUser != null)
            throw new ValidationEx("User with this email already exists.");

        var user = new User 
        { 
            Name = request.Name, 
            Email = request.Email,
            PasswordHash = request.Password
        };
        
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        
        return UserResponse.FromUser(user);
    }
}