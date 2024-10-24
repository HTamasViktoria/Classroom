using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;

namespace Classroom.Service.Repositories;

public interface ITeacherSubjectRepository
{
    IEnumerable<TeacherSubject> GetAll();
    IEnumerable<TeacherSubject> GetSubjectsByTeacherId(string teacherId);
    void Add(TeacherSubjectRequest request);
    Task<ClassOfStudents> GetStudentsByTeacherSubjectIdAsync(int teacherSubjectId);



}