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


    public IEnumerable<NotificationBase> GetByStudentId(int id)
    {
        return _dbContext.Notifications
            .Where(notification => notification.Students.Any(student => student.Id == id));
    }



    public void Add(NotificationRequest request)
    {
        var students = _dbContext.Students
            .Where(s => request.StudentIds.Contains(s.Id))
            .ToList();

        if (request.Type != "OtherNotification")
        {
            if (string.IsNullOrEmpty(request.Subject))
            {
                throw new ArgumentException("Subject is required when type is not 'OtherNotification'.");
            }

            if (!Enum.TryParse<Subjects>(request.Subject, out var subjectEnum))
            {
                throw new ArgumentException($"Invalid subject value: {request.Subject}");
            }

            var notification = new NotificationBase
            {
                TeacherId = request.TeacherId,
                TeacherName = request.TeacherName,
                Type = request.Type,
                Date = request.Date,
                Students = students,
                Read = request.Read,
                Description = request.Description,
                SubjectName = request.Subject,
                Subject = subjectEnum,
                OptionalDescription = request.OptionalDescription
            };

            _dbContext.Notifications.Add(notification);
            _dbContext.SaveChanges();
        }


    }
}