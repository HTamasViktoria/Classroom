using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Classroom.Service.Repositories;

namespace ClassromIntegrationTests.MockRepos;

public class MockStudentRepository : IStudentRepository
{
   public  void Add(StudentRequest request)   {
       throw new Exception("Mock exception for testing.");
   }
    public IEnumerable<Student> GetAll()   {
        throw new Exception("Mock exception for testing.");
    }
    public Student GetStudentById(string id)   {
        throw new Exception("Mock exception for testing.");
    }
}