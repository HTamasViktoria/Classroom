using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Classroom.Model.ResponseModels;

namespace Classroom.Service.Repositories
{
    public interface INotificationRepository
    {
        IEnumerable<NotificationBase> GetAll();
        void Add(NotificationRequest request);
        IEnumerable<NotificationBase> GetByStudentId(string id);
        void SetToRead(int id);
        IEnumerable<NotificationBase> GetHomeworks();
        IEnumerable<NotificationBase> GetOthers();
        IEnumerable<NotificationBase> GetMissingEquipments();
        IEnumerable<NotificationBase> GetExams();
        void Delete(int id);
        IEnumerable<NotificationBase> GetLastsByStudentId(string id);
        IEnumerable<NotificationResponse> GetLastsByTeacherId(string id);
        void SetToOfficiallyRead(int id);
        IEnumerable<NotificationBase> GetByTeacherId(string id);
        IEnumerable<NotificationBase> GetNewNotifsByStudentId(string id);
        int GetNewNotifsNumber(string id);
    }
}