using Microsoft.EntityFrameworkCore;

namespace TODOJava.Models
{
    public class TodoContext: DbContext
    {
        public DbSet<TodoElement> TodoElements { get; set; }
        public TodoContext (DbContextOptions<TodoContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
