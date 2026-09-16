namespace Tasks.Api.Tasks;

public class CreateTaskRequest
{
    public required string Title { get; set; }
    public string? Description { get; set; }
}

public class UpdateTaskRequest
{
    public required string Title { get; set; }
    public string? Description { get; set; }
}

public class UpdateTaskStatusRequest
{
    public required TaskStatusEnum Status { get; set; }
}

public class TaskResponse
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public TaskStatusEnum Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public List<TaskStatusHistory> StatusHistory { get; set; } = new();

    public static TaskResponse FromTask(TaskEntity task) => new()
    {
        Id = task.Id,
        Title = task.Title,
        Description = task.Description,
        Status = task.Status,
        CreatedAt = task.CreatedAt,
        UpdatedAt = task.UpdatedAt,
        StatusHistory = task.StatusHistory
    };
}