using Classroom.Model.DataModels;
using Classroom.Model.DataModels.Enums;
using Classroom.Model.RequestModels;

namespace Classroom.Service.Repositories;

public interface  IGradeRepository
{
    void Add(GradeRequest request);
    IEnumerable<Grade> GetAll();

    
}