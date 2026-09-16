using Tasks.Api.Exceptions;
using Tasks.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace Tasks.Api.Tasks;
public class TaskService(TasksDbContext _dbContext)
{
    public async Task<TaskResponse> CreateTask(CreateTaskRequest request, CancellationToken ct)
    {
        var task = new TaskEntity 
        { 
            Title = request.Title, 
            Description = request.Description,
            Status = TaskStatusEnum.ToDo
        };
        
        // Add initial status history
        task.StatusHistory.Add(new TaskStatusHistory 
        { 
            Status = TaskStatusEnum.ToDo,
            ChangedAt = DateTime.UtcNow
        });
        
        _dbContext.Tasks.Add(task);
        await _dbContext.SaveChangesAsync(ct);
        
        return TaskResponse.FromTask(task);
    }

    public async Task<TaskResponse> GetTask(int id, CancellationToken ct)
    {
        var task = await _dbContext.Tasks.FindAsync(id, ct);
        if (task == null)
            throw new NotFoundEx("Task not found.");
            
        return TaskResponse.FromTask(task);
    }

    public async Task<TaskResponse[]> GetAllTasks(CancellationToken ct)
    {
        var tasks = await _dbContext.Tasks.ToArrayAsync(ct);
        return tasks.Select(TaskResponse.FromTask).ToArray();
    }

    public async Task<TaskResponse> UpdateTask(int id, UpdateTaskRequest request, CancellationToken ct)
    {
        var task = await _dbContext.Tasks.FindAsync(id, ct);
        if (task == null)
            throw new NotFoundEx("Task not found.");

        task.Title = request.Title;
        task.Description = request.Description;
        task.UpdatedAt = DateTime.UtcNow;

        _dbContext.Tasks.Update(task);
        await _dbContext.SaveChangesAsync(ct);
        
        return TaskResponse.FromTask(task);
    }

    public async Task<TaskResponse> UpdateTaskStatus(int id, UpdateTaskStatusRequest request, CancellationToken ct)
    {
        var task = await _dbContext.Tasks.FindAsync(id, ct);
        if (task == null)
            throw new NotFoundEx("Task not found.");

        if (request.Status != task.Status)
        {
            task.Status = request.Status;
            task.UpdatedAt = DateTime.UtcNow;
            
            // Add to history
            task.StatusHistory.Add(new TaskStatusHistory 
            { 
                Status = request.Status,
                ChangedAt = DateTime.UtcNow
            });
        }

        _dbContext.Tasks.Update(task);
        await _dbContext.SaveChangesAsync(ct);
        
        return TaskResponse.FromTask(task);
    }

    public async Task DeleteTask(int id, CancellationToken ct)
    {
        var task = await _dbContext.Tasks.FindAsync(id, ct);
        if (task == null)
            throw new NotFoundEx("Task not found.");
            
        _dbContext.Tasks.Remove(task);
        await _dbContext.SaveChangesAsync(ct);
    }
}