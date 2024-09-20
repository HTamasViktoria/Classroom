using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;

namespace Classroom.Service.Repositories;

public interface ITeacherSubjectRepository
{
    IEnumerable<TeacherSubject> GetAll();
    IEnumerable<TeacherSubject> GetSubjectsByTeacherId(int teacherId);
    void Add(TeacherSubjectRequest request);
    Task<ClassOfStudents> GetStudentsByTeacherSubjectIdAsync(int teacherSubjectId);

    /*Task<IEnumerable<TeacherSubject>> GetSubjectsAndStudentsByTeacherIdAsync(int teacherId);*/

}