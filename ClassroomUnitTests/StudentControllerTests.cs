using Classroom.Controllers;
using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Moq;
using Classroom.Service.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClassroomUnitTests;

public class StudentControllerTests
{
    private Mock<ILogger<StudentController>> _loggerMock;
    private Mock<IStudentRepository> _studentRepositoryMock;
    private StudentController _studentController;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<StudentController>>();
        _studentRepositoryMock = new Mock<IStudentRepository>();
        _studentController =
            new StudentController(_loggerMock.Object, _studentRepositoryMock.Object);
    }
    
    
    [Test]
    public void GetAll_ShouldReturnOk_WhenStudentsExist()
    {
        // Arrange
        var students = new List<Student>
        {
            new Student { Id = "1", FirstName = "John", FamilyName = "Doe" },
            new Student { Id = "2", FirstName = "Jane", FamilyName = "Smith" }
        };
        
        _studentRepositoryMock
            .Setup(repo => repo.GetAll())
            .Returns(students);

        // Act
        var result = _studentController.GetAll();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(students, okResult.Value);
    }
    
    
    [Test]
    public void GetAll_ShouldReturnOk_WhenNoStudentsExist()
    {
        // Arrange
        var students = new List<Student>();

        _studentRepositoryMock
            .Setup(repo => repo.GetAll())
            .Returns(students);

        // Act
        var result = _studentController.GetAll();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(students, okResult.Value);
    }

    
    [Test]
    public void GetAll_ShouldReturnInternalServerError_WhenExceptionOccurs()
    {
        // Arrange
        _studentRepositoryMock
            .Setup(repo => repo.GetAll())
            .Throws(new Exception("Unexpected error"));

        // Act
        var result = _studentController.GetAll();

        // Assert
        var internalServerErrorResult = result.Result as ObjectResult;
        Assert.IsNotNull(internalServerErrorResult);
        Assert.AreEqual(500, internalServerErrorResult.StatusCode);
        Assert.AreEqual("Internal server error: Unexpected error", internalServerErrorResult.Value);
    }

    
    [Test]
    public void GetStudentById_ShouldReturnOk_WhenStudentExists()
    {
        // Arrange
        var studentId = "12345";
        var student = new Student { Id = studentId, FirstName = "John", FamilyName = "Doe" };

        _studentRepositoryMock
            .Setup(repo => repo.GetStudentById(studentId))
            .Returns(student);

        // Act
        var result = _studentController.GetStudentById(studentId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(student, okResult.Value);
    }

    
    [Test]
    public void GetStudentById_ShouldReturnBadRequest_WhenStudentNotFound()
    {
        // Arrange
        var studentId = "12345";

        _studentRepositoryMock
            .Setup(repo => repo.GetStudentById(studentId))
            .Returns((Student)null);

        // Act
        var result = _studentController.GetStudentById(studentId);

        // Assert
        var badRequestResult = result.Result as ObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
        Assert.AreEqual("Bad request:Student with the given id not found", badRequestResult.Value);
    }


    [Test]
    public void GetStudentById_ShouldReturnInternalServerError_WhenAnErrorOccurs()
    {
        // Arrange
        var studentId = "12345";
        var exceptionMessage = "Unexpected error";

        _studentRepositoryMock
            .Setup(repo => repo.GetStudentById(studentId))
            .Throws(new Exception(exceptionMessage));

        // Act
        var result = _studentController.GetStudentById(studentId);

        // Assert
        var internalServerErrorResult = result.Result as ObjectResult;
        Assert.IsNotNull(internalServerErrorResult);
        Assert.AreEqual(500, internalServerErrorResult.StatusCode);
        Assert.AreEqual($"Internal server error: {exceptionMessage}", internalServerErrorResult.Value);
    }

    

    
    
    [Test]
    public void Post_ShouldReturnCreated_WhenValidRequest()
    {
        // Arrange
        var validRequest = new StudentRequest
        {
            FirstName = "John",
            FamilyName = "Doe",
            Email = "john.doe@example.com",
            Username = "john_doe",
            Password = "Password123",
            BirthDate = "2000-01-01",
            BirthPlace = "Somewhere",
            StudentNo = "S12345"
        };

        _studentRepositoryMock.Setup(repo => repo.Add(It.IsAny<StudentRequest>())).Verifiable();

        // Act
        var result = _studentController.Post(validRequest);

        // Assert
        var createdAtActionResult = result.Result as CreatedAtActionResult;
        Assert.IsNotNull(createdAtActionResult);
        Assert.AreEqual(201, createdAtActionResult.StatusCode);
    
        Assert.AreEqual("Successfully added new student", createdAtActionResult.Value);

        
        _studentRepositoryMock.Verify(repo => repo.Add(It.IsAny<StudentRequest>()), Times.Once);
    }

    
  
    [Test]
    public void Post_ShouldReturnBadRequest_WhenStudentAlreadyExists()
    {
        // Arrange
        var request = new StudentRequest
        {
            FirstName = "Jane",
            FamilyName = "Doe",
            Email = "jane.doe@example.com",
            Username = "janedoe123",
            Password = "Password123",
            BirthDate = "2000-01-01",
            BirthPlace = "Somewhere",
            StudentNo = "S12345"
        };

       
        _studentRepositoryMock
            .Setup(repo => repo.Add(It.IsAny<StudentRequest>()))
            .Throws(new ArgumentException("A student with the same student number already exists."));

        // Act
        var result = _studentController.Post(request);

        // Assert
        var badRequestResult = result as ActionResult<string>;
        Assert.IsNotNull(badRequestResult);

        var objectResult = badRequestResult.Result as ObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(400, objectResult.StatusCode);
        Assert.AreEqual("Bad request: A student with the same student number already exists.", objectResult.Value);
    }

    
    [Test]
    public void Post_ShouldReturnInternalServerError_WhenDatabaseUpdateFails()
    {
        // Arrange
        var request = new StudentRequest
        {
            FirstName = "Jane",
            FamilyName = "Doe",
            Email = "jane.doe@example.com",
            Username = "janedoe123",
            Password = "Password123",
            BirthDate = "2000-01-01",
            BirthPlace = "Somewhere",
            StudentNo = "S12345"
        };

    
        _studentRepositoryMock
            .Setup(repo => repo.Add(It.IsAny<StudentRequest>()))
            .Throws(new DbUpdateException("Database update error"));

        // Act
        var result = _studentController.Post(request);

        // Assert
        var internalServerErrorResult = result as ActionResult<string>;
        Assert.IsNotNull(internalServerErrorResult);

        var objectResult = internalServerErrorResult.Result as ObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(500, objectResult.StatusCode);
        Assert.AreEqual("Internal server error: Database update error", objectResult.Value);
    }


    
    [Test]
    public void Post_ShouldReturnInternalServerError_WhenAnUnexpectedErrorOccurs()
    {
        // Arrange
        var request = new StudentRequest
        {
            FirstName = "Jane",
            FamilyName = "Doe",
            Email = "jane.doe@example.com",
            Username = "janedoe123",
            Password = "Password123",
            BirthDate = "2000-01-01",
            BirthPlace = "Somewhere",
            StudentNo = "S12345"
        };

        
        _studentRepositoryMock
            .Setup(repo => repo.Add(It.IsAny<StudentRequest>()))
            .Throws(new Exception("Unexpected error"));

        // Act
        var result = _studentController.Post(request);

        // Assert
        var internalServerErrorResult = result as ActionResult<string>;
        Assert.IsNotNull(internalServerErrorResult);

        var objectResult = internalServerErrorResult.Result as ObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(500, objectResult.StatusCode);
        Assert.AreEqual("Internal server error: Unexpected error", objectResult.Value);
    }


    
}