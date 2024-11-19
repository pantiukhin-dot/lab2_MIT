using Lab2_MIT.DAL.Models;

public class Course
{
    public int CourseId { get; set; }
    public string Title { get; set; }
    public int Credits { get; set; }
    public string Description { get; set; } // Нове поле
    public ICollection<StudentCourse> StudentCourses { get; set; }
}