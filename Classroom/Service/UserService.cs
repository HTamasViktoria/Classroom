using Classroom.Model.DataModels;
using Classroom.Service.Repositories;
using System.Collections.Generic;
using System.Linq;

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

        public Teacher GetTeacherById(int teacherId)
        {
            return _userRepository.GetTeacherById(teacherId);
        }

        public Parent GetParentById(int parentId)
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

       
        public User GetByEmail(string email)
        {
            
            var student = _userRepository.GetAllStudents().FirstOrDefault(s => s.Email == email);
            if (student != null) return student;

           
            var parent = _userRepository.GetAllParents().FirstOrDefault(p => p.Email == email);
            if (parent != null) return parent;

            
            var teacher = _userRepository.GetAllTeachers().FirstOrDefault(t => t.Email == email);
            if (teacher != null) return teacher;

            
            return null;
        }

    }
}