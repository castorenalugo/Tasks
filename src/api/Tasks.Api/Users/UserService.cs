using Tasks.Api.Exceptions;
using Tasks.Api.Database;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Tasks.Api.Users;

public class UserService(
    TasksDbContext _dbContext,
    IJwtService _jwtService)
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
            PasswordHash = HashPassword(request.Password)
        };
        
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        
        return UserResponse.FromUser(user);
    }

    public async Task<string> Authenticate(string email, string password)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
            throw new NotFoundEx("Invalid credentials.");

        // Simple password check (in real world, use bcrypt or similar)
        if (!VerifyPassword(password, user.PasswordHash))
            throw new ValidationEx("Invalid credentials.");

        return _jwtService.GenerateToken(user);
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private bool VerifyPassword(string password, string hash)
    {
        var hashedInput = HashPassword(password);
        return hashedInput == hash;
    }
}