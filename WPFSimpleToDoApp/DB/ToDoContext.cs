using Microsoft.EntityFrameworkCore;

namespace ToDoApp.DB
{
    public class ToDoContext : DbContext
    {
        public DbSet<Models.Task> Tasks { get; set; }

        public DbSet<Models.TaskFilters> TaskFilters { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=todo.db");
        }
    }
}
