using Classroom.Data;
using Classroom.Model.DataModels;

namespace Classroom.Service.Repositories;

public class ParentRepository : IParentRepository
{
    private ClassroomContext _dbContext;
    
    public ParentRepository(ClassroomContext context)
    {
        _dbContext = context;
    }
    
    public Parent GetParentById(string id)
    {
        return _dbContext.Parents.FirstOrDefault(parent=> parent.Id == id);
    }

    public IEnumerable<Parent> GetAllParents()
    {
        return _dbContext.Parents.ToList();
    }
    
    
    public IEnumerable<Parent> GetParentsByStudentId(string id)
    {
        bool studentFound = _dbContext.Students.Any(s => s.Id == id);
    
        if (!studentFound)
        {
            throw new ArgumentException("No student found with the given ID.");
        }

        return _dbContext.Parents.Where(parent => parent.StudentId == id);
    }

}