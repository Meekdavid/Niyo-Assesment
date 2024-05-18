using Microsoft.EntityFrameworkCore;
using SchoolManagement.Helpers.Models;
using System.Collections.Generic;

namespace SchoolManagement.Helpers.DBContext
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<AuthUser> Registrations { get; set; }
    }

}
