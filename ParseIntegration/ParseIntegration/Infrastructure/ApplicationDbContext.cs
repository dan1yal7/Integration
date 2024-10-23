using Microsoft.EntityFrameworkCore;
using ParseIntegration.Models;

namespace ParseIntegration.Infrastructure
{
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Employees> Employees { get; set; }
    }
}
