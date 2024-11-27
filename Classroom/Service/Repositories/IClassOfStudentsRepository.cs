using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Classroom.Model.ResponseModels;

namespace Classroom.Service.Repositories;

public interface IClassOfStudentsRepository
{
    IEnumerable<ClassOfStudents> GetAll();
    void Add(ClassOfStudentsRequest request);
    IEnumerable<Student> GetStudents(int classId);
    void AddStudent(AddingStudentToClassRequest request);
    IEnumerable<ClassOfStudents> GetClassesBySubject(string subject);
    IEnumerable<StudentWithClassResponse> GetAllStudentsWithClasses();
    

}