using System.Net;
using System.Text;
using System.Text.Json;
using Classroom.Data;
using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Classroom.Service.Repositories;
using Microsoft.Extensions.DependencyInjection;
using ClassromIntegrationTests.MockRepos;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ClassromIntegrationTests;

public class MessageControllerTests:IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly HttpClient _mockClient;
    private readonly HttpClient _usersClient;

    public MessageControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();

        var mockFactory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddTransient<IMessagesRepository, MockMessageRepository>();
            });
        });

        _mockClient = mockFactory.CreateClient();
        
        
        var usersMock = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddTransient<IUserRepository, UserRepository>();
            });
        });

        _usersClient = mockFactory.CreateClient();
    }


    [Fact]
    public async Task GetAllMessagesAsync_EmptyList_ReturnsEmptyList()
    {
        await ClearDatabaseAsync();
        await AddUsersWithoutMessages();

        var response = await _client.GetAsync("/api/messages/getall");

        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var messages = JsonSerializer.Deserialize<List<Message>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.Empty(messages);
    }



    [Fact]
    public async Task GetAllMessagesAsync_ReturnsMessages_IfThereAreAny()
    {
        await ClearDatabaseAsync();
        await AddUsersWithMessages();

        var response = await _client.GetAsync("/api/messages/getall");

        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var messages = JsonSerializer.Deserialize<List<Message>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotEmpty(messages);
        Assert.Equal(2, messages.Count);
       
    }



    [Fact]
    public async Task GetAllMessagesAsync_InternalServerError_Returns500()
    {
        var response = await _mockClient.GetAsync("/api/messages/getall");

        Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("Internal server error", responseString);
    }

    
    [Fact]
    public async Task GetNewMessagesNum_HappyPath_ReturnsNewMessagesCount()
    {
        await ClearDatabaseAsync();
        await AddUsersWithMessages();
        
        var getAllParentsResponse = await _client.GetAsync("/api/users/allparents");
        getAllParentsResponse.EnsureSuccessStatusCode();

        var responseString = await getAllParentsResponse.Content.ReadAsStringAsync();
        var parents = JsonSerializer.Deserialize<List<Parent>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        var id = parents.First().Id;
        
        var newMessagesResponse = await _client.GetAsync($"/api/messages/newmessagesnum/{id}");

        newMessagesResponse.EnsureSuccessStatusCode();
        var newMessagesCount = int.Parse(await newMessagesResponse.Content.ReadAsStringAsync());

        Assert.Equal(1, newMessagesCount);
    }


    
    [Fact]
    public async Task GetNewMessagesNum_InvalidUserId_ReturnsBadRequest()
    {
        await ClearDatabaseAsync();
        await AddUsersWithMessages();
        
        var invalidId = "invalid-user-id";
        
        var newMessagesResponse = await _client.GetAsync($"/api/messages/newmessagesnum/{invalidId}");
        
        Assert.Equal(System.Net.HttpStatusCode.NotFound, newMessagesResponse.StatusCode);
    }


    [Fact]
    public async Task GetNewMessagesNum_MockRepository_ThrowsException_ReturnsInternalServerError()
    {
        await ClearDatabaseAsync();
        await AddUsersWithMessages();
        
        var getAllParentsResponse = await _client.GetAsync("/api/users/allparents");
        getAllParentsResponse.EnsureSuccessStatusCode();

        var responseString = await getAllParentsResponse.Content.ReadAsStringAsync();
        var parents = JsonSerializer.Deserialize<List<Parent>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    
        var id = parents.First().Id;
      
        var newMessagesResponse = await _mockClient.GetAsync($"/api/messages/newmessagesnum/{id}");

        Assert.Equal(System.Net.HttpStatusCode.InternalServerError, newMessagesResponse.StatusCode);
    }


    [Fact]
    public async Task GetById_HappyPath_ReturnsCorrectMessage()
    {
        await ClearDatabaseAsync();
        await AddUsersWithMessages();

        var getAllMessagesResponse = await _client.GetAsync("/api/messages/getall");
        getAllMessagesResponse.EnsureSuccessStatusCode();

        var responseString = await getAllMessagesResponse.Content.ReadAsStringAsync();
        var messages = JsonSerializer.Deserialize<List<Message>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var messageId = messages.First().Id;

        var getMessageResponse = await _client.GetAsync($"/api/messages/{messageId}");
        getMessageResponse.EnsureSuccessStatusCode();

        var messageResponseString = await getMessageResponse.Content.ReadAsStringAsync();
        var message = JsonSerializer.Deserialize<Message>(messageResponseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (message.HeadText == "Message 1")
        {
            Assert.Equal("This is a message from parent to teacher.", message.Text);
        }
        else
        {
            throw new Exception("Message HeadText doesn't match expected value.");
        }
    }

    
    
    [Fact]
    public async Task GetById_InvalidMessageId_ReturnsBadRequest()
    {
        await ClearDatabaseAsync();
        await AddUsersWithMessages();

        var invalidMessageId = 9999;

        var getMessageResponse = await _client.GetAsync($"/api/messages/{invalidMessageId}");

        Assert.Equal(404, (int)getMessageResponse.StatusCode);

        var errorMessage = await getMessageResponse.Content.ReadAsStringAsync();
    
        Assert.Contains("Not found", errorMessage);
    }



    [Fact]
    public async Task GetById_ValidMessageId_InternalServerError()
    {
        await ClearDatabaseAsync();
        await AddUsersWithMessages();

        var getAllMessagesResponse = await _client.GetAsync("/api/messages/getall");
        getAllMessagesResponse.EnsureSuccessStatusCode();

        var responseString = await getAllMessagesResponse.Content.ReadAsStringAsync();
        var messages = JsonSerializer.Deserialize<List<Message>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var messageId = messages.First().Id;

        var getMessageResponse = await _mockClient.GetAsync($"/api/messages/{messageId}");

        Assert.Equal(500, (int)getMessageResponse.StatusCode);

        var errorMessage = await getMessageResponse.Content.ReadAsStringAsync();
    
        Assert.Contains("Internal server error", errorMessage);
    }

    
    
    [Fact]
    public async Task GetIncomings_HappyPath_ReturnsIncomings()
    {
        await ClearDatabaseAsync();
        await AddUsersWithMessages();

        var getAllParentsResponse = await _client.GetAsync("/api/users/allparents");
        getAllParentsResponse.EnsureSuccessStatusCode();

        var responseString = await getAllParentsResponse.Content.ReadAsStringAsync();
        var parents = JsonSerializer.Deserialize<List<Parent>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var id = parents.First().Id;

        var getIncomingsResponse = await _client.GetAsync($"/api/messages/incomings/{id}");
        getIncomingsResponse.EnsureSuccessStatusCode();

        var incomingsResponseString = await getIncomingsResponse.Content.ReadAsStringAsync();
        var incomings = JsonSerializer.Deserialize<List<Message>>(incomingsResponseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.Single(incomings);
    }


    [Fact]
    public async Task GetIncomings_NoMessages_ReturnsEmptyList()
    {
        await ClearDatabaseAsync();
        await AddUsersWithoutMessages();

        var getAllParentsResponse = await _client.GetAsync("/api/users/allparents");
        getAllParentsResponse.EnsureSuccessStatusCode();

        var responseString = await getAllParentsResponse.Content.ReadAsStringAsync();
        var parents = JsonSerializer.Deserialize<List<Parent>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var id = parents.First().Id;

        var getIncomingsResponse = await _client.GetAsync($"/api/messages/incomings/{id}");
        getIncomingsResponse.EnsureSuccessStatusCode();

        var incomingsResponseString = await getIncomingsResponse.Content.ReadAsStringAsync();
        var incomings = JsonSerializer.Deserialize<List<Message>>(incomingsResponseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.Empty(incomings);
    }

    
    
    [Fact]
    public async Task GetIncomings_InvalidId_ReturnsBadRequest()
    {
        var invalidId = "invalid-id";

        var getIncomingsResponse = await _client.GetAsync($"/api/messages/incomings/{invalidId}");
        var responseString = await getIncomingsResponse.Content.ReadAsStringAsync();

        Assert.Equal(400, (int)getIncomingsResponse.StatusCode);
        Assert.Contains("Not found", responseString);
    }

    [Fact]
    public async Task GetIncomings_ReturnsInternalServerError_WhenExceptionIsThrown()
    {
        await ClearDatabaseAsync();
        await AddUsersWithMessages();

        var getAllParentsResponse = await _client.GetAsync("/api/users/allparents");
        getAllParentsResponse.EnsureSuccessStatusCode();

        var responseString = await getAllParentsResponse.Content.ReadAsStringAsync();
        var parents = JsonSerializer.Deserialize<List<Parent>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var id = parents.First().Id;
   
        var getIncomingsResponse = await _mockClient.GetAsync($"/api/messages/incomings/{id}");

        Assert.Equal(500, (int)getIncomingsResponse.StatusCode);
        var responseString2 = await getIncomingsResponse.Content.ReadAsStringAsync();
    
        Assert.Contains("Internal server error", responseString2);
    }



    
    [Fact]
    public async Task GetDeleteds_ReturnsEmptyList_WhenNoMessagesExist()
    {
        await ClearDatabaseAsync();
        await AddUsersWithoutMessages();

        var getAllParentsResponse = await _client.GetAsync("/api/users/allparents");
        getAllParentsResponse.EnsureSuccessStatusCode();

        var responseString = await getAllParentsResponse.Content.ReadAsStringAsync();
        var parents = JsonSerializer.Deserialize<List<Parent>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var id = parents.First().Id;

        var getDeletedsResponse = await _client.GetAsync($"/api/messages/deleteds/{id}");
        getDeletedsResponse.EnsureSuccessStatusCode();

        var responseString2 = await getDeletedsResponse.Content.ReadAsStringAsync();
        var messages = JsonSerializer.Deserialize<List<Message>>(responseString2, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.Empty(messages);
    }

    
    
    [Fact]
    public async Task GetDeleteds_ReturnsDeletedMessage_WhenMessageIsDeletedByReceiver()
    {
        await ClearDatabaseAsync();
        await AddDeletedMessage();
        var getAllParentsResponse = await _usersClient.GetAsync("/api/users/allparents");
        getAllParentsResponse.EnsureSuccessStatusCode();

        var responseString = await getAllParentsResponse.Content.ReadAsStringAsync();
        var parents = JsonSerializer.Deserialize<List<Parent>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var id = parents.First().Id;

        var getDeletedsResponse = await _client.GetAsync($"/api/messages/deleteds/{id}");
        getDeletedsResponse.EnsureSuccessStatusCode();

        var responseString2 = await getDeletedsResponse.Content.ReadAsStringAsync();
        var deletedMessages = JsonSerializer.Deserialize<List<Message>>(responseString2, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var deletedMessage = deletedMessages.First();
        
        Assert.True(deletedMessage.DeletedByReceiver);
    }


    [Fact]
    public async Task GetDeleteds_ReturnsBadRequest_WhenInvalidIdIsProvided()
    {
        await ClearDatabaseAsync();
        await AddDeletedMessage();

        var invalidId = "invalid_id";

        var getDeletedsResponse = await _client.GetAsync($"/api/messages/deleteds/{invalidId}");
    
        Assert.Equal(400, (int)getDeletedsResponse.StatusCode);
        var responseString = await getDeletedsResponse.Content.ReadAsStringAsync();
        Assert.Contains("Not found", responseString);
    }

    [Fact]
    public async Task GetDeleteds_ReturnsInternalServerError_WhenExceptionIsThrown()
    {
        await ClearDatabaseAsync();
        await AddDeletedMessage();
        var getAllParentsResponse = await _usersClient.GetAsync("/api/users/allparents");
        getAllParentsResponse.EnsureSuccessStatusCode();

        var responseString = await getAllParentsResponse.Content.ReadAsStringAsync();
        var parents = JsonSerializer.Deserialize<List<Parent>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var id = parents.First().Id;

        var getDeletedsResponse = await _mockClient.GetAsync($"/api/messages/deleteds/{id}");
    
        Assert.Equal(500, (int)getDeletedsResponse.StatusCode);
        var responseString2 = await getDeletedsResponse.Content.ReadAsStringAsync();
        Assert.Contains("Internal server error", responseString2);
    }
    
    
    [Fact]
    public async Task PostMessage_ReturnsCreatedAtActionResult()
    {
        await ClearDatabaseAsync();
        await AddUsersWithoutMessages();

        var usersResponse = await _usersClient.GetAsync("/api/users/allparents");
        usersResponse.EnsureSuccessStatusCode();
        var parents = JsonConvert.DeserializeObject<List<Parent>>(await usersResponse.Content.ReadAsStringAsync());

        usersResponse = await _usersClient.GetAsync("/api/users/allteachers");
        usersResponse.EnsureSuccessStatusCode();
        var teachers = JsonConvert.DeserializeObject<List<Teacher>>(await usersResponse.Content.ReadAsStringAsync());

        var messageRequest = new MessageRequest
        {
            
            Date = DateTime.UtcNow,
            FromId = teachers.First().Id,
            ReceiverIds = new List<string> { parents.First().Id },
            HeadText = "Meeting Reminder",
            Text = "Don't forget about the meeting tomorrow at 10 AM."
        };

        var content = new StringContent(JsonConvert.SerializeObject(messageRequest), Encoding.UTF8, "application/json");

     
        var response = await _client.PostAsync("/api/messages", content);

      
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("Üzenet sikeresen elmentve az adatbázisba.", responseString);
    }


    [Fact]
    public async Task PostMessage_WithInvalidFromId_ReturnsBadRequest()
    {
        await ClearDatabaseAsync();
        await AddUsersWithoutMessages();

        var usersResponse = await _usersClient.GetAsync("/api/users/allparents");
        usersResponse.EnsureSuccessStatusCode();
        var parents = JsonConvert.DeserializeObject<List<Parent>>(await usersResponse.Content.ReadAsStringAsync());
        
        var invalidFromId = "invalid_teacher_id";

        var messageRequest = new MessageRequest
        {
            Date = DateTime.UtcNow,
            FromId = invalidFromId,
            ReceiverIds = new List<string> { parents.First().Id },
            HeadText = "Meeting Reminder",
            Text = "Don't forget about the meeting tomorrow at 10 AM."
        };

        var content = new StringContent(JsonConvert.SerializeObject(messageRequest), Encoding.UTF8, "application/json");

     
        var response = await _client.PostAsync("/api/messages", content);

       
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("Bad request", responseString);
    }


    
    [Fact]
    public async Task PostMessage_WithMockRepository_ReturnsInternalServerError()
    {
        await ClearDatabaseAsync();
        await AddUsersWithoutMessages();

        var usersResponse = await _usersClient.GetAsync("/api/users/allparents");
        usersResponse.EnsureSuccessStatusCode();
        var parents = JsonConvert.DeserializeObject<List<Parent>>(await usersResponse.Content.ReadAsStringAsync());

        usersResponse = await _usersClient.GetAsync("/api/users/allteachers");
        usersResponse.EnsureSuccessStatusCode();
        var teachers = JsonConvert.DeserializeObject<List<Teacher>>(await usersResponse.Content.ReadAsStringAsync());

        var messageRequest = new MessageRequest
        {
            Date = DateTime.UtcNow,
            FromId = teachers.First().Id,
            ReceiverIds = new List<string> { parents.First().Id },
            HeadText = "Meeting Reminder",
            Text = "Don't forget about the meeting tomorrow at 10 AM."
        };

        var content = new StringContent(JsonConvert.SerializeObject(messageRequest), Encoding.UTF8, "application/json");

 
        var response = await _mockClient.PostAsync("/api/messages", content);

    
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("Internal server error", responseString);
    }

   
    
    [Fact]
    public async Task SetToRead_ReturnsSuccess()
    {
        await ClearDatabaseAsync();
        await AddUsersWithMessages();

   
        var messagesResponse = await _client.GetAsync("/api/messages/getall");
        messagesResponse.EnsureSuccessStatusCode();
        var messages = JsonConvert.DeserializeObject<List<Message>>(await messagesResponse.Content.ReadAsStringAsync());

        var messageId = messages.First().Id;

        var response = await _client.GetAsync($"/api/messages/read/{messageId}");

 
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("Üzenet sikeresen olvasottra állítva.", responseString);

      
        messagesResponse = await _client.GetAsync("/api/messages/getall");
        messagesResponse.EnsureSuccessStatusCode();
        messages = JsonConvert.DeserializeObject<List<Message>>(await messagesResponse.Content.ReadAsStringAsync());

        var updatedMessage = messages.First(m => m.Id == messageId);
        Assert.True(updatedMessage.Read);
    }
    
    
    
    [Fact]
    public async Task SetToRead_InvalidMessageId_ReturnsBadRequest()
    {
        await ClearDatabaseAsync();
        await AddUsersWithMessages();

        var invalidMessageId = -1;

        var response = await _client.GetAsync($"/api/messages/read/{invalidMessageId}");

 
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("Bad request", responseString);
    }


    
    [Fact]
    public async Task SetToRead_InternalServerError_WhenExceptionIsThrown()
    {
        await ClearDatabaseAsync();
        await AddUsersWithMessages();

      
        var messagesResponse = await _client.GetAsync("/api/messages/getall");
        messagesResponse.EnsureSuccessStatusCode();
        var messages = JsonConvert.DeserializeObject<List<Message>>(await messagesResponse.Content.ReadAsStringAsync());

        var messageId = messages.First().Id;

   
        var response = await _mockClient.GetAsync($"/api/messages/read/{messageId}");

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("Internal server error", responseString);
    }


    
    [Fact]
    public async Task SetToUnread_ValidMessageId_SetsMessageToUnread()
    {
        await ClearDatabaseAsync();
        await AddUsersWithReadMessage();

    
        var messagesResponse = await _client.GetAsync("/api/messages/getall");
        messagesResponse.EnsureSuccessStatusCode();
        var messages = JsonConvert.DeserializeObject<List<Message>>(await messagesResponse.Content.ReadAsStringAsync());

        var messageId = messages.First().Id;

    
        var response = await _client.PostAsync($"/api/messages/unread/{messageId}", null);

     
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("Üzenet sikeresen olvasatlanra állítva.", responseString);

      
        messagesResponse = await _client.GetAsync("/api/messages/getall");
        messagesResponse.EnsureSuccessStatusCode();
        messages = JsonConvert.DeserializeObject<List<Message>>(await messagesResponse.Content.ReadAsStringAsync());

        var updatedMessage = messages.First(m => m.Id == messageId);
        Assert.False(updatedMessage.Read);
    }

    
    [Fact]
    public async Task SetToUnread_MessageWithoutReadFlag_ReturnsBadRequest()
    {
        await ClearDatabaseAsync();
        await AddUsersWithMessages();

     
        var messagesResponse = await _client.GetAsync("/api/messages/getall");
        messagesResponse.EnsureSuccessStatusCode();
        var messages = JsonConvert.DeserializeObject<List<Message>>(await messagesResponse.Content.ReadAsStringAsync());

      
        var messageId = messages.First().Id;

    
        var response = await _client.PostAsync($"/api/messages/unread/{messageId}", null);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("Bad request", responseString);
    }

    
    
    [Fact]
    public async Task SetToUnread_InvalidMessageId_ReturnsBadRequest()
    {
        await ClearDatabaseAsync();

       
        var invalidMessageId = 999999;
        var response = await _client.PostAsync($"/api/messages/unread/{invalidMessageId}", null);

       
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("Bad request", responseString);
    }

    [Fact]
    public async Task SetToUnread_InternalServerError_ReturnsInternalServerError()
    {
        await ClearDatabaseAsync();
        await AddUsersWithReadMessage();

      
        var messagesResponse = await _client.GetAsync("/api/messages/getall");
    
        
        if (!messagesResponse.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to retrieve messages. Status Code: {messagesResponse.StatusCode}");
        }

        var messages = JsonConvert.DeserializeObject<List<Message>>(await messagesResponse.Content.ReadAsStringAsync());

        
        var messageId = messages.First().Id;

     
        var response = await _mockClient.PostAsync($"/api/messages/unread/{messageId}", null);

     
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("Internal server error", responseString);
    }

    
    
    [Fact]
    public async Task DeleteMessageOnReceiverSide_Success_ReturnsOkAndMessageDeleted()
    {
        await ClearDatabaseAsync();
        await AddUsersWithReadMessage();

        var messagesResponse = await _client.GetAsync("/api/messages/getall");
        messagesResponse.EnsureSuccessStatusCode();

        var messagesContent = await messagesResponse.Content.ReadAsStringAsync();
        var messages = JsonConvert.DeserializeObject<List<Message>>(messagesContent);
        
        Assert.NotNull(messages);
        Assert.NotEmpty(messages);  
        var messageId = messages.First().Id;
        var response = await _client.DeleteAsync($"/api/messages/receiverdelete/{messageId}");
        response.EnsureSuccessStatusCode();

        var updatedMessagesResponse = await _client.GetAsync("/api/messages/getall");
        updatedMessagesResponse.EnsureSuccessStatusCode();
    
        var updatedMessages = JsonConvert.DeserializeObject<List<Message>>(await updatedMessagesResponse.Content.ReadAsStringAsync());

        var updatedMessage = updatedMessages.First(m => m.Id == messageId);
        Assert.True(updatedMessage.DeletedByReceiver, "A DeletedByReceiver érték nem true.");
    }

    
    [Fact]
    public async Task DeleteMessageOnReceiverSide_InvalidMessageId_ReturnsBadRequest()
    {
      
        var invalidMessageId = -1; 

        
        var response = await _client.DeleteAsync($"/api/messages/receiverdelete/{invalidMessageId}");
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var errorMessage = await response.Content.ReadAsStringAsync();
        Assert.Contains("Bad request", errorMessage);
    }

    
    
    [Fact]
    public async Task DeleteMessageOnReceiverSide_InternalServerError_ReturnsInternalServerError()
    {
        await ClearDatabaseAsync();
        await AddUsersWithMessages();

    
        var messagesResponse = await _client.GetAsync("/api/messages/getall");
        messagesResponse.EnsureSuccessStatusCode();

        var messages = JsonConvert.DeserializeObject<List<Message>>(await messagesResponse.Content.ReadAsStringAsync());

       
        var messageId = messages.First().Id;
        
        var response = await _mockClient.DeleteAsync($"/api/messages/receiverdelete/{messageId}");

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        
        var errorMessage = await response.Content.ReadAsStringAsync();
        Assert.Contains("Internal server error", errorMessage);
    }

    
    
    [Fact]
    public async Task GetOutgoings_Success_ReturnsMessages()
    {
        await ClearDatabaseAsync();
        await AddUsersWithMessages();

        var parentsResponse = await _usersClient.GetAsync("/api/users/allparents");
        parentsResponse.EnsureSuccessStatusCode();

        var parents = JsonConvert.DeserializeObject<List<Parent>>(await parentsResponse.Content.ReadAsStringAsync());
        
        var parentId = parents.First().Id;
        var response = await _client.GetAsync($"/api/messages/outgoings/{parentId}");

        response.EnsureSuccessStatusCode();

        var messages = JsonConvert.DeserializeObject<List<Message>>(await response.Content.ReadAsStringAsync());

        var message = messages.First();
        Assert.Equal("Mary Doe", message.SenderName);
        Assert.Equal("John Doe", message.ReceiverName);
        Assert.Equal("Message 1", message.HeadText);
        Assert.Equal("This is a message from parent to teacher.", message.Text);
        Assert.False(message.Read);
        Assert.False(message.DeletedBySender);
        Assert.False(message.DeletedByReceiver);
    }

    
    
    [Fact]
    public async Task GetSents_Success_ReturnsMessages()
    {
       
        await ClearDatabaseAsync();  
   
        await AddUsersWithMessages();  


        var parentsResponse = await _usersClient.GetAsync("/api/users/allparents");
        parentsResponse.EnsureSuccessStatusCode();

  
        var parents = JsonConvert.DeserializeObject<List<Parent>>(await parentsResponse.Content.ReadAsStringAsync());
    
     
        var parentId = parents.First().Id;

        
        var response = await _client.GetAsync($"/api/messages/sents/{parentId}");

        response.EnsureSuccessStatusCode();

        var messages = JsonConvert.DeserializeObject<List<Message>>(await response.Content.ReadAsStringAsync());


        var message = messages.First();
        Assert.Equal("Mary Doe", message.SenderName);
        Assert.Equal("John Doe", message.ReceiverName);
        Assert.Equal("Message 1", message.HeadText); 
        Assert.Equal("This is a message from parent to teacher.", message.Text);
        Assert.False(message.Read);
        Assert.False(message.DeletedBySender);
        Assert.False(message.DeletedByReceiver);
    }

    
    
    [Fact]
    public async Task GetSents_EmptyList_ReturnsEmptyList()
    {
        
        await ClearDatabaseAsync();  
    
        await AddUsersWithoutMessages();  

        var parentsResponse = await _usersClient.GetAsync("/api/users/allparents");
        parentsResponse.EnsureSuccessStatusCode();

    
        var parents = JsonConvert.DeserializeObject<List<Parent>>(await parentsResponse.Content.ReadAsStringAsync());
    
      
        var parentId = parents.First().Id;

     
        var response = await _client.GetAsync($"/api/messages/sents/{parentId}");

       
        response.EnsureSuccessStatusCode();

       
        var messages = JsonConvert.DeserializeObject<List<Message>>(await response.Content.ReadAsStringAsync());

     
        Assert.Empty(messages);
    }

    
    [Fact]
public async Task GetSents_InvalidId_ReturnsBadRequest()
{

    await ClearDatabaseAsync(); 


    var invalidId = "invalid-id";


    var response = await _client.GetAsync($"/api/messages/sents/{invalidId}");


    Assert.Equal(400, (int)response.StatusCode);

 
    var errorMessage = await response.Content.ReadAsStringAsync();
    Assert.Contains("Bad request", errorMessage);
}


    [Fact]
    public async Task GetSents_InternalServerError_ReturnsInternalServerError()
    {
        await ClearDatabaseAsync();
        await AddUsersWithMessages();

  
        var parentsResponse = await _usersClient.GetAsync("/api/users/allparents");
        parentsResponse.EnsureSuccessStatusCode();
    
        var parents = JsonConvert.DeserializeObject<List<Parent>>(await parentsResponse.Content.ReadAsStringAsync());
        var parentId = parents.First().Id;

        var response = await _mockClient.GetAsync($"/api/messages/sents/{parentId}");

        Assert.Equal(500, (int)response.StatusCode);

        var errorMessage = await response.Content.ReadAsStringAsync();
        Assert.Contains("Internal server error", errorMessage);
    }

    
    
    [Fact]
    public async Task Restore_Success_ReturnsMessageRestored()
    {
        await ClearDatabaseAsync();
        await AddDeletedMessage();

        var parentsResponse = await _usersClient.GetAsync("/api/users/allparents");
        parentsResponse.EnsureSuccessStatusCode();

        var parents = JsonConvert.DeserializeObject<List<Parent>>(await parentsResponse.Content.ReadAsStringAsync());
        var parentId = parents.First().Id;

    
        var messagesResponse = await _client.GetAsync("/api/messages/getall");
        messagesResponse.EnsureSuccessStatusCode();
        var messages = JsonConvert.DeserializeObject<List<Message>>(await messagesResponse.Content.ReadAsStringAsync());

        var messageId = messages.First().Id;

  
        var restoreResponse = await _client.GetAsync($"/api/messages/restore/{messageId}/{parentId}");

        restoreResponse.EnsureSuccessStatusCode();
        var restoreMessage = await restoreResponse.Content.ReadAsStringAsync();
        Assert.Equal("Üzenet sikeresen visszaállítva.", restoreMessage);

        var updatedMessagesResponse = await _client.GetAsync("/api/messages/getall");
        updatedMessagesResponse.EnsureSuccessStatusCode();
        var updatedMessages = JsonConvert.DeserializeObject<List<Message>>(await updatedMessagesResponse.Content.ReadAsStringAsync());

        var restoredMessage = updatedMessages.First(m => m.Id == messageId);
        Assert.False(restoredMessage.DeletedByReceiver, "A DeletedByReceiver érték nem lett false.");
    }

    
    
    [Fact]
    public async Task Restore_InvalidUserId_ReturnsBadRequest()
    {
        await ClearDatabaseAsync();
        await AddDeletedMessage();


      
        var messagesResponse = await _client.GetAsync("/api/messages/getall");
        messagesResponse.EnsureSuccessStatusCode();
        var messages = JsonConvert.DeserializeObject<List<Message>>(await messagesResponse.Content.ReadAsStringAsync());
        var messageId = messages.First().Id;

  
        var invalidUserId = "invalid-user-id";

        var restoreResponse = await _client.GetAsync($"/api/messages/restore/{messageId}/{invalidUserId}");


        Assert.Equal(400, (int)restoreResponse.StatusCode);
    
        var errorMessage = await restoreResponse.Content.ReadAsStringAsync();
        Assert.Contains("Bad request", errorMessage);
    }

    
    
    [Fact]
    public async Task Restore_InvalidMessageId_ReturnsBadRequest()
    {
        await ClearDatabaseAsync();
        await AddDeletedMessage();

        var parentsResponse = await _usersClient.GetAsync("/api/users/allparents");
        parentsResponse.EnsureSuccessStatusCode();

        var parents = JsonConvert.DeserializeObject<List<Parent>>(await parentsResponse.Content.ReadAsStringAsync());
        var parentId = parents.First().Id;

        var messagesResponse = await _client.GetAsync("/api/messages/getall");
        messagesResponse.EnsureSuccessStatusCode();
        var messages = JsonConvert.DeserializeObject<List<Message>>(await messagesResponse.Content.ReadAsStringAsync());
        var messageId = messages.First().Id;

        var invalidMessageId = messageId + 999;

        var restoreResponse = await _client.GetAsync($"/api/messages/restore/{invalidMessageId}/{parentId}");

   
        Assert.Equal(400, (int)restoreResponse.StatusCode);
    
        var errorMessage = await restoreResponse.Content.ReadAsStringAsync();
        Assert.Contains("Bad request", errorMessage);
    }

    
    
    [Fact]
    public async Task Restore_ValidIds_InternalServerError()
    {
        await ClearDatabaseAsync();
        await AddDeletedMessage();
        
        var parentsResponse = await _usersClient.GetAsync("/api/users/allparents");
        parentsResponse.EnsureSuccessStatusCode();

        var parents = JsonConvert.DeserializeObject<List<Parent>>(await parentsResponse.Content.ReadAsStringAsync());
        var parentId = parents.First().Id;
        
        var messagesResponse = await _client.GetAsync("/api/messages/getall");
        messagesResponse.EnsureSuccessStatusCode();
        var messages = JsonConvert.DeserializeObject<List<Message>>(await messagesResponse.Content.ReadAsStringAsync());
        var messageId = messages.First().Id;
        
        var restoreResponse = await _mockClient.GetAsync($"/api/messages/restore/{messageId}/{parentId}");

        Assert.Equal(500, (int)restoreResponse.StatusCode);

        var errorMessage = await restoreResponse.Content.ReadAsStringAsync();
        Assert.Contains("Internal server error", errorMessage);
    }

 


[Fact]
public async Task GetOutgoings_EmptyList_ReturnsEmptyList()
{
    await ClearDatabaseAsync();
    await AddUsersWithoutMessages();


    var parentsResponse = await _usersClient.GetAsync("/api/users/allparents");
    parentsResponse.EnsureSuccessStatusCode();

    var parents = JsonConvert.DeserializeObject<List<Parent>>(await parentsResponse.Content.ReadAsStringAsync());

 var parentId = parents.First().Id;


    var response = await _client.GetAsync($"/api/messages/outgoings/{parentId}");

 
    response.EnsureSuccessStatusCode();

    var messages = JsonConvert.DeserializeObject<List<Message>>(await response.Content.ReadAsStringAsync());


    Assert.Empty(messages);
}



[Fact]
public async Task GetOutgoings_InvalidId_ThrowsArgumentException()
{
    await ClearDatabaseAsync();

  
    var parentsResponse = await _usersClient.GetAsync("/api/users/allparents");
    parentsResponse.EnsureSuccessStatusCode();

    var parents = JsonConvert.DeserializeObject<List<Parent>>(await parentsResponse.Content.ReadAsStringAsync());

  
    var invalidId = "invalid-id";


    var response = await _client.GetAsync($"/api/messages/outgoings/{invalidId}");


    Assert.Equal(404, (int)response.StatusCode); 
}

[Fact]
public async Task GetOutgoings_InternalServerError_Throws500Error()
{
    await ClearDatabaseAsync();
    await AddUsersWithMessages();


    var parentsResponse = await _usersClient.GetAsync("/api/users/allparents");
    parentsResponse.EnsureSuccessStatusCode();

    var parents = JsonConvert.DeserializeObject<List<Parent>>(await parentsResponse.Content.ReadAsStringAsync());

 
    var parentId = parents.First().Id;

  
    var response = await _mockClient.GetAsync($"/api/messages/outgoings/{parentId}");


    Assert.Equal(500, (int)response.StatusCode);
}



private async Task ClearDatabaseAsync()
{
    using (var scope = _factory.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();
        
        var grades = context.Grades.ToList();
        context.Grades.RemoveRange(grades);
        var messages = context.Messages.ToList();
        context.Messages.RemoveRange(messages);
        var notifications = context.Notifications.ToList();
        context.Notifications.RemoveRange(notifications);
        var parents = context.Parents.ToList();
        context.Parents.RemoveRange(parents);
        var students = context.Students.ToList();
        context.Students.RemoveRange(students);
        var teachers = context.Teachers.ToList();
        context.Teachers.RemoveRange(teachers);
        var classes = context.ClassesOfStudents.ToList();
        context.ClassesOfStudents.RemoveRange(classes);
        var teacherSubjects = context.TeacherSubjects.ToList();
        context.TeacherSubjects.RemoveRange(teacherSubjects);

        await context.SaveChangesAsync();
    }
}

   
    
private async Task AddUsersWithMessages()
{
    using (var scope = _factory.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();
        
        var testTeachers = new List<Teacher>
        {
            new Teacher
            {
                UserName = "johndoe",
                FirstName = "John",
                FamilyName = "Doe",
                Role = "Teacher"
            }
        };

        foreach (var teacher in testTeachers)
        {
            var existingTeacher = await context.Teachers.FindAsync(teacher.Id);
            if (existingTeacher == null)
            {
                context.Teachers.Add(teacher);
            }
            else
            {
                context.Entry(existingTeacher).CurrentValues.SetValues(teacher);
            }
        }

        await context.SaveChangesAsync();

      
        var testStudents = new List<Student>
        {
            new Student
            {
                UserName = "student1",
                FirstName = "Jane",
                FamilyName = "Doe",
                BirthDate = new DateTime(2010, 1, 1),
                BirthPlace = "Budapest",
                StudentNo = "S12345",
                Role = "Student"
            }
        };

        foreach (var student in testStudents)
        {
            var existingStudent = await context.Students.FindAsync(student.Id);
            if (existingStudent == null)
            {
                context.Students.Add(student);
            }
            else
            {
                context.Entry(existingStudent).CurrentValues.SetValues(student);
            }
        }

        await context.SaveChangesAsync();

        var testParents = new List<Parent>
        {
            new Parent
            {
               
                UserName = "parent1",
                FirstName = "Mary",
                FamilyName = "Doe",
                Role = "Parent",
                ChildName = "Jane Doe",
                StudentId = testStudents[0].Id
            }
        };

        foreach (var parent in testParents)
        {
            var existingParent = await context.Parents.FindAsync(parent.Id);
            if (existingParent == null)
            {
                context.Parents.Add(parent);
            }
            else
            {
                context.Entry(existingParent).CurrentValues.SetValues(parent);
            }
        }

        await context.SaveChangesAsync();

    
        var testMessages = new List<Message>
        {
            new Message
            {
                Date = DateTime.Now,
                Sender = testParents[0],
                SenderName = "Mary Doe",
                Receiver = testTeachers[0],
                ReceiverName = "John Doe",
                HeadText = "Message 1",
                Text = "This is a message from parent to teacher.",
                Read = false,
                DeletedBySender = false,
                DeletedByReceiver = false
            },
            new Message
            {
                Date = DateTime.Now,
                Sender = testTeachers[0],
                SenderName = "John Doe",
                Receiver = testParents[0], 
                ReceiverName = "Mary Doe",
                HeadText = "Message 2",
                Text = "This is a message from teacher to parent.",
                Read = false,
                DeletedBySender = false,
                DeletedByReceiver = false
            }
        };

        foreach (var message in testMessages)
        {
            context.Messages.Add(message);
        }

        await context.SaveChangesAsync(); 
    }
}


private async Task AddUsersWithReadMessage()
{
    using (var scope = _factory.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();
        
        var testTeachers = new List<Teacher>
        {
            new Teacher
            {
                UserName = "johndoe",
                FirstName = "John",
                FamilyName = "Doe",
                Role = "Teacher"
            }
        };

        foreach (var teacher in testTeachers)
        {
            var existingTeacher = await context.Teachers.FindAsync(teacher.Id);
            if (existingTeacher == null)
            {
                context.Teachers.Add(teacher);
            }
            else
            {
                context.Entry(existingTeacher).CurrentValues.SetValues(teacher);
            }
        }

        await context.SaveChangesAsync(); 

      
        var testStudents = new List<Student>
        {
            new Student
            {
                UserName = "student1",
                FirstName = "Jane",
                FamilyName = "Doe",
                BirthDate = new DateTime(2010, 1, 1),
                BirthPlace = "Budapest",
                StudentNo = "S12345",
                Role = "Student"
            }
        };

        foreach (var student in testStudents)
        {
            var existingStudent = await context.Students.FindAsync(student.Id);
            if (existingStudent == null)
            {
                context.Students.Add(student);
            }
            else
            {
                context.Entry(existingStudent).CurrentValues.SetValues(student);
            }
        }

        await context.SaveChangesAsync(); 

    
        var testParents = new List<Parent>
        {
            new Parent
            {
               
                UserName = "parent1",
                FirstName = "Mary",
                FamilyName = "Doe",
                Role = "Parent",
                ChildName = "Jane Doe",
                StudentId = testStudents[0].Id 
            }
        };

        foreach (var parent in testParents)
        {
            var existingParent = await context.Parents.FindAsync(parent.Id);
            if (existingParent == null)
            {
                context.Parents.Add(parent);
            }
            else
            {
                context.Entry(existingParent).CurrentValues.SetValues(parent);
            }
        }

        await context.SaveChangesAsync(); 

        var message1=     new Message
            {
                Date = DateTime.Now,
                Sender = testParents[0], 
                SenderName = "Mary Doe",
                Receiver = testTeachers[0], 
                ReceiverName = "John Doe",
                HeadText = "Message 1",
                Text = "This is a message from parent to teacher.",
                Read = true,
                DeletedBySender = false,
                DeletedByReceiver = false
            
        };

            context.Messages.Add(message1);
     

        await context.SaveChangesAsync();
    }
}




private async Task AddDeletedMessage()
{
    using (var scope = _factory.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();
        
        var testTeachers = new List<Teacher>
        {
            new Teacher
            {
                UserName = "johndoe",
                FirstName = "John",
                FamilyName = "Doe",
                Role = "Teacher"
            }
        };

        foreach (var teacher in testTeachers)
        {
            var existingTeacher = await context.Teachers.FindAsync(teacher.Id);
            if (existingTeacher == null)
            {
                context.Teachers.Add(teacher);
            }
            else
            {
                context.Entry(existingTeacher).CurrentValues.SetValues(teacher);
            }
        }

        await context.SaveChangesAsync();

 
        var testStudents = new List<Student>
        {
            new Student
            {
                UserName = "student1",
                FirstName = "Jane",
                FamilyName = "Doe",
                BirthDate = new DateTime(2010, 1, 1),
                BirthPlace = "Budapest",
                StudentNo = "S12345",
                Role = "Student"
            }
        };

        foreach (var student in testStudents)
        {
            var existingStudent = await context.Students.FindAsync(student.Id);
            if (existingStudent == null)
            {
                context.Students.Add(student);
            }
            else
            {
                context.Entry(existingStudent).CurrentValues.SetValues(student);
            }
        }

        await context.SaveChangesAsync();

     
        var testParents = new List<Parent>
        {
            new Parent
            {
                Id = "2",
                UserName = "parent1",
                FirstName = "Mary",
                FamilyName = "Doe",
                Role = "Parent",
                ChildName = "Jane Doe",
                StudentId = testStudents[0].Id
            }
        };

        foreach (var parent in testParents)
        {
            var existingParent = await context.Parents.FindAsync(parent.Id);
            if (existingParent == null)
            {
                context.Parents.Add(parent);
            }
            else
            {
                context.Entry(existingParent).CurrentValues.SetValues(parent);
            }
        }

        await context.SaveChangesAsync(); 

    
        var testMessages = new List<Message>
        {
            new Message
            {
                Date = DateTime.Now,
                Receiver = testParents[0], 
                ReceiverName = "Mary Doe",
                Sender = testTeachers[0],
                SenderName = "John Doe",
                HeadText = "Message 1",
                Text = "This is a message from parent to teacher.",
                Read = false,
                DeletedBySender = false,
                DeletedByReceiver = true
            }
        };

        foreach (var message in testMessages)
        {
            context.Messages.Add(message);
        }

        await context.SaveChangesAsync();
    }
}
private async Task AddSentMessages()
{
    using (var scope = _factory.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();
        
        var testTeachers = new List<Teacher>
        {
            new Teacher
            {
                UserName = "johndoe",
                FirstName = "John",
                FamilyName = "Doe",
                Role = "Teacher"
            }
        };

        foreach (var teacher in testTeachers)
        {
            var existingTeacher = await context.Teachers.FindAsync(teacher.Id);
            if (existingTeacher == null)
            {
                context.Teachers.Add(teacher);
            }
            else
            {
                context.Entry(existingTeacher).CurrentValues.SetValues(teacher);
            }
        }

        await context.SaveChangesAsync();

        var testStudents = new List<Student>
        {
            new Student
            {
                UserName = "student1",
                FirstName = "Jane",
                FamilyName = "Doe",
                BirthDate = new DateTime(2010, 1, 1),
                BirthPlace = "Budapest",
                StudentNo = "S12345",
                Role = "Student"
            }
        };

        foreach (var student in testStudents)
        {
            var existingStudent = await context.Students.FindAsync(student.Id);
            if (existingStudent == null)
            {
                context.Students.Add(student);
            }
            else
            {
                context.Entry(existingStudent).CurrentValues.SetValues(student);
            }
        }

        await context.SaveChangesAsync();

        var testParents = new List<Parent>
        {
            new Parent
            {
                UserName = "parent1",
                FirstName = "Mary",
                FamilyName = "Doe",
                Role = "Parent",
                ChildName = "Jane Doe",
                StudentId = testStudents[0].Id
            }
        };

        foreach (var parent in testParents)
        {
            var existingParent = await context.Parents.FindAsync(parent.Id);
            if (existingParent == null)
            {
                context.Parents.Add(parent);
            }
            else
            {
                context.Entry(existingParent).CurrentValues.SetValues(parent);
            }
        }

        await context.SaveChangesAsync();

        var testMessages = new List<Message>
        {
            new Message
            {
                Date = DateTime.Now,
                Sender = testParents[0],
                SenderName = "Mary Doe",
                Receiver = testTeachers[0], 
                ReceiverName = "John Doe",
                HeadText = "Message 1",
                Text = "This is a message from parent to teacher.",
                Read = false,
                DeletedBySender = false,
                DeletedByReceiver = true 
            }
        };

        foreach (var message in testMessages)
        {
            context.Messages.Add(message);
        }

        await context.SaveChangesAsync();
    }
}



    private async Task AddUsersWithoutMessages()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();

            var testTeachers = new List<Teacher>
            {
                new Teacher
                {
                    Id = "Teacher1Id",
                    UserName = "johndoe",
                    FirstName = "John",
                    FamilyName = "Doe",
                    Role = "Teacher"
                }
            };

            var testStudents = new List<Student>
            {
                new Student
                {
                    Id ="Student1Id",
                    UserName = "student1",
                    FirstName = "Jane",
                    FamilyName = "Doe",
                    BirthDate = new DateTime(2010, 1, 1),
                    BirthPlace = "Budapest",
                    StudentNo = "S12345",
                    Role="Student"
                }
            };

            var testParents = new List<Parent>
            {
                new Parent
                {
                    Id = "Parent1Id",
                    UserName = "parent1",
                    FirstName = "Mary",
                    FamilyName = "Doe",
                    Role = "Parent",
                    ChildName = "Jane Doe",
                    StudentId = testStudents[0].Id
                }
            };

            foreach (var teacher in testTeachers)
            {
                var existingTeacher = await context.Teachers.FindAsync(teacher.Id);
                if (existingTeacher == null)
                {
                    context.Teachers.Add(teacher);
                }
                else
                {
                    context.Entry(existingTeacher).CurrentValues.SetValues(teacher);
                }
            }

            foreach (var student in testStudents)
            {
                var existingStudent = await context.Students.FindAsync(student.Id);
                if (existingStudent == null)
                {
                    context.Students.Add(student);
                }
                else
                {
                    context.Entry(existingStudent).CurrentValues.SetValues(student);
                }
            }

            foreach (var parent in testParents)
            {
                var existingParent = await context.Parents.FindAsync(parent.Id);
                if (existingParent == null)
                {
                    context.Parents.Add(parent);
                }
                else
                {
                    context.Entry(existingParent).CurrentValues.SetValues(parent);
                }
            }
            await context.SaveChangesAsync();
        }
    }
}