using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;

namespace Classroom.Service.Repositories;

public interface ITeacherRepository
{
    void Add(TeacherRequest request);
}