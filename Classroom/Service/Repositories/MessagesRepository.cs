using Classroom.Data;
using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Microsoft.EntityFrameworkCore;

namespace Classroom.Service.Repositories;

public class MessagesRepository : IMessagesRepository
{
    private ClassroomContext _dbContext;

    public MessagesRepository(ClassroomContext context)
    {
        _dbContext = context;
    }
    
  

    public IEnumerable<Message> GetIncomings(string id)
    {
        ValidateUser(id);
        return _dbContext.Messages.Where(m => (m.Receiver.Id == id) && (m.DeletedByReceiver==false)).ToList();
    }
    
    
    public IEnumerable<Message> GetSents(string id)
    {
        ValidateUser(id);
        return _dbContext.Messages.Where(m => (m.Sender.Id == id) && (m.DeletedBySender == false)).ToList();
    }
    
    
    public IEnumerable<Message> GetDeleteds(string id)
    {
        ValidateUser(id);
        return _dbContext.Messages.Where(m => ((m.Receiver.Id == id && m.DeletedByReceiver == true))
                                              || (m.Sender.Id == id && m.DeletedBySender == true)).ToList();
    }

    public IEnumerable<Message> GetOutgoings(string id)
    {
        ValidateUser(id);
        return _dbContext.Messages.Where(m => m.Sender.Id == id).ToList();
    }

  

    public User GetUserById(string id)
    {
        var teacher = _dbContext.Teachers.FirstOrDefault(t => t.Id == id);
        if (teacher != null)
        {
            return teacher;
        }
        var parent = _dbContext.Parents.FirstOrDefault(p => p.Id == id);
        if (parent != null)
        {
            return parent;
        }
        return null;
    }

    
    public bool DeleteOnReceiverSide(int messageId)
    {
        var message = _dbContext.Messages.FirstOrDefault(m => m.Id == messageId);

        if (message == null)
        {
            throw new ArgumentException($"Üzenet nem található a következő ID-val: {messageId}");
        }

        message.DeletedByReceiver = true;
        _dbContext.SaveChanges();
        return true;
    }

    
    
    public async Task<IEnumerable<Message>> GetAllMessagesAsync()
    {
        return await _dbContext.Messages.ToListAsync();
    }
    
   
    
   
    public bool Restore(int messageId, string userId)
    {
        ValidateUser(userId);

        var message = _dbContext.Messages
            .Include(m => m.Receiver)
            .Include(m => m.Sender)
            .FirstOrDefault(m => m.Id == messageId);

        if (message == null)
        {
            throw new ArgumentException($"Az üzenet ({messageId}) nem található.");
        }

        if (message.DeletedByReceiver && message.Receiver.Id == userId)
        {
            message.DeletedByReceiver = false;
            _dbContext.SaveChanges();
        }
        else if (message.DeletedBySender && message.Sender.Id == userId)
        {
            message.DeletedBySender = false;
            _dbContext.SaveChanges();
        }

        return true;
    }


    
    public int GetNewMessagesNum(string userId)
    {
        ValidateUser(userId);
        
        var newMessagesCount = _dbContext.Messages
            .Count(m => m.Receiver.Id == userId && m.Read == false);

        return newMessagesCount;
    }


    public Message GetById(int id)
    {
        var message = _dbContext.Messages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .FirstOrDefault(m => m.Id == id);

        if (message == null)
        {
            throw new ArgumentException($"Message with ID {id} not found.");
        }

        return message;
    }

    
    
    public bool SetToUnread(int messageId)
    {
        
        var message = _dbContext.Messages.FirstOrDefault(m => m.Id == messageId);
    
        if (message == null)
        {
            throw new ArgumentException($"Message with ID {messageId} not found.");
        }

        message.Read = false;
        _dbContext.SaveChanges();
        return true;
    }
    
    
    public bool SetToRead(int messageId)
    {
        
        var message = _dbContext.Messages.FirstOrDefault(m => m.Id == messageId);
    
        if (message == null)
        {
            throw new ArgumentException($"Message with ID {messageId} not found.");
        }

        message.Read = true;
        _dbContext.SaveChanges();
        return true;
    }



    public void AddMessage(MessageRequest request)
    {
        ValidateUser(request.FromId);
        var sender = GetUserById(request.FromId);
        var failedReceivers = new List<string>();
        var successfulMessages = new List<Message>();

        foreach (var receiverId in request.ReceiverIds)
        {
            var receiver = GetUserById(receiverId);

            if (receiver == null)
            {
                failedReceivers.Add(receiverId);
                continue;
            }

            var message = new Message
            {
                Date = request.Date,
                Sender = sender,
                SenderName = $"{sender.FirstName} {sender.FamilyName}",
                Receiver = receiver,
                ReceiverName = $"{receiver.FirstName} {receiver.FamilyName}",
                HeadText = request.HeadText,
                Text = request.Text,
                Read = false,
                DeletedBySender = false,
                DeletedByReceiver = false
            };

            _dbContext.Messages.Add(message);
            successfulMessages.Add(message);
        }
        
        if (failedReceivers.Any())
        {
            var failedReceiverList = string.Join(", ", failedReceivers);
            throw new ArgumentException($"Az üzenet küldése a következő felhasználók számára nem sikerült: {failedReceiverList}");
        }

        _dbContext.SaveChanges();
    }


    
    public void ValidateUser(string userId)
    {
        var userExists = _dbContext.Users.Any(u => u.Id == userId);
        if (!userExists)
        {
            throw new ArgumentException($"User with ID {userId} not found.");
        }
    }

}