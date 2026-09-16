using Microsoft.EntityFrameworkCore;
using Tasks.Api.Database;
using Tasks.Api.Exceptions;
using Tasks.Api.Users;
using Tasks.Api.Tasks;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services.AddControllers();
services.AddOpenApi();
services.AddProblemDetails();
services.AddExceptionHandler<CustomExceptionHandler>();

services.AddDbContext<TasksDbContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("Db")));
services.AddScoped<UserService>();
services.AddScoped<TaskService>();

var app = builder.Build();

app.UseExceptionHandler();
app.MapOpenApi();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
