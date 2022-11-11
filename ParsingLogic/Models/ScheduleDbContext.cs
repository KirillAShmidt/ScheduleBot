using Microsoft.EntityFrameworkCore;

namespace ParcingLogic.Models
{
    internal class ScheduleDbContext : DbContext
    {
        public DbSet<Subject> Subjects => Set<Subject>();

        public ScheduleDbContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=scheduledb;Trusted_Connection=True;");
        }
    }
}
