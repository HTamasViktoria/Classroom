using Classroom.Data;
using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Microsoft.EntityFrameworkCore;

namespace Classroom.Service.Repositories
{
    public class TeacherSubjectRepository : ITeacherSubjectRepository
    {
        private ClassroomContext _dbContext;

        public TeacherSubjectRepository(ClassroomContext context)
        {
            _dbContext = context;
        }

        public IEnumerable<TeacherSubject> GetAll()
        {
            return _dbContext.TeacherSubjects.ToList();
        }

        
        public IEnumerable<TeacherSubject> GetSubjectsByTeacherId(string teacherId)
        {
            var teacherExisting = _dbContext.Teachers.Any(t => t.Id == teacherId);
            if (!teacherExisting)
            {
                throw new ArgumentException($"Teacher with ID {teacherId} does not exist.");
            }

            var subjects = _dbContext.TeacherSubjects
                .Where(ts => ts.TeacherId == teacherId)
                .ToList();

            return subjects;
        }

        
        
        public void Add(TeacherSubjectRequest request)
        {
            bool existingTeacherSubject = _dbContext.TeacherSubjects.Any(ts => ts.TeacherId == request.TeacherId &&
                                                                               ts.ClassName == request.ClassName &&
                                                                               ts.Subject == request.Subject);
            if (existingTeacherSubject)
            {
                throw new ArgumentException("Already existing teachersubject");
            }
     
            var teacher = _dbContext.Teachers.FirstOrDefault(t => t.Id == request.TeacherId);
            if (teacher == null)
            {
                throw new ArgumentException($"Teacher with ID {request.TeacherId} not found.");
            }

   
            var classOfStudents = _dbContext.ClassesOfStudents.FirstOrDefault(c => c.Id == request.ClassOfStudentsId);
            if (classOfStudents == null)
            {
                throw new ArgumentException($"ClassOfStudents with ID {request.ClassOfStudentsId} not found.");
            }

  
            var teacherSubject = new TeacherSubject()
            {
                Subject = request.Subject,
                TeacherId = request.TeacherId,
                Teacher = teacher,
                ClassOfStudentsId = request.ClassOfStudentsId,
                ClassOfStudents = classOfStudents,
                ClassName = request.ClassName
            };
            
            _dbContext.TeacherSubjects.Add(teacherSubject);
            _dbContext.SaveChanges();
        }

        
        
        public async Task<ClassOfStudents> GetStudentsByTeacherSubjectIdAsync(int teacherSubjectId)
        {
            var teacherSubject = await _dbContext.TeacherSubjects
                .Include(ts => ts.ClassOfStudents)
                .ThenInclude(cs => cs.Students)
                .FirstOrDefaultAsync(ts => ts.Id == teacherSubjectId);

            if (teacherSubject == null)
            {
                throw new ArgumentException($"TeacherSubject with ID {teacherSubjectId} not found.");
            }
            
            return teacherSubject.ClassOfStudents;
        }


    }
}