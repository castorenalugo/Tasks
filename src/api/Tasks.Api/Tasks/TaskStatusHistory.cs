namespace Tasks.Api.Tasks;

public class TaskStatusHistory
{
    public int Id { get; set; }
    public TaskStatusEnum Status { get; set; }
    public DateTimeOffset ChangedAt { get; set; } = DateTimeOffset.UtcNow;
}