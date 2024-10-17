using Classroom.Model.DataModels;
using System.Collections.Generic;

namespace Classroom.Service
{
    public interface IUserService
    {
        IEnumerable<Teacher> GetAllTeachers();
        IEnumerable<Parent> GetAllParents();
        Teacher GetTeacherById(int teacherId);
        Parent GetParentById(int parentId);
        void AddTeacher(Teacher teacher);
        void AddParent(Parent parent);
        User GetByEmail(string email);
    }
}