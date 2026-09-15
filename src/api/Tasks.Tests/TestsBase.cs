using Microsoft.Extensions.DependencyInjection;
using Tasks.Api.Database;

namespace Tasks.Tests;

[TestClass]
public abstract class TestBase
{
    protected static CustomWebApplicationFactory Factory = new();
    protected HttpClient Client { get; private set; } = default!;

    [AssemblyInitialize]
    public static async Task Initialize(TestContext context) => await Factory.InitializeAsync();

    [TestInitialize]
    public void TestSetup() => Client = Factory.CreateClient();

    protected TasksDbContext GetDbContext()
    {
        var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TasksDbContext>();
        return dbContext;
    }
}