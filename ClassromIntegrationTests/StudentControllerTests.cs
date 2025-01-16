using System.Net;
using System.Net.Http.Json;
using System.Text;
using Classroom.Service.Repositories;
using Classroom.Data;
using ClassromIntegrationTests.MockRepos;
using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace ClassromIntegrationTests;

public class StudentControllerTests:IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly HttpClient _mockClient;

    public StudentControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();


        var mockFactory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddTransient<IStudentRepository, MockStudentRepository>();
            });
        });

        _mockClient = mockFactory.CreateClient();
    }
    
    
    [Fact]
    public async Task GetAll_ReturnsEmptyList_WhenNoStudents()
    {
        await ClearDatabaseAsync();
        
        var response = await _client.GetAsync("/api/students");

        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.Equal("[]", responseBody);
        await ClearDatabaseAsync();
    }


    [Fact]
    public async Task GetAll_ReturnsStudents_WhenStudentsExist()
    {
        await ClearDatabaseAsync();
        await SeedStudentsAsync();

        var response = await _client.GetAsync("/api/students");

        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();

        var students = JsonConvert.DeserializeObject<List<Student>>(responseBody);

        Assert.Contains(students, s => s.FirstName == "Elek" && s.FamilyName == "Kerekes");
        Assert.Contains(students, s => s.FirstName == "Klaudia" && s.FamilyName == "Tóth");
        
        Assert.Equal(2, students.Count);
        await ClearDatabaseAsync();
    }

    
    
    [Fact]
    public async Task GetAll_ReturnsInternalServerError_WhenExceptionThrown()
    {
        var response = await _mockClient.GetAsync("/api/students");

        Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);

        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("Internal server error", responseBody);
        await ClearDatabaseAsync();
    }

    
    [Fact]
    public async Task GetStudentById_ReturnsStudent_WhenStudentExists()
    {
        await ClearDatabaseAsync();
        await SeedStudentsAsync();

        var studentId = "ElekId";
        var response = await _client.GetAsync($"/api/students/{studentId}");
        response.EnsureSuccessStatusCode();
        var student = await response.Content.ReadFromJsonAsync<Student>();

        Assert.NotNull(student);
        Assert.Equal("Elek", student.FirstName);
        Assert.Equal("Kerekes", student.FamilyName);
        await ClearDatabaseAsync();
    }


    
    
    [Fact]
    public async Task GetStudentById_ReturnsBadRequest_WhenStudentDoesNotExist()
    {
        await ClearDatabaseAsync();
        await SeedStudentsAsync();
        
        var invalidStudentId = "-1";
        
        var response = await _client.GetAsync($"/api/students/{invalidStudentId}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var responseBody = await response.Content.ReadAsStringAsync();
        
        Assert.Contains("Bad request:Student with the given id not found", responseBody);
        await ClearDatabaseAsync();
    }

    
    [Fact]
    public async Task GetStudentById_ReturnsInternalServerError_WhenExceptionThrown()
    {
        await ClearDatabaseAsync();
        await SeedStudentsAsync();

        var studentId = "ElekId";
        var response = await _mockClient.GetAsync($"/api/students/{studentId}");
     
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
 
        var responseBody = await response.Content.ReadAsStringAsync();

        Assert.Contains("Internal server error", responseBody);
        await ClearDatabaseAsync();
    }

   
    [Fact]
    public async Task Post_AddsNewStudent_WhenRequestIsValid()
    {
        await ClearDatabaseAsync();

        try
        {
            var newStudentRequest = new StudentRequest
            {
                FirstName = "János",
                FamilyName = "Kovács",
                Email = "janos.kovacs@example.com",
                Username = "janoskovacs",
                Password = "password123",
                BirthDate = "2001-01-01",
                BirthPlace = "Budapest",
                StudentNo = "wrawervscef",
            };

            var content = new StringContent(JsonConvert.SerializeObject(newStudentRequest), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/students", content);
            
            var responseBody = await response.Content.ReadAsStringAsync();
        
            Console.WriteLine($"Response Status Code: {response.StatusCode}");
            Console.WriteLine($"Response Body: {responseBody}");
            
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Test failed with status code: {response.StatusCode} and response body: {responseBody}");
            }
            
            Assert.Contains("Successfully added new student", responseBody);
        }
        finally
        {
            await ClearDatabaseAsync();
        }
    }





    
    [Fact]
    public async Task Post_ReturnsBadRequest_WhenStudentWithSameStudentNoExists()
    {
        await ClearDatabaseAsync();
        await SeedStudentsAsync();

        var alreadyExistingStudentNo = "r3r3r";
        var newStudentRequest = new StudentRequest
        {
            FirstName = "János",
            FamilyName = "Kovács",
            Email = "janos.kovacs@example.com",
            Username = "janoskovacs",
            Password = "password123",
            BirthDate = "2010-05-01",
            BirthPlace = "Budapest",
            StudentNo = alreadyExistingStudentNo
        };

        var content = new StringContent(JsonConvert.SerializeObject(newStudentRequest), Encoding.UTF8, "application/json");
    
        var response = await _client.PostAsync("/api/students", content);

        Assert.Equal(400, (int)response.StatusCode);

        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("A student with the same student number already exists.", responseBody);
        await ClearDatabaseAsync();
    }

    
    [Fact]
    public async Task Post_ReturnsInternalServerError_WhenExceptionIsThrown()
    {
        var newStudentRequest = new StudentRequest
        {
            FirstName = "János",
            FamilyName = "Kovács",
            Email = "janos.kovacs@example.com",
            Username = "janoskovacs",
            Password = "password123",
            BirthDate = "2010-05-01",
            BirthPlace = "Budapest",
            StudentNo = "r3r3r",
        };

        var content = new StringContent(JsonConvert.SerializeObject(newStudentRequest), Encoding.UTF8, "application/json");
        
        var response = await _mockClient.PostAsync("/api/students", content);

        Assert.Equal(500, (int)response.StatusCode);

        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("Internal server error:", responseBody);
        await ClearDatabaseAsync();
    }

    private async Task SeedStudentsAsync()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();

            var student1 = new Student
            {
                Id="ElekId",
                FirstName = "Elek",
                FamilyName = "Kerekes",
                UserName = "kerekeselek",
                Email = "elek@gmail.com",
                BirthDate = new DateTime(2010, 5, 1),
                BirthPlace = "City A",
                StudentNo = "r3r3r",
                Role = "Student"
            };

            var student2 = new Student
            {
                Id = "KlaudiaId",
                FirstName = "Klaudia",
                FamilyName = "Tóth",
                UserName = "tothklaudia",
                Email = "klaudia@gmail.com",
                BirthDate = new DateTime(2010, 7, 10),
                BirthPlace = "City B",
                StudentNo = "sdfasr32r",
                Role = "Student"
            };
        
            await context.Students.AddRangeAsync(student1, student2);
            await context.SaveChangesAsync();
        }
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


}