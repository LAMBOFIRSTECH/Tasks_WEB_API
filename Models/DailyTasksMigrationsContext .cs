using Microsoft.EntityFrameworkCore;
namespace Tasks_WEB_API.Models;
public class DailyTasksMigrationsContext  : DbContext
{
    public DailyTasksMigrationsContext (DbContextOptions<DailyTasksMigrationsContext > options)
        : base(options)
    {
    }
   

    public DbSet<Utilisateur> Utilisateurs { get; set; } = null!;
    public DbSet<Tache> Taches { get; set; } = null!;
}

