using Classroom.Controllers;
using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Classroom.Model.ResponseModels;
using Classroom.Service.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ClassroomUnitTests;

public class ClassOfStudentsControllerTests
{

    private Mock<ILogger<ClassOfStudentsController>> _loggerMock;
    private Mock<IClassOfStudentsRepository> _classOfStudentsRepositoryMock;
    private ClassOfStudentsController _classOfStudentsController;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<ClassOfStudentsController>>();
        _classOfStudentsRepositoryMock = new Mock<IClassOfStudentsRepository>();
        _classOfStudentsController =
            new ClassOfStudentsController(_loggerMock.Object, _classOfStudentsRepositoryMock.Object);
    }


    [Test]
    public void GetAll_ReturnsOk_WhenClassesExist()
    {
        var classes = new List<ClassOfStudents>
        {
            new ClassOfStudents { Id = 1, Name = "Class 1" },
            new ClassOfStudents { Id = 2, Name = "Class 2" }
        };

        _classOfStudentsRepositoryMock.Setup(repo => repo.GetAll()).Returns(classes);

        var result = _classOfStudentsController.GetAll();

        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(classes, okResult.Value);
    }


    [Test]
    public void GetAll_ReturnsOk_WithEmptyList_WhenNoClassesExist()
    {
        var classes = new List<ClassOfStudents>();

        _classOfStudentsRepositoryMock.Setup(repo => repo.GetAll()).Returns(classes);

        var result = _classOfStudentsController.GetAll();

        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(classes, okResult.Value);
    }


    [Test]
    public void GetAll_ReturnsStatusCode200_WhenClassesExist()
    {
        // Arrange
        var classes = new List<ClassOfStudents> { new ClassOfStudents(), new ClassOfStudents() };
        _classOfStudentsRepositoryMock.Setup(repo => repo.GetAll()).Returns(classes);

        // Act
        var result = _classOfStudentsController.GetAll();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(classes, okResult.Value);
    }


    [Test]
    public void GetAll_ReturnsEmptyList_WhenNoClassesExist()
    {
        // Arrange
        var classes = new List<ClassOfStudents>();
        _classOfStudentsRepositoryMock.Setup(repo => repo.GetAll()).Returns(classes);

        // Act
        var result = _classOfStudentsController.GetAll();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.IsInstanceOf<List<ClassOfStudents>>(okResult.Value);
        Assert.AreEqual(0, ((List<ClassOfStudents>)okResult.Value).Count);
    }



    [Test]
    public void GetAll_ReturnsStatusCode500_WhenExceptionOccurs()
    {
        // Arrange
        _classOfStudentsRepositoryMock.Setup(repo => repo.GetAll()).Throws(new Exception("Database error"));

        // Act
        var result = _classOfStudentsController.GetAll();

        // Assert
        var statusCodeResult = result.Result as ObjectResult;
        Assert.IsNotNull(statusCodeResult);
        Assert.AreEqual(500, statusCodeResult.StatusCode);
        Assert.AreEqual("Internal server error: Database error", statusCodeResult.Value);
    }



    [Test]
    public void GetAllStudentsWithClasses_ReturnsStudents_WhenStudentsExist()
    {
        // Arrange
        var studentsWithClasses = new List<StudentWithClassResponse>
        {
            new StudentWithClassResponse { Id = "1", FirstName = "John", FamilyName = "Doe", NameOfClass = "Class A" },
            new StudentWithClassResponse { Id = "2", FirstName = "Jane", FamilyName = "Smith", NameOfClass = "Class B" }
        };

        _classOfStudentsRepositoryMock.Setup(repo => repo.GetAllStudentsWithClasses()).Returns(studentsWithClasses);

        // Act
        var result = _classOfStudentsController.GetAllStudentsWithClasses();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.IsInstanceOf<List<StudentWithClassResponse>>(okResult.Value);
        var returnedStudents = (List<StudentWithClassResponse>)okResult.Value;
        Assert.AreEqual(2, returnedStudents.Count);
        Assert.AreEqual("Class A", returnedStudents[0].NameOfClass);
        Assert.AreEqual("Class B", returnedStudents[1].NameOfClass);
    }



    [Test]
    public void GetAllStudentsWithClasses_ReturnsEmptyList_WhenNoStudentsExist()
    {
        // Arrange
        var studentsWithClasses = new List<StudentWithClassResponse>();
        _classOfStudentsRepositoryMock.Setup(repo => repo.GetAllStudentsWithClasses()).Returns(studentsWithClasses);

        // Act
        var result = _classOfStudentsController.GetAllStudentsWithClasses();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.IsInstanceOf<List<StudentWithClassResponse>>(okResult.Value);
        Assert.AreEqual(0, ((List<StudentWithClassResponse>)okResult.Value).Count);
    }



    [Test]
    public void GetAllStudentsWithClasses_ReturnsStatusCode500_WhenExceptionOccurs()
    {
        // Arrange
        _classOfStudentsRepositoryMock.Setup(repo => repo.GetAllStudentsWithClasses())
            .Throws(new Exception("Database error"));

        // Act
        var result = _classOfStudentsController.GetAllStudentsWithClasses();

        // Assert
        var statusCodeResult = result.Result as ObjectResult;
        Assert.IsNotNull(statusCodeResult);
        Assert.AreEqual(500, statusCodeResult.StatusCode);
        Assert.AreEqual("Internal server error: Database error", statusCodeResult.Value);
    }



    [Test]
    public void GetStudents_ReturnsStudents_WhenStudentsExist()
    {
        // Arrange
        var students = new List<Student>
        {
            new Student { Id = "1", FirstName = "John", FamilyName = "Doe" },
            new Student { Id = "2", FirstName = "Jane", FamilyName = "Smith" }
        };

        _classOfStudentsRepositoryMock.Setup(repo => repo.GetStudents(It.IsAny<int>())).Returns(students);

        // Act
        var result = _classOfStudentsController.GetStudents(1);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.IsInstanceOf<List<Student>>(okResult.Value);
        var returnedStudents = (List<Student>)okResult.Value;
        Assert.AreEqual(2, returnedStudents.Count);
        Assert.AreEqual("John", returnedStudents[0].FirstName);
        Assert.AreEqual("Jane", returnedStudents[1].FirstName);
    }



    [Test]
    public void GetStudents_ReturnsEmptyList_WhenNoStudentsExist()
    {
        // Arrange
        var students = new List<Student>();

        _classOfStudentsRepositoryMock.Setup(repo => repo.GetStudents(It.IsAny<int>())).Returns(students);

        // Act
        var result = _classOfStudentsController.GetStudents(1);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.IsInstanceOf<List<Student>>(okResult.Value);
        var returnedStudents = (List<Student>)okResult.Value;
        Assert.AreEqual(0, returnedStudents.Count);
    }


    [Test]
    public void GetStudents_ReturnsNotFound_WhenKeyNotFoundExceptionOccurs()
    {
        // Arrange
        var exceptionMessage = "Class not found";
        _classOfStudentsRepositoryMock.Setup(repo => repo.GetStudents(It.IsAny<int>()))
            .Throws(new KeyNotFoundException(exceptionMessage));

        // Act
        var result = _classOfStudentsController.GetStudents(1);

        // Assert
        var notFoundResult = result.Result as ObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(400, notFoundResult.StatusCode);
        Assert.AreEqual($"Bad request: {exceptionMessage}", notFoundResult.Value);
    }



    [Test]
    public void GetStudents_ReturnsInternalServerError_WhenExceptionOccurs()
    {
        // Arrange
        var exceptionMessage = "An unexpected error occurred";
        _classOfStudentsRepositoryMock.Setup(repo => repo.GetStudents(It.IsAny<int>()))
            .Throws(new Exception(exceptionMessage));

        // Act
        var result = _classOfStudentsController.GetStudents(1);

        // Assert
        var statusCodeResult = result.Result as ObjectResult;
        Assert.IsNotNull(statusCodeResult);
        Assert.AreEqual(500, statusCodeResult.StatusCode);
        Assert.AreEqual($"Internal server error: {exceptionMessage}", statusCodeResult.Value);
    }


    [Test]
    public void GetClassesBySubject_ReturnsOk_WhenClassesAreFound()
    {
        // Arrange
        var subject = "Mathematics";
        var classes = new List<ClassOfStudents>
        {
            new ClassOfStudents { Id = 1, Name = "Class A" },
            new ClassOfStudents { Id = 2, Name = "Class B" }
        };

        _classOfStudentsRepositoryMock.Setup(repo => repo.GetClassesBySubject(subject)).Returns(classes);

        // Act
        var result = _classOfStudentsController.GetClassesBySubject(subject);

        // Assert
        var okResult = result.Result as ObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(classes, okResult.Value);
    }


    [Test]
    public void GetClassesBySubject_ReturnsEmptyList_WhenNoClassesFound()
    {
        // Arrange
        var subject = "Mathematics";
        var classes = new List<ClassOfStudents>();

        _classOfStudentsRepositoryMock.Setup(repo => repo.GetClassesBySubject(subject)).Returns(classes);

        // Act
        var result = _classOfStudentsController.GetClassesBySubject(subject);

        // Assert
        var okResult = result.Result as ObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(classes, okResult.Value);
    }


    [Test]
    public void GetClassesBySubject_ReturnsBadRequest_WhenArgumentExceptionOccurs()
    {
        // Arrange
        var subject = "Mathematics";
        var exceptionMessage = "Invalid subject name";

        _classOfStudentsRepositoryMock.Setup(repo => repo.GetClassesBySubject(subject))
            .Throws(new ArgumentException(exceptionMessage));

        // Act
        var result = _classOfStudentsController.GetClassesBySubject(subject);

        // Assert
        var badRequestResult = result.Result as ObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
        Assert.AreEqual($"Bad request: {exceptionMessage}", badRequestResult.Value);
    }



    [Test]
    public void GetClassesBySubject_ReturnsInternalServerError_WhenExceptionOccurs()
    {
        // Arrange
        var subject = "Mathematics";
        var exceptionMessage = "An unexpected error occurred";

        _classOfStudentsRepositoryMock.Setup(repo => repo.GetClassesBySubject(subject))
            .Throws(new Exception(exceptionMessage));

        // Act
        var result = _classOfStudentsController.GetClassesBySubject(subject);

        // Assert
        var statusCodeResult = result.Result as ObjectResult;
        Assert.IsNotNull(statusCodeResult);
        Assert.AreEqual(500, statusCodeResult.StatusCode);
        Assert.AreEqual($"Internal server error: {exceptionMessage}", statusCodeResult.Value);
    }



    [Test]
    public void Post_ReturnsCreatedAtAction_WhenClassIsSuccessfullyAdded()
    {
        // Arrange
        var classRequest = new ClassOfStudentsRequest
        {
            Id = 1,
            Grade = "10",
            Section = "A",
        };

      
        _classOfStudentsRepositoryMock.Setup(repo => repo.Add(It.IsAny<ClassOfStudentsRequest>())).Verifiable();

        // Act
        var result = _classOfStudentsController.Post(classRequest);

        // Assert
        var createdAtActionResult = result as CreatedAtActionResult;
        Assert.IsNotNull(createdAtActionResult);
        Assert.AreEqual(201, createdAtActionResult.StatusCode);

        var responseValue = createdAtActionResult.Value as object;
        Assert.IsNotNull(responseValue);

        var messageProperty = responseValue.GetType().GetProperty("message");
        Assert.IsNotNull(messageProperty);
        var messageValue = messageProperty.GetValue(responseValue);
        Assert.AreEqual("Osztály sikeresen elmentve az adatbázisba.", messageValue);
        
        _classOfStudentsRepositoryMock.Verify(repo => repo.Add(It.IsAny<ClassOfStudentsRequest>()), Times.Once);
    }

    
    [Test]
    public void Post_ReturnsBadRequest_WhenArgumentExceptionIsThrown()
    {
        // Arrange
        var classRequest = new ClassOfStudentsRequest
        {
            Id = 1,
            Grade = "10",
            Section = "A"
        };

        var exceptionMessage = "Invalid argument";

       
        _classOfStudentsRepositoryMock.Setup(repo => repo.Add(It.IsAny<ClassOfStudentsRequest>()))
            .Throws(new ArgumentException(exceptionMessage));

        // Act
        var result = _classOfStudentsController.Post(classRequest);

        // Assert
        var objectResult = result as ObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(400, objectResult.StatusCode);
        Assert.AreEqual($"Bad request: {exceptionMessage}", objectResult.Value);
    }


    
    [Test]
    public void Post_ReturnsInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var classRequest = new ClassOfStudentsRequest
        {
            Id = 1,
            Grade = "10",
            Section = "A"
        };

        var exceptionMessage = "Test exception";

      
        _classOfStudentsRepositoryMock.Setup(repo => repo.Add(It.IsAny<ClassOfStudentsRequest>()))
            .Throws(new Exception(exceptionMessage));

        // Act
        var result = _classOfStudentsController.Post(classRequest);

        // Assert
        var objectResult = result as ObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(500, objectResult.StatusCode);
        Assert.AreEqual($"Internal server error: {exceptionMessage}", objectResult.Value);
    }

    
    

    
    [Test]
    public void Post_ReturnsBadRequest_WhenInvalidOperationExceptionIsThrown()
    {
        // Arrange
        var classRequest = new ClassOfStudentsRequest
        {
            Id = 1,
            Grade = "10",
            Section = "A"
        };

        var exceptionMessage = "A class with the same grade and section already exists.";

      
        _classOfStudentsRepositoryMock.Setup(repo => repo.Add(It.IsAny<ClassOfStudentsRequest>()))
            .Throws(new InvalidOperationException(exceptionMessage));

        // Act
        var result = _classOfStudentsController.Post(classRequest);

        // Assert
        var objectResult = result as ObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(400, objectResult.StatusCode);
        Assert.AreEqual($"Bad request: {exceptionMessage}", objectResult.Value);
    }

    
    [Test]
    public void Post_ReturnsOk_WhenStudentIsSuccessfullyAdded()
    {
        // Arrange
        var request = new AddingStudentToClassRequest
        {
            StudentId = "validStudentId",
            ClassId = 1
        };

        
        _classOfStudentsRepositoryMock.Setup(repo => repo.AddStudent(It.IsAny<AddingStudentToClassRequest>())).Verifiable();

        // Act
        var result = _classOfStudentsController.Post(request);

        // Assert
        var objectResult = result as ObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(200, objectResult.StatusCode);
        Assert.AreEqual("Student added to class", objectResult.Value);

      
        _classOfStudentsRepositoryMock.Verify(repo => repo.AddStudent(It.IsAny<AddingStudentToClassRequest>()), Times.Once);
    }

    
    [Test]
    public void AddStudent_ReturnsBadRequest_WhenInvalidOperationExceptionIsThrown()
    {
        // Arrange
        var request = new AddingStudentToClassRequest
        {
            StudentId = "validStudentId",
            ClassId = 1
        };

        var exceptionMessage = "Student with ID validStudentId is already in the class.";

        
        _classOfStudentsRepositoryMock.Setup(repo => repo.AddStudent(It.IsAny<AddingStudentToClassRequest>()))
            .Throws(new InvalidOperationException(exceptionMessage));

        // Act
        var result = _classOfStudentsController.Post(request);

        // Assert
        var objectResult = result as ObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(400, objectResult.StatusCode);
        Assert.AreEqual($"Bad request: {exceptionMessage}", objectResult.Value);
    }

    

    [Test]
    public void AddStudentToClass_ReturnsNotFound_WhenKeyNotFoundExceptionIsThrown()
    {
        // Arrange
        var request = new AddingStudentToClassRequest
        {
            StudentId = "validStudentId",
            ClassId = 1
        };

        var exceptionMessage = "Class with ID 1 not found.";

        
        _classOfStudentsRepositoryMock.Setup(repo => repo.AddStudent(It.IsAny<AddingStudentToClassRequest>()))
            .Throws(new KeyNotFoundException(exceptionMessage));

        // Act
        var result = _classOfStudentsController.Post(request);

        // Assert
        var objectResult = result as ObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(404, objectResult.StatusCode);
        Assert.AreEqual($"Not found: {exceptionMessage}", objectResult.Value);
    }

    
    
    
    [Test]
    public void AddStudentToClass_ReturnsInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var request = new AddingStudentToClassRequest
        {
            StudentId = "validStudentId",
            ClassId = 1
        };

        var exceptionMessage = "Test exception";
        
        _classOfStudentsRepositoryMock.Setup(repo => repo.AddStudent(It.IsAny<AddingStudentToClassRequest>()))
            .Throws(new Exception(exceptionMessage));

        // Act
        var result = _classOfStudentsController.Post(request);

        // Assert
        var objectResult = result as ObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(500, objectResult.StatusCode);
        Assert.AreEqual($"Internal Server error: {exceptionMessage}", objectResult.Value);
    }
    
}