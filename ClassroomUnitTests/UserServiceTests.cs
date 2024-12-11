using Moq;
using Classroom.Service;
using Classroom.Service.Repositories;
using Classroom.Model.DataModels;


namespace ClassroomUnitTests
{
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private UserService _userService;

        [SetUp]
        public void SetUp()
        {
          
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
        }

    
        [Test]
        public void GetAllTeachers_ReturnsTeachers()
        {
            // Arrange
            var teachers = new List<Teacher>
            {
                new Teacher { Id = "1", FirstName = "John", FamilyName = "Doe", Role = "Teacher" },
                new Teacher { Id = "2", FirstName = "Jane", FamilyName = "Smith", Role = "Teacher" }
            };

            _userRepositoryMock.Setup(repo => repo.GetAllTeachers()).Returns(teachers);

            // Act
            var result = _userService.GetAllTeachers();

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("John", result.First().FirstName);
            Assert.AreEqual("Doe", result.First().FamilyName);
        }

    
        [Test]
        public void GetAllParents_ReturnsParents()
        {
            // Arrange
            var parents = new List<Parent>
            {
                new Parent { Id = "1", FirstName = "Alice", FamilyName = "Johnson", Role = "Parent", ChildName = "John Doe" },
                new Parent { Id = "2", FirstName = "Bob", FamilyName = "Brown", Role = "Parent", ChildName = "Jane Brown" }
            };

            _userRepositoryMock.Setup(repo => repo.GetAllParents()).Returns(parents);

            // Act
            var result = _userService.GetAllParents();

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Alice", result.First().FirstName);
            Assert.AreEqual("Johnson", result.First().FamilyName);
        }

       
        [Test]
        public void GetTeacherById_ReturnsTeacher()
        {
            // Arrange
            var teacher = new Teacher { Id = "1", FirstName = "John", FamilyName = "Doe", Role = "Teacher" };

            _userRepositoryMock.Setup(repo => repo.GetTeacherById("1")).Returns(teacher);

            // Act
            var result = _userService.GetTeacherById("1");

            // Assert
            Assert.AreEqual("John", result.FirstName);
            Assert.AreEqual("Doe", result.FamilyName);
        }

       
        [Test]
        public void GetParentById_ReturnsParent()
        {
            // Arrange
            var parent = new Parent { Id = "1", FirstName = "Alice", FamilyName = "Johnson", Role = "Parent", ChildName = "John Doe" };

            _userRepositoryMock.Setup(repo => repo.GetParentById("1")).Returns(parent);

            // Act
            var result = _userService.GetParentById("1");

            // Assert
            Assert.AreEqual("Alice", result.FirstName);
            Assert.AreEqual("Johnson", result.FamilyName);
        }

     
        [Test]
        public void ValidateParentRegistration_ReturnsErrors_WhenInvalid()
        {
            // Arrange
            var studentId = "student1";
            var studentName = "John Doe";

            _userRepositoryMock.Setup(repo => repo.GetStudentFullNameById(studentId)).Returns("John Smith");
            _userRepositoryMock.Setup(repo => repo.CheckParentsNumber(studentId)).Returns(4);

            // Act
            var result = _userService.ValidateParentRegistration(studentId, studentName);

            // Assert
            Assert.Contains("Invalid student ID or child name.", result.ToList());
            Assert.Contains("The student already has the maximum number of registered parents.", result.ToList());
        }

   
        [Test]
        public void ValidateParentRegistration_ReturnsNoErrors_WhenValid()
        {
            // Arrange
            var studentId = "student1";
            var studentName = "John Doe";

            _userRepositoryMock.Setup(repo => repo.GetStudentFullNameById(studentId)).Returns("John Doe");
            _userRepositoryMock.Setup(repo => repo.CheckParentsNumber(studentId)).Returns(3);

            // Act
            var result = _userService.ValidateParentRegistration(studentId, studentName);

            // Assert
            Assert.AreEqual(0, result.Count());
        }


        [Test]
        public void GetByEmail_ReturnsUser_WhenFound()
        {
            // Arrange
            var email = "teacher@example.com";
            var teacher = new Teacher { Id = "1", FirstName = "John", FamilyName = "Doe", Email = email, Role = "Teacher" };
            _userRepositoryMock.Setup(repo => repo.GetAllTeachers()).Returns(new List<Teacher> { teacher });

            // Act
            var result = _userService.GetByEmail(email);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(email, result.Email);
        }

 
        [Test]
        public void GetByEmail_ReturnsNull_WhenNotFound()
        {
            // Arrange
            var email = "notfound@example.com";
            _userRepositoryMock.Setup(repo => repo.GetAllTeachers()).Returns(new List<Teacher>());
            _userRepositoryMock.Setup(repo => repo.GetAllParents()).Returns(new List<Parent>());
            _userRepositoryMock.Setup(repo => repo.GetAllStudents()).Returns(new List<Student>());

            // Act
            var result = _userService.GetByEmail(email);

            // Assert
            Assert.IsNull(result);
        }

 
        [Test]
        public void CheckStudentId_ReturnsTrue_WhenNameMatches()
        {
            // Arrange
            var studentId = "student1";
            var studentName = "John Doe";

            _userRepositoryMock.Setup(repo => repo.GetStudentFullNameById(studentId)).Returns(studentName);

            // Act
            var result = _userService.CheckStudentId(studentId, studentName);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CheckStudentId_ReturnsFalse_WhenNameDoesNotMatch()
        {
            // Arrange
            var studentId = "student1";
            var studentName = "John Smith";

            _userRepositoryMock.Setup(repo => repo.GetStudentFullNameById(studentId)).Returns("John Doe");

            // Act
            var result = _userService.CheckStudentId(studentId, studentName);

            // Assert
            Assert.IsFalse(result);
        }

     
        [Test]
        public void CheckParentsNumber_ReturnsFalse_WhenMaxParentsReached()
        {
            // Arrange
            var studentId = "student1";
            _userRepositoryMock.Setup(repo => repo.CheckParentsNumber(studentId)).Returns(4);

            // Act
            var result = _userService.CheckParentsNumber(studentId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void CheckParentsNumber_ReturnsTrue_WhenParentsNotMaxedOut()
        {
            // Arrange
            var studentId = "student1";
            _userRepositoryMock.Setup(repo => repo.CheckParentsNumber(studentId)).Returns(3);

            // Act
            var result = _userService.CheckParentsNumber(studentId);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
