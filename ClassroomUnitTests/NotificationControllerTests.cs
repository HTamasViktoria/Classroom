using Classroom.Controllers;
using Classroom.Model.DataModels;
using Classroom.Model.DataModels.Enums;
using Classroom.Model.RequestModels;
using Classroom.Service;
using Classroom.Service.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ClassroomUnitTests;

public class NotificationControllerTests
{
    private Mock<ILogger<NotificationController>> _loggerMock;
    private Mock<INotificationRepository> _notificationRepositoryMock;
    private NotificationController _notificationController;
    private Mock<INotificationService> _notificationServiceMock;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<NotificationController>>();
        _notificationRepositoryMock = new Mock<INotificationRepository>();
        _notificationServiceMock = new Mock<INotificationService>();
        _notificationController =
            new NotificationController(_loggerMock.Object, _notificationRepositoryMock.Object,
                _notificationServiceMock.Object);
    }


    [Test]
    public void GetAll_ShouldReturnOk_WhenNotificationsExist()
    {
        // Arrange
        var notifications = new List<NotificationBase>
        {
            new NotificationBase
            {
                Id = 1, TeacherName = "John", Type = "Test", Date = DateTime.Now, DueDate = DateTime.Now.AddDays(1),
                StudentName = "Student1", ParentName = "Parent1"
            },
            new NotificationBase
            {
                Id = 2, TeacherName = "Jane", Type = "Assignment", Date = DateTime.Now,
                DueDate = DateTime.Now.AddDays(2), StudentName = "Student2", ParentName = "Parent2"
            }
        };

        _notificationRepositoryMock
            .Setup(repo => repo.GetAll())
            .Returns(notifications);

        // Act
        var result = _notificationController.GetAll();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        var returnedNotifications = okResult.Value as List<NotificationBase>;
        Assert.IsNotNull(returnedNotifications);
        Assert.AreEqual(2, returnedNotifications.Count);
    }


    [Test]
    public void GetAll_ShouldReturnEmptyList_WhenNoNotificationsExist()
    {
        // Arrange
        var emptyNotifications = new List<NotificationBase>();

        _notificationRepositoryMock
            .Setup(repo => repo.GetAll())
            .Returns(emptyNotifications);

        // Act
        var result = _notificationController.GetAll();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        var returnedNotifications = okResult.Value as List<NotificationBase>;
        Assert.IsNotNull(returnedNotifications);
        Assert.AreEqual(0, returnedNotifications.Count);
    }


    [Test]
    public void GetAll_ShouldReturnInternalServerError_WhenExceptionOccurs()
    {
        // Arrange
        _notificationRepositoryMock
            .Setup(repo => repo.GetAll())
            .Throws(new Exception("Database error"));

        // Act
        var result = _notificationController.GetAll();

        // Assert
        var objectResult = result.Result as ObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(500, objectResult.StatusCode);
        Assert.AreEqual("Internal server error: Database error", objectResult.Value);
    }


    [Test]
    public void GetByStudentId_ShouldReturnOk_WhenNotificationsExist()
    {
        // Arrange
        string studentId = "student123";
        string parentId = "parent123";

        var notifications = new List<NotificationBase>
        {
            new NotificationBase
            {
                Id = 1, TeacherName = "John", Type = "Test", Date = DateTime.Now, DueDate = DateTime.Now.AddDays(1),
                StudentName = "Student1", ParentName = "Parent1", StudentId = studentId, ParentId = parentId
            },
            new NotificationBase
            {
                Id = 2, TeacherName = "Jane", Type = "Assignment", Date = DateTime.Now,
                DueDate = DateTime.Now.AddDays(2), StudentName = "Student2", ParentName = "Parent2",
                StudentId = studentId, ParentId = parentId
            }
        };

        _notificationRepositoryMock
            .Setup(repo => repo.GetByStudentId(studentId, parentId))
            .Returns(notifications);

        // Act
        var result = _notificationController.GetByStudentId(studentId, parentId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        var returnedNotifications = okResult.Value as List<NotificationBase>;
        Assert.IsNotNull(returnedNotifications);
        Assert.AreEqual(2, returnedNotifications.Count);
    }


    [Test]
    public void GetByStudentId_ShouldReturnEmptyList_WhenNoNotificationsExist()
    {
        // Arrange
        string studentId = "student123";
        string parentId = "parent123";

        var emptyNotifications = new List<NotificationBase>();

        _notificationRepositoryMock
            .Setup(repo => repo.GetByStudentId(studentId, parentId))
            .Returns(emptyNotifications);

        // Act
        var result = _notificationController.GetByStudentId(studentId, parentId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        var returnedNotifications = okResult.Value as List<NotificationBase>;
        Assert.IsNotNull(returnedNotifications);
        Assert.AreEqual(0, returnedNotifications.Count);
    }


    [Test]
    public void GetByStudentId_ShouldReturnInternalServerError_WhenExceptionOccurs()
    {
        // Arrange
        string studentId = "student123";
        string parentId = "parent123";

        _notificationRepositoryMock
            .Setup(repo => repo.GetByStudentId(studentId, parentId))
            .Throws(new Exception("Database error"));

        // Act
        var result = _notificationController.GetByStudentId(studentId, parentId);

        // Assert
        var objectResult = result.Result as ObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(500, objectResult.StatusCode);
        Assert.AreEqual("Internal server error: Database error", objectResult.Value);
    }


    [Test]
    public void GetByTeacherId_ShouldReturnOk_WhenNotificationsExist()
    {
        // Arrange
        string teacherId = "teacher123";
        var notifications = new List<NotificationBase>
        {
            new NotificationBase
            {
                Id = 1, TeacherName = "Teacher 1", Type = "Test", Date = DateTime.Now,
                DueDate = DateTime.Now.AddDays(1), TeacherId = teacherId
            },
            new NotificationBase
            {
                Id = 2, TeacherName = "Teacher 1", Type = "Assignment", Date = DateTime.Now,
                DueDate = DateTime.Now.AddDays(2), TeacherId = teacherId
            }
        };

        _notificationRepositoryMock
            .Setup(repo => repo.GetByTeacherId(teacherId))
            .Returns(notifications);

        // Act
        var result = _notificationController.GetByTeacherId(teacherId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        var returnedNotifications = okResult.Value as List<NotificationBase>;
        Assert.IsNotNull(returnedNotifications);
        Assert.AreEqual(2, returnedNotifications.Count);
    }

    
    [Test]
    public void GetByTeacherId_ShouldReturnEmptyList_WhenNoNotificationsExist()
    {
        // Arrange
        string teacherId = "teacher123";
        var emptyNotifications = new List<NotificationBase>();

        _notificationRepositoryMock
            .Setup(repo => repo.GetByTeacherId(teacherId))
            .Returns(emptyNotifications);

        // Act
        var result = _notificationController.GetByTeacherId(teacherId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        var returnedNotifications = okResult.Value as List<NotificationBase>;
        Assert.IsNotNull(returnedNotifications);
        Assert.AreEqual(0, returnedNotifications.Count);
    }

    
    [Test]
    public void GetByTeacherId_ShouldReturnBadRequest_WhenInvalidId()
    {
        // Arrange
        string invalidTeacherId = "invalidTeacherId";

        _notificationRepositoryMock
            .Setup(repo => repo.GetByTeacherId(invalidTeacherId))
            .Throws(new ArgumentException($"Nincs ilyen azonosító:{invalidTeacherId}"));

        // Act
        var result = _notificationController.GetByTeacherId(invalidTeacherId);

        // Assert
        var badRequestResult = result.Result as ObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
        Assert.AreEqual("Bad request:Nincs ilyen azonosító:invalidTeacherId", badRequestResult.Value);
    }

    
    [Test]
    public void GetByTeacherId_ShouldReturnInternalServerError_WhenExceptionOccurs()
    {
        // Arrange
        string teacherId = "teacher123";
        _notificationRepositoryMock
            .Setup(repo => repo.GetByTeacherId(teacherId))
            .Throws(new Exception("Database error"));

        // Act
        var result = _notificationController.GetByTeacherId(teacherId);

        // Assert
        var objectResult = result.Result as ObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(500, objectResult.StatusCode);
        Assert.AreEqual("Internal server error: Database error", objectResult.Value);
    }


    [Test]
    public void GetLastsByStudentId_ShouldReturnOk_WhenNotificationsExist()
    {
        // Arrange
        string studentId = "student123";
        string parentId = "parent123";
        var notifications = new List<NotificationBase>
        {
            new NotificationBase
            {
                Id = 1, StudentId = studentId, ParentId = parentId, Type = "Test", Date = DateTime.Now, Read = false
            },
            new NotificationBase
            {
                Id = 2, StudentId = studentId, ParentId = parentId, Type = "Assignment", Date = DateTime.Now,
                Read = false
            },
            new NotificationBase
            {
                Id = 3, StudentId = studentId, ParentId = parentId, Type = "Grade", Date = DateTime.Now, Read = false
            }
        };

        _notificationRepositoryMock
            .Setup(repo => repo.GetLastsByStudentId(studentId, parentId))
            .Returns(notifications);

        // Act
        var result = _notificationController.GetLastsByStudentId(studentId, parentId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        var returnedNotifications = okResult.Value as List<NotificationBase>;
        Assert.IsNotNull(returnedNotifications);
        Assert.AreEqual(3, returnedNotifications.Count);
    }


    [Test]
    public void GetLastsByStudentId_ShouldReturnEmptyList_WhenNoNotificationsExist()
    {
        // Arrange
        string studentId = "student123";
        string parentId = "parent123";
        var emptyNotifications = new List<NotificationBase>();

        _notificationRepositoryMock
            .Setup(repo => repo.GetLastsByStudentId(studentId, parentId))
            .Returns(emptyNotifications);

        // Act
        var result = _notificationController.GetLastsByStudentId(studentId, parentId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        var returnedNotifications = okResult.Value as List<NotificationBase>;
        Assert.IsNotNull(returnedNotifications);
        Assert.AreEqual(0, returnedNotifications.Count);
    }


    [Test]
    public void GetLastsByStudentId_ShouldReturnBadRequest_WhenInvalidIds()
    {
        // Arrange
        string invalidStudentId = "invalidStudentId";
        string invalidParentId = "invalidParentId";

        _notificationRepositoryMock
            .Setup(repo => repo.GetLastsByStudentId(invalidStudentId, invalidParentId))
            .Throws(new ArgumentException($"Érvénytelen azonosító: {invalidStudentId} vagy {invalidParentId}"));

        // Act
        var result = _notificationController.GetLastsByStudentId(invalidStudentId, invalidParentId);

        // Assert
        var badRequestResult = result.Result as ObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
        Assert.AreEqual("Bad request:Érvénytelen azonosító: invalidStudentId vagy invalidParentId",
            badRequestResult.Value);
    }


    [Test]
    public void GetLastsByStudentId_ShouldReturnInternalServerError_WhenExceptionOccurs()
    {
        // Arrange
        string studentId = "student123";
        string parentId = "parent123";
        _notificationRepositoryMock
            .Setup(repo => repo.GetLastsByStudentId(studentId, parentId))
            .Throws(new Exception("Database error"));

        // Act
        var result = _notificationController.GetLastsByStudentId(studentId, parentId);

        // Assert
        var objectResult = result.Result as ObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(500, objectResult.StatusCode);
        Assert.AreEqual("Internal server error: Database error", objectResult.Value);
    }


    [Test]
    public void GetNewNotifsNumber_ShouldReturnOk_WhenNewNotificationsExist()
    {
        // Arrange
        string studentId = "student123";
        string parentId = "parent123";
        int newNotificationsCount = 5;

        _notificationRepositoryMock
            .Setup(repo => repo.GetNewNotifsNumber(studentId, parentId))
            .Returns(newNotificationsCount);

        // Act
        var result = _notificationController.GetNewNotifsNumber(studentId, parentId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(newNotificationsCount, okResult.Value);
    }


    [Test]
    public void GetNewNotifsNumber_ShouldReturnBadRequest_WhenInvalidIds()
    {
        // Arrange
        string invalidStudentId = "invalidStudentId";
        string invalidParentId = "invalidParentId";

        _notificationRepositoryMock
            .Setup(repo => repo.GetNewNotifsNumber(invalidStudentId, invalidParentId))
            .Throws(new ArgumentException($"Érvénytelen azonosító: {invalidStudentId} vagy {invalidParentId}"));

        // Act
        var result = _notificationController.GetNewNotifsNumber(invalidStudentId, invalidParentId);

        // Assert
        var badRequestResult = result.Result as ObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
        Assert.AreEqual("Bad request:Érvénytelen azonosító: invalidStudentId vagy invalidParentId",
            badRequestResult.Value);
    }


    [Test]
    public void GetNewNotifsNumber_ShouldReturnInternalServerError_WhenExceptionOccurs()
    {
        // Arrange
        string studentId = "student123";
        string parentId = "parent123";
        _notificationRepositoryMock
            .Setup(repo => repo.GetNewNotifsNumber(studentId, parentId))
            .Throws(new Exception("Database error"));

        // Act
        var result = _notificationController.GetNewNotifsNumber(studentId, parentId);

        // Assert
        var objectResult = result.Result as ObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(500, objectResult.StatusCode);
        Assert.AreEqual("Internal server error: Database error", objectResult.Value);
    }


    [Test]
    public void GetNewNotifsByStudentId_ShouldReturnNotifications_WhenValidIds()
    {
        // Arrange
        string studentId = "validStudentId";
        string parentId = "validParentId";
        var notifications = new List<NotificationBase>
        {
            new NotificationBase
                { Id = 1, StudentId = studentId, ParentId = parentId, Description = "New notification" }
        };

        _notificationRepositoryMock
            .Setup(repo => repo.GetNewNotifsByStudentId(studentId, parentId))
            .Returns(notifications);

        // Act
        var result = _notificationController.GetNewNotifsByStudentId(studentId, parentId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(notifications, okResult.Value);
    }

    [Test]
    public void GetNewNotifsByStudentId_ShouldReturnEmptyList_WhenNoNewNotifications()
    {
        // Arrange
        string studentId = "validStudentId";
        string parentId = "validParentId";
        var notifications = new List<NotificationBase>();

        _notificationRepositoryMock
            .Setup(repo => repo.GetNewNotifsByStudentId(studentId, parentId))
            .Returns(notifications);

        // Act
        var result = _notificationController.GetNewNotifsByStudentId(studentId, parentId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(notifications, okResult.Value);
    }

    [Test]
    public void GetNewNotifsByStudentId_ShouldReturnBadRequest_WhenInvalidIds()
    {
        // Arrange
        string invalidStudentId = "invalidStudentId";
        string invalidParentId = "invalidParentId";

        _notificationRepositoryMock
            .Setup(repo => repo.GetNewNotifsByStudentId(invalidStudentId, invalidParentId))
            .Throws(new ArgumentException($"Érvénytelen azonosító: {invalidStudentId} vagy {invalidParentId}"));

        // Act
        var result = _notificationController.GetNewNotifsByStudentId(invalidStudentId, invalidParentId);

        // Assert
        var badRequestResult = result.Result as ObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
        Assert.AreEqual("Bad request:Érvénytelen azonosító: invalidStudentId vagy invalidParentId",
            badRequestResult.Value);
    }

    [Test]
    public void GetNewNotifsByStudentId_ShouldReturnInternalServerError_WhenExceptionOccurs()
    {
        // Arrange
        string studentId = "validStudentId";
        string parentId = "validParentId";

        _notificationRepositoryMock
            .Setup(repo => repo.GetNewNotifsByStudentId(studentId, parentId))
            .Throws(new Exception("Internal server error"));

        // Act
        var result = _notificationController.GetNewNotifsByStudentId(studentId, parentId);

        // Assert
        var internalServerErrorResult = result.Result as ObjectResult;
        Assert.IsNotNull(internalServerErrorResult);
        Assert.AreEqual(500, internalServerErrorResult.StatusCode);
        Assert.AreEqual("Internal server error: Internal server error", internalServerErrorResult.Value);
    }


    [Test]
    public void GetLastsByTeacherId_ShouldReturnNotifications_WhenValidId()
    {
        // Arrange
        string teacherId = "validTeacherId";
        var notifications = new List<NotificationResponse>
        {
            new NotificationResponse
            {
                Id = 1,
                TeacherId = teacherId,
                TeacherName = "Teacher One",
                Description = "New notification"
            }
        };

        _notificationRepositoryMock
            .Setup(repo => repo.GetLastsByTeacherId(teacherId))
            .Returns(notifications);

        // Act
        var result = _notificationController.GetLastsByTeacherId(teacherId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(notifications, okResult.Value);
    }

    [Test]
    public void GetLastsByTeacherId_ShouldReturnEmptyList_WhenNoNotifications()
    {
        // Arrange
        string teacherId = "validTeacherId";
        var notifications = new List<NotificationResponse>();

        _notificationRepositoryMock
            .Setup(repo => repo.GetLastsByTeacherId(teacherId))
            .Returns(notifications);

        // Act
        var result = _notificationController.GetLastsByTeacherId(teacherId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(notifications, okResult.Value);
    }

    [Test]
    public void GetLastsByTeacherId_ShouldReturnBadRequest_WhenInvalidId()
    {
        // Arrange
        string invalidTeacherId = "invalidTeacherId";

        _notificationRepositoryMock
            .Setup(repo => repo.GetLastsByTeacherId(invalidTeacherId))
            .Throws(new ArgumentException($"Érvénytelen azonosító: {invalidTeacherId}"));

        // Act
        var result = _notificationController.GetLastsByTeacherId(invalidTeacherId);

        // Assert
        var badRequestResult = result.Result as ObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
        Assert.AreEqual("Bad request:Érvénytelen azonosító: invalidTeacherId", badRequestResult.Value);
    }

    [Test]
    public void GetLastsByTeacherId_ShouldReturnInternalServerError_WhenExceptionOccurs()
    {
        // Arrange
        string teacherId = "validTeacherId";

        _notificationRepositoryMock
            .Setup(repo => repo.GetLastsByTeacherId(teacherId))
            .Throws(new Exception("Internal server error"));

        // Act
        var result = _notificationController.GetLastsByTeacherId(teacherId);

        // Assert
        var internalServerErrorResult = result.Result as ObjectResult;
        Assert.IsNotNull(internalServerErrorResult);
        Assert.AreEqual(500, internalServerErrorResult.StatusCode);
        Assert.AreEqual("Internal server error: Internal server error", internalServerErrorResult.Value);
    }


    [Test]
    public void GetNewestByTeacherId_ShouldReturnNotification_WhenValidId()
    {
        // Arrange
        string teacherId = "validTeacherId";
        var notification = new NotificationBase
        {
            Id = 1,
            TeacherId = teacherId,
            TeacherName = "Teacher One",
            Description = "Newest notification"
        };

        _notificationRepositoryMock
            .Setup(repo => repo.GetNewestByTeacherId(teacherId))
            .Returns(notification);

        // Act
        var result = _notificationController.GetNewestByTeacherId(teacherId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(notification, okResult.Value);
    }


    [Test]
    public void GetNewestByTeacherId_ShouldReturnEmptyNotification_WhenNoNotifications()
    {
        // Arrange
        string teacherId = "validTeacherId";

        _notificationRepositoryMock
            .Setup(repo => repo.GetNewestByTeacherId(teacherId))
            .Returns((NotificationBase?)null);

        // Act
        var result = _notificationController.GetNewestByTeacherId(teacherId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var returnedNotification = okResult.Value as NotificationBase;
        Assert.IsNotNull(returnedNotification);


        Assert.IsInstanceOf<NotificationBase>(returnedNotification);


        Assert.AreEqual(0, returnedNotification.Id);
        Assert.IsNull(returnedNotification.TeacherId);
        Assert.IsNull(returnedNotification.TeacherName);
        Assert.IsNull(returnedNotification.Type);
        Assert.AreEqual(default(DateTime), returnedNotification.Date);
        Assert.AreEqual(default(DateTime), returnedNotification.DueDate);
        Assert.IsNull(returnedNotification.StudentId);
        Assert.IsNull(returnedNotification.StudentName);
        Assert.IsNull(returnedNotification.ParentId);
        Assert.IsNull(returnedNotification.ParentName);
        Assert.IsNull(returnedNotification.Description);
        Assert.IsNull(returnedNotification.SubjectName);
        Assert.AreEqual(false, returnedNotification.Read);
        Assert.AreEqual(false, returnedNotification.OfficiallyRead);
        Assert.IsNull(returnedNotification.OptionalDescription);
    }


    [Test]
    public void GetNewestByTeacherId_ShouldReturnBadRequest_WhenInvalidId()
    {
        // Arrange
        string invalidTeacherId = "invalidTeacherId";

        _notificationRepositoryMock
            .Setup(repo => repo.GetNewestByTeacherId(invalidTeacherId))
            .Throws(new ArgumentException($"Érvénytelen azonosító: {invalidTeacherId}"));

        // Act
        var result = _notificationController.GetNewestByTeacherId(invalidTeacherId);

        // Assert
        var badRequestResult = result.Result as ObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
        Assert.AreEqual("Bad request:Érvénytelen azonosító: invalidTeacherId", badRequestResult.Value);
    }

    [Test]
    public void GetNewestByTeacherId_ShouldReturnInternalServerError_WhenExceptionOccurs()
    {
        // Arrange
        string teacherId = "validTeacherId";

        _notificationRepositoryMock
            .Setup(repo => repo.GetNewestByTeacherId(teacherId))
            .Throws(new Exception("Internal server error"));

        // Act
        var result = _notificationController.GetNewestByTeacherId(teacherId);

        // Assert
        var internalServerErrorResult = result.Result as ObjectResult;
        Assert.IsNotNull(internalServerErrorResult);
        Assert.AreEqual(500, internalServerErrorResult.StatusCode);
        Assert.AreEqual("Internal server error: Internal server error", internalServerErrorResult.Value);
    }


    [Test]
    public void Post_ShouldReturnCreated_WhenNotificationIsValid()
    {
        // Arrange
        var notificationRequest = new NotificationRequest
        {
            Date = DateTime.Now,
            DueDate = DateTime.Now.AddDays(1),
            StudentIds = new List<string> { "student1", "student2" },
            Description = "Test Notification Description",
            Type = "Homework",
            TeacherId = "teacher1",
            TeacherName = "Mr. Smith",
            Read = false,
            OfficiallyRead = false
        };

        _notificationServiceMock
            .Setup(service => service.PostToDb(It.IsAny<NotificationRequest>()))
            .Verifiable();

        // Act
        var result = _notificationController.Post(notificationRequest);

        // Assert
        var createdResult = result as CreatedAtActionResult;
        Assert.IsNotNull(createdResult);
        Assert.AreEqual(201, createdResult.StatusCode);

        var responseValue = createdResult.Value as Object;
        Assert.IsNotNull(responseValue);

        var messageProperty = responseValue.GetType().GetProperty("message");
        Assert.IsNotNull(messageProperty);
        var messageValue = messageProperty.GetValue(responseValue);
        Assert.AreEqual("Értesítés sikeresen elmentve az adatbázisba.", messageValue);

        _notificationServiceMock.Verify(service => service.PostToDb(It.IsAny<NotificationRequest>()), Times.Once);
    }


    [Test]
    public void Post_WhenArgumentExceptionIsThrown_ReturnsBadRequest()
    {
        var notificationRequest = new NotificationRequest
        {
            Date = default,
            StudentIds = null,
            Description = null,
            Type = "Exam",
            Subject = null
        };

        _notificationServiceMock
            .Setup(service => service.PostToDb(It.IsAny<NotificationRequest>()))
            .Throws(new ArgumentException("A 'Date' mező kötelező."));


        var result = _notificationController.Post(notificationRequest);

        var badRequestResult = result as ObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);


        var errorMessage = badRequestResult.Value as string;
        Assert.IsNotNull(errorMessage);
        Assert.AreEqual("Bad request:A 'Date' mező kötelező.", errorMessage);
    }
    
    
    [Test]
    public void Post_WhenExceptionIsThrown_ReturnsInternalServerError()
    {
        var notificationRequest = new NotificationRequest
        {
            Date = DateTime.Now,
            StudentIds = new List<string> { "1","2"},
            Description = "Test description",
            Type = "Exam",
            Subject = "Math"
        };

        _notificationServiceMock
            .Setup(service => service.PostToDb(It.IsAny<NotificationRequest>()))
            .Throws(new Exception("Valami hiba történt az adatbázis művelet során."));

      
        var result = _notificationController.Post(notificationRequest);

       
        var internalServerErrorResult = result as ObjectResult;
        Assert.IsNotNull(internalServerErrorResult); 
        Assert.AreEqual(500, internalServerErrorResult.StatusCode);

        
        var errorMessage = internalServerErrorResult.Value as string;
        Assert.IsNotNull(errorMessage);
        Assert.AreEqual("Internal server error: Valami hiba történt az adatbázis művelet során.", errorMessage); 
    }

 
    
    
    [Test]
    public void SetToRead_WhenNotificationFound_ReturnsOkWithMessage()
    {
        // Arrange
        var notificationId = 1;
        var notification = new NotificationBase 
        {
            Id = notificationId,
            Read = false,
            TeacherId = "teacher1",
            TeacherName = "Teacher One",
            Type = "Homework",
            Date = DateTime.Now,
            DueDate = DateTime.Now.AddDays(1),
            StudentId = "student1",
            StudentName = "Student One",
            ParentId = "parent1",
            ParentName = "Parent One",
            Description = "Test Description",
            Subject = Subjects.Matematika,
            OfficiallyRead = false
        };

        _notificationRepositoryMock.Setup(repo => repo.SetToRead(notificationId))
            .Callback<int>(id => notification.Read = !notification.Read);
    
        // Act
        var result = _notificationController.SetToRead(notificationId);

        // Assert
        var okResult = result as ObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual("Értesítés olvasottra állítva", okResult.Value);
    }



    [Test]
    public void SetToRead_WhenNotificationNotFound_ThrowsArgumentException()
    {
        // Arrange
        var notificationId = 1;
        _notificationRepositoryMock.Setup(repo => repo.SetToRead(notificationId))
            .Throws(new ArgumentException("Értesítés nem található."));
    
        // Act
        var result = _notificationController.SetToRead(notificationId);

        // Assert
        var badRequestResult = result as ObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
        Assert.AreEqual("Bad request:Értesítés nem található.", badRequestResult.Value);
    }

    
    
    
    
    [Test]
    public void SetToRead_WhenUnexpectedErrorOccurs_ReturnsInternalServerError()
    {
        // Arrange
        var notificationId = 1;
        _notificationRepositoryMock.Setup(repo => repo.SetToRead(notificationId))
            .Throws(new Exception("Unexpected error"));
    
        // Act
        var result = _notificationController.SetToRead(notificationId);

        // Assert
        var errorResult = result as ObjectResult;
        Assert.IsNotNull(errorResult);
        Assert.AreEqual(500, errorResult.StatusCode);
        Assert.AreEqual("Internal server error: Unexpected error", errorResult.Value);
    }

    
    [Test]
    public void SetToOfficiallyRead_WhenNotificationFound_ReturnsOkWithMessage()
    {
        // Arrange
        var notificationId = 1;
        var notification = new NotificationBase 
        {
            Id = notificationId,
            Read = false,
            TeacherId = "teacher1",
            TeacherName = "Teacher One",
            Type = "Homework",
            Date = DateTime.Now,
            DueDate = DateTime.Now.AddDays(1),
            StudentId = "student1",
            StudentName = "Student One",
            ParentId = "parent1",
            ParentName = "Parent One",
            Description = "Test Description",
            Subject = Subjects.Matematika,
            OfficiallyRead = false
        };

        _notificationRepositoryMock.Setup(repo => repo.SetToOfficiallyRead(notificationId))
            .Callback<int>(id => notification.OfficiallyRead = !notification.OfficiallyRead);
    
        // Act
        var result = _notificationController.SetToOfficiallyRead(notificationId);

        // Assert
        var okResult = result as ObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual("Értesítés olvasottra állítva", okResult.Value);
    }

    
    
    [Test]
    public void SetToOfficiallyRead_WhenNotificationNotFound_ThrowsArgumentException()
    {
        // Arrange
        var notificationId = 1;
        _notificationRepositoryMock.Setup(repo => repo.SetToOfficiallyRead(notificationId))
            .Throws(new ArgumentException("Értesítés nem található."));
    
        // Act
        var result = _notificationController.SetToOfficiallyRead(notificationId);

        // Assert
        var badRequestResult = result as ObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
        Assert.AreEqual("Bad request:Értesítés nem található.", badRequestResult.Value);
    }

    
    
    [Test]
    public void SetToOfficiallyRead_WhenUnexpectedErrorOccurs_ReturnsInternalServerError()
    {
        
        var notificationId = 1;
        _notificationRepositoryMock.Setup(repo => repo.SetToOfficiallyRead(notificationId))
            .Throws(new Exception("Unexpected error"));
    
       
        var result = _notificationController.SetToOfficiallyRead(notificationId);

      
        var errorResult = result as ObjectResult;
        Assert.IsNotNull(errorResult);
        Assert.AreEqual(500, errorResult.StatusCode);
        Assert.AreEqual("Internal server error: Unexpected error", errorResult.Value);
    }
}