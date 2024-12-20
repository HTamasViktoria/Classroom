using Classroom.Model.DataModels;
using Classroom.Model.DataModels.Enums;
using Classroom.Model.RequestModels;
using Classroom.Model.ResponseModels;

namespace Classroom.Service.Repositories;

public interface  IGradeRepository
{
    void Add(GradeRequest request);
    IEnumerable<Grade> GetAll();

    IEnumerable<Grade> GetByStudentId(string id); 
    Task<Dictionary<string, double>> GetClassAveragesByStudentId(string studentId);
    Task<Dictionary<string, double>> GetClassAveragesBySubject(string subject);
    Task<IEnumerable<Grade>> GetGradesByClassBySubject(int classId, string subject);
    Task<IEnumerable<Grade>> GetGradesByClass(int classId);
    void Edit(GradeRequest request, int id);
    void Delete(int id);
    Task<IEnumerable<Grade>> GetGradesBySubjectByStudent(string subject, string studentId);
    IEnumerable<Grade> GetNewGradesByStudentId(string studentId);
    int GetNewGradesNumber(string id);
    
    Task<LatestGradeResponse> GetTeachersLastGradeAsync(string teacherId);
    void SetToOfficiallyRead(int gradeId);



}