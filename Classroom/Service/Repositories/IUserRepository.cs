using Classroom.Model.DataModels;

namespace Classroom.Service.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<Teacher> GetAllTeachers();
        Teacher GetTeacherById(int teacherId);
        void AddTeacher(Teacher teacher);
        IEnumerable<Parent> GetAllParents();
        IEnumerable<Student> GetAllStudents();
        Parent GetParentById(int parentId);
        void AddParent(Parent parent);
    }
}