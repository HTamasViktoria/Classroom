using System.Net;
using System.Text;
using Classroom.Service.Repositories;
using Classroom.Data;
using Classroom.Model.DataModels;
using Classroom.Model.DataModels.Enums;
using ClassromIntegrationTests.MockRepos;
using Classroom.Model.RequestModels;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace ClassromIntegrationTests;
[Collection("IntegrationTests")]

public class NotificationControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly HttpClient _mockClient;

    
    public NotificationControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        
       

        var mockFactory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddTransient<INotificationRepository, MockNotificationRepository>();
            });
        });

        _mockClient = mockFactory.CreateClient();
    }


    [Fact]
    public async Task GetAll_ReturnsEmptyList_WhenNoNotificationsExist()
    {
        await ClearDatabaseAsync();
        var response = await _client.GetAsync("/api/notifications");

        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.Equal("[]", responseBody);

        await ClearDatabaseAsync();
    }




    [Fact]
    public async Task GetAll_Notifications_ReturnsCorrectNumberAndTypes()
    {
        await ClearDatabaseAsync();
        await SeedWithNotifications();

  
        var response = await _client.GetAsync("/api/notifications");

        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        var notifications = JsonConvert.DeserializeObject<List<NotificationBase>>(responseBody);

        Assert.Equal(4, notifications.Count);

        Assert.Contains(notifications, n => n.Type == "MissingEquipment");
        Assert.Contains(notifications, n => n.Type == "Exam");
        Assert.Contains(notifications, n => n.Type == "Homework");
        await ClearDatabaseAsync();
    }


    [Fact]
    public async Task GetAll_Notifications_ReturnsInternalServerError_WhenExceptionThrown()
    {
        var response = await _mockClient.GetAsync("/api/notifications");

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("Internal server error", responseBody);
        await ClearDatabaseAsync();
    }
    
    
    [Fact]
    public async Task GetByStudentId_ReturnsBadRequest_WhenParentIdIsInvalid()
    {
        await ClearDatabaseAsync();
        await SeedWithNotifications();

        var studentId = "BelaId";
        var parentId = "-1";
        var response = await _client.GetAsync($"/api/notifications/bystudent/{studentId}/byparent/{parentId}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("User with the given id not found", responseBody);
        await ClearDatabaseAsync();
    }



[Fact]
public async Task GetByStudentId_ReturnsCorrectNotifications()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();

    var studentId = "KalmanId";
    var parentId = "KalmanParentId";

    var responseHajdu = await _client.GetAsync($"/api/notifications/bystudent/{studentId}/byparent/{parentId}");
    var notificationsHajdu = JsonConvert.DeserializeObject<List<NotificationBase>>(await responseHajdu.Content.ReadAsStringAsync());

  
    Assert.Equal(2, notificationsHajdu.Count);

 
    var notification3 = notificationsHajdu.FirstOrDefault(n => n.StudentId == studentId);
    Assert.NotNull(notification3);
    Assert.Equal("Homework", notification3?.Type);
    Assert.Equal("Hajdu Kálmán", notification3?.StudentName);
    Assert.Equal("Complete your homework by next week.", notification3?.Description);

   
    var notification4 = notificationsHajdu.FirstOrDefault(n => n.StudentId == studentId && n.Type == "Exam");
    Assert.NotNull(notification4);
    Assert.Equal("Exam", notification4?.Type);
    Assert.Equal("Hajdu Kálmán", notification4?.StudentName);
    Assert.Equal("Your grade for the last test is ready.", notification4?.Description);
    await ClearDatabaseAsync();
}



[Fact]
public async Task GetByStudentId_ReturnsEmptyList_WhenNoNotificationsExist()
{
    
    await ClearDatabaseAsync();
    await SeederWithoutNotifications();

    var studentId = "BelaId";
    var parentId = "BelaParentId";
    
    var response = await _client.GetAsync($"/api/notifications/bystudent/{studentId}/byparent/{parentId}");
    var notifications = JsonConvert.DeserializeObject<List<NotificationBase>>(await response.Content.ReadAsStringAsync());
    
    Assert.Empty(notifications);
    await ClearDatabaseAsync();
}



[Fact]
public async Task GetByStudentId_ReturnsInternalServerError_WhenExceptionOccurs()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();
    var studentId = "BelaId";
    var parentId = "BelaParentId";

  
    var response = await _mockClient.GetAsync($"/api/notifications/bystudent/{studentId}/byparent/{parentId}");
    
    Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

    var responseBody = await response.Content.ReadAsStringAsync();
    Assert.Contains("Internal server error", responseBody);
    await ClearDatabaseAsync();
}




[Fact]
public async Task GetByTeacherId_ReturnsFourNotifications_WhenValidTeacherIdIsProvided()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();

    var teacherId = "TeacherDoeId";
  
    var response = await _client.GetAsync($"/api/notifications/teacher/{teacherId}");
    
    var notifications = JsonConvert.DeserializeObject<List<NotificationBase>>(await response.Content.ReadAsStringAsync());
    Assert.Equal(4, notifications.Count);
    await ClearDatabaseAsync();
}



[Fact]
public async Task GetByTeacherId_ReturnsEmptyList_WhenNoNotificationsExistForTeacher()
{
    await ClearDatabaseAsync();
    await SeederWithoutNotifications();

    var teacherId = "TeacherDoeId";
    var response = await _client.GetAsync($"/api/notifications/teacher/{teacherId}");
    
    var notifications = JsonConvert.DeserializeObject<List<NotificationBase>>(await response.Content.ReadAsStringAsync());
    Assert.Empty(notifications);
    await ClearDatabaseAsync();
}



[Fact]
public async Task GetByTeacherId_ReturnsBadRequest_WhenTeacherIdIsInvalid()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();

   
    var invalidTeacherId = "-1";
    
    var response = await _client.GetAsync($"/api/notifications/teacher/{invalidTeacherId}");
    
    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
 
    var errorMessage = await response.Content.ReadAsStringAsync();
    Assert.Contains("Bad request", errorMessage);
    await ClearDatabaseAsync();
}



[Fact]
public async Task GetByTeacherId_ReturnsInternalServerError_WhenExceptionIsThrown()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();
    var teacherId = "TeacherDoeId";
    
  
    var response = await _mockClient.GetAsync($"/api/notifications/teacher/{teacherId}");
    
    Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

    var responseBody = await response.Content.ReadAsStringAsync();
    Assert.Contains("Internal server error", responseBody);
    await ClearDatabaseAsync();
}

[Fact]
public async Task GetLastsByStudentId_ReturnsNotifications_WhenTheyExist()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();
    var studentId = "BelaId";
    var parentId = "BelaParentId";
    
    var response = await _client.GetAsync($"/api/notifications/studentslasts/{studentId}/{parentId}");
    
    response.EnsureSuccessStatusCode();
    var responseBody = await response.Content.ReadAsStringAsync();
    var notifications = JsonConvert.DeserializeObject<List<NotificationBase>>(responseBody);

    Assert.NotNull(notifications);
    Assert.NotEmpty(notifications);

    
    var expectedDescriptions = new List<string>
    {
        "Complete your homework by next week.",
        "Your grade for the last test is ready."
    };

    foreach (var description in expectedDescriptions)
    {
        Assert.Contains(notifications, n => n.Description == description);
    }
    await ClearDatabaseAsync();
}



[Fact]
public async Task GetLastsByStudentId_ReturnsEmptyList_WhenNoNotificationsExist()
{
    await ClearDatabaseAsync();
    await SeederWithoutNotifications();
    
    var studentId = "BelaId";
    var parentId = "BelaParentId";
  
    var response = await _client.GetAsync($"/api/notifications/studentslasts/{studentId}/{parentId}");

    response.EnsureSuccessStatusCode();
    var responseBody = await response.Content.ReadAsStringAsync();
    var notifications = JsonConvert.DeserializeObject<List<NotificationBase>>(responseBody);

    Assert.NotNull(notifications);
    Assert.Empty(notifications);
    await ClearDatabaseAsync();
}


[Fact]
public async Task GetLastsByStudentId_ReturnsBadRequest_WhenStudentIdIsInvalid()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();
    var invalidStudentId = "-1";
    var parentId = "BelaParentId";
    
    var response = await _client.GetAsync($"/api/notifications/studentslasts/{invalidStudentId}/{parentId}");
    
    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    var responseBody = await response.Content.ReadAsStringAsync();
    Assert.Contains("Bad request", responseBody);
    await ClearDatabaseAsync();
}


[Fact]
public async Task GetLastsByStudentId_ReturnsBadRequest_WhenParentIdIsInvalid()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();
    
    var studentId = "BelaId";
    var invalidParentId = "-1";
    
    var response = await _client.GetAsync($"/api/notifications/studentslasts/{studentId}/{invalidParentId}");
    
    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    var responseBody = await response.Content.ReadAsStringAsync();
    Assert.Contains("Bad request", responseBody);
    await ClearDatabaseAsync();
}


[Fact]
public async Task GetLastsByStudentId_ReturnsInternalServerError_WhenExceptionIsThrown()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();
    var studentId = "BelaId";
    var parentId = "BelaParentId";


    var response = await _mockClient.GetAsync($"/api/notifications/studentslasts/{studentId}/{parentId}");
    
    Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    var responseBody = await response.Content.ReadAsStringAsync();
    Assert.Contains("Internal server error", responseBody);
    await ClearDatabaseAsync();
}


[Fact]
public async Task GetNewNotifsNumber_ShouldReturnNumberOfNewNotifications_WhenValidStudentAndParentIdsAreGiven()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();

    var studentId = "BelaId";
    var parentId = "BelaParentId";
    var expectedNotificationCount = 2;

    var response = await _client.GetAsync($"/api/notifications/newnotifsnum/{studentId}/{parentId}");

    response.EnsureSuccessStatusCode();

    var content = await response.Content.ReadAsStringAsync();
    var result = int.Parse(content);

    Assert.Equal(expectedNotificationCount, result);
    await ClearDatabaseAsync();
}


[Fact]
public async Task GetNewNotifsNumber_ShouldReturnBadRequest_WhenArgumentExceptionIsThrown()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();

    var studentId = "InvalidStudentId";
    var parentId = "InvalidParentId";

    var response = await _client.GetAsync($"/api/notifications/newnotifsnum/{studentId}/{parentId}");

    Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);

    var content = await response.Content.ReadAsStringAsync();
    Assert.Equal("Bad request:User with the given id not found", content);
    await ClearDatabaseAsync();
}


[Fact]
public async Task GetNewNotifsNumber_ShouldReturnInternalServerError_WhenExceptionIsThrown()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();

    var studentId = "BelaId";
    var parentId = "BelaParentId";

    var response = await _mockClient.GetAsync($"/api/notifications/newnotifsnum/{studentId}/{parentId}");

    Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);

    var content = await response.Content.ReadAsStringAsync();
    Assert.Contains("Internal server error", content);
    await ClearDatabaseAsync();
}


[Fact]
public async Task GetLastsByTeacherId_ShouldReturnEmptyList_WhenNoNotificationsExist()
{
    await ClearDatabaseAsync();
    await SeederWithoutNotifications();

    var teacherId = "TeacherDoeId";

    var response = await _client.GetAsync($"/api/notifications/teacherslasts/{teacherId}");

    Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

    var content = await response.Content.ReadAsStringAsync();
    var result = JsonConvert.DeserializeObject<List<NotificationResponse>>(content);

    Assert.Empty(result);
    await ClearDatabaseAsync();
}

[Fact]
public async Task GetLastsByTeacherId_ShouldReturnNotifications_WhenNotificationsExist()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();

    var teacherId = "TeacherDoeId";

    var response = await _client.GetAsync($"/api/notifications/teacherslasts/{teacherId}");

    Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

    var content = await response.Content.ReadAsStringAsync();
    var result = JsonConvert.DeserializeObject<List<NotificationResponse>>(content);

    Assert.NotEmpty(result);
    Assert.Equal(4, result.Count);

    var notification = result.First();
    Assert.Equal("TeacherDoeId", notification.TeacherId);
    Assert.Equal("KalmanId", notification.StudentId);
    Assert.Equal("Exam", notification.Type);
    Assert.Equal("Your grade for the last test is ready.", notification.Description);
    Assert.False(notification.Read);
    Assert.False(notification.OfficiallyRead);

    Assert.Equal("Hajdu Kálmán", notification.StudentName);
    Assert.Equal("Hajdu Botond", notification.ParentName);
    await ClearDatabaseAsync();
    
}



[Fact]
public async Task GetNewestByTeacherId_ShouldReturnEmptyNotification_WhenNoNotificationsExist()
{
    await ClearDatabaseAsync();
    await SeederWithoutNotifications();

    var teacherId = "TeacherDoeId";

    var response = await _client.GetAsync($"/api/notifications/teachersnewest/{teacherId}");

    Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

    var content = await response.Content.ReadAsStringAsync();
    var result = JsonConvert.DeserializeObject<NotificationBase>(content);

    Assert.NotNull(result);
    Assert.Equal(0, result.Id);
    Assert.Null(result.TeacherId);
    Assert.Null(result.StudentId);
    Assert.Null(result.ParentId);
    Assert.Null(result.Description);
    await ClearDatabaseAsync();
}



[Fact]
public async Task GetLastsByTeacherId_ShouldReturnBadRequest_WhenInvalidTeacherIdIsGiven()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();

    var invalidTeacherId = "-1";

    var response = await _client.GetAsync($"/api/notifications/teacherslasts/{invalidTeacherId}");

    Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);

    var content = await response.Content.ReadAsStringAsync();
    Assert.Contains("Bad request", content);
    await ClearDatabaseAsync();
}


[Fact]
public async Task GetLastsByTeacherId_ShouldReturnInternalServerError_WhenExceptionIsThrown()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();

    var teacherId = "TeacherDoeId";

    var response = await _mockClient.GetAsync($"/api/notifications/teacherslasts/{teacherId}");

    Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);

    var content = await response.Content.ReadAsStringAsync();
    Assert.Contains("Internal server error", content);
    await ClearDatabaseAsync();
}



[Fact]
public async Task GetNewestByTeacherId_ShouldReturnNewestNotification_WhenNotificationsExist()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();

    var teacherId = "TeacherDoeId";

    var response = await _client.GetAsync($"/api/notifications/teachersnewest/{teacherId}");

    Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

    var content = await response.Content.ReadAsStringAsync();
    var result = JsonConvert.DeserializeObject<NotificationBase>(content);

    Assert.NotNull(result);
    Assert.Equal("TeacherDoeId", result.TeacherId);
    Assert.Equal("KalmanId", result.StudentId);
    Assert.Equal("Exam", result.Type);
    Assert.Equal("Your grade for the last test is ready.", result.Description);
    Assert.Equal(DateTime.Now.AddDays(4).Date, result.Date.Date);
    await ClearDatabaseAsync();
}



[Fact]
public async Task GetNewestByTeacherId_ShouldReturnBadRequest_WhenInvalidTeacherIdIsGiven()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();

    var invalidTeacherId = "InvalidTeacherId";

    var response = await _client.GetAsync($"/api/notifications/teachersnewest/{invalidTeacherId}");

    Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);

    var content = await response.Content.ReadAsStringAsync();
    Assert.Contains("Bad request", content);
    await ClearDatabaseAsync();
}


[Fact]
public async Task GetNewestByTeacherId_ShouldReturnInternalServerError_WhenExceptionIsThrown()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();

    var teacherId = "TeacherDoeId";

    var response = await _mockClient.GetAsync($"/api/notifications/teachersnewest/{teacherId}");

    Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);

    var content = await response.Content.ReadAsStringAsync();
    Assert.Contains("Internal server error", content);
    await ClearDatabaseAsync();
}


[Fact]
public async Task Post_ShouldReturnCreated_WhenNotificationIsValid()
{
    var request = new NotificationRequest
    {
        Type = "Homework",
        TeacherId = "TeacherDoeId",
        TeacherName = "John Doe",
        Date = DateTime.Now,
        DueDate = DateTime.Now.AddDays(7),
        Subject = "Matematika",
        Read = false,
        OfficiallyRead = false,
        Description = "Complete the math homework by next week.",
        StudentIds = new List<string> { "BelaId", "KalmanId" },
        OptionalDescription = "Optional notes for the homework."
    };

    var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

    var response = await _client.PostAsync("/api/notifications", content);

    Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);

    var result = await response.Content.ReadAsStringAsync();
    Assert.Contains("Értesítés sikeresen elmentve az adatbázisba.", result);
    await ClearDatabaseAsync();
}


[Fact]
public async Task Post_ShouldReturnBadRequest_WhenNotificationIsInvalid()
{
   
        var request = new NotificationRequest
        {
        Type = "Homework",
        TeacherId = "TeacherDoeId",
        TeacherName = "John Doe",
        Date = DateTime.Now,
        DueDate = DateTime.Now.AddDays(7),
        Subject = "Math",
        Read = false,
        OfficiallyRead = false,
        Description = "Complete the math homework by next week.",
        StudentIds = new List<string> { "BelaId", "KalmanId" },
        OptionalDescription = "Optional notes for the homework."

    };

    var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

    var response = await _client.PostAsync("/api/notifications", content);

    Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);

    var result = await response.Content.ReadAsStringAsync();
    Assert.Contains("Bad request", result);
    await ClearDatabaseAsync();
}


[Fact]
public async Task Post_ShouldReturnInternalServerError_WhenExceptionIsThrown()
{
    var request = new NotificationRequest
    {
        Type = "Homework",
        TeacherId = "TeacherDoeId",
        TeacherName = "John Doe",
        Date = DateTime.Now,
        DueDate = DateTime.Now.AddDays(7),
        Subject = "Matematika",
        Read = false,
        OfficiallyRead = false,
        Description = "Complete the homework by next week.",
        StudentIds = new List<string> { "BelaId", "KalmanId" }
    };

    var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");


    var response = await _mockClient.PostAsync("/api/notifications", content);

    Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);

    var result = await response.Content.ReadAsStringAsync();
    Assert.Contains("Internal server error", result);
    await ClearDatabaseAsync();
}



[Fact]
public async Task SetToRead_ShouldReturnSuccess_WhenNotificationIsMarkedAsRead()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();

    var response = await _client.GetAsync("/api/notifications");
    var content = await response.Content.ReadAsStringAsync();
    var notifications = JsonConvert.DeserializeObject<List<NotificationResponse>>(content);

    var notificationId = notifications.First().Id;

    var result = await _client.PostAsync($"/api/notifications/read/{notificationId}", null);

    Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);

    var resultMessage = await result.Content.ReadAsStringAsync();
    Assert.Contains("Értesítés olvasottra állítva", resultMessage);
    await ClearDatabaseAsync();
}


[Fact]
public async Task SetToRead_ShouldReturnBadRequest_WhenInvalidNotificationIdIsProvided()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();

    var result = await _client.PostAsync("/api/notifications/read/99999", null);
    
    Assert.Equal(System.Net.HttpStatusCode.BadRequest, result.StatusCode);

    var resultMessage = await result.Content.ReadAsStringAsync();
    Assert.Contains("Bad request", resultMessage);
    await ClearDatabaseAsync();
}

[Fact]
public async Task SetToRead_ShouldReturnInternalServerError_WhenAnExceptionOccurs()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();

    var response = await _client.GetAsync("/api/notifications");
    var content = await response.Content.ReadAsStringAsync();
    var notifications = JsonConvert.DeserializeObject<List<NotificationResponse>>(content);

    var notificationId = notifications.First().Id;
    
    var result = await _mockClient.PostAsync($"/api/notifications/read/{notificationId}", null);

    Assert.Equal(System.Net.HttpStatusCode.InternalServerError, result.StatusCode);

    var resultMessage = await result.Content.ReadAsStringAsync();
    Assert.Contains("Internal server error", resultMessage);
    await ClearDatabaseAsync();
}


[Fact]
public async Task SetToOfficiallyRead_ShouldReturnOk_WhenNotificationIsSuccessfullyUpdated()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();

    var response = await _client.GetAsync("/api/notifications");
    var content = await response.Content.ReadAsStringAsync();
    var notifications = JsonConvert.DeserializeObject<List<NotificationResponse>>(content);

    var notificationId = notifications.First().Id;

    var result = await _client.PostAsync($"/api/notifications/officiallyread/{notificationId}", null);

    Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);

    var resultMessage = await result.Content.ReadAsStringAsync();
    Assert.Contains("Értesítés olvasottra állítva", resultMessage);
    await ClearDatabaseAsync();
}



[Fact]
public async Task SetToOfficiallyRead_ShouldReturnBadRequest_WhenInvalidIdIsProvided()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();

    var invalidId = -1;

    var result = await _client.PostAsync($"/api/notifications/officiallyread/{invalidId}", null);

    Assert.Equal(System.Net.HttpStatusCode.BadRequest, result.StatusCode);

    var resultMessage = await result.Content.ReadAsStringAsync();
    Assert.Contains("Bad request", resultMessage);
    await ClearDatabaseAsync();
}



[Fact]
public async Task SetToOfficiallyRead_ShouldReturnInternalServerError_WhenAnExceptionOccurs()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();

    var response = await _client.GetAsync("/api/notifications");
    var content = await response.Content.ReadAsStringAsync();
    var notifications = JsonConvert.DeserializeObject<List<NotificationResponse>>(content);

    var notificationId = notifications.First().Id;


    var result = await _mockClient.PostAsync($"/api/notifications/officiallyread/{notificationId}", null);

    Assert.Equal(System.Net.HttpStatusCode.InternalServerError, result.StatusCode);

    var resultMessage = await result.Content.ReadAsStringAsync();
    Assert.Contains("Internal server error", resultMessage);
    await ClearDatabaseAsync();
}



[Fact]
public async Task GetHomeworks_ShouldReturnHomeworks_WhenHomeworksExist()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();

    var response = await _client.GetAsync("/api/notifications/homeworks");

    Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

    var content = await response.Content.ReadAsStringAsync();
    var homeworks = JsonConvert.DeserializeObject<List<NotificationBase>>(content);

    Assert.NotEmpty(homeworks);
    Assert.All(homeworks, hw => Assert.Equal("Homework", hw.Type));
    await ClearDatabaseAsync();
}


[Fact]
public async Task GetHomeworks_ShouldReturnEmptyList_WhenNoHomeworksExist()
{
    await ClearDatabaseAsync();

    var response = await _client.GetAsync("/api/notifications/homeworks");

    Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

    var content = await response.Content.ReadAsStringAsync();
    var homeworks = JsonConvert.DeserializeObject<List<NotificationBase>>(content);

    Assert.Empty(homeworks);
    await ClearDatabaseAsync();
}



[Fact]
public async Task GetHomeworks_ShouldReturnInternalServerError_WhenExceptionOccurs()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();

    var result = await _mockClient.GetAsync("/api/notifications/homeworks");

    Assert.Equal(System.Net.HttpStatusCode.InternalServerError, result.StatusCode);

    var resultMessage = await result.Content.ReadAsStringAsync();
    Assert.Contains("Internal server error", resultMessage);
    await ClearDatabaseAsync();
}


[Fact]
public async Task GetExams_ShouldReturnExams_WhenExamsExist()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();

    var response = await _client.GetAsync("/api/notifications/exams");

    Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

    var content = await response.Content.ReadAsStringAsync();
    var exams = JsonConvert.DeserializeObject<List<NotificationBase>>(content);

    Assert.NotEmpty(exams);
    Assert.All(exams, exam => Assert.Equal("Exam", exam.Type));
    await ClearDatabaseAsync();
}

[Fact]
public async Task GetExams_ShouldReturnEmptyList_WhenNoExamsExist()
{
    await ClearDatabaseAsync();

    var response = await _client.GetAsync("/api/notifications/exams");

    Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

    var content = await response.Content.ReadAsStringAsync();
    var exams = JsonConvert.DeserializeObject<List<NotificationBase>>(content);

    Assert.Empty(exams);
    await ClearDatabaseAsync();
}

[Fact]
public async Task GetExams_ShouldReturnInternalServerError_WhenExceptionOccurs()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();

    var result = await _mockClient.GetAsync("/api/notifications/exams");

    Assert.Equal(System.Net.HttpStatusCode.InternalServerError, result.StatusCode);

    var resultMessage = await result.Content.ReadAsStringAsync();
    Assert.Contains("Internal server error", resultMessage);
    await ClearDatabaseAsync();
}

[Fact]
public async Task GetOthers_ShouldReturnOthers_WhenOthersExist()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();

    var response = await _client.GetAsync("/api/notifications/others");

    Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

    var content = await response.Content.ReadAsStringAsync();
    var others = JsonConvert.DeserializeObject<List<NotificationBase>>(content);
    
    Assert.All(others, other => Assert.Equal("Other", other.Type));
    await ClearDatabaseAsync();
}


[Fact]
public async Task GetOthers_ShouldReturnEmptyList_WhenNoOthersExist()
{
    await ClearDatabaseAsync();

    var response = await _client.GetAsync("/api/notifications/others");

    Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

    var content = await response.Content.ReadAsStringAsync();
    var others = JsonConvert.DeserializeObject<List<NotificationBase>>(content);

    Assert.Empty(others);
    await ClearDatabaseAsync();
}



[Fact]
public async Task GetOthers_ShouldReturnInternalServerError_WhenExceptionOccurs()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();

    var result = await _mockClient.GetAsync("/api/notifications/others");

    Assert.Equal(System.Net.HttpStatusCode.InternalServerError, result.StatusCode);

    var resultMessage = await result.Content.ReadAsStringAsync();
    Assert.Contains("Internal server error", resultMessage);
    await ClearDatabaseAsync();
}



[Fact]
public async Task GetMissingEquipments_ShouldReturnEmptyList_WhenNoMissingEquipmentsExist()
{
    await ClearDatabaseAsync();

    var response = await _client.GetAsync("/api/notifications/missingequipments");

    Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

    var content = await response.Content.ReadAsStringAsync();
    var missingEquipments = JsonConvert.DeserializeObject<List<NotificationBase>>(content);

    Assert.Empty(missingEquipments);
    await ClearDatabaseAsync();
}

[Fact]
public async Task GetMissingEquipments_ShouldReturnMissingEquipments_WhenMissingEquipmentsExist()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();

    var response = await _client.GetAsync("/api/notifications/missingequipments");

    Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

    var content = await response.Content.ReadAsStringAsync();
    var missingEquipments = JsonConvert.DeserializeObject<List<NotificationBase>>(content);

    Assert.NotEmpty(missingEquipments);
    await ClearDatabaseAsync();
}

[Fact]
public async Task GetMissingEquipments_ShouldReturnInternalServerError_WhenExceptionOccurs()
{
    var response = await _mockClient.GetAsync("/api/notifications/missingequipments");

    Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);

    var resultMessage = await response.Content.ReadAsStringAsync();
    Assert.Contains("Internal server error", resultMessage);
    await ClearDatabaseAsync();
}

[Fact]
public async Task Delete_ShouldReturnOk_WhenNotificationIsDeleted()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();

    var response = await _client.GetAsync("/api/notifications");
    var content = await response.Content.ReadAsStringAsync();
    var notifications = JsonConvert.DeserializeObject<List<NotificationResponse>>(content);

    var notificationId = notifications.First().Id;

    var deleteResponse = await _client.DeleteAsync($"/api/notifications/{notificationId}");

    Assert.Equal(System.Net.HttpStatusCode.OK, deleteResponse.StatusCode);

    var deleteContent = await deleteResponse.Content.ReadAsStringAsync();
    Assert.Equal("Notification successfully deleted", deleteContent);

    var getResponse = await _client.GetAsync("/api/notifications");
    var getContent = await getResponse.Content.ReadAsStringAsync();
    var remainingNotifications = JsonConvert.DeserializeObject<List<NotificationResponse>>(getContent);

    Assert.DoesNotContain(remainingNotifications, n => n.Id == notificationId);
    await ClearDatabaseAsync();
}



[Fact]
public async Task Delete_ShouldReturnNotFound_WhenNotificationDoesNotExist()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();
    var response = await _client.DeleteAsync("/api/notifications/99999");

    Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);

    var content = await response.Content.ReadAsStringAsync();
    Assert.Contains("Not found", content);
    await ClearDatabaseAsync();
}

[Fact]
public async Task Delete_ShouldReturnInternalServerError_WhenExceptionOccurs()
{
    await ClearDatabaseAsync();
    await SeedWithNotifications();

    var response = await _client.GetAsync("/api/notifications");
    var content = await response.Content.ReadAsStringAsync();
    var notifications = JsonConvert.DeserializeObject<List<NotificationResponse>>(content);

    var notificationId = notifications.First().Id;

    var deleteResponse = await _mockClient.DeleteAsync($"/api/notifications/{notificationId}");

    Assert.Equal(System.Net.HttpStatusCode.InternalServerError, deleteResponse.StatusCode);

    var deleteContent = await deleteResponse.Content.ReadAsStringAsync();
    Assert.Contains("Internal server error", deleteContent);
    await ClearDatabaseAsync();
}


private async Task ClearDatabaseAsync()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();
            var grades = context.Grades.ToList();
            context.Grades.RemoveRange(grades);
            
            var messages = context.Messages.ToList();
            context.Messages.RemoveRange(messages);
            
            var notifications = context.Notifications.ToList();
            context.Notifications.RemoveRange(notifications);
            var parents = context.Parents.ToList();
            context.Parents.RemoveRange(parents);

            var students = context.Students.ToList();
            context.Students.RemoveRange(students);

            var teachers = context.Teachers.ToList();
            context.Teachers.RemoveRange(teachers);
            
            var classes = context.ClassesOfStudents.ToList();
            context.ClassesOfStudents.RemoveRange(classes);
            
            var teacherSubjects = context.TeacherSubjects.ToList();
            context.TeacherSubjects.RemoveRange(teacherSubjects);

            await context.SaveChangesAsync();
        }
    }

    
    
       private async Task SeederWithoutNotifications()
{
    using (var scope = _factory.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();
        
        var student1 = new Student
        {
            Id = "BelaId",
            UserName = "kovacsbela",
            FirstName = "Béla",
            FamilyName = "Kovács",
            BirthDate = new DateTime(2005, 5, 5),
            BirthPlace = "Budapest",
            StudentNo = "S12345",
            Role = "Student"
        };
      
                context.Students.Add(student1);
                await context.SaveChangesAsync();

        var parent1 = new Parent
        {
            Id = "BelaParentId",
            UserName = "kovacserno",
            Email = "parent1@example.com",
            FirstName = "Ernő",
            FamilyName = "Kovács",
            ChildName = "Kovács Béla",
            StudentId = student1.Id,
            Role = "Parent"
        };
        
        context.Users.AddRange(parent1);
        await context.SaveChangesAsync();
        
        var class1 = new ClassOfStudents
        {
            Name = "1A",
            Grade = "1",
            Section = "A",
            Students = new List<Student> { student1 }
        };

        context.ClassesOfStudents.Add(class1);
        await context.SaveChangesAsync();

        var teacher1 = new Teacher
        {
            Id = "TeacherDoeId",
            FirstName = "John",
            FamilyName = "Doe",
            UserName = "johndoe",
            Email = "johndoe@example.com",
            Role = "Teacher"
        };

        context.Teachers.Add(teacher1);

        var teacherSubject1 = new TeacherSubject
        {
            Subject = "Matematika",
            TeacherId = teacher1.Id,
            Teacher = teacher1,
            ClassOfStudentsId = class1.Id,
            ClassOfStudents = class1,
            ClassName = class1.Name
        };

        context.TeacherSubjects.Add(teacherSubject1);
        await context.SaveChangesAsync();

    }
}
    
    
   private async Task SeedWithNotifications()
{
    using (var scope = _factory.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();
        
        var student1 = new Student
        { Id = "BelaId",
            UserName = "kovacsbela",
            FirstName = "Béla",
            FamilyName = "Kovács",
            BirthDate = new DateTime(2005, 5, 5),
            BirthPlace = "Budapest",
            StudentNo = "S12345",
            Role = "Student"
        };

        var student2 = new Student
        {
            Id = "KalmanId",
            UserName = "hajdukalman",
            FirstName = "Kálmán",
            FamilyName = "Hajdu",
            BirthDate = new DateTime(2006, 6, 6),
            BirthPlace = "Debrecen",
            StudentNo = "S67890",
            Role = "Student"
        };

        var students = new List<Student> { student1, student2};

        foreach (var student in students)
        {
            var existingStudent = await context.Students.FindAsync(student.Id);
            if (existingStudent == null)
            {
                context.Students.Add(student);
            }
            else
            {
                context.Entry(existingStudent).CurrentValues.SetValues(student);
            }
        }
        await context.SaveChangesAsync();
   
        var parent1 = new Parent
        {
            Id="BelaParentId",
            UserName = "kovacserno",
            Email = "parent1@example.com",
            FirstName = "Ernő",
            FamilyName = "Kovács",
            ChildName = "Béla Kovács",
            StudentId = student1.Id,
            Role = "Parent"
        };

        var parent2 = new Parent
        {
            Id = "KalmanParentId",
            UserName = "hajdubotond",
            Email = "parent2@example.com",
            FirstName = "Botond",
            FamilyName = "Hajdu",
            ChildName = "Kálmán Hajdu",
            StudentId = student2.Id,
            Role = "Parent"
        };

   
        context.Users.AddRange(parent1, parent2);
        await context.SaveChangesAsync();

        var class1 = new ClassOfStudents
        {
            Name = "1A",
            Grade = "1",
            Section = "A",
            Students = new List<Student> { student1, student2}
        };

        context.ClassesOfStudents.Add(class1);
        await context.SaveChangesAsync();

        var teacher1 = new Teacher
        {
            Id="TeacherDoeId",
            FirstName = "John",
            FamilyName = "Doe",
            UserName = "johndoe",
            Email = "johndoe@example.com",
            Role = "Teacher"
        };

        context.Teachers.Add(teacher1);

        var teacherSubject1 = new TeacherSubject
        {
            Subject = "Matematika",
            TeacherId = teacher1.Id,
            Teacher = teacher1,
            ClassOfStudentsId = class1.Id,
            ClassOfStudents = class1,
            ClassName = class1.Name
        };

        context.TeacherSubjects.Add(teacherSubject1);
        await context.SaveChangesAsync();
        
        var notification1 = new NotificationBase
        {
            TeacherId = teacher1.Id,
            TeacherName = teacher1.FamilyName + " " + teacher1.FirstName,
            Type = "Homework",
            Date = DateTime.Now,
            DueDate = DateTime.Now.AddDays(7),
            StudentId = student1.Id,
            StudentName = student1.FamilyName + " " + student1.FirstName,
            ParentId = parent1.Id,
            ParentName = parent1.FamilyName + " " + parent1.FirstName,
            Description = "Complete your homework by next week.",
            Subject = Subjects.Matematika,
            Read = false,
            OfficiallyRead = false
        };

        var notification2 = new NotificationBase
        {
            TeacherId = teacher1.Id,
            TeacherName = teacher1.FamilyName + " " + teacher1.FirstName,
            Type = "MissingEquipment",
            Date = DateTime.Now,
            DueDate = DateTime.Now.AddDays(3),
            StudentId = student1.Id,
            StudentName = student1.FamilyName + " " + student1.FirstName,
            ParentId = parent1.Id,
            ParentName = parent1.FamilyName + " " + parent1.FirstName,
            Description = "Your grade for the last test is ready.",
            Subject = Subjects.Matematika,
            Read = false,
            OfficiallyRead = false
        };

        var notification3 = new NotificationBase
        {
            TeacherId = teacher1.Id,
            TeacherName = teacher1.FamilyName + " " + teacher1.FirstName,
            Type = "Homework",
            Date = DateTime.Now,
            DueDate = DateTime.Now.AddDays(7),
            StudentId = student2.Id,
            StudentName = student2.FamilyName + " " + student2.FirstName,
            ParentId = parent2.Id,
            ParentName = parent2.FamilyName + " " + parent2.FirstName,
            Description = "Complete your homework by next week.",
            Subject = Subjects.Matematika,
            Read = false,
            OfficiallyRead = false
        };

        var notification4 = new NotificationBase
        {
            TeacherId = teacher1.Id,
            TeacherName = teacher1.FamilyName + " " + teacher1.FirstName,
            Type = "Exam",
            Date = DateTime.Now.AddDays(4),
            DueDate = DateTime.Now.AddDays(3),
            StudentId = student2.Id,
            StudentName = student2.FamilyName + " " + student2.FirstName,
            ParentId = parent2.Id,
            ParentName = parent2.FamilyName + " " + parent2.FirstName,
            Description = "Your grade for the last test is ready.",
            Subject = Subjects.Matematika,
            Read = false,
            OfficiallyRead = false
        };

        context.Notifications.AddRange(notification1, notification2, notification3, notification4);
        await context.SaveChangesAsync();
    }
}


    }