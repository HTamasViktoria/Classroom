using Classroom.Controllers;
using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Classroom.Service.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ClassroomUnitTests;

public class MessageControllerTests
{
    
    private Mock<ILogger<MessagesController>> _loggerMock;
    private Mock<IMessagesRepository> _messageRepositoryMock;
    private MessagesController _messagesController;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<MessagesController>>();
        _messageRepositoryMock = new Mock<IMessagesRepository>();
        _messagesController =
            new MessagesController(_loggerMock.Object, _messageRepositoryMock.Object);
    }
    
    [Test]
    public async Task GetAllMessagesAsync_ReturnsOkWithMessages_WhenMessagesExist()
    {
        // Arrange
        var messages = new List<Message>
        {
            new Message { Id = 1, Text = "Message 1" },
            new Message { Id = 2, Text = "Message 2" }
        };

        _messageRepositoryMock.Setup(repo => repo.GetAllMessagesAsync())
            .ReturnsAsync(messages);

        // Act
        var result = await _messagesController.GetAllMessagesAsync() as OkObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);
        Assert.IsInstanceOf<List<Message>>(result.Value);
        var returnedMessages = result.Value as List<Message>;
        Assert.AreEqual(2, returnedMessages.Count);
    }

    
    [Test]
    public async Task GetAllMessagesAsync_ReturnsOkWithEmptyList_WhenNoMessagesExist()
    {
        // Arrange
        var messages = new List<Message>();

        _messageRepositoryMock.Setup(repo => repo.GetAllMessagesAsync())
            .ReturnsAsync(messages);

        // Act
        var result = await _messagesController.GetAllMessagesAsync() as OkObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);
        Assert.IsInstanceOf<List<Message>>(result.Value);
        var returnedMessages = result.Value as List<Message>;
        Assert.AreEqual(0, returnedMessages.Count);
    }

    
    [Test]
    public async Task GetAllMessagesAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var exceptionMessage = "Test exception";
        _messageRepositoryMock.Setup(repo => repo.GetAllMessagesAsync())
            .ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _messagesController.GetAllMessagesAsync() as ObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(500, result.StatusCode);
        Assert.IsInstanceOf<string>(result.Value);
        var responseMessage = result.Value as string;
        StringAssert.Contains("Internal server error", responseMessage);
        StringAssert.Contains(exceptionMessage, responseMessage);
    }

    
    [Test]
    public void GetNewMessagesNum_ReturnsOkWithNewMessagesNum_WhenIdIsValid()
    {
        // Arrange
        var validId = "validId";
        var newMessagesNum = 5;

        _messageRepositoryMock.Setup(repo => repo.GetNewMessagesNum(validId))
            .Returns(newMessagesNum);

        // Act
        var result = _messagesController.GetNewMessagesNum(validId);
    
        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(newMessagesNum, okResult.Value);
    }

    
    
    [Test]
    public void GetNewMessagesNum_ReturnsNotFound_WhenArgumentExceptionIsThrown()
    {
        // Arrange
        var invalidId = "invalidId";
        var exceptionMessage = "Invalid ID format";
    
        _messageRepositoryMock.Setup(repo => repo.GetNewMessagesNum(invalidId))
            .Throws(new ArgumentException(exceptionMessage));

        // Act
        var result = _messagesController.GetNewMessagesNum(invalidId);

        // Assert
        var notFoundResult = result.Result as ObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
        Assert.AreEqual($"ˇNot found: {exceptionMessage}", notFoundResult.Value);
    }

    
    
    
    [Test]
    public void GetNewMessagesNum_ReturnsInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var validId = "validId";
        var exceptionMessage = "Test exception";
    
        _messageRepositoryMock.Setup(repo => repo.GetNewMessagesNum(validId))
            .Throws(new Exception(exceptionMessage));

        // Act
        var result = _messagesController.GetNewMessagesNum(validId);

        // Assert
        var internalServerErrorResult = result.Result as ObjectResult;
        Assert.IsNotNull(internalServerErrorResult);
        Assert.AreEqual(500, internalServerErrorResult.StatusCode);
        Assert.AreEqual($"Internal server error: {exceptionMessage}", internalServerErrorResult.Value);
    }

    
    [Test]
    public void GetById_ReturnsOkWithMessage_WhenMessageExists()
    {
        // Arrange
        var messageId = 1;
        var message = new Message { Id = messageId, Text = "Test message" };

        _messageRepositoryMock.Setup(repo => repo.GetById(messageId))
            .Returns(message);

        // Act
        var result = _messagesController.GetById(messageId);

        // Assert
        var actionResult = result as ActionResult<Message>;
        Assert.IsNotNull(actionResult);
        var okResult = actionResult.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.IsInstanceOf<Message>(okResult.Value);
        var returnedMessage = okResult.Value as Message;
        Assert.AreEqual(messageId, returnedMessage.Id);
        Assert.AreEqual("Test message", returnedMessage.Text);
    }

    
    [Test]
    public void GetById_ReturnsNotFound_WhenArgumentExceptionIsThrown()
    {
        // Arrange
        var invalidId = 999;
        var exceptionMessage = "Message not found";

        _messageRepositoryMock.Setup(repo => repo.GetById(invalidId))
            .Throws(new ArgumentException(exceptionMessage));

        // Act
        var result = _messagesController.GetById(invalidId);

        // Assert
        var notFoundResult = result as ActionResult<Message>;
        var objectResult = notFoundResult.Result as ObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(404, objectResult.StatusCode);
        Assert.AreEqual($"ˇNot found: {exceptionMessage}", objectResult.Value);
    }

    
    
    [Test]
    public void GetById_ReturnsInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var validId = 1;
        var exceptionMessage = "Test exception";

        _messageRepositoryMock.Setup(repo => repo.GetById(validId))
            .Throws(new Exception(exceptionMessage));

        // Act
        var result = _messagesController.GetById(validId);

        // Assert
        var actionResult = result as ActionResult<Message>;
        var internalServerErrorResult = actionResult.Result as ObjectResult;
        Assert.IsNotNull(internalServerErrorResult);
        Assert.AreEqual(500, internalServerErrorResult.StatusCode);
        Assert.AreEqual($"Internal server error: {exceptionMessage}", internalServerErrorResult.Value);
    }

    
    [Test]
    public void GetIncomings_ReturnsOkWithMessages_WhenMessagesExist()
    {
        // Arrange
        var validId = "validId";
        var messages = new List<Message>
        {
            new Message { Id = 1, Text = "Message 1" },
            new Message { Id = 2, Text = "Message 2" }
        };

        _messageRepositoryMock.Setup(repo => repo.GetIncomings(validId))
            .Returns(messages);

        // Act
        var result = _messagesController.GetIncomings(validId);

        // Assert
        var okResult = result as ActionResult<IEnumerable<Message>>;
        var objectResult = okResult.Result as OkObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(200, objectResult.StatusCode);
        Assert.IsInstanceOf<IEnumerable<Message>>(objectResult.Value);
        var returnedMessages = objectResult.Value as IEnumerable<Message>;
        Assert.AreEqual(2, returnedMessages.Count());
    }

    
    
    [Test]
    public void GetIncomings_ReturnsOkWithEmptyList_WhenNoMessagesExist()
    {
        // Arrange
        var validId = "validId";
        var messages = new List<Message>();

        _messageRepositoryMock.Setup(repo => repo.GetIncomings(validId))
            .Returns(messages);

        // Act
        var result = _messagesController.GetIncomings(validId);

        // Assert
        var okResult = result as ActionResult<IEnumerable<Message>>;
        var objectResult = okResult.Result as OkObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(200, objectResult.StatusCode);
        Assert.IsInstanceOf<IEnumerable<Message>>(objectResult.Value);
        var returnedMessages = objectResult.Value as IEnumerable<Message>;
        Assert.AreEqual(0, returnedMessages.Count());
    }

    
    
    
    [Test]
    public void GetIncomings_ReturnsNotFound_WhenArgumentExceptionIsThrown()
    {
        // Arrange
        var invalidId = "invalidId";
        var exceptionMessage = "Invalid ID format";

        _messageRepositoryMock.Setup(repo => repo.GetIncomings(invalidId))
            .Throws(new ArgumentException(exceptionMessage));

        // Act
        var result = _messagesController.GetIncomings(invalidId);

        // Assert
        var actionResult = result as ActionResult<IEnumerable<Message>>;
        var objectResult = actionResult.Result as ObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(404, objectResult.StatusCode);
        Assert.AreEqual($"ˇNot found: {exceptionMessage}", objectResult.Value);
    }

    
    
    
    [Test]
    public void GetIncomings_ReturnsInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var validId = "validId";
        var exceptionMessage = "Test exception";

        _messageRepositoryMock.Setup(repo => repo.GetIncomings(validId))
            .Throws(new Exception(exceptionMessage));

        // Act
        var result = _messagesController.GetIncomings(validId);

        // Assert
        var actionResult = result as ActionResult<IEnumerable<Message>>;
        var internalServerErrorResult = actionResult.Result as ObjectResult;
        Assert.IsNotNull(internalServerErrorResult);
        Assert.AreEqual(500, internalServerErrorResult.StatusCode);
        Assert.AreEqual($"Internal server error: {exceptionMessage}", internalServerErrorResult.Value);
    }

    
    
    [Test]
    public void GetDeleteds_ReturnsOkWithMessages_WhenMessagesExist()
    {
        // Arrange
        var validId = "validId";
        var messages = new List<Message>
        {
            new Message { Id = 1, Text = "Deleted message 1" },
            new Message { Id = 2, Text = "Deleted message 2" }
        };

        _messageRepositoryMock.Setup(repo => repo.GetDeleteds(validId))
            .Returns(messages);

        // Act
        var result = _messagesController.GetDeleteds(validId);

        // Assert
        var okResult = result as ActionResult<IEnumerable<Message>>;
        var objectResult = okResult.Result as OkObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(200, objectResult.StatusCode);
        Assert.IsInstanceOf<IEnumerable<Message>>(objectResult.Value);
        var returnedMessages = objectResult.Value as IEnumerable<Message>;
        Assert.AreEqual(2, returnedMessages.Count());
    }

    
    
    [Test]
    public void GetDeleteds_ReturnsOkWithEmptyList_WhenNoMessagesExist()
    {
        // Arrange
        var validId = "validId";
        var messages = new List<Message>();

        _messageRepositoryMock.Setup(repo => repo.GetDeleteds(validId))
            .Returns(messages);

        // Act
        var result = _messagesController.GetDeleteds(validId);

        // Assert
        var okResult = result as ActionResult<IEnumerable<Message>>;
        var objectResult = okResult.Result as OkObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(200, objectResult.StatusCode);
        Assert.IsInstanceOf<IEnumerable<Message>>(objectResult.Value);
        var returnedMessages = objectResult.Value as IEnumerable<Message>;
        Assert.AreEqual(0, returnedMessages.Count());
    }

    
    
    
    [Test]
    public void GetDeleteds_ReturnsNotFound_WhenArgumentExceptionIsThrown()
    {
        // Arrange
        var invalidId = "invalidId";
        var exceptionMessage = "Invalid ID format";

        _messageRepositoryMock.Setup(repo => repo.GetDeleteds(invalidId))
            .Throws(new ArgumentException(exceptionMessage));

        // Act
        var result = _messagesController.GetDeleteds(invalidId);

        // Assert
        var actionResult = result as ActionResult<IEnumerable<Message>>;
        var objectResult = actionResult.Result as ObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(404, objectResult.StatusCode);
        Assert.AreEqual($"ˇNot found: {exceptionMessage}", objectResult.Value);
    }

    
    
    
    [Test]
    public void GetDeleteds_ReturnsInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var validId = "validId";
        var exceptionMessage = "Test exception";

        _messageRepositoryMock.Setup(repo => repo.GetDeleteds(validId))
            .Throws(new Exception(exceptionMessage));

        // Act
        var result = _messagesController.GetDeleteds(validId);

        // Assert
        var actionResult = result as ActionResult<IEnumerable<Message>>;
        var internalServerErrorResult = actionResult.Result as ObjectResult;
        Assert.IsNotNull(internalServerErrorResult);
        Assert.AreEqual(500, internalServerErrorResult.StatusCode);
        Assert.AreEqual($"Internal server error: {exceptionMessage}", internalServerErrorResult.Value);
    }


    [Test]
    public void GetSents_ReturnsOkWithMessages_WhenMessagesExist()
    {
        // Arrange
        var validId = "validId";
        var messages = new List<Message>
        {
            new Message { Id = 1, Text = "Sent message 1" },
            new Message { Id = 2, Text = "Sent message 2" }
        };

        _messageRepositoryMock.Setup(repo => repo.GetSents(validId))
            .Returns(messages);

        // Act
        var result = _messagesController.GetSents(validId);

        // Assert
        var okResult = result as ActionResult<IEnumerable<Message>>;
        var objectResult = okResult.Result as OkObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(200, objectResult.StatusCode);
        Assert.IsInstanceOf<IEnumerable<Message>>(objectResult.Value);
        var returnedMessages = objectResult.Value as IEnumerable<Message>;
        Assert.AreEqual(2, returnedMessages.Count());
    }

    
    
    [Test]
    public void GetSents_ReturnsOkWithEmptyList_WhenNoMessagesExist()
    {
        // Arrange
        var validId = "validId";
        var messages = new List<Message>();

        _messageRepositoryMock.Setup(repo => repo.GetSents(validId))
            .Returns(messages);

        // Act
        var result = _messagesController.GetSents(validId);

        // Assert
        var okResult = result as ActionResult<IEnumerable<Message>>;
        var objectResult = okResult.Result as OkObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(200, objectResult.StatusCode);
        Assert.IsInstanceOf<IEnumerable<Message>>(objectResult.Value);
        var returnedMessages = objectResult.Value as IEnumerable<Message>;
        Assert.AreEqual(0, returnedMessages.Count());
    }

    
    
    [Test]
    public void GetSents_ReturnsNotFound_WhenArgumentExceptionIsThrown()
    {
        // Arrange
        var invalidId = "invalidId";
        var exceptionMessage = "Invalid ID format";

        _messageRepositoryMock.Setup(repo => repo.GetSents(invalidId))
            .Throws(new ArgumentException(exceptionMessage));

        // Act
        var result = _messagesController.GetSents(invalidId);

        // Assert
        var actionResult = result as ActionResult<IEnumerable<Message>>;
        var objectResult = actionResult.Result as ObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(404, objectResult.StatusCode);
        Assert.AreEqual($"ˇNot found: {exceptionMessage}", objectResult.Value);
    }

    
    
    [Test]
    public void GetSents_ReturnsInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var validId = "validId";
        var exceptionMessage = "Test exception";

        _messageRepositoryMock.Setup(repo => repo.GetSents(validId))
            .Throws(new Exception(exceptionMessage));

        // Act
        var result = _messagesController.GetSents(validId);

        // Assert
        var actionResult = result as ActionResult<IEnumerable<Message>>;
        var internalServerErrorResult = actionResult.Result as ObjectResult;
        Assert.IsNotNull(internalServerErrorResult);
        Assert.AreEqual(500, internalServerErrorResult.StatusCode);
        Assert.AreEqual($"Internal server error: {exceptionMessage}", internalServerErrorResult.Value);
    }

    
    [Test]
public void GetOutgoings_ReturnsOkWithMessages_WhenMessagesExist()
{
    // Arrange
    var validId = "validId";
    var messages = new List<Message>
    {
        new Message { Id = 1, Text = "Outgoing message 1" },
        new Message { Id = 2, Text = "Outgoing message 2" }
    };

    _messageRepositoryMock.Setup(repo => repo.GetOutgoings(validId))
                          .Returns(messages);

    // Act
    var result = _messagesController.GetOutgoings(validId);

    // Assert
    var okResult = result as ActionResult<IEnumerable<Message>>;
    var objectResult = okResult.Result as OkObjectResult;
    Assert.IsNotNull(objectResult);
    Assert.AreEqual(200, objectResult.StatusCode);
    Assert.IsInstanceOf<IEnumerable<Message>>(objectResult.Value);
    var returnedMessages = objectResult.Value as IEnumerable<Message>;
    Assert.AreEqual(2, returnedMessages.Count());
}

[Test]
public void GetOutgoings_ReturnsOkWithEmptyList_WhenNoMessagesExist()
{
    // Arrange
    var validId = "validId";
    var messages = new List<Message>();

    _messageRepositoryMock.Setup(repo => repo.GetOutgoings(validId))
                          .Returns(messages);

    // Act
    var result = _messagesController.GetOutgoings(validId);

    // Assert
    var okResult = result as ActionResult<IEnumerable<Message>>;
    var objectResult = okResult.Result as OkObjectResult;
    Assert.IsNotNull(objectResult);
    Assert.AreEqual(200, objectResult.StatusCode);
    Assert.IsInstanceOf<IEnumerable<Message>>(objectResult.Value);
    var returnedMessages = objectResult.Value as IEnumerable<Message>;
    Assert.AreEqual(0, returnedMessages.Count());
}


[Test]
public void GetOutgoings_ReturnsNotFound_WhenArgumentExceptionIsThrown()
{
    // Arrange
    var invalidId = "invalidId";
    var exceptionMessage = "Invalid ID format";

    _messageRepositoryMock.Setup(repo => repo.GetOutgoings(invalidId))
                          .Throws(new ArgumentException(exceptionMessage));

    // Act
    var result = _messagesController.GetOutgoings(invalidId);

    // Assert
    var actionResult = result as ActionResult<IEnumerable<Message>>;
    var objectResult = actionResult.Result as ObjectResult;
    Assert.IsNotNull(objectResult);
    Assert.AreEqual(404, objectResult.StatusCode);
    Assert.AreEqual($"ˇNot found: {exceptionMessage}", objectResult.Value);
}

[Test]
public void GetOutgoings_ReturnsInternalServerError_WhenExceptionIsThrown()
{
    // Arrange
    var validId = "validId";
    var exceptionMessage = "Test exception";

    _messageRepositoryMock.Setup(repo => repo.GetOutgoings(validId))
                          .Throws(new Exception(exceptionMessage));

    // Act
    var result = _messagesController.GetOutgoings(validId);

    // Assert
    var actionResult = result as ActionResult<IEnumerable<Message>>;
    var internalServerErrorResult = actionResult.Result as ObjectResult;
    Assert.IsNotNull(internalServerErrorResult);
    Assert.AreEqual(500, internalServerErrorResult.StatusCode);
    Assert.AreEqual($"Internal server error: {exceptionMessage}", internalServerErrorResult.Value);
}





[Test]
public void Post_ReturnsCreatedAtAction_WhenMessageIsSuccessfullyAdded()
{
    // Arrange
    var messageRequest = new MessageRequest
    {
        Date = DateTime.Now,
        FromId = "validSenderId",
        ReceiverIds = new List<string> { "validReceiverId1", "validReceiverId2" },
        HeadText = "Test Head",
        Text = "Test message",
        Id = 1
    };

    _messageRepositoryMock.Setup(repo => repo.AddMessage(messageRequest)).Verifiable();

    // Act
    var result = _messagesController.Post(messageRequest);

    // Assert
    var createdAtActionResult = result as CreatedAtActionResult;
    Assert.IsNotNull(createdAtActionResult);
    Assert.AreEqual(201, createdAtActionResult.StatusCode);

    var responseValue = createdAtActionResult.Value as Object;
    Assert.IsNotNull(responseValue);

    var messageProperty = responseValue.GetType().GetProperty("message");
    Assert.IsNotNull(messageProperty);
    var messageValue = messageProperty.GetValue(responseValue);
    Assert.AreEqual("Üzenet sikeresen elmentve az adatbázisba.", messageValue);

    _messageRepositoryMock.Verify(repo => repo.AddMessage(It.IsAny<MessageRequest>()), Times.Once);
}



[Test]
public void Post_ReturnsBadRequest_WhenArgumentExceptionIsThrown()
{
    var messageRequest = new MessageRequest
    {
        Date = DateTime.Now,
        FromId = "validSenderId",
        ReceiverIds = new List<string> { "invalidReceiverId" },
        HeadText = "Test Head",
        Text = "Test message"
    };

    var exceptionMessage = "Az üzenet küldése a következő felhasználók számára nem sikerült: invalidReceiverId";

    _messageRepositoryMock.Setup(repo => repo.AddMessage(It.IsAny<MessageRequest>()))
                          .Throws(new ArgumentException(exceptionMessage));

    var result = _messagesController.Post(messageRequest);

    var objectResult = result as ObjectResult;
    Assert.IsNotNull(objectResult);
    Assert.AreEqual(400, objectResult.StatusCode);
    Assert.AreEqual($"Bad request:{exceptionMessage}", objectResult.Value);
}

[Test]
public void Post_ReturnsInternalServerError_WhenExceptionIsThrown()
{
    var messageRequest = new MessageRequest
    {
        Date = DateTime.Now,
        FromId = "validSenderId",
        ReceiverIds = new List<string> { "validReceiverId1", "validReceiverId2" },
        HeadText = "Test Head",
        Text = "Test message"
    };

    var exceptionMessage = "Test exception";

    _messageRepositoryMock.Setup(repo => repo.AddMessage(It.IsAny<MessageRequest>()))
                          .Throws(new Exception(exceptionMessage));

    var result = _messagesController.Post(messageRequest);

    var objectResult = result as ObjectResult;
    Assert.IsNotNull(objectResult);
    Assert.AreEqual(500, objectResult.StatusCode);
    Assert.AreEqual($"Internal server error: {exceptionMessage}", objectResult.Value);
}



[Test]
public void DeleteMessageOnReceiverSide_ShouldReturnOk_WhenMessageIsDeletedSuccessfully()
{
    // Arrange
    var messageId = 1;
    _messageRepositoryMock
        .Setup(repo => repo.DeleteOnReceiverSide(messageId))
        .Returns(true);

    // Act
    var result = _messagesController.DeleteMessageOnReceiverSide(messageId);

    // Assert
    var okResult = result as ObjectResult;
    Assert.IsNotNull(okResult);
    Assert.AreEqual(200, okResult.StatusCode);
    Assert.AreEqual("Üzenet sikeresen törölve a fogadó fél oldalán.", okResult.Value);
}



[Test]
public void DeleteMessageOnReceiverSide_ShouldReturnBadRequest_WhenMessageNotFound()
{
    // Arrange
    var messageId = 999;
    _messageRepositoryMock
        .Setup(repo => repo.DeleteOnReceiverSide(messageId))
        .Throws(new ArgumentException("Üzenet nem található."));

    // Act
    var result = _messagesController.DeleteMessageOnReceiverSide(messageId);

    // Assert
    var badRequestResult = result as ObjectResult;
    Assert.IsNotNull(badRequestResult);
    Assert.AreEqual(400, badRequestResult.StatusCode);
    Assert.AreEqual("Bad request: Üzenet nem található.", badRequestResult.Value);
}




[Test]
public void DeleteMessageOnReceiverSide_ShouldReturnInternalServerError_WhenUnexpectedErrorOccurs()
{
    // Arrange
    var messageId = 1;
    _messageRepositoryMock
        .Setup(repo => repo.DeleteOnReceiverSide(messageId))
        .Throws(new Exception("Unexpected error during delete"));

    // Act
    var result = _messagesController.DeleteMessageOnReceiverSide(messageId);

    // Assert
    var internalServerErrorResult = result as ObjectResult;
    Assert.IsNotNull(internalServerErrorResult);
    Assert.AreEqual(500, internalServerErrorResult.StatusCode);
    Assert.AreEqual("Internal server error: Unexpected error during delete", internalServerErrorResult.Value);
}
[Test]
public void Restore_ShouldReturnOk_WhenMessageIsRestoredSuccessfully()
{
    // Arrange
    var messageId = 1;
    var userId = "user1";
    _messageRepositoryMock
        .Setup(repo => repo.Restore(messageId, userId))
        .Returns(true);

    // Act
    var result = _messagesController.Restore(messageId, userId);

    // Assert
    var okResult = result as ObjectResult;
    Assert.IsNotNull(okResult);
    Assert.AreEqual(200, okResult.StatusCode);
    Assert.AreEqual("Üzenet sikeresen visszaállítva.", okResult.Value);
}

[Test]
public void Restore_ShouldReturnBadRequest_WhenMessageNotFound()
{
    // Arrange
    var messageId = 999;
    var userId = "user1";
    _messageRepositoryMock
        .Setup(repo => repo.Restore(messageId, userId))
        .Throws(new ArgumentException("Az üzenet nem található."));

    // Act
    var result = _messagesController.Restore(messageId, userId);

    // Assert
    var badRequestResult = result as ObjectResult;
    Assert.IsNotNull(badRequestResult);
    Assert.AreEqual(400, badRequestResult.StatusCode);
    Assert.AreEqual("Bad request: Az üzenet nem található.", badRequestResult.Value);
}

[Test]
public void Restore_ShouldReturnInternalServerError_WhenUnexpectedErrorOccurs()
{
    // Arrange
    var messageId = 1;
    var userId = "user1";
    _messageRepositoryMock
        .Setup(repo => repo.Restore(messageId, userId))
        .Throws(new Exception("Unexpected error during restore"));

    // Act
    var result = _messagesController.Restore(messageId, userId);

    // Assert
    var internalServerErrorResult = result as ObjectResult;
    Assert.IsNotNull(internalServerErrorResult);
    Assert.AreEqual(500, internalServerErrorResult.StatusCode);
    Assert.AreEqual("Internal server error: Unexpected error during restore", internalServerErrorResult.Value);
}

   
    


[Test]
public void SetToUnread_ShouldReturnOk_WhenMessageIsSetToUnreadSuccessfully()
{
    // Arrange
    var messageId = 1;
    _messageRepositoryMock
        .Setup(repo => repo.SetToUnread(messageId))
        .Returns(true);

    // Act
    var result = _messagesController.SetToUnread(messageId);

    // Assert
    var okResult = result as ObjectResult;
    Assert.IsNotNull(okResult);
    Assert.AreEqual(200, okResult.StatusCode);
    Assert.AreEqual("Üzenet sikeresen olvasatlanra állítva.", okResult.Value);
}

[Test]
public void SetToUnread_ShouldReturnBadRequest_WhenMessageNotFound()
{
    // Arrange
    var messageId = 999;
    _messageRepositoryMock
        .Setup(repo => repo.SetToUnread(messageId))
        .Throws(new ArgumentException("Message with ID 999 not found."));

    // Act
    var result = _messagesController.SetToUnread(messageId);

    // Assert
    var badRequestResult = result as ObjectResult;
    Assert.IsNotNull(badRequestResult);
    Assert.AreEqual(400, badRequestResult.StatusCode);
    Assert.AreEqual("Bad request: Message with ID 999 not found.", badRequestResult.Value);
}

[Test]
public void SetToUnread_ShouldReturnInternalServerError_WhenUnexpectedErrorOccurs()
{
    // Arrange
    var messageId = 1;
    _messageRepositoryMock
        .Setup(repo => repo.SetToUnread(messageId))
        .Throws(new Exception("Unexpected error during setting to unread"));

    // Act
    var result = _messagesController.SetToUnread(messageId);

    // Assert
    var internalServerErrorResult = result as ObjectResult;
    Assert.IsNotNull(internalServerErrorResult);
    Assert.AreEqual(500, internalServerErrorResult.StatusCode);
    Assert.AreEqual("Internal server error: Unexpected error during setting to unread", internalServerErrorResult.Value);
}


[Test]
public void SetToRead_ShouldReturnOk_WhenMessageIsSetToReadSuccessfully()
{
    // Arrange
    var messageId = 1;
    _messageRepositoryMock
        .Setup(repo => repo.SetToRead(messageId))
        .Returns(true);

    // Act
    var result = _messagesController.SetToRead(messageId);

    // Assert
    var okResult = result as ObjectResult;
    Assert.IsNotNull(okResult);
    Assert.AreEqual(200, okResult.StatusCode);
    Assert.AreEqual("Üzenet sikeresen olvasottra állítva.", okResult.Value);
}

[Test]
public void SetToRead_ShouldReturnBadRequest_WhenMessageNotFound()
{
    // Arrange
    var messageId = 999;
    _messageRepositoryMock
        .Setup(repo => repo.SetToRead(messageId))
        .Throws(new ArgumentException("Message with ID 999 not found."));

    // Act
    var result = _messagesController.SetToRead(messageId);

    // Assert
    var badRequestResult = result as ObjectResult;
    Assert.IsNotNull(badRequestResult);
    Assert.AreEqual(400, badRequestResult.StatusCode);
    Assert.AreEqual("Bad request: Message with ID 999 not found.", badRequestResult.Value);
}

[Test]
public void SetToRead_ShouldReturnInternalServerError_WhenUnexpectedErrorOccurs()
{
    // Arrange
    var messageId = 1;
    _messageRepositoryMock
        .Setup(repo => repo.SetToRead(messageId))
        .Throws(new Exception("Unexpected error during setting to read"));

    // Act
    var result = _messagesController.SetToRead(messageId);

    // Assert
    var internalServerErrorResult = result as ObjectResult;
    Assert.IsNotNull(internalServerErrorResult);
    Assert.AreEqual(500, internalServerErrorResult.StatusCode);
    Assert.AreEqual("Internal server error: Unexpected error during setting to read", internalServerErrorResult.Value);
}

}