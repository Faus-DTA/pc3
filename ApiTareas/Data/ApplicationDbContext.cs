using Microsoft.EntityFrameworkCore;
using ApiTareas.Models;

namespace ApiTareas.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tarea> Tareas { get; set; }
    }
}
