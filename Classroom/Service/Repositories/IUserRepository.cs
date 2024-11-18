using Classroom.Model.DataModels;

namespace Classroom.Service.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<Teacher> GetAllTeachers();
        Teacher GetTeacherById(string teacherId);
        void AddTeacher(Teacher teacher);
        IEnumerable<Parent> GetAllParents();
        IEnumerable<Student> GetAllStudents();
        Parent GetParentById(string parentId);
        void AddParent(Parent parent);
        string GetStudentFullNameById(string studentId);
        int CheckParentsNumber(string studentId);
    }
}