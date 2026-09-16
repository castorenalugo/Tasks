namespace Tasks.Api.Tasks;

public class TaskEntity
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public TaskStatusEnum Status { get; set; } = TaskStatusEnum.ToDo;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }

    // History of status changes
    public List<TaskStatusHistory> StatusHistory { get; set; } = new();
}
