using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;

namespace Classroom.Service.Repositories;

public interface ITeacherRepository
{
    IEnumerable<Teacher> GetAll();
    void Add(TeacherRequest request);
    Teacher GetTeacherById(int id);

}