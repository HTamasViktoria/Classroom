using Classroom.Controllers;
using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Classroom.Service.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Classroom.Service;

namespace ClassroomTests
{
    public class TeacherSubjectControllerTests
    {
        private Mock<ILogger<TeacherSubjectController>> _loggerMock;
        private Mock<ITeacherSubjectRepository> _teacherSubjectRepositoryMock;
        private TeacherSubjectController _teacherSubjectController;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<TeacherSubjectController>>();
            _teacherSubjectRepositoryMock = new Mock<ITeacherSubjectRepository>();
            _teacherSubjectController =
                new TeacherSubjectController(_loggerMock.Object, _teacherSubjectRepositoryMock.Object);
        }

        
        [Test]
        public void GetSubjectsByTeacherId_ShouldReturnSubjects_WhenTeacherExists()
        {
            // Arrange
            var teacherId = "12345";
            var expectedSubjects = new List<TeacherSubject>
            {
                new TeacherSubject { TeacherId = teacherId, Subject = "Math" },
                new TeacherSubject { TeacherId = teacherId, Subject = "Physics" }
            };

            
            _teacherSubjectRepositoryMock
                .Setup(repo => repo.GetSubjectsByTeacherId(teacherId))
                .Returns(expectedSubjects);

            // Act
            var result = _teacherSubjectController.GetSubjectsByTeacherId(teacherId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(expectedSubjects, okResult.Value);
        }

        
        [Test]
        public void IsValidId_ShouldThrowArgumentException_WhenIdIsWhitespace()
        {
            // Arrange
            var invalidId = "   ";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => StringValidationHelper.IsValidId(invalidId));
            Assert.AreEqual("The given identifier cannot be null, empty or whitespace.", ex.Message);
        }

        
        [Test]
        public void GetSubjectsByTeacherId_ShouldReturnEmptyList_WhenTeacherHasNoSubjects()
        {
            // Arrange
            var teacherId = "12345";
            var emptySubjects = new List<TeacherSubject>();
            
            _teacherSubjectRepositoryMock
                .Setup(repo => repo.GetSubjectsByTeacherId(teacherId))
                .Returns(emptySubjects);

            // Act
            var result = _teacherSubjectController.GetSubjectsByTeacherId(teacherId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(emptySubjects, okResult.Value);
        }

        
        
        [Test]
        public void GetSubjectsByTeacherId_ShouldReturnBadRequest_WhenTeacherIdIsInvalid()
        {
            // Arrange
            var invalidTeacherId = "invalidId";

            _teacherSubjectRepositoryMock
                .Setup(repo => repo.GetSubjectsByTeacherId(invalidTeacherId))
                .Throws(new ArgumentException($"Teacher with ID {invalidTeacherId} does not exist."));

            // Act
            var result = _teacherSubjectController.GetSubjectsByTeacherId(invalidTeacherId);

            // Assert
            var badRequestResult = result.Result as ObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("Bad request:Teacher with ID invalidId does not exist.", badRequestResult.Value);
        }

        
        [Test]
        public void GetSubjectsByTeacherId_ShouldReturnInternalServerError_WhenAnUnexpectedErrorOccurs()
        {
            // Arrange
            var teacherId = "12345";
    
            _teacherSubjectRepositoryMock
                .Setup(repo => repo.GetSubjectsByTeacherId(teacherId))
                .Throws(new Exception("Unexpected error"));

            // Act
            var result = _teacherSubjectController.GetSubjectsByTeacherId(teacherId);

            // Assert
            var internalServerErrorResult = result.Result as ObjectResult;
            Assert.IsNotNull(internalServerErrorResult);
            Assert.AreEqual(500, internalServerErrorResult.StatusCode);
            Assert.AreEqual("Internal server error: Unexpected error", internalServerErrorResult.Value);
        }

        
        [Test]
        public async Task GetStudentsByTeacherSubjectId_ShouldReturnStudents_WhenTeacherSubjectExists()
        {
            var teacherSubjectId = 1;
            var students = new List<Student>
            {
                new Student { Id = "1", FirstName = "Alice", FamilyName = "Johnson" },
                new Student { Id = "2", FirstName = "Bob", FamilyName = "Smith" }
            };

            var classOfStudents = new ClassOfStudents
            {
                Id = 1,
                Name = "ClassA",
                Grade = "10",
                Section = "A",
                Students = students
            };

            _teacherSubjectRepositoryMock
                .Setup(repo => repo.GetStudentsByTeacherSubjectIdAsync(teacherSubjectId))
                .ReturnsAsync(classOfStudents);

            var result = await _teacherSubjectController.GetStudentsByTeacherSubjectId(teacherSubjectId);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(students, okResult.Value);
        }

        
        [Test]
        public async Task GetStudentsByTeacherSubjectId_ShouldReturnBadRequest_WhenTeacherSubjectDoesNotExist()
        {
            var teacherSubjectId = 1;

            _teacherSubjectRepositoryMock
                .Setup(repo => repo.GetStudentsByTeacherSubjectIdAsync(teacherSubjectId))
                .ReturnsAsync((ClassOfStudents)null);

            var result = await _teacherSubjectController.GetStudentsByTeacherSubjectId(teacherSubjectId);

            var badRequestResult = result as ObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual($"Bad request: No ClassOfStudents found for TeacherSubject ID {teacherSubjectId}.", badRequestResult.Value);
        }

       
        [Test]
        public async Task GetStudentsByTeacherSubjectId_ShouldReturnInternalServerError_WhenAnUnexpectedErrorOccurs()
        {
            var teacherSubjectId = 1;

            _teacherSubjectRepositoryMock
                .Setup(repo => repo.GetStudentsByTeacherSubjectIdAsync(teacherSubjectId))
                .ThrowsAsync(new Exception("Unexpected error"));

            var result = await _teacherSubjectController.GetStudentsByTeacherSubjectId(teacherSubjectId);

            var internalServerErrorResult = result as ObjectResult;
            Assert.IsNotNull(internalServerErrorResult);
            Assert.AreEqual(500, internalServerErrorResult.StatusCode);
            Assert.AreEqual("Internal server error: Unexpected error", internalServerErrorResult.Value);
        }

        
        
        [Test]
        public void Post_ShouldReturnBadRequest_WhenTeacherSubjectAlreadyExists()
        {
            // Arrange
            var request = new TeacherSubjectRequest
            {
                TeacherId = "12345",
                Subject = "Math",
                ClassOfStudentsId = 1,
                ClassName = "ClassA"
            };

            _teacherSubjectRepositoryMock
                .Setup(repo => repo.Add(It.IsAny<TeacherSubjectRequest>()))
                .Throws(new ArgumentException("Already existing teachersubject"));

            // Act
            var result = _teacherSubjectController.Post(request);

            // Assert
            var badRequestResult = result as ActionResult<object>;
            Assert.IsNotNull(badRequestResult);
            var objectResult = badRequestResult.Result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(400, objectResult.StatusCode);
            Assert.AreEqual("Bad request: Already existing teachersubject", objectResult.Value);
        }

        
        
        [Test]
        public void Post_ShouldReturnBadRequest_WhenTeacherNotFound()
        {
            // Arrange
            var request = new TeacherSubjectRequest
            {
                TeacherId = "12345",
                Subject = "Math",
                ClassOfStudentsId = 1,
                ClassName = "ClassA"
            };

            _teacherSubjectRepositoryMock
                .Setup(repo => repo.Add(It.IsAny<TeacherSubjectRequest>()))
                .Throws(new ArgumentException("Teacher with ID 12345 not found."));

            // Act
            var result = _teacherSubjectController.Post(request);

            // Assert
            var badRequestResult = result as ActionResult<object>;
            Assert.IsNotNull(badRequestResult);
            var objectResult = badRequestResult.Result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(400, objectResult.StatusCode);
            Assert.AreEqual("Bad request: Teacher with ID 12345 not found.", objectResult.Value);
        }

        
        
        [Test]
        public void Post_ShouldReturnBadRequest_WhenClassOfStudentsNotFound()
        {
            // Arrange
            var request = new TeacherSubjectRequest
            {
                TeacherId = "12345",
                Subject = "Math",
                ClassOfStudentsId = 1,
                ClassName = "ClassA"
            };

            _teacherSubjectRepositoryMock
                .Setup(repo => repo.Add(It.IsAny<TeacherSubjectRequest>()))
                .Throws(new ArgumentException("ClassOfStudents with ID 1 not found."));

            // Act
            var result = _teacherSubjectController.Post(request);

            // Assert
            var badRequestResult = result as ActionResult<object>;
            Assert.IsNotNull(badRequestResult);
            var objectResult = badRequestResult.Result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(400, objectResult.StatusCode);
            Assert.AreEqual("Bad request: ClassOfStudents with ID 1 not found.", objectResult.Value);
        }

        
        
        [Test]
        public void Post_ShouldReturnCreated_WhenValidRequest()
        {
            // Arrange
            var validRequest = new TeacherSubjectRequest
            {
                TeacherId = "12345",
                Subject = "Math",
                ClassOfStudentsId = 1,
                ClassName = "ClassA"
            };

            var teacher = new Teacher { Id = "12345", FirstName = "John", FamilyName = "Doe" };
            var classOfStudents = new ClassOfStudents { Id = 1, Name = "ClassA" };
            
            _teacherSubjectRepositoryMock
                .Setup(repo => repo.Add(It.IsAny<TeacherSubjectRequest>()))
                .Verifiable();

            // Act
            var result = _teacherSubjectController.Post(validRequest);

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual(201, createdAtActionResult.StatusCode);

            
            Assert.AreEqual("Successfully added new teacherSubject", createdAtActionResult.Value);

        
            _teacherSubjectRepositoryMock.Verify(repo => repo.Add(It.IsAny<TeacherSubjectRequest>()), Times.Once);
        }



    }
}
