using Classroom.Model.DataModels;
using Classroom.Model.DataModels.Enums;
using Classroom.Model.RequestModels;

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



}