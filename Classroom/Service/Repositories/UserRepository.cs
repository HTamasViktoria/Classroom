using Microsoft.EntityFrameworkCore;
using Classroom.Data;
using Classroom.Model.DataModels;
using System.Collections.Generic;
using System.Linq;

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


     
    }
}