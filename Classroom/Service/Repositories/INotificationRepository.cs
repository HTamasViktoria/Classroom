using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Classroom.Model.ResponseModels;

namespace Classroom.Service.Repositories
{
    public interface INotificationRepository
    {
        IEnumerable<NotificationBase> GetAll();
        void Add(NotificationRequest request);
        IEnumerable<NotificationBase> GetByStudentId(string studentId, string parentId);
        void SetToRead(int id);
        IEnumerable<NotificationBase> GetHomeworks();
        IEnumerable<NotificationBase> GetOthers();
        IEnumerable<NotificationBase> GetMissingEquipments();
        IEnumerable<NotificationBase> GetExams();
        void Delete(int id);
        IEnumerable<NotificationBase> GetLastsByStudentId(string studentId, string parentId);
        IEnumerable<NotificationResponse> GetLastsByTeacherId(string id);
        void SetToOfficiallyRead(int id);
        IEnumerable<NotificationBase> GetByTeacherId(string id);
        IEnumerable<NotificationBase> GetNewNotifsByStudentId(string studentId, string parentId);
        int GetNewNotifsNumber(string studentId, string parentId);
        NotificationBase? GetNewestByTeacherId(string teacherId);
    }
}