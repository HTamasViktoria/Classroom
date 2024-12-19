using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Classroom.Service.Repositories;


    namespace ClassromIntegrationTests.MockRepos;


public class MockTeacherSubjectRepository : ITeacherSubjectRepository
{
    public IEnumerable<TeacherSubject> GetAll()
    {
        throw new Exception("Mock exception for testing.");
    }

    public IEnumerable<TeacherSubject> GetSubjectsByTeacherId(string teacherId)
    {
        throw new Exception("Mock exception for testing.");
    }

    public void Add(TeacherSubjectRequest request)
    {
        throw new Exception("Mock exception for testing.");
    }

    public Task<ClassOfStudents> GetStudentsByTeacherSubjectIdAsync(int teacherSubjectId)
    {
        throw new Exception("Mock exception for testing.");
    }
}