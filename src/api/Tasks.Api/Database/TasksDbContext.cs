using Microsoft.EntityFrameworkCore;
using Tasks.Api.Users;
using Tasks.Api.Tasks;

namespace Tasks.Api.Database;

public class TasksDbContext(DbContextOptions<TasksDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<TaskEntity> Tasks => Set<TaskEntity>();
}