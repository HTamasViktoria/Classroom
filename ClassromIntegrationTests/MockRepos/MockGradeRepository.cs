using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Classroom.Model.ResponseModels;
using Classroom.Service.Repositories;

namespace ClassromIntegrationTests.MockRepos;

public class MockGradeRepository : IGradeRepository
{
    public void Add(GradeRequest request)  {
        throw new Exception("Mock exception for testing.");
    }
   public  IEnumerable<Grade> GetAll()  {
       throw new Exception("Mock exception for testing.");
   }

    public IEnumerable<Grade> GetByStudentId(string id)  {
        throw new Exception("Mock exception for testing.");
    }
    public Task<Dictionary<string, double>> GetClassAveragesByStudentId(string studentId)  {
        throw new Exception("Mock exception for testing.");
    }
    public Task<Dictionary<string, double>> GetClassAveragesBySubject(string subject)  {
        throw new Exception("Mock exception for testing.");
    }
    public Task<IEnumerable<Grade>> GetGradesByClassBySubject(int classId, string subject)  {
        throw new Exception("Mock exception for testing.");
    }
    public Task<IEnumerable<Grade>> GetGradesByClass(int classId)  {
        throw new Exception("Mock exception for testing.");
    }
    public void Edit(GradeRequest request, int id)  {
        throw new Exception("Mock exception for testing.");
    }
    public void Delete(int id)  {
        throw new Exception("Mock exception for testing.");
    }
    public Task<IEnumerable<Grade>> GetGradesBySubjectByStudent(string subject, string studentId)  {
        throw new Exception("Mock exception for testing.");
    }
    public IEnumerable<Grade> GetNewGradesByStudentId(string studentId)  {
        throw new Exception("Mock exception for testing.");
    }
   public  int GetNewGradesNumber(string id)  {
       throw new Exception("Mock exception for testing.");
   }
    
    public Task<LatestGradeResponse> GetTeachersLastGradeAsync(string teacherId)  {
        throw new Exception("Mock exception for testing.");
    }
    public void SetToOfficiallyRead(int gradeId)  {
        throw new Exception("Mock exception for testing.");
    }


}