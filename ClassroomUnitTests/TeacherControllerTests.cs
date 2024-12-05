using Classroom.Controllers;
using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Classroom.Service.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;

namespace ClassroomTests;

public class TeacherControllerTests
{
    private Mock<ILogger<TeacherController>> _loggerMock;
    private Mock<ITeacherRepository> _teacherRepositoryMock;
    private TeacherController _teacherController;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<TeacherController>>();
        _teacherRepositoryMock = new Mock<ITeacherRepository>();
        _teacherController = new TeacherController(_loggerMock.Object, _teacherRepositoryMock.Object);
    }

    [Test]
    public void GetByTeacherId_ReturnsTeacher_WhenTeacherExists()
    {
        // Arrange
        var teacherId = "teacher123";
        var expectedTeacher = new Teacher
        {
            Id = teacherId,
            FirstName = "John",
            FamilyName = "Doe",
            Role = "Teacher"
        };

        _teacherRepositoryMock
            .Setup(repo => repo.GetTeacherById(teacherId))
            .Returns(expectedTeacher);

        // Act
        var result = _teacherController.GetByTeacherId(teacherId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.AreEqual(200, okResult?.StatusCode);

        var teacher = okResult?.Value as Teacher;
        Assert.NotNull(teacher);
        Assert.AreEqual(teacherId, teacher?.Id);
        Assert.AreEqual("John", teacher?.FirstName);
        Assert.AreEqual("Doe", teacher?.FamilyName);
        Assert.AreEqual("Teacher", teacher?.Role);
    }

    [Test]
    public void GetByTeacherId_ReturnsNotFound_WhenTeacherDoesNotExist()
    {
        // Arrange
        var teacherId = "nonExistentTeacherId";

        _teacherRepositoryMock
            .Setup(repo => repo.GetTeacherById(teacherId))
            .Returns((Teacher)null);

        // Act
        var result = _teacherController.GetByTeacherId(teacherId);

        // Assert
        var notFoundResult = result.Result as ObjectResult;
        Assert.NotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult?.StatusCode);

        var response = notFoundResult?.Value as string;
        Assert.NotNull(response);
        Assert.AreEqual("Bad request: Teacher with this id not found.", response);
    }

    [Test]
    public void GetByTeacherId_ReturnsInternalServerError_WhenExceptionOccurs()
    {
        // Arrange
        var teacherId = "validTeacherId";

        _teacherRepositoryMock
            .Setup(repo => repo.GetTeacherById(teacherId))
            .Throws(new Exception("Database error"));

        // Act
        var result = _teacherController.GetByTeacherId(teacherId);

        // Assert
        var statusCodeResult = result.Result as ObjectResult;
        Assert.NotNull(statusCodeResult);
        Assert.AreEqual(500, statusCodeResult?.StatusCode);

        var response = statusCodeResult?.Value as string;
        Assert.NotNull(response);
        Assert.AreEqual("Internal server error: Database error", response);
    }

    [Test]
    public void GetByTeacherId_InvalidWhitespaceId_ReturnsBadRequest()
    {
        // Arrange
        var invalidId = "   ";

        // Act
        var result = _teacherController.GetByTeacherId(invalidId);

        // Assert
        var badRequestResult = result.Result as ObjectResult;
        Assert.NotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult?.StatusCode);

        var response = badRequestResult?.Value as string;
        Assert.NotNull(response);
        Assert.AreEqual("Bad request:The given identifier cannot be null, empty or whitespace.", response);
    }

    [Test]
    public void GetAll_HappyPath_ReturnsListOfTeachers()
    {
        // Arrange
        var teachers = new List<Teacher>
        {
            new Teacher { Id = "teacher1", FirstName = "John", FamilyName = "Doe" },
            new Teacher { Id = "teacher2", FirstName = "Jane", FamilyName = "Smith" }
        };

        _teacherRepositoryMock.Setup(repo => repo.GetAll()).Returns(teachers);

        // Act
        var result = _teacherController.GetAll();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.AreEqual(200, okResult?.StatusCode);

        var returnedTeachers = okResult?.Value as IEnumerable<Teacher>;
        Assert.NotNull(returnedTeachers);
        Assert.AreEqual(2, returnedTeachers.Count());
        Assert.AreEqual("teacher1", returnedTeachers.ElementAt(0).Id);
        Assert.AreEqual("John", returnedTeachers.ElementAt(0).FirstName);
        Assert.AreEqual("Doe", returnedTeachers.ElementAt(0).FamilyName);
        Assert.AreEqual("teacher2", returnedTeachers.ElementAt(1).Id);
        Assert.AreEqual("Jane", returnedTeachers.ElementAt(1).FirstName);
        Assert.AreEqual("Smith", returnedTeachers.ElementAt(1).FamilyName);
    }

    [Test]
    public void GetAll_NoTeachers_ReturnsEmptyList()
    {
        // Arrange
        var teachers = new List<Teacher>();

        _teacherRepositoryMock.Setup(repo => repo.GetAll()).Returns(teachers);

        // Act
        var result = _teacherController.GetAll();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.AreEqual(200, okResult?.StatusCode);

        var returnedTeachers = okResult?.Value as IEnumerable<Teacher>;
        Assert.NotNull(returnedTeachers);
        Assert.IsEmpty(returnedTeachers);
    }

    [Test]
    public void GetAll_InternalServerError_ReturnsStatusCode500()
    {
        // Arrange
        _teacherRepositoryMock.Setup(repo => repo.GetAll()).Throws(new Exception("Database error"));

        // Act
        var result = _teacherController.GetAll();

        // Assert
        var statusCodeResult = result.Result as ObjectResult;
        Assert.NotNull(statusCodeResult);
        Assert.AreEqual(500, statusCodeResult?.StatusCode);

        var message = statusCodeResult?.Value as string;
        Assert.AreEqual("Internal server error: Database error", message);
    }

    [Test]
    public void Post_ValidRequest_Returns201Created()
    {
        // Arrange
        var teacherRequest = new TeacherRequest
        {
            FirstName = "John",
            FamilyName = "Doe",
            Email = "john.doe@example.com",
            Username = "johndoe123",
            Password = "password123"
        };

        _teacherRepositoryMock.Setup(repo => repo.Add(It.IsAny<TeacherRequest>())).Verifiable();

        // Act
        var result = _teacherController.Post(teacherRequest);

        // Assert
        var createdResult = result.Result as CreatedAtActionResult;
        Assert.NotNull(createdResult);
        Assert.AreEqual(201, createdResult?.StatusCode);
        Assert.AreEqual("Successfully added new teacher", createdResult?.Value);

        _teacherRepositoryMock.Verify();
    }

    [Test]
    public void Post_UnexpectedError_ReturnsInternalServerError()
    {
        // Arrange
        var teacherRequest = new TeacherRequest
        {
            FirstName = "John",
            FamilyName = "Doe",
            Email = "john.doe@example.com",
            Username = "johndoe123",
            Password = "password123"
        };

        _teacherRepositoryMock.Setup(repo => repo.Add(It.IsAny<TeacherRequest>()))
            .Throws(new Exception("Unexpected error"));

        // Act
        var result = _teacherController.Post(teacherRequest);

        // Assert
        var objectResult = result.Result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.AreEqual(500, objectResult?.StatusCode);

        var response = objectResult?.Value as string;
        Assert.NotNull(response);
        Assert.AreEqual("Internal server error: Unexpected error", response);
    }

    [Test]
    public void Post_ExistingTeacher_ReturnsBadRequest()
    {
        // Arrange
        var teacherRequest = new TeacherRequest
        {
            FirstName = "John",
            FamilyName = "Doe",
            Email = "john.doe@example.com",
            Username = "johndoe123",
            Password = "password123"
        };

        _teacherRepositoryMock.Setup(repo => repo.Add(It.IsAny<TeacherRequest>()))
            .Throws(new ArgumentException("Teacher already added"));

        // Act
        var result = _teacherController.Post(teacherRequest);

        // Assert
        var objectResult = result.Result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.AreEqual(400, objectResult?.StatusCode);

        var response = objectResult?.Value as string;
        Assert.NotNull(response);
        Assert.AreEqual("Bad request:Teacher already added", response);
    }

    [Test]
    public void Post_DbUpdateException_ReturnsInternalServerError()
    {
        // Arrange
        var teacherRequest = new TeacherRequest
        {
            FirstName = "John",
            FamilyName = "Doe",
            Email = "john.doe@example.com",
            Username = "johndoe123",
            Password = "password123"
        };

        _teacherRepositoryMock.Setup(repo => repo.Add(It.IsAny<TeacherRequest>()))
            .Throws(new DbUpdateException("Database update error"));

        // Act
        var result = _teacherController.Post(teacherRequest);

        // Assert
        var objectResult = result.Result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.AreEqual(500, objectResult?.StatusCode);

        var response = objectResult?.Value as string;
        Assert.NotNull(response);
        Assert.AreEqual("Internal server error: Database update error", response);
    }
}
