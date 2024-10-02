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
    IEnumerable<NotificationBase> GetOthers();
    IEnumerable<NotificationBase> GetMissingEquipments();
    IEnumerable<NotificationBase> GetExams();
    
    void Delete(int id);
    IEnumerable<NotificationBase> GetLastsByStudentId(int id);
    
}