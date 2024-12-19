using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Classroom.Service.Repositories;

namespace ClassromIntegrationTests.MockRepos;

public class MockTeacherRepository : ITeacherRepository
{
    public IEnumerable<Teacher> GetAll()
    {
        throw new Exception("Mock exception for testing.");}


    public void Add(TeacherRequest request)
    {
        throw new Exception("Mock exception for testing.");}

    public Teacher GetTeacherById(string id)
    {
        throw new Exception("Mock exception for testing.");
    }
}