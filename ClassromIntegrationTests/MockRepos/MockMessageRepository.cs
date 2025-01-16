using Classroom.Service.Repositories;
using Classroom.Data;
using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;

namespace ClassromIntegrationTests.MockRepos;

public class MockMessageRepository : IMessagesRepository
{
   public  IEnumerable<Message> GetIncomings(string id)   {
       throw new Exception("Mock exception for testing.");
   }

   public  IEnumerable<Message> GetOutgoings(string id)   {
       throw new Exception("Mock exception for testing.");
   }

    public void AddMessage(MessageRequest request)   {
        throw new Exception("Mock exception for testing.");
    }

   public  bool DeleteOnReceiverSide(int messageId)   {
       throw new Exception("Mock exception for testing.");
   }

    public IEnumerable<Message> GetDeleteds(string id)   {
        throw new Exception("Mock exception for testing.");
    }

    public IEnumerable<Message> GetSents(string id)   {
        throw new Exception("Mock exception for testing.");
    }

   public  bool Restore(int messageId, string userId)   {
       throw new Exception("Mock exception for testing.");
   }

   public  bool SetToUnread(int messageId)   {
       throw new Exception("Mock exception for testing.");
   }

   public bool SetToRead(int messageId)   {
       throw new Exception("Mock exception for testing.");
   }

  public  Task<IEnumerable<Message>> GetAllMessagesAsync()   {
      throw new Exception("Mock exception for testing.");
  }

    public int GetNewMessagesNum(string userId)   {
        throw new Exception("Mock exception for testing.");
    }

  public  Message GetById(int id)   {
      throw new Exception("Mock exception for testing.");
  }

}