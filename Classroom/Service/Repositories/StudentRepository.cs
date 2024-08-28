using Classroom.Data;
using Classroom.Model.DataModels;

namespace Classroom.Service.Repositories;

public class StudentRepository : IStudentRepository
{
    private ClassroomContext _dbContext;
    
    public StudentRepository(ClassroomContext context)
    {
        _dbContext = context;
    }
    
    
    public void Add(Student student)
    {
        _dbContext.Add(student);
        _dbContext.SaveChanges();
    }
}