using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;

namespace Classroom.Service.Repositories;

public interface IClassOfStudentsRepository
{
    IEnumerable<ClassOfStudents> GetAll();
    void Add(ClassOfStudentsRequest request);
    IEnumerable<Student> GetStudents(int classId);
    void AddStudent(AddingStudentToClassRequest request);

}