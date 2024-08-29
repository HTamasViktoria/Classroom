using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;

namespace Classroom.Service.Repositories;

public interface IStudentRepository
{
        void Add(StudentRequest request);
        IEnumerable<Student> GetAll();
}