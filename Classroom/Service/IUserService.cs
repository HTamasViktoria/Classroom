using Classroom.Model.DataModels;
using System.Collections.Generic;

namespace Classroom.Service
{
    public interface IUserService
    {
        IEnumerable<Teacher> GetAllTeachers();
        IEnumerable<Parent> GetAllParents();
        Teacher GetTeacherById(string teacherId);
        Parent GetParentById(string parentId);
        void AddTeacher(Teacher teacher);
        void AddParent(Parent parent);
        User GetByEmail(string email);
        string GetIdByName(string name);
    }
}