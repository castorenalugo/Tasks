using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Tasks.Api.Database;

namespace Tasks.Tests;

[TestClass]
public class UserTests : TestBase
{
    [TestMethod]
    public async Task GetUsers_ReturnsOkResult()
    {
        // Arrange
        var user = new User { Name = "Test User", Email = "test@example.com" };
        SetupDbRecord(user);

        // Act
        var response = await Client.GetAsync("/users");
        var responseModel = await response.Content.ReadFromJsonAsync<User[]>();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(responseModel);
        Assert.HasCount(1, responseModel);
        Assert.AreEqual(user.Name, responseModel[0].Name);
        Assert.AreEqual(user.Email, responseModel[0].Email);
        Assert.AreEqual(user.Id, responseModel[0].Id);
    }


    private void SetupDbRecord(User user)
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TasksDbContext>();
        dbContext.Users.Add(user);
        dbContext.SaveChanges();
    }
}