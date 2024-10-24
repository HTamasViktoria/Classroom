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

    public Teacher GetTeacherById(string id)
    {
        return _dbContext.Teachers.FirstOrDefault(teacher=> teacher.Id == id);
    }

    public IEnumerable<Teacher> GetAll()
    {
        return _dbContext.Teachers.ToList();
    }
    
    public void Add(TeacherRequest request)
    {
        var teacher = new Teacher
        {
            FamilyName = request.FamilyName,
            FirstName = request.FirstName
        };
        _dbContext.Add(teacher);
        _dbContext.SaveChanges();
    }
    
    

}