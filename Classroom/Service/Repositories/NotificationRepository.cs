using Classroom.Data;
using Classroom.Model.DataModels;
using Classroom.Model.DataModels.Enums;
using Classroom.Model.RequestModels;


namespace Classroom.Service.Repositories;

public class NotificationRepository : INotificationRepository
{
    private ClassroomContext _dbContext;

    public NotificationRepository(ClassroomContext context)
    {
        _dbContext = context;
    }

    public IEnumerable<NotificationBase> GetAll()
    {
        return _dbContext.Notifications.ToList();
    }

    public void ValidateUser(string userId)
    {
        var userFound = _dbContext.Users.Any(u => u.Id == userId);
        if (!userFound)
        {
            throw new ArgumentException($"User with the given id not found");
        }
    }

    public IEnumerable<NotificationBase> GetByStudentId(string studentId, string parentId)
    {
        ValidateUser(studentId);
        ValidateUser(parentId);
        
        var studentExists = _dbContext.Students.Any(s => s.Id == studentId);
        var parentExists = _dbContext.Parents.Any(p => p.Id == parentId);

        if (!studentExists)
        {
            throw new ArgumentException($"Student with ID {studentId} does not exist.");
        }

        if (!parentExists)
        {
            throw new ArgumentException($"Parent with ID {parentId} does not exist.");
        }

        var notifications = _dbContext.Notifications
            .Where(n => n.StudentId == studentId && n.ParentId == parentId)
            .ToList();

        return notifications;
    }



    public int GetNewNotifsNumber(string studentId, string parentId)
    {
        ValidateUser(studentId);
        ValidateUser(parentId);
        return _dbContext.Notifications
            .Count(n => n.StudentId == studentId && n.ParentId == parentId && !n.Read);
    }

    
    public IEnumerable<NotificationBase> GetByTeacherId(string id)
    {
        ValidateUser(id);
        return _dbContext.Notifications
            .Where(n => n.TeacherId == id).ToList();
    }

    
    
    public IEnumerable<NotificationBase> GetNewNotifsByStudentId(string studentId, string parentId)
    {
        ValidateUser(studentId);
        ValidateUser(parentId);
        return _dbContext.Notifications
            .Where(n => n.StudentId == studentId && n.ParentId == parentId && !n.Read)
            .OrderByDescending(n => n.Date)
            .ToList();
    }


    public IEnumerable<NotificationBase> GetLastsByStudentId(string studentId, string parentId)
    {
        ValidateUser(studentId);
        ValidateUser(parentId);
        return _dbContext.Notifications
            .Where(n => n.StudentId == studentId && n.ParentId == parentId && !n.Read)
            .OrderByDescending(n => n.Date)
            .Take(3)
            .ToList();
    }

    public IEnumerable<NotificationResponse> GetLastsByTeacherId(string id)
    {
        ValidateUser(id);
        var notifications = _dbContext.Notifications
            .Where(not => not.TeacherId == id)
            .OrderByDescending(not => not.Date)
            .ToList();

        return notifications.Select(not => new NotificationResponse
        {
            Id = not.Id,
            TeacherId = not.TeacherId,
            TeacherName = not.TeacherName,
            Type = not.Type,
            Date = not.Date,
            DueDate = not.DueDate,
            StudentId = not.StudentId, 
            ParentName = not.ParentName,
            StudentName = not.StudentName,
            Description = not.Description,
            SubjectName = not.SubjectName,
            Read = not.Read,
            OfficiallyRead = not.OfficiallyRead,
            OptionalDescription = not.OptionalDescription
        });
    }


    
    public NotificationBase? GetNewestByTeacherId(string teacherId)
    {
        ValidateUser(teacherId);
        return _dbContext.Notifications
            .Where(notification => notification.TeacherId == teacherId)
            .OrderByDescending(notification => notification.Date)
            .FirstOrDefault();
    }


    public void ValidateSubject(string subject)
    {
        if (string.IsNullOrWhiteSpace(subject) || !Enum.IsDefined(typeof(Subjects), subject))
        {
            throw new ArgumentException($"Subject {subject} is not a valid subject.");
        }
    }

    public void CreateNotificationsBasedOnStudentsList(bool subjectNeeded, IEnumerable<Student> students, NotificationRequest request)
    {
        foreach (var student in students)
        {
            var studentName = $"{student.FirstName} {student.FamilyName}";

            var parents = _dbContext.Parents
                .Where(p => p.StudentId == student.Id)
                .ToList();

            foreach (var parent in parents)
            {
                var notification = new NotificationBase
                {
                    TeacherId = request.TeacherId,
                    TeacherName = request.TeacherName,
                    Type = request.Type,
                    Date = request.Date,
                    DueDate = request.DueDate,
                    StudentId = student.Id,
                    StudentName = studentName,
                    ParentId = parent.Id, 
                    ParentName = $"{parent.FamilyName} {parent.FirstName}",
                    Read = request.Read,
                    OfficiallyRead = request.OfficiallyRead,
                    Description = request.Description,
                    SubjectName = request.Subject,
                    OptionalDescription = request.OptionalDescription
                };
                
                if (subjectNeeded && Enum.TryParse<Subjects>(request.Subject, out var subjectEnum))
                {
                    notification.Subject = subjectEnum;
                }
            
                _dbContext.Notifications.Add(notification);
            }
        }

        _dbContext.SaveChanges();
    }


    public void Add(NotificationRequest request)
    {
        var allStudents = _dbContext.Students.ToList();
        var invalidStudents = new List<string>();

        var students = allStudents
            .Where(s => request.StudentIds.Contains(s.Id.ToString()))
            .ToList();

        foreach (var student in students)
        {
            try
            {
                ValidateUser(student.Id.ToString());
            }
            catch (ArgumentException ex)
            {
                invalidStudents.Add(student.Id.ToString());
                students.Remove(student);
            }
        }
        
        if (request.Type != "OtherNotification")
        {
            ValidateSubject(request.Subject);
            CreateNotificationsBasedOnStudentsList(true, students, request);
        }
        else
        {
            CreateNotificationsBasedOnStudentsList(false, students, request);
        }

        _dbContext.SaveChanges();
    }




    
    public void SetToRead(int id)
    {
        var notification = _dbContext.Notifications.FirstOrDefault(not => not.Id == id);
        if (notification == null)
        {
            throw new ArgumentException("Értesítés nem található.");
        }

        notification.Read = !notification.Read;
        _dbContext.SaveChanges();
    }


    public void SetToOfficiallyRead(int id)
    {
        var notification = _dbContext.Notifications.FirstOrDefault(n => n.Id == id);
    
        if (notification == null)
        {
            throw new ArgumentException("Értesítés nem található.");
        }

        notification.OfficiallyRead = true;
    
        _dbContext.SaveChanges();
    }

    

    
    
    public IEnumerable<NotificationBase> GetHomeworks()
    {
        return _dbContext.Notifications.Where(n => n.Type == "Homework").ToList();
    }
    
    public IEnumerable<NotificationBase> GetOthers()
    {
        return _dbContext.Notifications.Where(n => n.Type == "Other").ToList();
    }
    
    public IEnumerable<NotificationBase> GetMissingEquipments()
    {
        return _dbContext.Notifications.Where(n => n.Type == "MissingEquipment").ToList();
    }
    
    public IEnumerable<NotificationBase> GetExams()
    {
        return _dbContext.Notifications.Where(n => n.Type == "Exam").ToList();
    }


    public void Delete(int id)
    {
        var notification = _dbContext.Notifications.FirstOrDefault(n => n.Id == id);
    
        if (notification == null)
        {
            throw new KeyNotFoundException($"Notification with ID {id} not found.");
        }

        _dbContext.Notifications.Remove(notification);
        _dbContext.SaveChanges();
    }

}