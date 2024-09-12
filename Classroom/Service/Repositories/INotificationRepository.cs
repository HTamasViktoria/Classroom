using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;

namespace Classroom.Service.Repositories;

public interface INotificationRepository
{
    IEnumerable<NotificationBase> GetAll();
    void Add(NotificationRequest request);
}