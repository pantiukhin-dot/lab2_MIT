using Lab2_MIT.DAL.Models;
using Lab2_MIT.DAL.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2_MIT.DAL.Services
{
    public class StudentService
    {
        private readonly IMongoCollection<Student> _studentsCollection;

        public StudentService(IOptions<MongoDBSettings> mongoDBSettings, IMongoClient mongoClient)
        {
            var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _studentsCollection = mongoDatabase.GetCollection<Student>("Students");
        }

        public async Task<List<Student>> GetAsync() =>
            await _studentsCollection.Find(s => true).ToListAsync();

        public async Task<Student> GetByIdAsync(string id)
        {
            int studentId;
            if (int.TryParse(id, out studentId))
            {
                return await _studentsCollection.Find(s => s.StudentId == studentId).FirstOrDefaultAsync();
            }
            return null;
        }

        public async Task CreateAsync(StudentDto studentDto)
        {
            var student = new Student
            {
                FirstName = studentDto.Name.Split(' ')[0],
                LastName = studentDto.Name.Split(' ')[1],
                BirthDate = studentDto.Birth
            };

            await _studentsCollection.InsertOneAsync(student);
        }

        public async Task UpdateAsync(string id, Student updatedStudent)
        {
            int studentId;
            if (int.TryParse(id, out studentId))
            {
                await _studentsCollection.ReplaceOneAsync(s => s.StudentId == studentId, updatedStudent);
            }
            else
            {
                throw new ArgumentException("Invalid ID format.");
            }
        }

        public async Task RemoveAsync(string id)
        {
            int studentId;
            if (int.TryParse(id, out studentId))
            {
                await _studentsCollection.DeleteOneAsync(s => s.StudentId == studentId);
            }
            else
            {
                throw new ArgumentException("Invalid ID format.");
            }
        }

    }
}
