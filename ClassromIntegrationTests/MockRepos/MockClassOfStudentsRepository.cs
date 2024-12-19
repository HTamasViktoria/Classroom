using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Classroom.Model.ResponseModels;
using Classroom.Service.Repositories;

namespace ClassromIntegrationTests.MockRepos
{
    public class MockClassOfStudentsRepository : IClassOfStudentsRepository
    {
        public IEnumerable<ClassOfStudents> GetAll()
        {
            throw new Exception("Mock exception for testing.");
        }

        public void Add(ClassOfStudentsRequest request)
        {
            throw new NotImplementedException("Add method is not implemented.");
        }

        public IEnumerable<Student> GetStudents(int classId)
        {
            throw new NotImplementedException("GetStudents method is not implemented.");
        }

        public void AddStudent(AddingStudentToClassRequest request)
        {
            throw new NotImplementedException("AddStudent method is not implemented.");
        }

        public IEnumerable<ClassOfStudents> GetClassesBySubject(string subject)
        {
            throw new NotImplementedException("GetClassesBySubject method is not implemented.");
        }

        public IEnumerable<StudentWithClassResponse> GetAllStudentsWithClasses()
        {
            throw new Exception("Mock exception for testing.");
        }
    }
}