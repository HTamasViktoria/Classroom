using Classroom.Controllers;
using Classroom.Model.DataModels;
using Classroom.Model.DataModels.Enums;
using Classroom.Model.RequestModels;
using Classroom.Model.ResponseModels;
using Classroom.Service.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ClassroomUnitTests;

public class GradeControllerTests
{
    private Mock<ILogger<GradeController>> _loggerMock;
    private Mock<IGradeRepository> _gradeRepositoryMock;
    private GradeController _gradeController;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<GradeController>>();
        _gradeRepositoryMock = new Mock<IGradeRepository>();
        _gradeController =
            new GradeController(_loggerMock.Object, _gradeRepositoryMock.Object);
    }
    
    
    [Test]
    public void GetAll_ShouldReturnOk_WhenGradesExist()
    {
        // Arrange
        var grades = new List<Grade>
        {
            new Grade
            {
                Id = 1,
                TeacherId = "teacher1",
                StudentId = "student1",
                Subject = "Math",
                ForWhat = "Test",
                Value = GradeValues.Elégséges,
                Date = DateTime.Now
            },
            new Grade
            {
                Id = 2,
                TeacherId = "teacher2",
                StudentId = "student2",
                Subject = "English",
                ForWhat = "Assignment",
                Value = GradeValues.Jó,
                Date = DateTime.Now
            }
        };

        _gradeRepositoryMock
            .Setup(repo => repo.GetAll())
            .Returns(grades);

        // Act
        var result = _gradeController.GetAll();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        var returnedGrades = okResult.Value as List<Grade>;
        Assert.IsNotNull(returnedGrades);
        Assert.AreEqual(2, returnedGrades.Count);
    }

    [Test]
    public void GetAll_ShouldReturnOk_WhenNoGradesExist()
    {
        // Arrange
        var grades = new List<Grade>();
        _gradeRepositoryMock
            .Setup(repo => repo.GetAll())
            .Returns(grades);

        // Act
        var result = _gradeController.GetAll();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        var returnedGrades = okResult.Value as List<Grade>;
        Assert.IsNotNull(returnedGrades);
        Assert.AreEqual(0, returnedGrades.Count);
    }


    [Test]
    public void GetAll_ShouldReturnInternalServerError_WhenExceptionOccurs()
    {
        // Arrange
        _gradeRepositoryMock
            .Setup(repo => repo.GetAll())
            .Throws(new Exception("Unexpected error"));

        // Act
        var result = _gradeController.GetAll();

        // Assert
        var internalServerErrorResult = result.Result as ObjectResult;
        Assert.IsNotNull(internalServerErrorResult);
        Assert.AreEqual(500, internalServerErrorResult.StatusCode);
        Assert.AreEqual("Internal server error: Unexpected error", internalServerErrorResult.Value);
    }

   
    [Test]
    public void GetAllValues_ShouldReturnOk_WhenGradeValuesExist()
    {
        // Arrange
        var expectedValues = new List<string>
        {
            "Elégtelen = 1",
            "Elégséges = 2",
            "Közepes = 3",
            "Jó = 4",
            "Jeles = 5"
        };

        // Act
        var result = _gradeController.GetAllValues();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        var returnedValues = okResult.Value as List<string>;
        Assert.IsNotNull(returnedValues);
        Assert.AreEqual(expectedValues.Count, returnedValues.Count);
        for (int i = 0; i < expectedValues.Count; i++)
        {
            Assert.AreEqual(expectedValues[i], returnedValues[i]);
        }
    }

   
    
    [Test]
    public void Post_ShouldReturnCreated_WhenGradeIsValid()
    {
        // Arrange
        var gradeRequest = new GradeRequest
        {
            TeacherId = "teacher1",
            StudentId = "student1",
            Subject = "Math",
            ForWhat = "Test",
            Read = true,
            Value = "Jeles",
            Date = "2024-12-10"
        };

        _gradeRepositoryMock
            .Setup(repo => repo.Add(It.IsAny<GradeRequest>()))
            .Verifiable();

        // Act
        var result = _gradeController.Post(gradeRequest);

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

        _gradeRepositoryMock.Verify(repo => repo.Add(It.IsAny<GradeRequest>()), Times.Once);
    }

    
    [Test]
    public void Post_ShouldReturnBadRequest_WhenArgumentExceptionOccurs()
    {
        // Arrange
        var gradeRequest = new GradeRequest
        {
            TeacherId = "teacher1",
            StudentId = "student1",
            Subject = "Math",
            ForWhat = "Test",
            Read = true,
            Value = "InvalidGrade",
            Date = "2024-12-10"
        };

        _gradeRepositoryMock
            .Setup(repo => repo.Add(It.IsAny<GradeRequest>()))
            .Throws(new ArgumentException("Invalid grade value"));

        // Act
        var result = _gradeController.Post(gradeRequest);

        // Assert
        var badRequestResult = result as ObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
        Assert.AreEqual("Bad request: Invalid grade value", badRequestResult.Value);

        _gradeRepositoryMock.Verify(repo => repo.Add(It.IsAny<GradeRequest>()), Times.Once);
    }



    [Test]
    public void Post_ShouldReturnInternalServerError_WhenExceptionOccurs()
    {
        // Arrange
        var gradeRequest = new GradeRequest
        {
            TeacherId = "teacher1",
            StudentId = "student1",
            Subject = "Math",
            ForWhat = "Test",
            Read = true,
            Value = "Jeles",
            Date = "2024-12-10"
        };

        _gradeRepositoryMock
            .Setup(repo => repo.Add(It.IsAny<GradeRequest>()))
            .Throws(new Exception("Unexpected error"));

        // Act
        var result = _gradeController.Post(gradeRequest);

        // Assert
        var internalServerErrorResult = result as ObjectResult;
        Assert.IsNotNull(internalServerErrorResult);
        Assert.AreEqual(500, internalServerErrorResult.StatusCode);
        Assert.AreEqual("Internal server error: Unexpected error", internalServerErrorResult.Value);

        _gradeRepositoryMock.Verify(repo => repo.Add(It.IsAny<GradeRequest>()), Times.Once);
    }


    [Test]
    public async Task GetGradesBySubjectByStudent_ShouldReturnOk_WhenGradesExist()
    {
        // Arrange
        var subject = "Math";
        var studentId = "student1";
        var grades = new List<Grade>
        {
            new Grade { Id = 1, TeacherId = "teacher1", StudentId = studentId, Subject = subject, ForWhat = "Test", Value = GradeValues.Jeles, Date = DateTime.Now },
            new Grade { Id = 2, TeacherId = "teacher2", StudentId = studentId, Subject = subject, ForWhat = "Exam", Value = GradeValues.Jó, Date = DateTime.Now }
        };

        _gradeRepositoryMock
            .Setup(repo => repo.GetGradesBySubjectByStudent(subject, studentId))
            .ReturnsAsync(grades);

        // Act
        var result = await _gradeController.GetGradesBySubjectByStudent(subject, studentId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        var returnedGrades = okResult.Value as List<Grade>;
        Assert.IsNotNull(returnedGrades);
        Assert.AreEqual(2, returnedGrades.Count);
    }



    [Test]
    public async Task GetGradesBySubjectByStudent_ShouldReturnBadRequest_WhenArgumentExceptionOccurs()
    {
        // Arrange
        var subject = "Math";
        var studentId = "student1";

        _gradeRepositoryMock
            .Setup(repo => repo.GetGradesBySubjectByStudent(subject, studentId))
            .ThrowsAsync(new ArgumentException("Invalid argument"));

        // Act
        var result = await _gradeController.GetGradesBySubjectByStudent(subject, studentId);

        // Assert
        var badRequestResult = result.Result as ObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
        Assert.AreEqual("Bad request: Invalid argument", badRequestResult.Value);
    }

    
    
    
    
    [Test]
    public async Task GetGradesBySubjectByStudent_ShouldReturnInternalServerError_WhenUnexpectedExceptionOccurs()
    {
        // Arrange
        var subject = "Math";
        var studentId = "student1";

        _gradeRepositoryMock
            .Setup(repo => repo.GetGradesBySubjectByStudent(subject, studentId))
            .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _gradeController.GetGradesBySubjectByStudent(subject, studentId);

        // Assert
        var internalServerErrorResult = result.Result as ObjectResult;
        Assert.IsNotNull(internalServerErrorResult);
        Assert.AreEqual(500, internalServerErrorResult.StatusCode);
        Assert.AreEqual("Internal server error: Unexpected error", internalServerErrorResult.Value);
    }

    
    [Test]
    public async Task GetGradesBySubjectByStudent_ShouldReturnOk_WhenNoGradesExist()
    {
        // Arrange
        var subject = "Math";
        var studentId = "student1";
        var emptyGradesList = new List<Grade>();

        _gradeRepositoryMock
            .Setup(repo => repo.GetGradesBySubjectByStudent(subject, studentId))
            .ReturnsAsync(emptyGradesList);

        // Act
        var result = await _gradeController.GetGradesBySubjectByStudent(subject, studentId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        var returnedGrades = okResult.Value as List<Grade>;
        Assert.IsNotNull(returnedGrades);
        Assert.AreEqual(0, returnedGrades.Count);
    }

    
  
    [Test]
    public void Put_ShouldReturnOk_WhenGradeIsUpdatedSuccessfully()
    {
        // Arrange
        var gradeRequest = new GradeRequest
        {
            TeacherId = "teacher1",
            StudentId = "student1",
            Subject = "Math",
            ForWhat = "Test",
            Value = "Jó = 4",
            Date = "2024-12-10"
        };

        _gradeRepositoryMock
            .Setup(repo => repo.Edit(It.IsAny<GradeRequest>(), It.IsAny<int>()))
            .Verifiable();

        // Act
        var result = _gradeController.Put(gradeRequest, 1);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var responseValue = okResult.Value as object;
        Assert.IsNotNull(responseValue);

        var messageProperty = responseValue.GetType().GetProperty("message");
        Assert.IsNotNull(messageProperty);
        var messageValue = messageProperty.GetValue(responseValue);
        Assert.AreEqual("Successfully updated grade", messageValue);

        _gradeRepositoryMock.Verify(repo => repo.Edit(It.IsAny<GradeRequest>(), It.IsAny<int>()), Times.Once);
    }

    
    
    
    [Test]
    public void Put_ShouldReturnNotFound_WhenGradeDoesNotExist()
    {
        // Arrange
        var gradeRequest = new GradeRequest
        {
            TeacherId = "teacher1",
            StudentId = "student1",
            Subject = "Math",
            ForWhat = "Test",
            Value = "Jó = 4",
            Date = "2024-12-10"
        };

        _gradeRepositoryMock
            .Setup(repo => repo.Edit(It.IsAny<GradeRequest>(), It.IsAny<int>()))
            .Throws(new KeyNotFoundException("Grade with Id 1 not found."));

        // Act
        var result = _gradeController.Put(gradeRequest, 1);

        // Assert
        var notFoundResult = result as ObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);

        var responseValue = notFoundResult.Value as string;
        Assert.AreEqual("Grade with Id 1 not found.", responseValue);
    }

    
    
    
    
    [Test]
    public void Put_ShouldReturnBadRequest_WhenGradeRequestIsInvalid()
    {
        // Arrange
        var gradeRequest = new GradeRequest
        {
            TeacherId = "teacher1",
            StudentId = "student1",
            Subject = "Math",
            ForWhat = "Test",
            Value = "InvalidValue",
            Date = "invalid-date" 
        };

        _gradeRepositoryMock
            .Setup(repo => repo.Edit(It.IsAny<GradeRequest>(), It.IsAny<int>()))
            .Throws(new ArgumentException("Invalid grade value: InvalidValue"));

        // Act
        var result = _gradeController.Put(gradeRequest, 1);

        // Assert
        var badRequestResult = result as ObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);

        var responseValue = badRequestResult.Value as string;
        Assert.AreEqual("Bad request: Invalid grade value: InvalidValue", responseValue);
    }

    
    [Test]
    public void Put_ShouldReturnInternalServerError_WhenAnUnexpectedErrorOccurs()
    {
        // Arrange
        var gradeRequest = new GradeRequest
        {
            TeacherId = "teacher1",
            StudentId = "student1",
            Subject = "Math",
            ForWhat = "Test",
            Value = "Jó = 4",
            Date = "2024-12-10"
        };

        _gradeRepositoryMock
            .Setup(repo => repo.Edit(It.IsAny<GradeRequest>(), It.IsAny<int>()))
            .Throws(new Exception("An unexpected error occurred"));

        // Act
        var result = _gradeController.Put(gradeRequest, 1);

        // Assert
        var internalServerErrorResult = result as ObjectResult;
        Assert.IsNotNull(internalServerErrorResult);
        Assert.AreEqual(500, internalServerErrorResult.StatusCode);

        var responseValue = internalServerErrorResult.Value as string;
        Assert.AreEqual("Internal server error: An unexpected error occurred", responseValue);
    }

    
    [Test]
    public void Delete_ShouldReturnOk_WhenGradeIsSuccessfullyDeleted()
    {
        // Arrange
        var gradeId = 1;

        _gradeRepositoryMock
            .Setup(repo => repo.Delete(gradeId))
            .Verifiable();

        // Act
        var result = _gradeController.Delete(gradeId);

        // Assert
        var okResult = result as ObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual("Osztályzat sikeresen törölve.", okResult.Value);

        _gradeRepositoryMock.Verify(repo => repo.Delete(gradeId), Times.Once);
    }

    
    [Test]
    public void Delete_ShouldReturnNotFound_WhenGradeDoesNotExist()
    {
        // Arrange
        var gradeId = 999;
        _gradeRepositoryMock
            .Setup(repo => repo.Delete(gradeId))
            .Throws(new ArgumentException($"Grade with Id {gradeId} not found."));

        // Act
        var result = _gradeController.Delete(gradeId);

        // Assert
        var notFoundResult = result as ObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
        Assert.AreEqual($"Grade with Id {gradeId} not found.", notFoundResult.Value);

        _gradeRepositoryMock.Verify(repo => repo.Delete(gradeId), Times.Once);
    }

    
    
    [Test]
    public void Delete_ShouldReturnInternalServerError_WhenUnexpectedErrorOccurs()
    {
        // Arrange
        var gradeId = 1;
        _gradeRepositoryMock
            .Setup(repo => repo.Delete(gradeId))
            .Throws(new Exception("Unexpected error"));

        // Act
        var result = _gradeController.Delete(gradeId);

        // Assert
        var internalServerErrorResult = result as ObjectResult;
        Assert.IsNotNull(internalServerErrorResult);
        Assert.AreEqual(500, internalServerErrorResult.StatusCode);
        Assert.AreEqual("Internal server error: Unexpected error", internalServerErrorResult.Value);

        _gradeRepositoryMock.Verify(repo => repo.Delete(gradeId), Times.Once);
    }

    
    
    [Test]
    public void GetGradesByStudentId_ShouldReturnGrades_WhenGradesExistForStudent()
    {
        // Arrange
        var studentId = "student1";
        var grades = new List<Grade>
        {
            new Grade
            {
                Id = 1,
                StudentId = studentId,
                TeacherId = "teacher1",
                Subject = "Math",
                ForWhat = "Test",
                Value = GradeValues.Jó,
                Date = DateTime.Now
            },
            new Grade
            {
                Id = 2,
                StudentId = studentId,
                TeacherId = "teacher2",
                Subject = "Science",
                ForWhat = "Assignment",
                Value = GradeValues.Közepes,
                Date = DateTime.Now
            }
        };

        _gradeRepositoryMock
            .Setup(repo => repo.GetByStudentId(studentId))
            .Returns(grades);

        // Act
        var result = _gradeController.GetGradesByStudentId(studentId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var responseGrades = okResult.Value as IEnumerable<Grade>;
        Assert.IsNotNull(responseGrades);
        Assert.AreEqual(grades.Count, responseGrades.Count());

        _gradeRepositoryMock.Verify(repo => repo.GetByStudentId(studentId), Times.Once);
    }

    
    [Test]
    public void GetGradesByStudentId_ShouldReturnBadRequest_WhenArgumentExceptionOccurs()
    {
        // Arrange
        var invalidStudentId = "invalidStudentId";

        _gradeRepositoryMock
            .Setup(repo => repo.GetByStudentId(invalidStudentId))
            .Throws(new ArgumentException("Invalid student ID"));

        // Act
        var result = _gradeController.GetGradesByStudentId(invalidStudentId);

        // Assert
        var badRequestResult = result.Result as ObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
        Assert.AreEqual("Bad request: Invalid student ID", badRequestResult.Value);

        _gradeRepositoryMock.Verify(repo => repo.GetByStudentId(invalidStudentId), Times.Once);
    }

    
    [Test]
    public void GetGradesByStudentId_ShouldReturnInternalServerError_WhenExceptionOccurs()
    {
        // Arrange
        var studentId = "student1";

        _gradeRepositoryMock
            .Setup(repo => repo.GetByStudentId(studentId))
            .Throws(new Exception("An unknown error occurred"));

        // Act
        var result = _gradeController.GetGradesByStudentId(studentId);

        // Assert
        var internalServerErrorResult = result.Result as ObjectResult;
        Assert.IsNotNull(internalServerErrorResult);
        Assert.AreEqual(500, internalServerErrorResult.StatusCode);
        Assert.AreEqual("Internal server error: An unknown error occurred", internalServerErrorResult.Value);

        _gradeRepositoryMock.Verify(repo => repo.GetByStudentId(studentId), Times.Once);
    }

    
    [Test]
    public void GetGradesByStudentId_ShouldReturnEmptyList_WhenNoGradesFound()
    {
        // Arrange
        var studentId = "student1";
        var grades = new List<Grade>();

        _gradeRepositoryMock
            .Setup(repo => repo.GetByStudentId(studentId))
            .Returns(grades);

        // Act
        var result = _gradeController.GetGradesByStudentId(studentId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
    
        var returnedGrades = okResult.Value as List<Grade>;
        Assert.IsNotNull(returnedGrades);
        Assert.IsEmpty(returnedGrades);

        _gradeRepositoryMock.Verify(repo => repo.GetByStudentId(studentId), Times.Once);
    }

    
    
    [Test]
    public async Task GetTeachersLastGrade_ShouldReturnLastGrade_WhenGradeExists()
    {
        // Arrange
        var teacherId = "teacher1";
        var grade = new LatestGradeResponse
        {
            Id = 1,
            TeacherId = teacherId,
            StudentId = "student1",
            StudentName = "John Doe",
            Subject = "Math",
            ForWhat = "Homework",
            Read = true,
            Value = GradeValues.Elégséges,
            Date = DateTime.Now
        };

        _gradeRepositoryMock
            .Setup(repo => repo.GetTeachersLastGradeAsync(teacherId))
            .ReturnsAsync(grade);

        // Act
        var result = await _gradeController.GetTeachersLastGrade(teacherId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var returnedGrade = okResult.Value as LatestGradeResponse;
        Assert.IsNotNull(returnedGrade);
        Assert.AreEqual(grade.Id, returnedGrade.Id);
        Assert.AreEqual(grade.TeacherId, returnedGrade.TeacherId);
        Assert.AreEqual(grade.StudentName, returnedGrade.StudentName);
        Assert.AreEqual(grade.Subject, returnedGrade.Subject);
        Assert.AreEqual(grade.Value, returnedGrade.Value);

        _gradeRepositoryMock.Verify(repo => repo.GetTeachersLastGradeAsync(teacherId), Times.Once);
    }

    
    
    [Test]
    public async Task GetTeachersLastGrade_ShouldReturnEmptyResponse_WhenNoGradeExistsForTeacher()
    {
        // Arrange
        var teacherId = "teacher1";

        _gradeRepositoryMock
            .Setup(repo => repo.GetTeachersLastGradeAsync(teacherId))
            .ReturnsAsync((LatestGradeResponse)null);

        // Act
        var result = await _gradeController.GetTeachersLastGrade(teacherId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var returnedGrade = okResult.Value as LatestGradeResponse;
        Assert.IsNotNull(returnedGrade);
        Assert.AreEqual(0, returnedGrade.Id);
        Assert.AreEqual(null, returnedGrade.StudentId);
        Assert.AreEqual(null, returnedGrade.StudentName);
        Assert.AreEqual(null, returnedGrade.Subject);
        Assert.AreEqual(default(GradeValues), returnedGrade.Value);
        Assert.AreEqual(DateTime.MinValue, returnedGrade.Date);
    }

    
    
    
    [Test]
    public async Task GetTeachersLastGrade_ShouldReturnBadRequest_WhenArgumentExceptionIsThrown()
    {
        // Arrange
        var teacherId = "invalidTeacherId";
        
        _gradeRepositoryMock
            .Setup(repo => repo.GetTeachersLastGradeAsync(teacherId))
            .ThrowsAsync(new ArgumentException("Invalid teacher ID"));

        // Act
        var result = await _gradeController.GetTeachersLastGrade(teacherId);

        // Assert
        var badRequestResult = result.Result as ObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
        Assert.AreEqual("Bad request: Invalid teacher ID", badRequestResult.Value);
    }

    
    
    [Test]
    public async Task GetTeachersLastGrade_ShouldReturnInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var teacherId = "teacher1";
    
        _gradeRepositoryMock
            .Setup(repo => repo.GetTeachersLastGradeAsync(teacherId))
            .ThrowsAsync(new Exception("Some internal error"));

        // Act
        var result = await _gradeController.GetTeachersLastGrade(teacherId);

        // Assert
        var objectResult = result.Result as ObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(500, objectResult.StatusCode);
        Assert.AreEqual("Internal server error: Some internal error", objectResult.Value);
    }

    
    
    [Test]
    public void GetNewGradesNumber_ShouldReturnOk_WhenGradesExist()
    {
        // Arrange
        var studentId = "student1";
        var expectedNewGradesNumber = 3;

        _gradeRepositoryMock
            .Setup(repo => repo.GetNewGradesNumber(studentId))
            .Returns(expectedNewGradesNumber);

        // Act
        var result = _gradeController.GetNewGradesNumber(studentId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(expectedNewGradesNumber, okResult.Value);
    }

    
    [Test]
    public void GetNewGradesNumber_ReturnsOkResult_WithNewGradesCount()
    {
        string studentId = "validId";
        int expectedGradesCount = 5;
        _gradeRepositoryMock.Setup(repo => repo.GetNewGradesNumber(studentId)).Returns(expectedGradesCount);

        var result = _gradeController.GetNewGradesNumber(studentId);
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(expectedGradesCount, okResult.Value);
    }
    
  
    
    [Test]
    public void GetNewGradesNumber_ReturnsOkResult_WithZeroGrades()
    {
        string studentId = "validId";
        int expectedGradesCount = 0;
        _gradeRepositoryMock.Setup(repo => repo.GetNewGradesNumber(studentId)).Returns(expectedGradesCount);

        var result = _gradeController.GetNewGradesNumber(studentId);
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(expectedGradesCount, okResult.Value);
    }
    
    
    [Test]
    public void GetNewGradesNumber_ReturnsBadRequest_ForInvalidStudentId()
    {
        string studentId = "invalidId";
        _gradeRepositoryMock.Setup(repo => repo.GetNewGradesNumber(studentId)).Throws(new ArgumentException("Invalid student ID"));

        var result = _gradeController.GetNewGradesNumber(studentId);
        var badRequestResult = result.Result as BadRequestObjectResult;

        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual("Bad request: Invalid student ID", badRequestResult.Value);
    }

    
    
    [Test]
    public void GetNewGradesNumber_ReturnsInternalServerError_ForException()
    {
        string studentId = "validId";
        _gradeRepositoryMock.Setup(repo => repo.GetNewGradesNumber(studentId)).Throws(new Exception("Unexpected error"));

        var result = _gradeController.GetNewGradesNumber(studentId);
        var internalServerErrorResult = result.Result as ObjectResult;

        Assert.IsNotNull(internalServerErrorResult);
        Assert.AreEqual(500, internalServerErrorResult.StatusCode);
        Assert.AreEqual("Internal server error: Unexpected error", internalServerErrorResult.Value);
    }

    
    
    [Test]
    public void GetNewGradesByStudentId_ReturnsOkResult_WithNewGrades()
    {
        string studentId = "validId";
        var expectedGrades = new List<Grade>
        {
            new Grade
            {
                Id = 1,
                TeacherId = "teacher1",
                StudentId = studentId,
                Subject = "Math",
                ForWhat = "Exam",
                Read = false,
                Value = GradeValues.Közepes,
                Date = DateTime.Now
            },
            new Grade
            {
                Id = 2,
                TeacherId = "teacher2",
                StudentId = studentId,
                Subject = "Science",
                ForWhat = "Quiz",
                Read = false,
                Value = GradeValues.Elégséges,
                Date = DateTime.Now
            }
        };
        _gradeRepositoryMock.Setup(repo => repo.GetNewGradesByStudentId(studentId)).Returns(expectedGrades);

        var result = _gradeController.GetNewGradesByStudentId(studentId);
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.IsInstanceOf<IEnumerable<Grade>>(okResult.Value);
        Assert.AreEqual(expectedGrades, okResult.Value);
    }

    
    [Test]
    public void GetNewGradesByStudentId_ReturnsOkResult_WithEmptyGrades()
    {
        string studentId = "validId";
        var expectedGrades = new List<Grade>();
        _gradeRepositoryMock.Setup(repo => repo.GetNewGradesByStudentId(studentId)).Returns(expectedGrades);

        var result = _gradeController.GetNewGradesByStudentId(studentId);
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.IsInstanceOf<IEnumerable<Grade>>(okResult.Value);
        Assert.AreEqual(expectedGrades, okResult.Value);
    }

    
    
    
    [Test]
    public void GetNewGradesByStudentId_ReturnsBadRequest_ForInvalidStudentId()
    {
        string studentId = "invalidId";
        _gradeRepositoryMock.Setup(repo => repo.GetNewGradesByStudentId(studentId)).Throws(new ArgumentException("Invalid student ID"));

        var result = _gradeController.GetNewGradesByStudentId(studentId);
        var badRequestResult = result.Result as BadRequestObjectResult;

        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual("Bad request: Invalid student ID", badRequestResult.Value);
    }

    
    [Test]
    public void GetNewGradesByStudentId_ReturnsInternalServerError_ForException()
    {
        string studentId = "validId";
        _gradeRepositoryMock.Setup(repo => repo.GetNewGradesByStudentId(studentId)).Throws(new Exception("Unexpected error"));

        var result = _gradeController.GetNewGradesByStudentId(studentId);
        var internalServerErrorResult = result.Result as ObjectResult;

        Assert.IsNotNull(internalServerErrorResult);
        Assert.AreEqual(500, internalServerErrorResult.StatusCode);
        Assert.AreEqual("Internal server error: Unexpected error", internalServerErrorResult.Value);
    }

    
    [Test]
    public async Task GetGradesByClassBySubject_ReturnsOkResult_WithGrades()
    {
        int classId = 1;
        string subject = "Math";
        var expectedGrades = new List<Grade>
        {
            new Grade
            {
                Id = 1,
                TeacherId = "teacher1",
                StudentId = "student1",
                Subject = subject,
                ForWhat = "Exam",
                Read = false,
                Value = GradeValues.Jeles,
                Date = DateTime.Now
            },
            new Grade
            {
                Id = 2,
                TeacherId = "teacher2",
                StudentId = "student2",
                Subject = subject,
                ForWhat = "Quiz",
                Read = false,
                Value = GradeValues.Jó,
                Date = DateTime.Now
            }
        };
        _gradeRepositoryMock.Setup(repo => repo.GetGradesByClassBySubject(classId, subject)).ReturnsAsync(expectedGrades);

        var result = await _gradeController.GetGradesByClassBySubject(classId, subject);
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.IsInstanceOf<IEnumerable<Grade>>(okResult.Value);
        Assert.AreEqual(expectedGrades, okResult.Value);
    }

    
    
    [Test]
    public async Task GetGradesByClassBySubject_ReturnsOkResult_WithEmptyGrades()
    {
        int classId = 1;
        string subject = "Math";
        var expectedGrades = new List<Grade>();
        _gradeRepositoryMock.Setup(repo => repo.GetGradesByClassBySubject(classId, subject)).ReturnsAsync(expectedGrades);

        var result = await _gradeController.GetGradesByClassBySubject(classId, subject);
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.IsInstanceOf<IEnumerable<Grade>>(okResult.Value);
        Assert.AreEqual(expectedGrades, okResult.Value);
    }

    [Test]
    public async Task GetGradesByClassBySubject_ReturnsBadRequest_ForInvalidClassId()
    {
        int classId = 9999;
        string subject = "Math";
        _gradeRepositoryMock.Setup(repo => repo.GetGradesByClassBySubject(classId, subject)).Throws(new ArgumentException("Invalid class ID"));

        var result = await _gradeController.GetGradesByClassBySubject(classId, subject);
        var badRequestResult = result.Result as BadRequestObjectResult;

        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual("Bad request: Invalid class ID", badRequestResult.Value);
    }

    
    
    [Test]
    public async Task GetGradesByClassBySubject_ReturnsInternalServerError_ForException()
    {
        int classId = 1;
        string subject = "Math";
        _gradeRepositoryMock.Setup(repo => repo.GetGradesByClassBySubject(classId, subject)).ThrowsAsync(new Exception("Unexpected error"));

        var result = await _gradeController.GetGradesByClassBySubject(classId, subject);
        var internalServerErrorResult = result.Result as ObjectResult;

        Assert.IsNotNull(internalServerErrorResult);
        Assert.AreEqual(500, internalServerErrorResult.StatusCode);
        Assert.AreEqual("Internal server error: Unexpected error", internalServerErrorResult.Value);
    }

    [Test]
    public async Task GetGradesByClass_ReturnsOkResult_WithGrades()
    {
        int classId = 1;
        var expectedGrades = new List<Grade>
        {
            new Grade
            {
                Id = 1,
                TeacherId = "teacher1",
                StudentId = "student1",
                Subject = "Math",
                ForWhat = "Exam",
                Read = false,
                Value = GradeValues.Közepes,
                Date = DateTime.Now
            },
            new Grade
            {
                Id = 2,
                TeacherId = "teacher2",
                StudentId = "student2",
                Subject = "Math",
                ForWhat = "Quiz",
                Read = false,
                Value = GradeValues.Közepes,
                Date = DateTime.Now
            }
        };
        _gradeRepositoryMock.Setup(repo => repo.GetGradesByClass(classId)).ReturnsAsync(expectedGrades);

        var result = await _gradeController.GetGradesByClass(classId);
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.IsInstanceOf<IEnumerable<Grade>>(okResult.Value);
        Assert.AreEqual(expectedGrades, okResult.Value);
    }


    [Test]
    public async Task GetGradesByClass_ReturnsOkResult_WithEmptyGrades()
    {
        int classId = 1;
        var expectedGrades = new List<Grade>();
        _gradeRepositoryMock.Setup(repo => repo.GetGradesByClass(classId)).ReturnsAsync(expectedGrades);

        var result = await _gradeController.GetGradesByClass(classId);
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.IsInstanceOf<IEnumerable<Grade>>(okResult.Value);
        Assert.AreEqual(expectedGrades, okResult.Value);
    }

    
    [Test]
    public async Task GetGradesByClass_ReturnsBadRequest_ForInvalidClassId()
    {
        int classId = 9999;
        _gradeRepositoryMock.Setup(repo => repo.GetGradesByClass(classId)).Throws(new ArgumentException("Invalid class ID"));

        var result = await _gradeController.GetGradesByClass(classId);
        var badRequestResult = result.Result as BadRequestObjectResult;

        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual("Bad request: Invalid class ID", badRequestResult.Value);
    }

    
    
    
    [Test]
    public async Task GetGradesByClass_ReturnsInternalServerError_ForException()
    {
        int classId = 1;
        _gradeRepositoryMock.Setup(repo => repo.GetGradesByClass(classId)).ThrowsAsync(new Exception("Unexpected error"));

        var result = await _gradeController.GetGradesByClass(classId);
        var internalServerErrorResult = result.Result as ObjectResult;

        Assert.IsNotNull(internalServerErrorResult);
        Assert.AreEqual(500, internalServerErrorResult.StatusCode);
        Assert.AreEqual("Internal server error: Unexpected error", internalServerErrorResult.Value);
    }

    
    
    [Test]
    public async Task GetClassAveragesByStudentId_ReturnsOkResult_WithAverages()
    {
        string studentId = "validStudentId";
        var expectedAverages = new Dictionary<string, double>
        {
            { "Math", 4.5 },
            { "Science", 3.8 },
            { "History", 4.0 }
        };
        _gradeRepositoryMock.Setup(repo => repo.GetClassAveragesByStudentId(studentId)).ReturnsAsync(expectedAverages);

        var result = await _gradeController.GetClassAveragesByStudentId(studentId);
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.IsInstanceOf<Dictionary<string, double>>(okResult.Value);
        Assert.AreEqual(expectedAverages, okResult.Value);
    }

    
    
    [Test]
    public async Task GetClassAveragesByStudentId_ReturnsOkResult_WithEmptyAverages()
    {
        string studentId = "validStudentId";
        var expectedAverages = new Dictionary<string, double>();
        _gradeRepositoryMock.Setup(repo => repo.GetClassAveragesByStudentId(studentId)).ReturnsAsync(expectedAverages);

        var result = await _gradeController.GetClassAveragesByStudentId(studentId);
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.IsInstanceOf<Dictionary<string, double>>(okResult.Value);
        Assert.AreEqual(expectedAverages, okResult.Value);
    }

    
    
    [Test]
    public async Task GetClassAveragesByStudentId_ReturnsBadRequest_ForInvalidStudentId()
    {
        string studentId = "invalidStudentId";
        _gradeRepositoryMock.Setup(repo => repo.GetClassAveragesByStudentId(studentId)).Throws(new ArgumentException("Invalid student ID"));

        var result = await _gradeController.GetClassAveragesByStudentId(studentId);
        var badRequestResult = result.Result as BadRequestObjectResult;

        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual("Bad request: Invalid student ID", badRequestResult.Value);
    }

    
    
    [Test]
    public async Task GetClassAveragesByStudentId_ReturnsInternalServerError_ForException()
    {
        string studentId = "validStudentId";
        _gradeRepositoryMock.Setup(repo => repo.GetClassAveragesByStudentId(studentId)).ThrowsAsync(new Exception("Unexpected error"));

        var result = await _gradeController.GetClassAveragesByStudentId(studentId);
        var internalServerErrorResult = result.Result as ObjectResult;

        Assert.IsNotNull(internalServerErrorResult);
        Assert.AreEqual(500, internalServerErrorResult.StatusCode);
        Assert.AreEqual("Internal server error: Unexpected error", internalServerErrorResult.Value);
    }

    
    [Test]
    public async Task GetClassAveragesBySubject_ReturnsOkResult_WithAverages()
    {
        string subject = "Math";
        var expectedAverages = new Dictionary<string, double>
        {
            { "Class 1", 4.5 },
            { "Class 2", 3.8 },
            { "Class 3", 4.0 }
        };
        _gradeRepositoryMock.Setup(repo => repo.GetClassAveragesBySubject(subject)).ReturnsAsync(expectedAverages);

        var result = await _gradeController.GetClassAveragesBySubject(subject);
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.IsInstanceOf<Dictionary<string, double>>(okResult.Value);
        Assert.AreEqual(expectedAverages, okResult.Value);
    }

    
    
    [Test]
    public async Task GetClassAveragesBySubject_ReturnsOkResult_WithEmptyAverages()
    {
        string subject = "Math";
        var expectedAverages = new Dictionary<string, double>();
        _gradeRepositoryMock.Setup(repo => repo.GetClassAveragesBySubject(subject)).ReturnsAsync(expectedAverages);

        var result = await _gradeController.GetClassAveragesBySubject(subject);
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.IsInstanceOf<Dictionary<string, double>>(okResult.Value);
        Assert.AreEqual(expectedAverages, okResult.Value);
    }

    
    
    
    [Test]
    public async Task GetClassAveragesBySubject_ReturnsBadRequest_ForInvalidSubject()
    {
        string subject = "InvalidSubject";
        _gradeRepositoryMock.Setup(repo => repo.GetClassAveragesBySubject(subject)).Throws(new ArgumentException("Invalid subject"));

        var result = await _gradeController.GetClassAveragesBySubject(subject);
        var badRequestResult = result.Result as BadRequestObjectResult;

        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual("Bad request: Invalid subject", badRequestResult.Value);
    }

    
    
    
    [Test]
    public async Task GetClassAveragesBySubject_ReturnsInternalServerError_ForException()
    {
        string subject = "Math";
        _gradeRepositoryMock.Setup(repo => repo.GetClassAveragesBySubject(subject)).ThrowsAsync(new Exception("Unexpected error"));

        var result = await _gradeController.GetClassAveragesBySubject(subject);
        var internalServerErrorResult = result.Result as ObjectResult;

        Assert.IsNotNull(internalServerErrorResult);
        Assert.AreEqual(500, internalServerErrorResult.StatusCode);
        Assert.AreEqual("Internal server error: Unexpected error", internalServerErrorResult.Value);
    }
    
}