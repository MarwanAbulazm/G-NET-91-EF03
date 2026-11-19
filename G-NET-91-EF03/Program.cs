using G_NET_91_EF03.Data;
using G_NET_91_EF03.Models;
using Microsoft.EntityFrameworkCore;

namespace G_NET_91_EF03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AddInstructor();
            AddCourse(1, 1);
            AddCourse(99, 1);
            AddStudents();
            EnrollStudent(2, 3);
            EnrollStudent(2, 5);
            EnrollStudent(1, 1);
            EnrollStudent(99, 1);

            FindCourse(2, "C# Fundamentals");
            FindCourse(99, "Unknown Course");
            FilterCourses(300, 900);
            PrintSubCategories(1);
            CourseRoster(1);

            ChangePrice(2, 800);
            ChangePrice(99, 800);
            UpdateProgress(1, 2, 100);
            UpdateProgress(1, 2, 150);
            UpdateProgress(99, 99, 50);
            PublishCourse(5);

            Unenroll(2, 4);
            Unenroll(99, 99);
            DeleteCategoryUnsafe(1);
            DeleteCategorySafe(1);
            DeleteCategorySafe(5);
            DeleteCourse(1);
        }

        static void AddInstructor()
        {
            Console.WriteLine("--- Add an instructor ---");
            using var context = new OnlineLearningContext();

            var instructor = new Instructor
            {
                FullName = "Khaled Samir",
                Email = "khaled.samir@example.com",
                Bio = "Cloud and DevOps trainer",
                JoinedDate = DateTime.Now
            };

            context.Instructors.Add(instructor);
            context.SaveChanges();

            Console.WriteLine($"Instructor added with Id: {instructor.Id}");
        }

        static void AddCourse(int instructorId, int categoryId)
        {
            Console.WriteLine("--- Add a course ---");
            using var context = new OnlineLearningContext();

            if (!context.Instructors.Any(i => i.Id == instructorId))
            {
                Console.WriteLine($"Instructor with Id {instructorId} does not exist.");
                return;
            }

            if (!context.Categories.Any(c => c.Id == categoryId))
            {
                Console.WriteLine($"Category with Id {categoryId} does not exist.");
                return;
            }

            var course = new Course
            {
                Title = "LINQ Mastery",
                Description = "Master LINQ queries",
                Price = 350,
                Level = CourseLevel.Intermediate,
                IsPublished = false,
                InstructorId = instructorId,
                CategoryId = categoryId
            };

            context.Courses.Add(course);
            context.SaveChanges();

            Console.WriteLine($"Course added with Id: {course.Id}");
        }

        static void AddStudents()
        {
            Console.WriteLine("--- Add several students ---");
            using var context = new OnlineLearningContext();

            var students = new List<Student>
            {
                new Student { FullName = "Youssef Ali", Email = "youssef.ali@example.com", DateOfBirth = new DateTime(2001, 1, 20) },
                new Student { FullName = "Nour Ibrahim", Email = "nour.ibrahim@example.com", DateOfBirth = new DateTime(2002, 7, 8) },
                new Student { FullName = "Hana Tarek", Email = "hana.tarek@example.com", DateOfBirth = new DateTime(2000, 11, 3) }
            };

            context.Students.AddRange(students);
            context.SaveChanges();

            foreach (var student in students)
            {
                Console.WriteLine($"{student.FullName} => Id: {student.Id}");
            }
        }

        static void EnrollStudent(int studentId, int courseId)
        {
            Console.WriteLine("--- Enroll a student ---");
            using var context = new OnlineLearningContext();

            var student = context.Students.Find(studentId);
            if (student is null)
            {
                Console.WriteLine($"Enrollment refused: student with Id {studentId} does not exist.");
                return;
            }

            var course = context.Courses.Find(courseId);
            if (course is null)
            {
                Console.WriteLine($"Enrollment refused: course with Id {courseId} does not exist.");
                return;
            }

            if (!course.IsPublished)
            {
                Console.WriteLine($"Enrollment refused: course '{course.Title}' is not published.");
                return;
            }

            if (context.Enrollments.Any(e => e.StudentId == studentId && e.CourseId == courseId))
            {
                Console.WriteLine($"Enrollment refused: {student.FullName} is already enrolled in '{course.Title}'.");
                return;
            }

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                EnrollmentDate = DateTime.Now,
                ProgressPercentage = 0,
                Status = EnrollmentStatus.Active
            };

            context.Enrollments.Add(enrollment);
            context.SaveChanges();

            Console.WriteLine($"{student.FullName} enrolled in '{course.Title}' successfully.");
        }

        static void FindCourse(int courseId, string title)
        {
            Console.WriteLine("--- Find a course ---");
            using var context = new OnlineLearningContext();

            var courseById = context.Courses.Find(courseId);
            Console.WriteLine(courseById is null
                ? $"Course with Id {courseId} not found."
                : $"Found by Id: {courseById.Title}");

            var courseByTitle = context.Courses.FirstOrDefault(c => c.Title == title);
            Console.WriteLine(courseByTitle is null
                ? $"Course with title '{title}' not found."
                : $"Found by title: {courseByTitle.Title} (Id: {courseByTitle.Id})");
        }

        static void FilterCourses(decimal minPrice, decimal maxPrice)
        {
            Console.WriteLine("--- Filter courses ---");
            using var context = new OnlineLearningContext();

            var courses = context.Courses
                .Where(c => c.Price >= minPrice && c.Price <= maxPrice)
                .OrderByDescending(c => c.Price)
                .Select(c => new { c.Title, c.Level, c.Price })
                .ToList();

            foreach (var course in courses)
            {
                Console.WriteLine($"{course.Title} | {course.Level} | {course.Price}");
            }
        }

        static void PrintSubCategories(int categoryId)
        {
            Console.WriteLine("--- Sub-categories ---");
            using var context = new OnlineLearningContext();

            var subCategories = context.Categories
                .Where(c => c.ParentCategoryId == categoryId)
                .Select(c => c.Name)
                .ToList();

            Console.WriteLine($"Sub-categories of category {categoryId}:");
            foreach (var name in subCategories)
            {
                Console.WriteLine(name);
            }

            var topLevelCategories = context.Categories
                .Where(c => c.ParentCategoryId == null)
                .Select(c => c.Name)
                .ToList();

            Console.WriteLine("Top-level categories:");
            foreach (var name in topLevelCategories)
            {
                Console.WriteLine(name);
            }
        }

        static void CourseRoster(int courseId)
        {
            Console.WriteLine("--- Course roster ---");
            using var context = new OnlineLearningContext();

            var studentIds = context.Enrollments
                .Where(e => e.CourseId == courseId)
                .Select(e => e.StudentId)
                .ToList();

            Console.WriteLine($"Students enrolled in course {courseId}: {studentIds.Count}");

            var studentNames = context.Students
                .Where(s => studentIds.Contains(s.Id))
                .Select(s => s.FullName)
                .ToList();

            foreach (var name in studentNames)
            {
                Console.WriteLine(name);
            }
        }

        static void ChangePrice(int courseId, decimal newPrice)
        {
            Console.WriteLine("--- Change a price ---");
            using var context = new OnlineLearningContext();

            if (newPrice <= 0)
            {
                Console.WriteLine("Invalid price. It must be greater than zero.");
                return;
            }

            var course = context.Courses.Find(courseId);
            if (course is null)
            {
                Console.WriteLine($"Course with Id {courseId} not found.");
                return;
            }

            course.Price = newPrice;
            context.SaveChanges();

            Console.WriteLine($"Price of '{course.Title}' changed to {newPrice}.");
        }

        static void UpdateProgress(int studentId, int courseId, int progress)
        {
            Console.WriteLine("--- Update progress ---");
            using var context = new OnlineLearningContext();

            if (progress < 0 || progress > 100)
            {
                Console.WriteLine("Invalid progress. It must be between 0 and 100.");
                return;
            }

            var enrollment = context.Enrollments.Find(studentId, courseId);
            if (enrollment is null)
            {
                Console.WriteLine($"Enrollment for student {studentId} in course {courseId} not found.");
                return;
            }

            enrollment.ProgressPercentage = progress;
            if (progress == 100)
            {
                enrollment.Status = EnrollmentStatus.Completed;
            }

            context.SaveChanges();

            Console.WriteLine($"Progress updated to {progress}%. Status: {enrollment.Status}");
        }

        static void PublishCourse(int courseId)
        {
            Console.WriteLine("--- Publish a course ---");
            using var context = new OnlineLearningContext();

            var course = context.Courses.Find(courseId);
            if (course is null)
            {
                Console.WriteLine($"Course with Id {courseId} not found.");
                return;
            }

            if (course.IsPublished)
            {
                Console.WriteLine($"Course '{course.Title}' is already published.");
                return;
            }

            course.IsPublished = true;
            course.PublishedDate = DateTime.Now;
            context.SaveChanges();

            Console.WriteLine($"Course '{course.Title}' published on {course.PublishedDate}.");
        }

        static void Unenroll(int studentId, int courseId)
        {
            Console.WriteLine("--- Unenroll ---");
            using var context = new OnlineLearningContext();

            var enrollment = context.Enrollments.Find(studentId, courseId);
            if (enrollment is null)
            {
                Console.WriteLine($"Enrollment for student {studentId} in course {courseId} not found.");
                return;
            }

            context.Enrollments.Remove(enrollment);
            context.SaveChanges();

            Console.WriteLine($"Student {studentId} unenrolled from course {courseId}.");
        }

        static void DeleteCategoryUnsafe(int categoryId)
        {
            Console.WriteLine("--- Delete a category (unsafe) ---");
            using var context = new OnlineLearningContext();

            var category = context.Categories.Find(categoryId);
            if (category is null)
            {
                Console.WriteLine($"Category with Id {categoryId} not found.");
                return;
            }

            try
            {
                context.Categories.Remove(category);
                context.SaveChanges();
                Console.WriteLine($"Category '{category.Name}' deleted.");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Delete failed: {ex.Message}");
            }
        }

        static void DeleteCategorySafe(int categoryId)
        {
            Console.WriteLine("--- Delete a category (safe) ---");
            using var context = new OnlineLearningContext();

            var category = context.Categories.Find(categoryId);
            if (category is null)
            {
                Console.WriteLine($"Category with Id {categoryId} not found.");
                return;
            }

            if (context.Categories.Any(c => c.ParentCategoryId == categoryId))
            {
                Console.WriteLine($"Category '{category.Name}' cannot be deleted because it still has sub-categories.");
                return;
            }

            context.Categories.Remove(category);
            context.SaveChanges();

            Console.WriteLine($"Category '{category.Name}' deleted.");
        }

        static void DeleteCourse(int courseId)
        {
            Console.WriteLine("--- Delete a course ---");
            using var context = new OnlineLearningContext();

            var course = context.Courses.Find(courseId);
            if (course is null)
            {
                Console.WriteLine($"Course with Id {courseId} not found.");
                return;
            }

            var enrollmentsBefore = context.Enrollments.Count(e => e.CourseId == courseId);
            Console.WriteLine($"Enrollments before delete: {enrollmentsBefore}");

            context.Courses.Remove(course);
            context.SaveChanges();

            var enrollmentsAfter = context.Enrollments.Count(e => e.CourseId == courseId);
            Console.WriteLine($"Enrollments after delete: {enrollmentsAfter}");

        }
    }
}