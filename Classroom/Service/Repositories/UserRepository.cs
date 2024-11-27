using Classroom.Data;
using Classroom.Model.DataModels;
using Classroom.Model.ResponseModels;


namespace Classroom.Service.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ClassroomContext _dbContext;

        public UserRepository(ClassroomContext context)
        {
            _dbContext = context;
        }

    
        public IEnumerable<Teacher> GetAllTeachers()
        {
            return _dbContext.Teachers.ToList();
        }

    
        public IEnumerable<Parent> GetAllParents()
        {
            return _dbContext.Parents.ToList();
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return _dbContext.Students.ToList();
        }

      
        public Teacher GetTeacherById(string teacherId)
        {
            return _dbContext.Teachers.FirstOrDefault(t => t.Id == teacherId);
        }

     
        public Parent GetParentById(string parentId)
        {
            return _dbContext.Parents.FirstOrDefault(p => p.Id == parentId);
        }

        public int CheckParentsNumber(string studentId)
        {
          
            return _dbContext.Parents.Count(p => p.StudentId == studentId);
        }

        
        public string GetStudentFullNameById(string studentId)
        {
            var student = _dbContext.Students.FirstOrDefault(s => s.Id == studentId);
            if (student != null)
            {
               
                return ($"{student.FamilyName} {student.FirstName}");
            }
            return (null);
        }


        public void AddTeacher(Teacher teacher)
        {
            _dbContext.Teachers.Add(teacher);
            _dbContext.SaveChanges();
        }

    
        public void AddParent(Parent parent)
        {
            _dbContext.Parents.Add(parent);
            _dbContext.SaveChanges();
        }


        public IEnumerable<ReceiverResponse> GetTeachersAsReceivers()
        {
            var teachers = _dbContext.Teachers.ToList();
  
            var responseList = teachers.Select(t => new ReceiverResponse
            {
                Name = t.FirstName + " " + t.FamilyName,
                Id = t.Id,  
                Role = t.Role
            }).ToList();

            return responseList;
        }
        
        
        public IEnumerable<ReceiverResponse> GetParentsAsReceivers()
        {
            var parents = _dbContext.Parents.ToList();
  
            var responseList = parents.Select(p => new ReceiverResponse
            {
                Name = p.FirstName + " " + p.FamilyName,
                Id = p.Id,  
                Role = p.Role
            }).ToList();

            return responseList;
        }


     
    }
}