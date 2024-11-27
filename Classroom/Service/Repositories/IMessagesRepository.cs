using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;

namespace Classroom.Service.Repositories;

public interface IMessagesRepository
{
    IEnumerable<Message> GetIncomings(string id);
    IEnumerable<Message> GetOutgoings(string id);
    void AddMessage(MessageRequest request);
    bool DeleteOnReceiverSide(int messageId);
    IEnumerable<Message> GetDeleteds(string id);
    IEnumerable<Message> GetSents(string id);
    bool Restore(int messageId, string userId);
    bool SetToUnread(int messageId);
    bool SetToRead(int messageId);
    IEnumerable<Message> GetAllMessages();
    int GetNewMessagesNum(string userId);
    Message GetById(int id);
}