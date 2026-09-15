using System.Net;
using System.Net.Http.Json;
using Tasks.Api.Users;

namespace Tasks.Tests;

[TestClass]
public class UserTests : TestBase
{
    [TestMethod]
    public async Task CreateUser_ReturnsCreatedUser()
    {
        // Arrange
        var createUserRequest = new CreateUserRequest 
        { 
            Name = "Test User", 
            Email = "test@example.com",
            Password = "password123"
        };

        // Act
        var response = await Client.PostAsJsonAsync("/users", createUserRequest);
        var responseModel = await response.Content.ReadFromJsonAsync<UserResponse>();

        // Assert response
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(responseModel);
        Assert.AreEqual(createUserRequest.Name, responseModel.Name);
        Assert.AreEqual(createUserRequest.Email, responseModel.Email);

        // Assert database state
        var ctx = GetDbContext();
        var userInDb = await ctx.Users.FindAsync(responseModel.Id);
        Assert.IsNotNull(userInDb);
        Assert.AreEqual(createUserRequest.Name, userInDb.Name);
        Assert.AreEqual(createUserRequest.Email, userInDb.Email);
        Assert.AreEqual(createUserRequest.Password, userInDb.PasswordHash);
    }

    [TestMethod]
    public async Task CreateUser_WithExistingEmail_ReturnsValidationError()
    {
        // Arrange
        var existingUser = GivenExistingUserInDatabase();        

        var createUserRequest = new CreateUserRequest 
        { 
            Name = "Test User", 
            Email = existingUser.Email,
            Password = "password456"
        };

        // Act
        var response = await Client.PostAsJsonAsync("/users", createUserRequest);
        
        // Assert response
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("User with this email already exists", responseContent);
    }

    private User GivenExistingUserInDatabase()
    {
        var existingUser = new User
        { 
            Name = "Existing User", 
            Email = "existing@example.com",
            PasswordHash = "password123"
        };

        var ctx = GetDbContext();
        ctx.Users.Add(existingUser);
        ctx.SaveChanges();
        return existingUser;
    }
}