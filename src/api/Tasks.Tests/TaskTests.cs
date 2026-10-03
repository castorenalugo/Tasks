using System.Net;
using System.Net.Http.Json;
using Tasks.Api.Tasks;

namespace Tasks.Tests;

[TestClass]
public class TaskTests : TestBase
{
    [TestMethod]
    public async Task CreateTask_ReturnsCreatedTask()
    {
        // Arrange
        var createTaskRequest = new CreateTaskRequest 
        { 
            Title = "Test Task", 
            Description = "Test Description"
        };

        // Act
        var response = await Client.PostAsJsonAsync("/tasks", createTaskRequest);
        var responseModel = await response.Content.ReadFromJsonAsync<TaskResponse>();

        // Assert response
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        Assert.IsNotNull(responseModel);
        Assert.AreEqual(createTaskRequest.Title, responseModel.Title);
        Assert.AreEqual(createTaskRequest.Description, responseModel.Description);
        Assert.AreEqual(TaskStatusEnum.ToDo, responseModel.Status);

        // Assert database state
        var ctx = GetDbContext();
        var taskInDb = await ctx.Tasks.FindAsync(responseModel.Id);
        Assert.IsNotNull(taskInDb);
        Assert.AreEqual(createTaskRequest.Title, taskInDb.Title);
        Assert.AreEqual(createTaskRequest.Description, taskInDb.Description);
        Assert.AreEqual(TaskStatusEnum.ToDo, taskInDb.Status);
    }

    [TestMethod]
    public async Task UpdateTaskStatus_UpdatesOnlyStatus()
    {
        // Arrange
        var createTaskRequest = new CreateTaskRequest 
        { 
            Title = "Test Task", 
            Description = "Test Description"
        };

        var createResponse = await Client.PostAsJsonAsync("/tasks", createTaskRequest);
        var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskResponse>();
        
        // Update only status
        var updateStatusRequest = new UpdateTaskStatusRequest
        {
            Status = TaskStatusEnum.Done
        };

        // Act
        var response = await Client.PutAsJsonAsync($"/tasks/{createdTask!.Id}/status", updateStatusRequest);
        var responseModel = await response.Content.ReadFromJsonAsync<TaskResponse>();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(responseModel);
        Assert.AreEqual(TaskStatusEnum.Done, responseModel.Status);
        Assert.AreEqual(createdTask.Title, responseModel.Title); // Title should remain unchanged
        Assert.AreEqual(createdTask.Description, responseModel.Description); // Description should remain unchanged
        Assert.IsNotEmpty(responseModel.StatusHistory);
    }
}