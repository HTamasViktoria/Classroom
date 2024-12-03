using Classroom.Model.DataModels;
using Classroom.Service.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Classroom.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<Teacher> GetAllTeachers()
        {
            return _userRepository.GetAllTeachers();
        }

        public IEnumerable<Parent> GetAllParents()
        {
            return _userRepository.GetAllParents();
        }

        public Teacher GetTeacherById(string teacherId)
        {
            return _userRepository.GetTeacherById(teacherId);
        }

        public Parent GetParentById(string parentId)
        {
            return _userRepository.GetParentById(parentId);
        }

        public void AddTeacher(Teacher teacher)
        {
            _userRepository.AddTeacher(teacher);
        }

        public void AddParent(Parent parent)
        {
            _userRepository.AddParent(parent);
        }

       
        public User? GetByEmail(string email)
        {
          
            var student = _userRepository.GetAllStudents()
                .AsQueryable()
                .FirstOrDefault(s => s.Email == email);
            if (student != null) return student;

            var parent = _userRepository.GetAllParents()
                .AsQueryable()
                .Include(p => p.Student) 
                .FirstOrDefault(p => p.Email == email);
            if (parent != null) return parent;


            var teacher = _userRepository.GetAllTeachers()
                .AsQueryable()
                .FirstOrDefault(t => t.Email == email);
            if (teacher != null) return teacher;

            return null;
        }


        public bool CheckStudentId(string studentId, string studentName)
        {
            var nameParts = studentName.Split(' ');
            if (nameParts.Length < 2)
            {
                return false;
            }

            var familyName = nameParts[0];
            var firstName = nameParts[1];

            var studentFullName = _userRepository.GetStudentFullNameById(studentId);
          

            if (studentFullName == studentName)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckParentsNumber(string studentId)
        {
            int parentsNumberAlready = _userRepository.CheckParentsNumber(studentId);
            if (parentsNumberAlready >= 4)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        
        
        public IEnumerable<string> ValidateParentRegistration(string studentId, string studentName)
        {
            var errors = new List<string>();

      
            var studentIdIsValid = CheckStudentId(studentId, studentName);
            if (!studentIdIsValid)
            {
                errors.Add("Invalid student ID or child name.");
            }

            
            var parentsNumberOk = CheckParentsNumber(studentId);
            if (!parentsNumberOk)
            {
                errors.Add("The student already has the maximum number of registered parents.");
            }

            return errors;
        }



    }
}