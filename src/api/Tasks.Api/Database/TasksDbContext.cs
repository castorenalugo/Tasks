using Microsoft.EntityFrameworkCore;

namespace Tasks.Api.Database;

public class TasksDbContext(DbContextOptions<TasksDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
}