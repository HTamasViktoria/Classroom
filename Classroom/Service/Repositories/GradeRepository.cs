using Classroom.Data;
using Classroom.Model.DataModels;
using Classroom.Model.DataModels.Enums;
using Classroom.Model.RequestModels;
using Microsoft.EntityFrameworkCore;


namespace Classroom.Service.Repositories;


public class GradeRepository : IGradeRepository
{
    private ClassroomContext _dbContext;
    
    public GradeRepository(ClassroomContext context)
    {
        _dbContext = context;
    }

    public IEnumerable<Grade> GetAll()
    {
        return _dbContext.Grades.ToList();
    }


    public void Add(GradeRequest request)
    {
        
        if (!int.TryParse(request.TeacherId, out var teacherId))
        {
            throw new ArgumentException("Invalid teacher ID format");
        }

        if (!int.TryParse(request.StudentId, out var studentId))
        {
            throw new ArgumentException("Invalid student ID format");
        }

        
        if (!Enum.TryParse<GradeValues>(ExtractGradeValue(request.Value), out var gradeValue))
        {
            throw new ArgumentException($"Invalid grade value: {request.Value}");
        }

        
        if (!DateTime.TryParse(request.Date, out var date))
        {
            throw new ArgumentException($"Invalid date format: {request.Date}");
        }

       
        var grade = new Grade
        {
            TeacherId = teacherId,
            StudentId = studentId,
            Subject = request.Subject,
            ForWhat = request.ForWhat,
            Value = gradeValue,
            Date = date
        };

        _dbContext.Grades.Add(grade);
        _dbContext.SaveChanges();
    }

    public IEnumerable<Grade> GetByStudentId(int id)
    {
        return _dbContext.Grades.Where(grade => grade.StudentId == id).ToList();
    }
    
    
    public async Task<Dictionary<string, double>> GetClassAverageGradesBySubjectAsync(int studentId)
    {
        var student = await _dbContext.Students.FirstOrDefaultAsync(s => s.Id == studentId);

        if (student == null)
        {
            throw new Exception("Student not found");
        }

        var classOfStudent = await _dbContext.ClassesOfStudents
            .Include(c => c.Students)
            .FirstOrDefaultAsync(c => c.Students.Any(s => s.Id == studentId));

        if (classOfStudent == null)
        {
            throw new Exception("Class not found for the given student");
        }

       
        var grades = await _dbContext.Grades
            .Where(g => g.StudentId == studentId)
            .ToListAsync();

        
        var subjectAverages = grades
            .GroupBy(g => g.Subject)
            .Select(g => new 
            {
                Subject = g.Key,
                Average = g.Average(x => (int)x.Value)
            })
            .ToDictionary(g => g.Subject, g => g.Average);

        return subjectAverages;
    }


    
    private string ExtractGradeValue(string valueWithLabel)
    {
        
        var parts = valueWithLabel.Split('=');
        return parts.Length > 1 ? parts[0].Trim() : valueWithLabel.Trim();
    }
    
    
    
    
    
    
}


