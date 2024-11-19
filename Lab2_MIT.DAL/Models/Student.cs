using Lab2_MIT.DAL.Models;

public class Student
{
    public int StudentId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; } // Нове поле
    public ICollection<StudentCourse> StudentCourses { get; set; }
}