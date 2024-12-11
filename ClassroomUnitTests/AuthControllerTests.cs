using Classroom.Service;
using Classroom.Service.Authentication;
using Classroom.Controllers;
using Classroom.Model.RequestModels;
using Classroom.Model.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace ClassroomUnitTests;

public class AuthControllerTests
{
     
    private Mock<IConfiguration> _configurationMock;
    private Mock<IAuthService> _authenticationServiceMock;
    private Mock<IUserService> _userServiceMock;
    private AuthController _authController;

    [SetUp]
    public void SetUp()
    {
        _configurationMock = new Mock<IConfiguration>();
        _authenticationServiceMock = new Mock<IAuthService>();
        _userServiceMock = new Mock<IUserService>();
        _authController =
            new AuthController(_authenticationServiceMock.Object, _configurationMock.Object, _userServiceMock.Object);
    }
    
    
    [Test]
    public async Task Register_ReturnsCreatedAtAction_WhenRegistrationIsSuccessful()
    {
        // Arrange
        var request = new StudentRequest
        {
            Email = "test@student.com",
            Username = "testuser",
            Password = "TestPassword123!",
            FirstName = "John",
            FamilyName = "Doe",
            BirthDate = "2000-01-01",
            BirthPlace = "City",
            StudentNo = "S12345"
        };

        var authResult = new AuthResult
        {
            Success = true,
            Email = "test@student.com",
            UserName = "testuser"
        };

       
        _authenticationServiceMock.Setup(service => service.RegisterAsync(
                request.Email, request.Username, request.Password, "Student",
                request.FirstName, request.FamilyName, DateTime.Parse(request.BirthDate),
                request.BirthPlace, request.StudentNo, null, null))
            .ReturnsAsync(authResult);

        // Act
        var result = await _authController.Register(request);

        // Assert
        var actionResult = result as ActionResult<RegistrationResponse>;
        Assert.IsNotNull(actionResult);
        Assert.IsInstanceOf<CreatedAtActionResult>(actionResult.Result);
        var createdAtActionResult = actionResult.Result as CreatedAtActionResult;

        Assert.IsNotNull(createdAtActionResult);
        Assert.AreEqual(201, createdAtActionResult.StatusCode);
        Assert.AreEqual("Register", createdAtActionResult.ActionName);
        Assert.AreEqual("test@student.com", ((RegistrationResponse)createdAtActionResult.Value).Email);
        Assert.AreEqual("testuser", ((RegistrationResponse)createdAtActionResult.Value).UserName);
    }
    
}