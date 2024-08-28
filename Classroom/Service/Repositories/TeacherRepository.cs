using Classroom.Data;
using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;

namespace Classroom.Service.Repositories;

public class TeacherRepository : ITeacherRepository
{
    private ClassroomContext _dbContext;
    
    public TeacherRepository(ClassroomContext context)
    {
        _dbContext = context;
    }


    public void Add(TeacherRequest request)
    {
        _dbContext.Add(request);
        _dbContext.SaveChanges();
    }

}