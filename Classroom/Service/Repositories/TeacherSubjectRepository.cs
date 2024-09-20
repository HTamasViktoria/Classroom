using Classroom.Data;
using Classroom.Model.DataModels;
using System.Collections.Generic;
using System.Linq;
using Classroom.Model.DataModels.Enums;
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

        
        public IEnumerable<TeacherSubject> GetSubjectsByTeacherId(int teacherId)
        {
            return _dbContext.TeacherSubjects.Where(ts => ts.TeacherId == teacherId).ToList();
        }
        
        
        public void Add(TeacherSubjectRequest request)
        {
        
            var teacher = _dbContext.Teachers.FirstOrDefault(t => t.Id == request.TeacherId);
            var classOfStudents = _dbContext.ClassesOfStudents.FirstOrDefault(c => c.Id == request.ClassOfStudentsId);

            if (teacher == null)
            {
                throw new ArgumentException($"Teacher with ID {request.TeacherId} not found.");
            }

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
                ClassOfStudents = classOfStudents
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
                return null; // Vagy dobj egy kivételt, ha szükséges
            }

            return teacherSubject.ClassOfStudents; // Visszaadja az osztályt, ami tartalmazza a diákokat
        }

        
        
        /*public async Task<IEnumerable<TeacherSubject>> GetSubjectsAndStudentsByTeacherIdAsync(int teacherId)
        {
            return await _dbContext.TeacherSubjects
                .Include(ts => ts.ClassOfStudents)
                .ThenInclude(cs => cs.Students)
                .Where(ts => ts.TeacherId == teacherId)
                .ToListAsync();
        }*/
    
    }
}