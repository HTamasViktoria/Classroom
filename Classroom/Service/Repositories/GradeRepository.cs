using Classroom.Data;
using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;


namespace Classroom.Service.Repositories;


public class GradeRepository : IGradeRepository
{
    private ClassroomContext _dbContext;
    
    public GradeRepository(ClassroomContext context)
    {
        _dbContext = context;
    }
    
      
    public void Add(GradeRequest request)
    {
        _dbContext.Add(request);
        _dbContext.SaveChanges();
    }
}