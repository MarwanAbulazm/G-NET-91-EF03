using System;
using System.Collections.Generic;
using System.Text;
using G_NET_91_EF03.Models;
using Microsoft.EntityFrameworkCore;

namespace G_NET_91_EF03.Data
{
    public class OnlineLearningContext : DbContext
    {
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-ARTE257;Database=OnlineLearningDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OnlineLearningContext).Assembly);
            modelBuilder.SeedData();
        }
    }
}