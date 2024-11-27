using Classroom.Model.RequestModels;

namespace Classroom.Service;

public interface INotificationService
{
    void PostToDb(NotificationRequest request);
}