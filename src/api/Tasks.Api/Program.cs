using Microsoft.EntityFrameworkCore;
using Tasks.Api.Database;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services.AddControllers();
services.AddOpenApi();

services.AddDbContext<TasksDbContext>(opts =>
    opts.UseNpgsql(builder.Configuration.GetConnectionString("Db")));

var app = builder.Build();

app.MapOpenApi();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
