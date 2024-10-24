using Classroom.Data;
using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;

namespace Classroom.Service.Repositories;

public class StudentRepository : IStudentRepository
{
    private ClassroomContext _dbContext;
    
    public StudentRepository(ClassroomContext context)
    {
        _dbContext = context;
    }

    public IEnumerable<Student> GetAll()
    {
        return _dbContext.Students.ToList();
    }


    public Student GetStudentById(string id)
    {
        return _dbContext.Students.FirstOrDefault(student=> student.Id == id);
    }
    
    
    
    public void Add(StudentRequest request)
    {
        DateTime birthDate;
        if (!DateTime.TryParse(request.BirthDate, out birthDate))
        {
            throw new ArgumentException("Invalid birth date format.");
        }
        
        var student = new Student
        {
            FamilyName = request.FamilyName,
            FirstName = request.FirstName,
            BirthDate = birthDate,
            BirthPlace = request.BirthPlace,
            StudentNo = request.StudentNo,
            Grades = new List<Grade>()
        };
        _dbContext.Add(student);
        _dbContext.SaveChanges();
    }
}