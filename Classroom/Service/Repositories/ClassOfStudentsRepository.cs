using Classroom.Data;
using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Classroom.Model.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Classroom.Service.Repositories
{
    public class ClassOfStudentsRepository : IClassOfStudentsRepository
    {
        private readonly ClassroomContext _dbContext;

        public ClassOfStudentsRepository(ClassroomContext context)
        {
            _dbContext = context;
        }

       
        public IEnumerable<ClassOfStudents> GetAll()
        {
            return _dbContext.ClassesOfStudents.ToList();
        }


        public IEnumerable<Student> GetStudents(int classId)
        {
            var classOfStudents = _dbContext.ClassesOfStudents
                .Include(c => c.Students)
                .FirstOrDefault(c => c.Id == classId);

            return classOfStudents?.Students ?? Enumerable.Empty<Student>();
        }


        public IEnumerable<StudentWithClassResponse> GetAllStudentsWithClasses()
        {
            var classesWithStudents = _dbContext.ClassesOfStudents
                .Include(c => c.Students)
                .ToList();

            var studentResponses = classesWithStudents.SelectMany(classOfStudents => classOfStudents.Students,
                    (classOfStudents, student) => new StudentWithClassResponse
                    {
                        Id = student.Id,
                        FirstName = student.FirstName,
                        FamilyName = student.FamilyName,
                        NameOfClass = classOfStudents.Name
                    })
                .ToList();

            return studentResponses;
        }

   
        public IEnumerable<ClassOfStudents> GetClassesBySubject(string subject)
        {
            var classes = _dbContext.TeacherSubjects
                .Where(ts => ts.Subject == subject)
                .Select(ts => ts.ClassOfStudents)
                .Distinct()
                .ToList();

            return classes;
        }


        public void Add(ClassOfStudentsRequest request)
        {
            var classOfStudents = new ClassOfStudents()
            {
                Grade = request.Grade,
                Section = request.Section,
                Name = $"{request.Grade} {request.Section}",
                Students = new List<Student>()
            };

            _dbContext.ClassesOfStudents.Add(classOfStudents);
            _dbContext.SaveChanges();
        }

      
        public void AddStudent(AddingStudentToClassRequest request)
        {
            var classOfStudents = _dbContext.ClassesOfStudents
                .Include(c => c.Students)
                .FirstOrDefault(c => c.Id == request.ClassId);

            if (classOfStudents == null)
            {
                throw new KeyNotFoundException($"Class with ID {request.ClassId} not found.");
            }

            var student = _dbContext.Students.Find(request.StudentId);

            if (student == null)
            {
                throw new KeyNotFoundException($"Student with ID {request.StudentId} not found.");
            }

          
            if (!classOfStudents.Students.Any(s => s.Id == student.Id))
            {
                classOfStudents.Students.Add(student);
                _dbContext.SaveChanges();
            }
        }
    }
}
