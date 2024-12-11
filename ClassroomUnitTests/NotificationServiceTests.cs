using Classroom.Model.RequestModels;
using Classroom.Service;
using Moq;
using Microsoft.Extensions.Logging;
using Classroom.Service.Repositories;


namespace ClassroomUnitTests;

public class NotificationServiceTests
{
    private Mock<ILogger<NotificationService>> _loggerMock;
    private Mock<INotificationRepository> _notificationRepositoryMock;
    private NotificationService _notificationService;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<NotificationService>>();
        _notificationRepositoryMock = new Mock<INotificationRepository>();
        _notificationService = new NotificationService(_loggerMock.Object, _notificationRepositoryMock.Object);
    }


    [Test]
    public void PostToDb_MissingDate_ThrowsArgumentExceptionAndLogsError()
    {
        // Arrange
        var request = new NotificationRequest
        {
            Date = default,
            StudentIds = new List<string> { "student1", "student2" },
            Description = "Test description",
            Type = "Exam",
            Subject = "Math"
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => _notificationService.PostToDb(request));
        Assert.AreEqual("A 'Date' mező kötelező.", ex.Message);
    }


    [Test]
    public void PostToDb_MissingDate_ThrowsArgumentException()
    {
        // Arrange
        var request = new NotificationRequest
        {
            Date = default,
            StudentIds = new List<string> { "student1", "student2" },
            Description = "Test description",
            Type = "Exam",
            Subject = "Math"
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => _notificationService.PostToDb(request));
        Assert.AreEqual("A 'Date' mező kötelező.", ex.Message);
    }

    [Test]
    public void PostToDb_MissingStudents_ThrowsArgumentException()
    {
        // Arrange
        var request = new NotificationRequest
        {
            Date = DateTime.Now,
            StudentIds = null,
            Description = "Test description",
            Type = "Exam",
            Subject = "Math"
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => _notificationService.PostToDb(request));
        Assert.AreEqual("A 'Students' mező kötelező.", ex.Message);
    }

    [Test]
    public void PostToDb_MissingDescription_ThrowsArgumentException()
    {
        // Arrange
        var request = new NotificationRequest
        {
            Date = DateTime.Now,
            StudentIds = new List<string> { "student1", "student2" },
            Description = null,
            Type = "Exam",
            Subject = "Math"
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => _notificationService.PostToDb(request));
        Assert.AreEqual("A 'Description' mező kötelező.", ex.Message);
    }

    [Test]
    public void PostToDb_InvalidSubjectForExam_ThrowsArgumentException()
    {
        // Arrange
        var request = new NotificationRequest
        {
            Date = DateTime.Now,
            StudentIds = new List<string> { "student1", "student2" },
            Description = "Test description",
            Type = "Exam",
            Subject = "InvalidSubject"
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => _notificationService.PostToDb(request));
        Assert.AreEqual("A 'Subject' mező értéke érvénytelen.", ex.Message);
    }

    [Test]
    public void PostToDb_MissingSubjectForExam_ThrowsArgumentException()
    {
        // Arrange
        var request = new NotificationRequest
        {
            Date = DateTime.Now,
            StudentIds = new List<string> { "student1", "student2" },
            Description = "Test description",
            Type = "Exam",
            Subject = null
        };

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => _notificationService.PostToDb(request));
        Assert.AreEqual("A 'Subject' mező kötelező az 'Exam', 'Homework' és 'MissingEquipment' típusú értesítéseknél.",
            ex.Message);
    }
}