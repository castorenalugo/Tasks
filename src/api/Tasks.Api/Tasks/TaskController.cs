using Microsoft.AspNetCore.Mvc;

namespace Tasks.Api.Tasks;

[ApiController]
[Route("tasks")]
public class TaskController(TaskService _service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request, CancellationToken ct)
    {
        var record = await _service.CreateTask(request, ct);
        return Ok(record);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTask(int id, CancellationToken ct)
    {
        var record = await _service.GetTask(id, ct);
        return Ok(record);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTasks(CancellationToken ct)
    {
        var records = await _service.GetAllTasks(ct);
        return Ok(records);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskRequest request, CancellationToken ct)
    {
        var record = await _service.UpdateTask(id, request, ct);
        return Ok(record);
    }

    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] UpdateTaskStatusRequest request, CancellationToken ct)
    {
        var record = await _service.UpdateTaskStatus(id, request, ct);
        return Ok(record);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTask(int id, CancellationToken ct)
    {
        await _service.DeleteTask(id, ct);
        return Ok();
    }
}