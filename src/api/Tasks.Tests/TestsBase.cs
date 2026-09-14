namespace Tasks.Tests;

[TestClass]
public abstract class TestBase
{
    protected static CustomWebApplicationFactory Factory = new();
    protected HttpClient Client { get; private set; } = default!;

    [AssemblyInitialize]
    public static async Task Initialize(TestContext context) => await Factory.InitializeAsync();

    [AssemblyCleanup]
    public static async Task Cleanup() => await Factory.DisposeAsync();

    [TestInitialize]
    public void TestSetup()
    {
        Client = Factory.CreateClient();
    }
}