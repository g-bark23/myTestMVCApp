using Microsoft.EntityFrameworkCore;

namespace myTestApp.Models
{
    public class MyTestAppContext : DbContext
    {
        public MyTestAppContext(DbContextOptions<MyTestAppContext> options)
            : base(options)
        {
        }

        public DbSet<myTestApp.Models.User> User { get; set; }
    }
}
