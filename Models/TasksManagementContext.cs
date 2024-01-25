using Microsoft.EntityFrameworkCore;
namespace Tasks_WEB_API.Models;
public class TasksManagementContext : DbContext
{
    public TasksManagementContext(DbContextOptions<TasksManagementContext> options)
        : base(options)
    {
    }
    public DbSet<Utilisateur> Utilisateurs { get; set; } = null!;
    public DbSet<Tache> Taches { get; set; } = null!;
}