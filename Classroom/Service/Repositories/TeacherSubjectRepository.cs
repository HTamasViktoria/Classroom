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
         
            if (string.IsNullOrWhiteSpace(teacherId))
            {
                throw new ArgumentException("Teacher ID cannot be null or empty.", nameof(teacherId));
            }

           
            var subjects = _dbContext.TeacherSubjects
                .Where(ts => ts.TeacherId == teacherId)
                .ToList();

     
            return subjects;
        }


        
        
        public void Add(TeacherSubjectRequest request)
        {
      
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(request.Subject))
            {
                throw new ArgumentException("Subject cannot be null or empty.", nameof(request.Subject));
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
                return null;
            }

            return teacherSubject.ClassOfStudents;
        }

    }
}