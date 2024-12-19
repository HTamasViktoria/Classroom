using Classroom.Model.DataModels;
using Classroom.Model.ResponseModels;
using Classroom.Service.Repositories;

namespace ClassromIntegrationTests.MockRepos
{
    public class MockUserRepository : IUserRepository
    {
        public IEnumerable<Teacher> GetAllTeachers()
        {
            throw new Exception("Mock exception for testing.");
        }

        public Teacher GetTeacherById(string teacherId)
        {
            throw new Exception("Mock exception for testing.");
        }

        public void AddTeacher(Teacher teacher)
        {
            throw new Exception("Mock exception for testing.");
        }

        public IEnumerable<Parent> GetAllParents()
        {
            throw new Exception("Mock exception for testing.");
        }

        public IEnumerable<Student> GetAllStudents()
        {
            throw new Exception("Mock exception for testing.");
        }

        public Parent GetParentById(string parentId)
        {
            throw new Exception("Mock exception for testing.");
        }

        public void AddParent(Parent parent)
        {
            throw new Exception("Mock exception for testing.");
        }

        public string GetStudentFullNameById(string studentId)
        {
            throw new Exception("Mock exception for testing.");
        }

        public int CheckParentsNumber(string studentId)
        {
            throw new Exception("Mock exception for testing.");
        }

        public IEnumerable<ReceiverResponse> GetTeachersAsReceivers()
        {
            throw new Exception("Mock exception for testing.");
        }

        public IEnumerable<ReceiverResponse> GetParentsAsReceivers()
        {
            throw new Exception("Mock exception for testing.");
        }
    }
}