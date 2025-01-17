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
        return _dbContext.Teachers.FirstOrDefault(teacher => teacher.Id == id);
    }

    public IEnumerable<Teacher> GetAll()
    {
        return _dbContext.Teachers.ToList();
    }

    public void Add(TeacherRequest request)
    {
        bool teacherExisting =
            _dbContext.Teachers.Any(t => 
                t.FamilyName == request.FamilyName && t.FirstName == request.FirstName);
        if (teacherExisting)
        {
            throw new ArgumentException("Teacher already added");
        }

        var teacher = new Teacher
        {
            FamilyName = request.FamilyName,
            FirstName = request.FirstName,
            Role = "Teacher"
        };
        _dbContext.Add(teacher);
        _dbContext.SaveChanges();
    }
}