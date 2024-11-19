using Lab2_MIT.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Lab2_MIT.DAL.Services
{
    public static class DatabaseSeeder
    {
        public static void Seed(IServiceProvider services)
        {
            // Використовуємо IServiceScopeFactory для створення області
            using (var scope = services.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<LabDbContext>();

                if (!_context.Students.Any())
                {
                    var students = new List<Student>
                    {
                        new Student { FirstName = "John", LastName = "Doe" },
                        new Student { FirstName = "Jane", LastName = "Doe" }
                    };

                    _context.Students.AddRange(students);
                    _context.SaveChanges();
                }

                if (!_context.Courses.Any())
                {
                    var courses = new List<Course>
                    {
                         new Course { Title = "Mathematics", Credits = 3, Description = "Basic math course" },
                        new Course { Title = "History", Credits = 4, Description = "Introduction to world history" }
                    };

                    _context.Courses.AddRange(courses);
                    _context.SaveChanges();
                }
            }
        }
    }
}