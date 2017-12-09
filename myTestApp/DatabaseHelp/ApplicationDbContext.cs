using Microsoft.EntityFrameworkCore;
using myTestApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myTestApp.DatabaseHelp
{
        public class ApplicationDbContext : DbContext
        {
            public DbSet<User> User { get; set; }
            public DbSet<Project> Projects { get; set; }
            public DbSet<Group> Groups { get; set; }
            public DbSet<TimeCard> Timecards { get; set; }
            public DbSet<UserToGroup> UserToGroup { get; set; }

            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
            }
        }
}
