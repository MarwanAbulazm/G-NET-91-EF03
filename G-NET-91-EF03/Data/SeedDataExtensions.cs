using System;
using System.Collections.Generic;
using System.Text;
using G_NET_91_EF03.Models;
using Microsoft.EntityFrameworkCore;

namespace G_NET_91_EF03.Data
{
    public static class SeedDataExtensions
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Instructor>().HasData(
                new Instructor { Id = 1, FullName = "Ahmed Hassan", Email = "ahmed.hassan@example.com", Bio = "Senior backend developer", JoinedDate = new DateTime(2022, 1, 10) },
                new Instructor { Id = 2, FullName = "Sara Mohamed", Email = "sara.mohamed@example.com", Bio = null, JoinedDate = new DateTime(2023, 3, 5) }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Programming", Description = "Programming courses", ParentCategoryId = null },
                new Category { Id = 2, Name = "Backend", Description = null, ParentCategoryId = 1 },
                new Category { Id = 3, Name = "ASP.NET", Description = null, ParentCategoryId = 2 },
                new Category { Id = 4, Name = "Design", Description = "Design courses", ParentCategoryId = null },
                new Category { Id = 5, Name = "UI/UX", Description = null, ParentCategoryId = 4 }
            );

            modelBuilder.Entity<Course>().HasData(
                new Course { Id = 1, Title = "C# Fundamentals", Description = "Learn C# from scratch", Price = 300.00m, Level = CourseLevel.Beginner, IsPublished = true, PublishedDate = new DateTime(2024, 1, 15), InstructorId = 1, CategoryId = 1 },
                new Course { Id = 2, Title = "ASP.NET Core Web API", Description = "Build REST APIs", Price = 750.00m, Level = CourseLevel.Intermediate, IsPublished = true, PublishedDate = new DateTime(2024, 2, 1), InstructorId = 1, CategoryId = 3 },
                new Course { Id = 3, Title = "Advanced EF Core", Description = "Deep dive into EF Core", Price = 900.00m, Level = CourseLevel.Advanced, IsPublished = true, PublishedDate = new DateTime(2024, 3, 1), InstructorId = 1, CategoryId = 2 },
                new Course { Id = 4, Title = "UI Design Basics", Description = null, Price = 450.00m, Level = CourseLevel.Beginner, IsPublished = true, PublishedDate = new DateTime(2024, 3, 20), InstructorId = 2, CategoryId = 4 },
                new Course { Id = 5, Title = "Microservices Architecture", Description = "Design microservices", Price = 1200.00m, Level = CourseLevel.Advanced, IsPublished = false, PublishedDate = null, InstructorId = 2, CategoryId = 2 }
            );

            modelBuilder.Entity<Student>().HasData(
                new Student { Id = 1, FullName = "Omar Khaled", Email = "omar.khaled@example.com", DateOfBirth = new DateTime(2000, 5, 12) },
                new Student { Id = 2, FullName = "Mona Adel", Email = "mona.adel@example.com", DateOfBirth = new DateTime(1999, 9, 30) }
            );

            modelBuilder.Entity<Enrollment>().HasData(
                new Enrollment { StudentId = 1, CourseId = 1, EnrollmentDate = new DateTime(2024, 2, 1), ProgressPercentage = 100, Status = EnrollmentStatus.Completed, FinalGrade = 92.50m },
                new Enrollment { StudentId = 1, CourseId = 2, EnrollmentDate = new DateTime(2024, 3, 10), ProgressPercentage = 40, Status = EnrollmentStatus.Active, FinalGrade = null },
                new Enrollment { StudentId = 2, CourseId = 1, EnrollmentDate = new DateTime(2024, 2, 15), ProgressPercentage = 60, Status = EnrollmentStatus.Active, FinalGrade = null },
                new Enrollment { StudentId = 2, CourseId = 4, EnrollmentDate = new DateTime(2024, 4, 1), ProgressPercentage = 0, Status = EnrollmentStatus.Active, FinalGrade = null }
            );
        }
    }
}
