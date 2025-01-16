using Classroom.Controllers;
using Classroom.Model.DataModels;
using Classroom.Model.ResponseModels;
using Classroom.Service.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ClassroomUnitTests
{
    public class UserControllerTests
    {
        private Mock<ILogger<UserController>> _loggerMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private UserController _userController;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<UserController>>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _userController = new UserController(_loggerMock.Object, _userRepositoryMock.Object);
        }

        [Test]
        public void GetAllTeachers_ReturnsTeachersList_IfDBHasData()
        {
            var teachers = new List<Teacher>
            {
                new Teacher { Id = "1", FirstName = "John", FamilyName = "Doe", Role = "Teacher" },
                new Teacher { Id = "2", FirstName = "Jane", FamilyName = "Smith", Role = "Teacher" }
            };

            _userRepositoryMock.Setup(repo => repo.GetAllTeachers()).Returns(teachers);

            var result = _userController.GetAllTeachers();

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            var actual = okResult.Value as IEnumerable<Teacher>;

            Assert.That(actual, Is.EqualTo(teachers));
        }

        [Test]
        public void GetAllTeachers_ReturnsEmptyList_WhenNoTeachersExist()
        {
            // Arrange
            var teachers = new List<Teacher>();

            _userRepositoryMock.Setup(repo => repo.GetAllTeachers()).Returns(teachers);

            // Act
            var result = _userController.GetAllTeachers();

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            var actual = okResult.Value as IEnumerable<Teacher>;
            Assert.That(actual, Is.Empty);
        }

        [Test]
        public void GetAllTeachers_ThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetAllTeachers()).Throws(new Exception("Unexpected error"));

            // Act
            var result = _userController.GetAllTeachers();

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ObjectResult>());
            var objectResult = result.Result as ObjectResult;

            Assert.That(objectResult.StatusCode, Is.EqualTo(500));

            var response = objectResult.Value;

            var jsonResponse = JsonConvert.SerializeObject(response);
            var expectedJson = "{\"message\":\"Internal server error. Please try again later.\"}";

            Assert.That(jsonResponse, Is.EqualTo(expectedJson));
        }

        [Test]
        public void GetAllParents_ReturnsParentsList_IfDBHasData()
        {
            var parents = new List<Parent>
            {
                new Parent { Id = "1", FirstName = "John", FamilyName = "Doe", Role = "Parent" },
                new Parent { Id = "2", FirstName = "Jane", FamilyName = "Smith", Role = "Parent" }
            };

            _userRepositoryMock.Setup(repo => repo.GetAllParents()).Returns(parents);

            var result = _userController.GetAllParents();

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            var actual = okResult.Value as IEnumerable<Parent>;

            Assert.That(actual, Is.EqualTo(parents));
        }

        [Test]
        public void GetAllParents_ReturnsEmptyList_WhenNoParentsExist()
        {
            // Arrange
            var parents = new List<Parent>();

            _userRepositoryMock.Setup(repo => repo.GetAllParents()).Returns(parents);

            // Act
            var result = _userController.GetAllParents();

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            var actual = okResult.Value as IEnumerable<Parent>;
            Assert.That(actual, Is.Empty);
        }

        [Test]
        public void GetAllParents_ThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.GetAllParents()).Throws(new Exception("Unexpected error"));

            // Act
            var result = _userController.GetAllParents();

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ObjectResult>());

            var objectResult = result.Result as ObjectResult;

            Assert.That(objectResult.StatusCode, Is.EqualTo(500));


            var response = objectResult.Value;

            var jsonResponse = JsonConvert.SerializeObject(response);
            var expectedJson = "{\"message\":\"Internal server error. Please try again later.\"}";

            Assert.That(jsonResponse, Is.EqualTo(expectedJson));
        }


        [Test]
        public void GetTeacherById_ReturnsTeacher_WhenTeacherExists()
        {
            // Arrange
            var teacherId = "1";
            var teacher = new Teacher
            {
                Id = teacherId,
                FirstName = "John",
                FamilyName = "Doe",
                Role = "Teacher"
            };

            _userRepositoryMock.Setup(repo => repo.GetTeacherById(teacherId)).Returns(teacher);

            // Act
            var result = _userController.GetTeacherById(teacherId);

            // Assert

            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            var actualTeacher = okResult.Value as Teacher;

            Assert.That(actualTeacher, Is.EqualTo(teacher));
        }


        [Test]
        public void GetTeacherById_TeacherNotFound_ReturnsBadRequest()
        {
            // Arrange
            var teacherId = "nonexistent-id";
            var exceptionMessage = $"Teacher with ID {teacherId} not found.";

            _userRepositoryMock.Setup(repo => repo.GetTeacherById(teacherId))
                .Throws(new ArgumentException(exceptionMessage));

            // Act
            var result = _userController.GetTeacherById(teacherId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ObjectResult>());
            var objectResult = result.Result as ObjectResult;

            Assert.That(objectResult.StatusCode, Is.EqualTo(400));

            var response = objectResult.Value;

            var jsonResponse = JsonConvert.SerializeObject(response);
            var expectedJson = "{\"message\":\"Teacher with ID nonexistent-id not found.\"}";

            Assert.That(jsonResponse, Is.EqualTo(expectedJson));
        }


        [Test]
        public void GetTeacherById_InternalServerError_ReturnsInternalServerError()
        {
            // Arrange
            var teacherId = "some-id";
            var exceptionMessage = "Unexpected error";
            _userRepositoryMock.Setup(repo => repo.GetTeacherById(teacherId))
                .Throws(new Exception(exceptionMessage));

            var expectedJson = $"{{\"message\":\"Internal server error: {exceptionMessage}\"}}";

            // Act
            var result = _userController.GetTeacherById(teacherId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ObjectResult>());

            var objectResult = result.Result as ObjectResult;

            Assert.That(objectResult.StatusCode, Is.EqualTo(500));

            var response = objectResult.Value;
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);

            Assert.That(jsonResponse, Is.EqualTo(expectedJson));
        }


        [Test]
        public void GetTeacherById_InvalidWhitespaceId_ReturnsBadRequest()
        {
            // Arrange
            var teacherId = "   ";
            _userRepositoryMock.Setup(repo => repo.GetTeacherById(teacherId))
                .Throws(new ArgumentException("The given identifier cannot be null, empty or whitespace."));

            // Act
            var result = _userController.GetTeacherById(teacherId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ObjectResult>());

            var objectResult = result.Result as ObjectResult;


            Assert.That(objectResult.StatusCode, Is.EqualTo(400));

            var response = objectResult.Value;


            var expectedMessage = "The given identifier cannot be null, empty or whitespace.";


            Assert.That(response, Is.InstanceOf<object>());
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            Assert.That(jsonResponse, Contains.Substring(expectedMessage));
        }


        [Test]
        public void GetParentById_Success_ReturnsOk()
        {
            // Arrange
            var parentId = "1";
            var parent = new Parent()
            {
                Id = parentId,
                FirstName = "John",
                FamilyName = "Smith",
                Role = "Parent",
                ChildName = "Jane",
                StudentId = "12345",
                Student = new Student { Id = "12345", FirstName = "Jane", FamilyName = "Smith" }
            };

            _userRepositoryMock.Setup(repo => repo.GetParentById(parentId)).Returns(parent);

            // Act
            var result = _userController.GetParentById(parentId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ObjectResult>());

            var objectResult = result.Result as ObjectResult;
            Assert.That(objectResult.StatusCode, Is.EqualTo(200));

            var response = objectResult.Value as Parent;
            Assert.That(response, Is.EqualTo(parent));
        }

        [Test]
        public void GetParentById_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var parentId = "-1";
            var expectedMessage = "The given identifier cannot be null, empty or whitespace.";
            
            _userRepositoryMock.Setup(repo => repo.GetParentById(It.IsAny<string>()))
                .Throws(new ArgumentException(expectedMessage));

            // Act
            var result = _userController.GetParentById(parentId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ObjectResult>());

            var objectResult = result.Result as ObjectResult;

            Assert.That(objectResult.StatusCode, Is.EqualTo(400));

            var response = objectResult.Value;

            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            Assert.That(jsonResponse, Contains.Substring(expectedMessage));
        }



        [Test]
        public void GetParentById_UnexpectedError_ReturnsInternalServerError()
        {
            // Arrange
            var parentId = "valid-parent-id";
            _userRepositoryMock.Setup(repo => repo.GetParentById(parentId)).Throws(new Exception("Unexpected error"));

            // Act
            var result = _userController.GetParentById(parentId);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ObjectResult>());

            var objectResult = result.Result as ObjectResult;
            Assert.That(objectResult.StatusCode, Is.EqualTo(500));

            var response = objectResult.Value;
            var expectedMessage = "Internal server error: Unexpected error";

            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            Assert.That(jsonResponse, Contains.Substring(expectedMessage));
        }


      

        [Test]
        public void AddTeacher_ValidTeacher_ReturnsCreated()
        {
            // Arrange
            var teacher = new Teacher
            {
                Id = "2",
                FirstName = "Thomas",
                FamilyName = "Wilhelm",
                Role = "Teacher"
            };

            // Mocks
            _userRepositoryMock.Setup(repo => repo.AddTeacher(It.IsAny<Teacher>())).Verifiable();

            // Act
            var result = _userController.AddTeacher(teacher);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());

            var createdResult = result.Result as CreatedAtActionResult;

            Assert.That(createdResult.StatusCode, Is.EqualTo(201));

            Assert.That(createdResult.RouteValues["teacherId"], Is.EqualTo(teacher.Id));

            var returnedTeacher = createdResult.Value as Teacher;
            Assert.That(returnedTeacher, Is.EqualTo(teacher));
        }


        [Test]
        public void AddTeacher_ExistingTeacher_ReturnsBadRequest()
        {
            // Arrange
            var teacher = new Teacher
            {
                Id = "teacher-123",
                FirstName = "John",
                FamilyName = "Doe",
                Role = "Teacher"
            };

            var exceptionMessage = "Teacher already exists";
            _userRepositoryMock.Setup(repo => repo.AddTeacher(It.IsAny<Teacher>()))
                .Throws(new ArgumentException(exceptionMessage));
            
            var expectedJson = $"{{\"message\":\"{exceptionMessage}\"}}";

            // Act
            var result = _userController.AddTeacher(teacher);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ObjectResult>());

            var objectResult = result.Result as ObjectResult;

            Assert.That(objectResult.StatusCode, Is.EqualTo(400));

            var response = objectResult.Value;
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);

            Assert.That(jsonResponse, Is.EqualTo(expectedJson));
        }




        [Test]
        public void AddTeacher_InternalServerError_ReturnsInternalServerError()
        {
            // Arrange
            var teacher = new Teacher
            {
                Id = "teacher-123",
                FirstName = "John",
                FamilyName = "Doe",
                Role = "Teacher"
            };

            var exceptionMessage = "Unexpected error";
            _userRepositoryMock.Setup(repo => repo.AddTeacher(It.IsAny<Teacher>()))
                .Throws(new Exception(exceptionMessage));

            var expectedJson = $"{{\"message\":\"Internal server error: {exceptionMessage}\"}}";

            var result = _userController.AddTeacher(teacher);

            Assert.That(result.Result, Is.InstanceOf<ObjectResult>());

            var objectResult = result.Result as ObjectResult;

            Assert.That(objectResult.StatusCode, Is.EqualTo(500));

            var response = objectResult.Value;
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);

            Assert.That(jsonResponse, Is.EqualTo(expectedJson));
        }


        [Test]
        public void AddParent_ValidParent_ReturnsCreated()
        {
            // Arrange
            var parent = new Parent
            {
                Id = "parent-123",
                FirstName = "Mary",
                FamilyName = "Johnson",
                Role = "Parent",
                ChildName = "John",
                StudentId = "001"
            };
            
            _userRepositoryMock.Setup(repo => repo.AddParent(It.IsAny<Parent>())).Verifiable();

            // Act
            var result = _userController.AddParent(parent);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());

            var createdResult = result.Result as CreatedAtActionResult;
            Assert.That(createdResult.StatusCode, Is.EqualTo(201));
            Assert.That(createdResult.RouteValues["parentId"], Is.EqualTo(parent.Id));

            var returnedParent = createdResult.Value as Parent;
            Assert.That(returnedParent, Is.EqualTo(parent));
        }

        [Test]
        public void AddParent_ExistingParent_ReturnsBadRequest()
        {
            // Arrange
            var parent = new Parent
            {
                Id = "parent-123",
                FirstName = "Mary",
                FamilyName = "Johnson",
                Role = "Parent",
                ChildName = "John",
                StudentId = "001"
            };
            
            _userRepositoryMock.Setup(repo => repo.AddParent(It.IsAny<Parent>()))
                .Throws(new ArgumentException(
                    $"Parent with name {parent.FirstName} {parent.FamilyName} already exists."));
            
            var expectedJson =
                $"{{\"message\":\"Parent with name {parent.FirstName} {parent.FamilyName} already exists.\"}}";

            // Act
            var result = _userController.AddParent(parent);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());

            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));

            var response = badRequestResult.Value;
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);

            Assert.That(jsonResponse, Is.EqualTo(expectedJson));
        }

        [Test]
        public void AddParent_DatabaseUpdateError_ReturnsInternalServerError()
        {
            // Arrange
            var parent = new Parent
            {
                Id = "parent-123",
                FirstName = "Mary",
                FamilyName = "Johnson",
                Role = "Parent",
                ChildName = "John",
                StudentId = "001"
            };

            var dbUpdateException = new DbUpdateException("Database update failed");
            
            _userRepositoryMock.Setup(repo => repo.AddParent(It.IsAny<Parent>())).Throws(dbUpdateException);

            var expectedJson = "{\"message\":\"An error occurred while saving to the database.\"}";

            // Act
            var result = _userController.AddParent(parent);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ObjectResult>());

            var objectResult = result.Result as ObjectResult;
            Assert.That(objectResult.StatusCode, Is.EqualTo(500));

            var response = objectResult.Value;
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);

            Assert.That(jsonResponse, Is.EqualTo(expectedJson));
        }

        [Test]
        public void AddParent_InternalServerError_ReturnsInternalServerError()
        {
            // Arrange
            var parent = new Parent
            {
                Id = "parent-123",
                FirstName = "Mary",
                FamilyName = "Johnson",
                Role = "Parent",
                ChildName = "John",
                StudentId = "001"
            };

            var exceptionMessage = "Unexpected error";
            _userRepositoryMock.Setup(repo => repo.AddParent(It.IsAny<Parent>()))
                .Throws(new Exception(exceptionMessage));

            var expectedJson = $"{{\"message\":\"Internal server error: {exceptionMessage}\"}}";

            // Act
            var result = _userController.AddParent(parent);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ObjectResult>());

            var objectResult = result.Result as ObjectResult;
            Assert.That(objectResult.StatusCode, Is.EqualTo(500));

            var response = objectResult.Value;
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);

            Assert.That(jsonResponse, Is.EqualTo(expectedJson));
        }
        
        
        
        [Test]
        public void GetTeacherReceivers_ReturnsTeachersList_IfDBHasData()
        {
            // Arrange
            var receivers = new List<ReceiverResponse>
            {
                new ReceiverResponse { Name = "Teacher 1", Id = "1", Role = "Teacher" },
                new ReceiverResponse { Name = "Teacher 2", Id = "2", Role = "Teacher" }
            };
            
            _userRepositoryMock.Setup(repo => repo.GetTeachersAsReceivers()).Returns(receivers);

            // Act
            var result = _userController.GetTeacherReceivers();

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
    
            var okResult = result.Result as OkObjectResult;
            var actual = okResult.Value as IEnumerable<ReceiverResponse>;
            
            Assert.That(actual, Is.EqualTo(receivers));
        }


        [Test]
        public void GetTeacherReceivers_ReturnsEmptyList_WhenNoTeachersFound()
        {
            // Arrange
            var emptyReceiversList = new List<ReceiverResponse>();

        
            _userRepositoryMock.Setup(repo => repo.GetTeachersAsReceivers()).Returns(emptyReceiversList);

            // Act
            var result = _userController.GetTeacherReceivers();

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            var actual = okResult.Value as IEnumerable<ReceiverResponse>;
            
            Assert.That(actual, Is.EqualTo(emptyReceiversList));
        }

        
        [Test]
        public void GetTeacherReceivers_ReturnsStatus500_WhenExceptionIsThrown()
        {
            // Arrange
            var exceptionMessage = "Test exception message";
            _userRepositoryMock.Setup(repo => repo.GetTeachersAsReceivers()).Throws(new Exception(exceptionMessage));

            // Act
            var result = _userController.GetTeacherReceivers();

            // Assert
       
            Assert.That(result.Result, Is.InstanceOf<ObjectResult>());
    
            var objectResult = result.Result as ObjectResult;
            Assert.AreEqual(500, objectResult.StatusCode);
           
            Assert.AreEqual($"Hiba történt: {exceptionMessage}", objectResult.Value);
        }


        
        [Test]
        public void GetParentReceivers_ReturnsParentReceivers_WhenParentsAreFound()
        {
            // Arrange
            var mockParents = new List<ReceiverResponse>
            {
                new ReceiverResponse { Name = "John Doe", Id = "1", Role = "Parent" },
                new ReceiverResponse { Name = "Jane Smith", Id = "2", Role = "Parent" }
            };
    
           
            _userRepositoryMock.Setup(repo => repo.GetParentsAsReceivers()).Returns(mockParents);

            // Act
            var result = _userController.GetParentReceivers();

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            var returnedUsers = okResult.Value as List<ReceiverResponse>;
    
            Assert.AreEqual(2, returnedUsers.Count);
            Assert.AreEqual("John Doe", returnedUsers[0].Name);
            Assert.AreEqual("Jane Smith", returnedUsers[1].Name);
        }

        
        
        [Test]
        public void GetParentReceivers_ReturnsEmptyList_WhenNoParentsFound()
        {
            // Arrange
            var emptyReceiversList = new List<ReceiverResponse>();

        
            _userRepositoryMock.Setup(repo => repo.GetParentsAsReceivers()).Returns(emptyReceiversList);

            // Act
            var result = _userController.GetParentReceivers();

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            var actual = okResult.Value as IEnumerable<ReceiverResponse>;
            
            Assert.That(actual, Is.EqualTo(emptyReceiversList));
        }

        
        [Test]
        public void GetParentReceivers_ReturnsStatus500_WhenExceptionIsThrown()
        {
            // Arrange
            var exceptionMessage = "Test exception message";
            _userRepositoryMock.Setup(repo => repo.GetParentsAsReceivers()).Throws(new Exception(exceptionMessage));

            // Act
            var result = _userController.GetParentReceivers();

            // Assert
       
            Assert.That(result.Result, Is.InstanceOf<ObjectResult>());
    
            var objectResult = result.Result as ObjectResult;
            Assert.AreEqual(500, objectResult.StatusCode);
           
            Assert.AreEqual($"Hiba történt: {exceptionMessage}", objectResult.Value);
        }

        


    }
}