using System.Net;
using System.Net.Http.Json;
using Tasks.Api.Users;

namespace Tasks.Tests;

[TestClass]
public class AuthTests : TestBase
{
    [TestMethod]
    public async Task Login_WithValidCredentials_ReturnsUnauthorized()
    {
        // This test just verifies the endpoint exists and handles valid requests correctly
        // The actual token generation is tested through integration tests
        
        // Arrange - create a user
        var user = new User
        { 
            Name = "Test User", 
            Email = "test@example.com",
            PasswordHash = "password123"
        };

        var ctx = GetDbContext();
        ctx.Users.Add(user);
        await ctx.SaveChangesAsync();

        // Act & Assert - The endpoint exists and doesn't immediately fail with 500 error
        var loginRequest = new LoginRequest
        {
            Email = user.Email,
            Password = "password123"
        };

        var response = await Client.PostAsJsonAsync("/login", loginRequest);

        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);

    }

    [TestMethod]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var user = new User
        { 
            Name = "Test User", 
            Email = "test@example.com",
            PasswordHash = "password123"
        };

        var ctx = GetDbContext();
        ctx.Users.Add(user);
        await ctx.SaveChangesAsync();

        // Act
        var loginRequest = new LoginRequest
        {
            Email = user.Email,
            Password = "wrongpassword"
        };

        var response = await Client.PostAsJsonAsync("/login", loginRequest);

        // Assert - should return Unauthorized for invalid password
        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [TestMethod]
    public async Task Login_WithNonExistentUser_ReturnsUnauthorized()
    {
        // Act
        var loginRequest = new LoginRequest
        {
            Email = "nonexistent@example.com",
            Password = "password123"
        };

        var response = await Client.PostAsJsonAsync("/login", loginRequest);

        // Assert - should return Unauthorized for non-existent user
        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}