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
        
        if (string.IsNullOrWhiteSpace(request.TeacherId))
        {
            throw new ArgumentException("Teacher ID cannot be null or empty.");
        }

        if (string.IsNullOrWhiteSpace(request.StudentId))
        {
            throw new ArgumentException("Student ID cannot be null or empty.");
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
            TeacherId = request.TeacherId,
            StudentId = request.StudentId,
            Subject = request.Subject,
            ForWhat = request.ForWhat,
            Value = gradeValue,
            Date = date
        };

        _dbContext.Grades.Add(grade);
        _dbContext.SaveChanges();
    }

    public IEnumerable<Grade> GetByStudentId(string studentId)
    {
        return _dbContext.Grades.Where(grade => grade.StudentId == studentId).ToList();
    }
    
    public void Delete(int id)
    {
        var gradeToDelete = _dbContext.Grades.FirstOrDefault(g => g.Id == id);
    
        if (gradeToDelete == null)
        {
            throw new ArgumentException($"Grade with Id {id} not found.");
        }
    
        _dbContext.Grades.Remove(gradeToDelete);
        _dbContext.SaveChanges();
    }


    public async Task<Dictionary<string, double>> GetClassAveragesBySubject(string subject)
    {
        var classAverages = new Dictionary<string, double>();
        
        var teacherSubjects = await _dbContext.TeacherSubjects
            .Include(ts => ts.ClassOfStudents)
            .Where(ts => ts.Subject == subject)
            .ToListAsync();
        
        var classGroups = teacherSubjects
            .GroupBy(ts => ts.ClassOfStudentsId)
            .ToList();

    
        foreach (var classGroup in classGroups)
        {
            var classId = classGroup.Key;
            var studentIds = await _dbContext.ClassesOfStudents
                .Where(c => c.Id == classId)
                .SelectMany(c => c.Students.Select(s => s.Id))
                .ToListAsync();
            
            var grades = await _dbContext.Grades
                .Where(g => studentIds.Contains(g.Id.ToString()) && g.Subject == subject)
                .ToListAsync();
            
            if (grades.Count == 0)
            {
                classAverages[classGroup.First().ClassOfStudents.Name] = 0;
                continue;
            }
            
            var average = grades.Average(g => (double)g.Value);
            var roundedAverage = Math.Round(average, 2);

            classAverages[classGroup.First().ClassOfStudents.Name] = roundedAverage;
        }

        return classAverages;
    }


    public async Task<IEnumerable<Grade>> GetGradesByClassBySubject(int classId, string subject)
    {
       
        var studentIds = await _dbContext.ClassesOfStudents
            .Where(c => c.Id == classId)
            .SelectMany(c => c.Students.Select(s => s.Id))
            .ToListAsync();

       
        var grades = await _dbContext.Grades
            .Where(g => studentIds.Contains(g.StudentId) && g.Subject == subject)
            .ToListAsync();

        return grades;
    }

    
    
    public async Task<IEnumerable<Grade>> GetGradesByClass(int classId)
    {
        var studentIds = await _dbContext.ClassesOfStudents
            .Where(c => c.Id == classId)
            .SelectMany(c => c.Students.Select(s => s.Id))
            .ToListAsync();

        // A Grade StudentId mezőjét kell ellenőrizni, nem az Id-t
        var grades = await _dbContext.Grades
            .Where(g => studentIds.Contains(g.StudentId))
            .ToListAsync();

        return grades;
    }



    
    public async Task<Dictionary<string, double>> GetClassAveragesByStudentId(string studentId)
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
    
        var studentIds = classOfStudent.Students.Select(s => s.Id).ToList();
    
        var grades = await _dbContext.Grades
            .Where(g => studentIds.Contains(g.StudentId))
            .ToListAsync();
    
        var subjectAverages = grades
            .GroupBy(g => g.Subject)
            .Select(g => new 
            {
                Subject = g.Key,
                Average = Math.Round(g.Average(x => (double)x.Value), 2)
            })
            .ToDictionary(g => g.Subject, g => g.Average);

        return subjectAverages;
    }

    
    

    public void Edit(GradeRequest request, int id)
    {
        var gradeToUpdate = _dbContext.Grades.FirstOrDefault(g => g.Id == id);

        if (gradeToUpdate != null)
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

            gradeToUpdate.TeacherId = teacherId.ToString();
            gradeToUpdate.StudentId = studentId.ToString();
            gradeToUpdate.Date = date;
            gradeToUpdate.ForWhat = request.ForWhat;
            gradeToUpdate.Value = gradeValue;
            _dbContext.SaveChanges();
        }
        else
        {
            throw new Exception($"Grade with Id {id} not found.");
        }
    }

    
    private string ExtractGradeValue(string valueWithLabel)
    {
        
        var parts = valueWithLabel.Split('=');
        return parts.Length > 1 ? parts[0].Trim() : valueWithLabel.Trim();
    }
    
    
    
    
    
    
}


