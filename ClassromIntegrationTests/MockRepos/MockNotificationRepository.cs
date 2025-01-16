using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Classroom.Service.Repositories;

namespace ClassromIntegrationTests.MockRepos;

public class MockNotificationRepository : INotificationRepository
{
    public IEnumerable<NotificationBase> GetAll() {
        throw new Exception("Mock exception for testing.");
    }
    public void Add(NotificationRequest request) {
        throw new Exception("Mock exception for testing.");
    }
    public IEnumerable<NotificationBase> GetByStudentId(string studentId, string parentId) {
        throw new Exception("Mock exception for testing.");
    }
    public void SetToRead(int id) {
        throw new Exception("Mock exception for testing.");
    }
    public IEnumerable<NotificationBase> GetHomeworks() {
        throw new Exception("Mock exception for testing.");
    }
    public IEnumerable<NotificationBase> GetOthers() {
        throw new Exception("Mock exception for testing.");
    }
    public IEnumerable<NotificationBase> GetMissingEquipments() {
        throw new Exception("Mock exception for testing.");
    }
    public IEnumerable<NotificationBase> GetExams() {
        throw new Exception("Mock exception for testing.");
    }
    public void Delete(int id) {
        throw new Exception("Mock exception for testing.");
    }
    public IEnumerable<NotificationBase> GetLastsByStudentId(string studentId, string parentId) {
        throw new Exception("Mock exception for testing.");
    }
    public IEnumerable<NotificationResponse> GetLastsByTeacherId(string id) {
        throw new Exception("Mock exception for testing.");
    }
    public void SetToOfficiallyRead(int id) {
        throw new Exception("Mock exception for testing.");
    }
    public IEnumerable<NotificationBase> GetByTeacherId(string id) {
        throw new Exception("Mock exception for testing.");
    }
    public IEnumerable<NotificationBase> GetNewNotifsByStudentId(string studentId, string parentId) {
        throw new Exception("Mock exception for testing.");
    }
    public int GetNewNotifsNumber(string studentId, string parentId) {
        throw new Exception("Mock exception for testing.");
    }
    public NotificationBase? GetNewestByTeacherId(string teacherId) {
        throw new Exception("Mock exception for testing.");
    }
}