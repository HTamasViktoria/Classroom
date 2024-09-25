using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;

namespace Classroom.Service.Repositories;

public interface INotificationRepository
{
    IEnumerable<NotificationBase> GetAll();
    void Add(NotificationRequest request);
    IEnumerable<NotificationBase> GetByStudentId(int id);
    void SetToRead(int id);
    IEnumerable<NotificationBase> GetHomeworks();
    void Delete(int id);
}