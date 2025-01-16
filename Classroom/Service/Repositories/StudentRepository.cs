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
        var existingStudent = _dbContext.Students.FirstOrDefault(s => s.StudentNo == request.StudentNo);
        if (existingStudent != null)
        {
            throw new ArgumentException("A student with the same student number already exists.");
        }
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
            Role = "Student",
            Grades = new List<Grade>()
        };
        _dbContext.Add(student);
        _dbContext.SaveChanges();
    }
}