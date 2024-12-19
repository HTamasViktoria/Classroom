using Classroom.Data;
using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Classroom.Model.ResponseModels;
using Microsoft.EntityFrameworkCore;
using Classroom.Model.DataModels.Enums;

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

            if (classOfStudents == null)
            {
                throw new KeyNotFoundException($"Class with ID {classId} not found.");
            }

            return classOfStudents.Students;
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
            if (!Enum.IsDefined(typeof(Subjects), subject))
            {
                throw new ArgumentException($"The subject '{subject}' is not valid.");
            }

            var classes = _dbContext.TeacherSubjects
                .Where(ts => ts.Subject == subject)
                .Select(ts => ts.ClassOfStudents)
                .Distinct()
                .ToList();

            return classes;
        }


        public void Add(ClassOfStudentsRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Grade) || string.IsNullOrWhiteSpace(request.Section))
            {
                throw new ArgumentException("Grade and Section cannot be empty or whitespace.");
            }
            
            var existingClass = _dbContext.ClassesOfStudents
                .FirstOrDefault(c => c.Grade == request.Grade && c.Section == request.Section);

            if (existingClass != null)
            {
                throw new InvalidOperationException($"Class with grade {request.Grade} and section {request.Section} already exists.");
            }
            
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

            if (classOfStudents.Students.Any(s => s.Id == student.Id))
            {
                throw new ArgumentException($"Student with ID {student.Id} is already in the class.");
            }

            classOfStudents.Students.Add(student);
            _dbContext.SaveChanges();
        }


    }
}
