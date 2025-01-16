using Classroom.Data;
using Classroom.Model.DataModels;
using Classroom.Model.DataModels.Enums;
using Classroom.Model.RequestModels;
using Classroom.Model.ResponseModels;
using Microsoft.EntityFrameworkCore;


namespace Classroom.Service.Repositories;

public class GradeRepository : IGradeRepository
{
    private ClassroomContext _dbContext;
    private IUserRepository _userRepository;

    public GradeRepository(ClassroomContext context, IUserRepository userRepository)
    {
        _dbContext = context;
        _userRepository = userRepository;
    }

    public IEnumerable<Grade> GetAll()
    {
        return _dbContext.Grades.ToList();
    }



    public void Add(GradeRequest request)
    {
        ValidateTeacher(request.TeacherId);
        ValidateStudent(request.StudentId);
        ValidateSubject(request.Subject);
        ValidateForWhat(request.ForWhat);

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
        ValidateStudent(studentId);

        return _dbContext.Grades.Where(grade => grade.StudentId == studentId).ToList();
    }


    public async Task<LatestGradeResponse> GetTeachersLastGradeAsync(string teacherId)
    {
        ValidateTeacher(teacherId);

        var grade = await _dbContext.Grades
            .Where(g => g.TeacherId == teacherId)
            .OrderByDescending(g => g.Date)
            .FirstOrDefaultAsync();

        if (grade == null)
        {
            return null;
        }

        var studentFullName = _userRepository.GetStudentFullNameById(grade.StudentId);

        var gradeResponse = new LatestGradeResponse
        {
            Id = grade.Id,
            TeacherId = grade.TeacherId,
            StudentId = grade.StudentId,
            StudentName = studentFullName,
            Subject = grade.Subject,
            ForWhat = grade.ForWhat,
            Read = grade.Read,
            Value = grade.Value,
            Date = grade.Date
        };

        return gradeResponse;
    }


    public IEnumerable<Grade> GetNewGradesByStudentId(string studentId)
    {
        ValidateStudent(studentId);

        return _dbContext.Grades.Where(grade => grade.StudentId == studentId && grade.Read == false)
            .OrderByDescending(grade => grade.Date).ToList();
    }


    public int GetNewGradesNumber(string studentId)
    {
        ValidateStudent(studentId);
        return _dbContext.Grades.Count(grade => grade.StudentId == studentId && !grade.Read);
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
        ValidateSubject(subject);
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
            var className = classGroup.First().ClassOfStudents.Name;
            var studentIds = await _dbContext.ClassesOfStudents
                .Where(c => c.Id == classId)
                .SelectMany(c => c.Students.Select(s => s.Id))
                .ToListAsync();


            var grades = await _dbContext.Grades
                .Where(g => studentIds.Contains(g.StudentId) && g.Subject == subject)
                .ToListAsync();

            if (grades.Count == 0)
            {
                classAverages[className] = 0;
                continue;
            }

            var average = grades.Average(g => (double)g.Value);
            var roundedAverage = Math.Round(average, 2);

            classAverages[className] = roundedAverage;
        }

        return classAverages;
    }

    

    public async Task<IEnumerable<Grade>> GetGradesByClassBySubject(int classId, string subject)
    {
        try
        {
            ValidateClassOfStudents(classId);
            ValidateSubject(subject);

            var subjectExistsForClass =
                await _dbContext.TeacherSubjects.AnyAsync(ts => ts.ClassOfStudentsId == classId && ts.Subject == subject);
            if (!subjectExistsForClass)
            {
                throw new ArgumentException($"Class with ID {classId} does not have subject {subject}.");  // Továbbra is ArgumentException
            }

            var studentIds = await _dbContext.ClassesOfStudents
                .Where(c => c.Id == classId)
                .SelectMany(c => c.Students.Select(s => s.Id))
                .ToListAsync();

            var grades = await _dbContext.Grades
                .Where(g => studentIds.Contains(g.StudentId) && g.Subject == subject)
                .ToListAsync();

            return grades;
        }
        catch (ArgumentException ex)
        {
           
            throw new ArgumentException("An error occurred while fetching grades by subject.", ex);
        }
    }



    public async Task<IEnumerable<Grade>> GetGradesByClass(int classId)
    {
       
        await ValidateClassOfStudents(classId);

       
        var studentIds = await _dbContext.ClassesOfStudents
            .Where(c => c.Id == classId)
            .SelectMany(c => c.Students.Select(s => s.Id))
            .ToListAsync();

    
        var grades = await _dbContext.Grades
            .Where(g => studentIds.Contains(g.StudentId))
            .ToListAsync();

        return grades;
    }


    public async Task<IEnumerable<Grade>> GetGradesBySubjectByStudent(string subject, string studentId)
    {
        ValidateSubject(subject);
        ValidateStudent(studentId);
        try
        {
            var grades = await _dbContext.Grades
                .Where(g => g.Subject == subject && g.StudentId == studentId)
                .ToListAsync();

            return grades;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the grades.", ex);
        }
    }


    public async Task<Dictionary<string, double>> GetClassAveragesByStudentId(string studentId)
    {
        ValidateStudent(studentId);

        var classOfStudent = await _dbContext.ClassesOfStudents
            .Include(c => c.Students)
            .FirstOrDefaultAsync(c => c.Students.Any(s => s.Id == studentId));

        if (classOfStudent == null)
        {
            throw new Exception("Class not found for the given student.");
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

        if (gradeToUpdate == null)
        {
            throw new KeyNotFoundException($"Grade with Id {id} not found.");
        }

        if (!Enum.TryParse<GradeValues>(ExtractGradeValue(request.Value), out var gradeValue))
        {
            throw new ArgumentException($"Invalid grade value: {request.Value}");
        }

        if (!DateTime.TryParse(request.Date, out var date))
        {
            throw new ArgumentException($"Invalid date format: {request.Date}");
        }

        ValidateStudent(request.StudentId);
        ValidateTeacher(request.TeacherId);

        gradeToUpdate.TeacherId = request.TeacherId;
        gradeToUpdate.StudentId = request.StudentId;
        gradeToUpdate.Date = date;
        gradeToUpdate.ForWhat = request.ForWhat;
        gradeToUpdate.Value = gradeValue;

        _dbContext.SaveChanges();
    }


    private string ExtractGradeValue(string valueWithLabel)
    {
        var parts = valueWithLabel.Split('=');
        return parts.Length > 1 ? parts[0].Trim() : valueWithLabel.Trim();
    }


    public void SetToOfficiallyRead(int gradeId)
    {
        var grade = _dbContext.Grades.FirstOrDefault(g => g.Id == gradeId);

        if (grade == null)
        {
            throw new ArgumentException($"Grade with ID {gradeId} not found.");
        }


        if (grade.Read == true)
        {
            throw new ArgumentException($"Grade with ID {gradeId} is already marked as read.");
        }

        grade.Read = true;
        _dbContext.SaveChanges();
    }
    
    
    private void ValidateTeacher(string teacherId)
    {
        if (string.IsNullOrWhiteSpace(teacherId))
        {
            throw new ArgumentException("Teacher ID cannot be null or empty.");
        }

        var teacher = _dbContext.Teachers.FirstOrDefault(t => t.Id == teacherId);
        if (teacher == null)
        {
            throw new ArgumentException($"Teacher with ID {teacherId} not found.");
        }
    }


    public void ValidateStudent(string studentId)
    {
        if (string.IsNullOrWhiteSpace(studentId))
        {
            throw new ArgumentException("Student ID cannot be null or empty.");
        }

        var student = _dbContext.Students.FirstOrDefault(s => s.Id == studentId);
        if (student == null)
        {
            throw new ArgumentException($"Student with ID {studentId} not found.");
        }
    }


    public void ValidateSubject(string subject)
    {
        if (string.IsNullOrWhiteSpace(subject) || !Enum.IsDefined(typeof(Subjects), subject))
        {
            throw new ArgumentException($"Subject {subject} is not a valid subject.");
        }
    }


    public void ValidateForWhat(string forWhat)
    {
        if (string.IsNullOrWhiteSpace(forWhat))
        {
            throw new ArgumentException("ForWhat cannot be null or empty.");
        }
    }
    
    
    public async Task ValidateClassOfStudents(int classId)
    {
        var classExists = await _dbContext.ClassesOfStudents.AnyAsync(c => c.Id == classId);
        if (!classExists)
        {
            throw new ArgumentException($"Class with ID {classId} not found.");
        }
    }

}