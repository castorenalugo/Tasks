using Microsoft.EntityFrameworkCore;
using Tasks.Api.Database;
using Tasks.Api.Exceptions;
using Tasks.Api.Users;
using Tasks.Api.Tasks;
using System.Text;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllers();
services.AddOpenApi();
services.AddProblemDetails();
services.AddExceptionHandler<CustomExceptionHandler>();
services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]!))
        };
    });


services.AddDbContext<TasksDbContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("Db")));
services.AddScoped<UserService>();
services.AddScoped<TaskService>();
services.AddScoped<IJwtService, JwtService>();

var app = builder.Build();

app.UseExceptionHandler();
app.MapOpenApi();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();