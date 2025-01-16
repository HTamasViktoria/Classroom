using System.Net;
using ClassromIntegrationTests.MockRepos;
using Classroom.Data;
using Classroom.Service.Repositories;
using Classroom.Model.DataModels;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace ClassromIntegrationTests;

[Collection("IntegrationTests")]
public class ParentControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly HttpClient _mockClient;

    public ParentControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();

        var mockFactory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddTransient<IParentRepository, MockParentRepository>();
            });
        });

        _mockClient = mockFactory.CreateClient();
    }

    
    [Fact]
    public async Task GetAllParents_ShouldReturnEmpty_WhenNoParentsExist()
    {
        await ClearDatabaseAsync();
        var response = await _client.GetAsync("/api/parents");
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<Parent>>(responseBody);

        Assert.NotNull(result);
        Assert.IsType<List<Parent>>(result);
        Assert.Empty(result);

        await ClearDatabaseAsync();
    }

    [Fact]
    public async Task GetAllParents_ShouldReturnList_WhenParentsExist()
    {
        await ClearDatabaseAsync();
        await SeedDatabaseAsync();

        var response = await _client.GetAsync("/api/parents");
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<List<Parent>>(responseBody);
        Assert.NotNull(result);

        Assert.IsType<List<Parent>>(result);
        Assert.NotEmpty(result);

        Assert.Contains(result, p => p.Id == "BelaParentId" && p.FirstName == "Etelka" && p.ChildName == "Kovács Béla");
        Assert.Contains(result, p => p.Id == "KalmanParentId" && p.FirstName == "Emőke" && p.ChildName == "Hajdu Kálmán");
        await ClearDatabaseAsync();
    }

    
    [Fact]
    public async Task GetAllParents_ShouldReturnInternalServerError_WhenExceptionThrown()
    {
     
        var response = await _mockClient.GetAsync("/api/parents");
        
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("Internal server error", responseBody);
        await ClearDatabaseAsync();
    }
    
    
    [Fact]
    public async Task GetParentsByStudentId_ShouldReturnParents_WhenParentsExistForStudent()
    {
        await ClearDatabaseAsync();
        await SeedDatabaseAsync();

        var studentId = "KalmanId";
        
        var response = await _client.GetAsync($"/api/parents/bystudent/{studentId}");
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<List<Parent>>(responseBody);
        Assert.NotNull(result);
        Assert.IsType<List<Parent>>(result);
        Assert.NotEmpty(result);

        Assert.Contains(result, p => p.Id == "KalmanParentId" && p.FirstName == "Emőke" && p.ChildName == "Hajdu Kálmán");
        await ClearDatabaseAsync();
    }

   
    
    [Fact]
    public async Task GetParentsByStudentId_ShouldReturnEmptyList_WhenNoParentsExistForStudent()
    {
        await ClearDatabaseAsync();
        await SeedDatabaseAsync();
        var studentId = "KalmanId";
        await DeleteParentsByStudentId(studentId);
    
        var response = await _client.GetAsync($"/api/parents/bystudent/{studentId}");
        
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<Parent>>(responseBody);
        Assert.NotNull(result);
        Assert.Empty(result);
        await ClearDatabaseAsync();
    }


    
    [Fact]
    public async Task GetParentsByStudentId_ShouldReturnBadRequest_WhenStudentIdDoesNotExist()
    {
   
        await ClearDatabaseAsync();
        await SeedDatabaseAsync();
        var nonExistentStudentId = "-1";
        
        var response = await _client.GetAsync($"/api/parents/bystudent/{nonExistentStudentId}");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("Bad request", responseBody);
        await ClearDatabaseAsync();
    }

    
    [Fact]
    public async Task GetParentsByStudentId_ShouldReturnInternalServerError_WhenExceptionThrown()
    {
        var studentId = "KalmanId";
        var response = await _mockClient.GetAsync($"/api/parents/bystudent/{studentId}");
        
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("Internal server error", responseBody);
        await ClearDatabaseAsync();
    }

    
    [Fact]
    public async Task GetByParentId_ShouldReturnParent_WhenParentExists()
    {
        await ClearDatabaseAsync();
        await SeedDatabaseAsync();
        
        var parentId = "BelaParentId";
        
        var response = await _client.GetAsync($"/api/parents/{parentId}");
        
        response.EnsureSuccessStatusCode();
        
        var responseBody = await response.Content.ReadAsStringAsync();
        var parent = JsonConvert.DeserializeObject<Parent>(responseBody);
        Assert.NotNull(parent);
        Assert.Equal(parentId, parent.Id);
        Assert.Equal("Etelka", parent.FirstName);
        Assert.Equal("Kovács", parent.FamilyName);
        Assert.Equal("Kovács Béla", parent.ChildName);
        Assert.Equal("BelaId", parent.StudentId);
        await ClearDatabaseAsync();
    }

    
    
    [Fact]
    public async Task GetByParentId_ShouldReturnBadRequest_WhenParentDoesNotExist()
    {
      
        await ClearDatabaseAsync();
        await SeedDatabaseAsync();

        var nonExistentParentId = "-1";
        
        var response = await _client.GetAsync($"/api/parents/{nonExistentParentId}");
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.Contains("Bad request: No parent found with the given id", responseBody);
        await ClearDatabaseAsync();
    }

    
    [Fact]
    public async Task GetByParentId_ShouldReturnInternalServerError_WhenExceptionThrown()
    {
        await ClearDatabaseAsync();
        await SeedDatabaseAsync();
        
        var parentId = "BelaParentId";
        
        var response = await _mockClient.GetAsync($"/api/parents/{parentId}");
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("Internal server error:", responseBody);
        Assert.Contains("Mock exception for testing", responseBody);
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
    
    
    
    private async Task SeedDatabaseAsync()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();

            var testStudents = new List<Student>
            {
                new Student
                {
                    Id = "BelaId",
                    UserName = "kovacsbela",
                    FirstName = "Béla",
                    FamilyName = "Kovács",
                    BirthDate = new DateTime(2005, 5, 5),
                    BirthPlace = "Budapest",
                    StudentNo = "S12345",
                    Role = "Student"
                },
                new Student
                {
                    Id = "KalmanId",
                    UserName = "hajdukalman",
                    FirstName = "Kálmán",
                    FamilyName = "Hajdu",
                    BirthDate = new DateTime(2006, 6, 6),
                    BirthPlace = "Debrecen",
                    StudentNo = "S67890",
                    Role = "Student"
                }
            };

            var testParents = new List<Parent>
            {
                new Parent
                {
                    Id = "BelaParentId",
                    FamilyName = "Kovács",
                    FirstName = "Etelka",
                    ChildName = "Kovács Béla",
                    StudentId = "BelaId",
                    Role = "Parent"
                },
                new Parent
                {
                    Id = "KalmanParentId",
                    FamilyName = "Hajdu",
                    FirstName = "Emőke",
                    ChildName = "Hajdu Kálmán",
                    StudentId = "KalmanId",
                    Role = "Parent"
                }
            };

            foreach (var student in testStudents)
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

            foreach (var parent in testParents)
            {
                var existingParent = await context.Parents.FindAsync(parent.Id);
                if (existingParent == null)
                {
                    context.Parents.Add(parent);
                }
                else
                {
                    context.Entry(existingParent).CurrentValues.SetValues(parent);
                }
            }
            await context.SaveChangesAsync();
        }
    }
    
    
    
    
    private async Task DeleteParentsByStudentId(string studentId)
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();
            var parentsToDelete = context.Parents.Where(p => p.StudentId == studentId).ToList();
            
            if (parentsToDelete.Any())
            {
                context.Parents.RemoveRange(parentsToDelete);
                await context.SaveChangesAsync();
            }
        }
    }

}
