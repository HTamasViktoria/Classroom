using Classroom.Controllers;
using Classroom.Model.DataModels;
using Classroom.Service.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ClassroomTests;

public class ParentControllerTests
{
    private Mock<ILogger<ParentController>> _loggerMock;
    private Mock<IParentRepository> _parentRepositoryMock;
    private ParentController _parentController;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<ParentController>>();
        _parentRepositoryMock = new Mock<IParentRepository>();
        _parentController =
            new ParentController(_loggerMock.Object, _parentRepositoryMock.Object);
    }
    
    
    [Test]
    public void GetByParentId_ShouldReturnOk_WhenParentExists()
    {
        // Arrange
        var parentId = "parent123";
        var expectedParent = new Parent
        {
            Id = parentId,
            FirstName = "John",
            FamilyName = "Doe",
            Email = "john.doe@example.com"
        };

    
        _parentRepositoryMock
            .Setup(repo => repo.GetParentById(parentId))
            .Returns(expectedParent);

        // Act
        var result = _parentController.GetByParentId(parentId);

        // Assert
        var okResult = result as ActionResult<Parent>;
        Assert.IsNotNull(okResult);
    
        var objectResult = okResult.Result as OkObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(200, objectResult.StatusCode);

        var returnedParent = objectResult.Value as Parent;
        Assert.IsNotNull(returnedParent);
        Assert.AreEqual(expectedParent.Id, returnedParent.Id);
        Assert.AreEqual(expectedParent.FirstName, returnedParent.FirstName);
        Assert.AreEqual(expectedParent.FamilyName, returnedParent.FamilyName);
        Assert.AreEqual(expectedParent.Email, returnedParent.Email);
    }

    
    
    [Test]
    public void GetByParentId_ShouldReturnBadRequest_WhenParentNotFound()
    {
        // Arrange
        var parentId = "parent123";
        
        _parentRepositoryMock
            .Setup(repo => repo.GetParentById(parentId))
            .Returns((Parent)null);

        // Act
        var result = _parentController.GetByParentId(parentId);

        // Assert
        var badRequestResult = result as ActionResult<Parent>;
        Assert.IsNotNull(badRequestResult);
    
        var objectResult = badRequestResult.Result as ObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(400, objectResult.StatusCode);

     
        Assert.AreEqual("Bad request: No parent found with the given id", objectResult.Value);
    }
    
    
    
    [Test]
    public void GetByParentId_ShouldReturnInternalServerError_WhenExceptionOccurs()
    {
        // Arrange
        var parentId = "parent123";
        
        _parentRepositoryMock
            .Setup(repo => repo.GetParentById(parentId))
            .Throws(new Exception("Database connection error"));

        // Act
        var result = _parentController.GetByParentId(parentId);

        // Assert
        var internalServerErrorResult = result as ActionResult<Parent>;
        Assert.IsNotNull(internalServerErrorResult);
    
        var objectResult = internalServerErrorResult.Result as ObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(500, objectResult.StatusCode);

        Assert.AreEqual("Internal server error: Database connection error", objectResult.Value);
    }


    [Test]
    public void GetParentsByStudentId_ShouldReturnOk_WhenParentsFound()
    {
        // Arrange
        var studentId = "student123";

        var parents = new List<Parent>
        {
            new Parent { Id = "parent1", FirstName = "John", FamilyName = "Doe", StudentId = "student123" },
            new Parent { Id = "parent2", FirstName = "Jane", FamilyName = "Doe", StudentId = "student123" }
        };

        _parentRepositoryMock
            .Setup(repo => repo.GetParentsByStudentId(studentId))
            .Returns(parents);

        // Act
        var result = _parentController.GetParentsByStudentId(studentId);

        // Assert
        var okResult = result as ActionResult<IEnumerable<Parent>>;
        Assert.IsNotNull(okResult);

        var objectResult = okResult.Result as ObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(200, objectResult.StatusCode);

        var returnedParents = objectResult.Value as List<Parent>;
        Assert.IsNotNull(returnedParents);
        Assert.AreEqual(2, returnedParents.Count);
        Assert.AreEqual("parent1", returnedParents[0].Id);
        Assert.AreEqual("parent2", returnedParents[1].Id);
    }

    
    [Test]
    public void GetParentsByStudentId_ShouldReturnEmptyList_WhenNoParentsFound()
    {
        // Arrange
        var studentId = "student123";
        
        _parentRepositoryMock
            .Setup(repo => repo.GetParentsByStudentId(studentId))
            .Returns(new List<Parent>());

        // Act
        var result = _parentController.GetParentsByStudentId(studentId);

        // Assert
        var okResult = result as ActionResult<IEnumerable<Parent>>;
        Assert.IsNotNull(okResult);

        var objectResult = okResult.Result as ObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(200, objectResult.StatusCode);

        var returnedParents = objectResult.Value as List<Parent>;
        Assert.IsNotNull(returnedParents);
        Assert.AreEqual(0, returnedParents.Count);
    }

    
    [Test]
    public void GetParentsByStudentId_ShouldReturnInternalServerError_WhenExceptionOccurs()
    {
        // Arrange
        var studentId = "student123";
        
        _parentRepositoryMock
            .Setup(repo => repo.GetParentsByStudentId(studentId))
            .Throws(new Exception("Database error occurred"));

        // Act
        var result = _parentController.GetParentsByStudentId(studentId);

        // Assert
        var objectResult = result.Result as ObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(500, objectResult.StatusCode);
        Assert.AreEqual("Internal server error: Database error occurred", objectResult.Value);
    }

    
    [Test]
    public void GetParentsByStudentId_ShouldReturnBadRequest_WhenStudentNotFound()
    {
        // Arrange
        var studentId = "student123";
        
        _parentRepositoryMock
            .Setup(repo => repo.GetParentsByStudentId(studentId))
            .Throws(new ArgumentException("No student found with the given ID."));

        // Act
        var result = _parentController.GetParentsByStudentId(studentId);

        // Assert
        var objectResult = result.Result as ObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(400, objectResult.StatusCode);
        Assert.AreEqual("Bad request: No student found with the given ID.", objectResult.Value);
    }

    
    
    [Test]
    public void GetAllParents_ShouldReturnOk_WhenParentsExist()
    {
        // Arrange
        var parentsList = new List<Parent>
        {
            new Parent { Id = "1", FamilyName = "Balogh", FirstName = "Béla",StudentId = "student1" },
            new Parent { Id = "2", FamilyName = "Kovács",FirstName = "Ottó", StudentId = "student2" }
        };

        _parentRepositoryMock
            .Setup(repo => repo.GetAllParents())
            .Returns(parentsList);

        // Act
        var result = _parentController.GetAllParents();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(parentsList, okResult.Value);

        _parentRepositoryMock.Verify(repo => repo.GetAllParents(), Times.Once);
    }

    
    
    [Test]
    public void GetAllParents_ShouldReturnOk_WhenNoParentsExistForStudent()
    {
        // Arrange
        var parentsList = new List<Parent>();

        _parentRepositoryMock
            .Setup(repo => repo.GetAllParents())
            .Returns(parentsList);

        // Act
        var result = _parentController.GetAllParents();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(parentsList, okResult.Value);

        _parentRepositoryMock.Verify(repo => repo.GetAllParents(), Times.Once);
    }

    
    
    [Test]
    public void GetAllParents_ShouldReturnInternalServerError_WhenExceptionOccurs()
    {
        // Arrange
        _parentRepositoryMock
            .Setup(repo => repo.GetAllParents())
            .Throws(new Exception("Database error"));

        // Act
        var result = _parentController.GetAllParents();

        // Assert
        var objectResult = result.Result as ObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(500, objectResult.StatusCode);
        Assert.AreEqual("Internal server error: Database error", objectResult.Value);

        _parentRepositoryMock.Verify(repo => repo.GetAllParents(), Times.Once);
    }

    
}