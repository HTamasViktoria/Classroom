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
        return _dbContext.Messages.Where(m => (m.Receiver.Id == id) && (m.DeletedByReceiver==false)).ToList();
    }
    
    
    public IEnumerable<Message> GetSents(string id)
    {
        return _dbContext.Messages.Where(m => (m.Sender.Id == id) && (m.DeletedBySender == false)).ToList();
    }
    
    
    public IEnumerable<Message> GetDeleteds(string id)
    {
        return _dbContext.Messages.Where(m => ((m.Receiver.Id == id && m.DeletedByReceiver == true))
                                              || (m.Sender.Id == id && m.DeletedBySender == true)).ToList();
    }

    public IEnumerable<Message> GetOutgoings(string id)
    {
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
            return false;
        }
        
        message.DeletedByReceiver = true;
        _dbContext.SaveChanges();
        return true;
    }
    
    
    public IEnumerable<Message> GetAllMessages()
    {
        return _dbContext.Messages.ToList();
    }
    
    
   
    
   
    public bool Restore(int messageId, string userId)
    {
        var message = _dbContext.Messages
            .Include(m => m.Receiver)
            .Include(m => m.Sender)
            .FirstOrDefault(m => m.Id == messageId);

        if (message == null)
        {
            return false;
        }
        
        if (message.DeletedByReceiver == true && message.Receiver.Id == userId)
        {
            message.DeletedByReceiver = false;
            _dbContext.SaveChanges();
        }
        if (message.DeletedBySender == true && message.Sender.Id == userId)
        {
            message.DeletedBySender = false;
            _dbContext.SaveChanges();
        }

        return true;
    }
    
    
    public int GetNewMessagesNum(string userId)
    {
      
        var newMessages = _dbContext.Messages
            .Where(m => m.Receiver.Id == userId && m.Read == false)
            .ToList();

        return newMessages.Count();
    }


    
    public Message GetById(int id)
    {
        return _dbContext.Messages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .FirstOrDefault(m => m.Id == id);
    }
    
    public bool SetToUnread(int messageId)
    {
        
        var message = _dbContext.Messages.FirstOrDefault(m => m.Id == messageId);
    
        if (message == null)
        {
            return false;
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
            return false;
        }

        message.Read = true;
        _dbContext.SaveChanges();
        return true;
    }



    public void AddMessage(MessageRequest request)
    {
        var sender = GetUserById(request.FromId);

        if (sender == null)
        {
            throw new ArgumentException("A küldő felhasználó nem található.");
        }

        foreach (var receiverId in request.ReceiverIds)
        {
            var receiver = GetUserById(receiverId);

            if (receiver == null)
            {
                receiver = _dbContext.Parents.FirstOrDefault(p => p.Id == receiverId);
            }
            
            if (receiver == null)
            {
                throw new ArgumentException($"A címzett ({receiverId}) nem található.");
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
        }

        _dbContext.SaveChanges();
    }



}