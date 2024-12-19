using System.Net;
using System.Net.Http.Json;
using ClassromIntegrationTests.MockRepos;
using Classroom.Service.Repositories;
using Classroom.Data;
using Classroom.Model.DataModels;
using Classroom.Model.ResponseModels;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;

namespace ClassromIntegrationTests;

public class UserControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly HttpClient _mockClient;
    private readonly HttpClient _studentsClient;

    public UserControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();

        var mockFactory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddTransient<IUserRepository, MockUserRepository>();
                services.AddTransient<IStudentRepository, MockStudentRepository>();
            });
        });

        _mockClient = mockFactory.CreateClient();
    
      
        var mockStudentFactory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddTransient<IStudentRepository, MockStudentRepository>();
            });
        });

        _studentsClient = mockStudentFactory.CreateClient();
    }


    [Fact]
    public async Task GetAllTeachers_IfNoTeacherExists_ReturnsEmptyList()
    {
        await ClearDatabaseAsync();

        var response = await _client.GetAsync("/api/users/allteachers");

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var teachers = JsonConvert.DeserializeObject<List<Teacher>>(content);

        Assert.Empty(teachers);
    }


    [Fact]
    public async Task GetAllTeachers_IfThereAreTeachers_ReturnsTeachersList()
    {
        await ClearDatabaseAsync();
        await SeedTeachersAsync();

        var response = await _client.GetAsync("/api/users/allteachers");

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var teachers = JsonConvert.DeserializeObject<List<Teacher>>(content);

        Assert.Equal(2, teachers.Count);
        Assert.All(teachers, teacher => Assert.IsType<Teacher>(teacher));
    }

    [Fact]
    public async Task PostTeacher_ReturnsInternalServerError_WhenExceptionOccurs()
    {
        await ClearDatabaseAsync();

        var teacher = new Teacher
        {
            FirstName = "Etelka",
            FamilyName = "Kapusi",
            Email = "etus@gmail.com",
           Role = "Teacher"
        };

        var response = await _mockClient.PostAsJsonAsync("/api/users/teachers", teacher);

        Assert.Equal(500, (int)response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("Internal server error", responseContent);
    }



    [Fact]
    public async Task GetAllParents_ReturnsParentsList_WhenParentsExist()
    {
        await ClearDatabaseAsync();
        await SeedStudentsAndParentsAsync();

        var response = await _client.GetAsync("/api/users/allparents");

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var parents = JsonConvert.DeserializeObject<List<Parent>>(content);

        Assert.Equal(2, parents.Count);
        Assert.All(parents, parent => Assert.IsType<Parent>(parent));
        Assert.Contains(parents, p => p.FirstName == "Eszter" && p.FamilyName == "Tóthné Varga");
        Assert.Contains(parents, p => p.FirstName == "Emil" && p.FamilyName == "Kerekes");
    }


    [Fact]
    public async Task GetAllParents_ReturnsEmptyList_WhenNoParentsExist()
    {
        await ClearDatabaseAsync();

        var response = await _client.GetAsync("/api/users/allparents");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var parents = JsonConvert.DeserializeObject<List<Parent>>(content);

        Assert.Empty(parents);
    }



    [Fact]
    public async Task GetAllParents_ReturnsInternalServerError_WhenExceptionOccurs()
    {
        await ClearDatabaseAsync();

        var response = await _mockClient.GetAsync("/api/users/allparents");

        Assert.Equal(500, (int)response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Contains("Internal server error", responseContent);
    }




    [Fact]
    public async Task GetTeacherById_ReturnsTeacher_WhenTeacherExists()
    {
        await ClearDatabaseAsync();
        await SeedTeachersAsync();

        var teacherId = "teacher2Id";

        var teacherResponse = await _client.GetAsync($"/api/users/teachers/{teacherId}");

        teacherResponse.EnsureSuccessStatusCode();

        var teacherContent = await teacherResponse.Content.ReadAsStringAsync();
        var teacherById = JsonConvert.DeserializeObject<Teacher>(teacherContent);

        Assert.Equal(teacherId, teacherById.Id);
        Assert.Equal("Jane", teacherById.FirstName);
        Assert.Equal("Smith", teacherById.FamilyName);
        Assert.Equal("jane.smith@example.com", teacherById.Email);
    }



    [Fact]
    public async Task GetTeacherById_ThrowsArgumentException_WhenTeacherDoesNotExist()
    {
        await ClearDatabaseAsync();
        await SeedTeachersAsync();

        var invalidTeacherId = "-1";

        var response = await _client.GetAsync($"/api/users/teachers/{invalidTeacherId}");

        Assert.Equal(400, (int)response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains($"Teacher with ID {invalidTeacherId} not found.", content);
    }



    [Fact]
    public async Task GetTeacherById_ReturnsInternalServerError_WhenExceptionOccurs()
    {

        await ClearDatabaseAsync();
        await SeedTeachersAsync();

        var validTeacherId = "teacher1Id";

        var response = await _mockClient.GetAsync($"/api/users/teachers/{validTeacherId}");

        Assert.Equal(500, (int)response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Internal server error", content);
    }



    [Fact]
    public async Task GetParentById_ReturnsParent_WhenParentExists()
    {
        await ClearDatabaseAsync();
        await SeedStudentsAndParentsAsync();

        var parentId = "Parent1Id";

        var parentResponse = await _client.GetAsync($"/api/users/parents/{parentId}");
        parentResponse.EnsureSuccessStatusCode();
        var parentContent = await parentResponse.Content.ReadAsStringAsync();
        var parentById = JsonConvert.DeserializeObject<Parent>(parentContent);

        Assert.IsType<Parent>(parentById);
        Assert.Equal(parentId, parentById.Id);
        Assert.Equal("Emil", parentById.FirstName);
        Assert.Equal("Kerekes", parentById.FamilyName);
        Assert.Equal("emil@gmail.com", parentById.Email);
    }


    [Fact]
    public async Task GetParentById_ThrowsArgumentException_WhenParentDoesNotExist()
    {
        await ClearDatabaseAsync();
        await SeedStudentsAndParentsAsync();

        var nonExistentParentId = "-1";

        var response = await _client.GetAsync($"/api/users/parents/{nonExistentParentId}");

        Assert.Equal(400, (int)response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains($"Parent with ID {nonExistentParentId} not found.", content);
    }


    [Fact]
    public async Task GetParentById_ReturnsInternalServerError_WhenExceptionOccurs()
    {
        await ClearDatabaseAsync();
        await SeedStudentsAndParentsAsync();

        var validParentId = "Parent2Id";

        var response = await _mockClient.GetAsync($"/api/users/parents/{validParentId}");

        Assert.Equal(500, (int)response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Internal server error", content);
    }


    [Fact]
    public async Task AddTeacher_ReturnsCreated_WhenTeacherIsAddedSuccessfully()
    {
        var newTeacher = new Teacher
        {
            FirstName = "Alice",
            FamilyName = "Johnson",
            UserName = "alicejohnson",
            Email = "alice.johnson@example.com",
            Role = "Teacher"
        };

        var response = await _client.PostAsJsonAsync("/api/users/teachers", newTeacher);

        Assert.Equal(201, (int)response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var createdTeacher = JsonConvert.DeserializeObject<Teacher>(content);

        Assert.Equal(newTeacher.FirstName, createdTeacher.FirstName);
        Assert.Equal(newTeacher.FamilyName, createdTeacher.FamilyName);
        Assert.Equal(newTeacher.UserName, createdTeacher.UserName);
        Assert.Equal(newTeacher.Email, createdTeacher.Email);
    }



    [Fact]
    public async Task AddTeacher_ReturnsBadRequest_WhenTeacherAlreadyExists()
    {
        var existingTeacher = new Teacher
        {
            FirstName = "John",
            FamilyName = "Doe",
            UserName = "johndoe",
            Email = "john.doe@example.com",
            Role = "Teacher"
        };

        await _client.PostAsJsonAsync("/api/users/teachers", existingTeacher);

        var response = await _client.PostAsJsonAsync("/api/users/teachers", existingTeacher);

        Assert.Equal(400, (int)response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Teacher with name John Doe already exists.", content);
    }



    [Fact]
    public async Task AddTeacher_ReturnsInternalServerError_WhenExceptionOccurs()
    {
        var teacher = new Teacher
        {
            FirstName = "Test",
            FamilyName = "Teacher",
            UserName = "testteacher",
            Email = "test.teacher@example.com",
            Role = "Teacher"
        };


        var response = await _mockClient.PostAsJsonAsync("/api/users/teachers/", teacher);

        Assert.Equal(500, (int)response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Internal server error", content);
    }


    [Fact]
    public async Task GetTeacherReceivers_ReturnsEmptyList_WhenNoTeachersExist()
    {
        await ClearDatabaseAsync();
        
        var response = await _client.GetAsync("/api/users/teacherreceivers");

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("[]", content);
    }

    
    [Fact]
    public async Task GetTeacherReceivers_ReturnsTeachers_WhenTeachersExist()
    {
        await SeedTeachersAsync();
        var response = await _client.GetAsync("/api/users/teacherreceivers");

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
     
        var content = await response.Content.ReadAsStringAsync();
    
        Assert.Contains("John Doe", content);
        Assert.Contains("Jane Smith", content);
        
        var teacherReceivers = JsonConvert.DeserializeObject<List<ReceiverResponse>>(content);
        Assert.NotNull(teacherReceivers);
        Assert.Equal(2, teacherReceivers.Count);
        
        var teacher1 = teacherReceivers.FirstOrDefault(t => t.Name == "John Doe");
        var teacher2 = teacherReceivers.FirstOrDefault(t => t.Name == "Jane Smith");
        
        Assert.NotNull(teacher1);
        Assert.Equal("John Doe", teacher1.Name);
        Assert.Equal("Teacher", teacher1.Role);
        Assert.NotNull(teacher2);
        Assert.Equal("Jane Smith", teacher2.Name);
        Assert.Equal("Teacher", teacher2.Role);
    }

    [Fact]
    public async Task GetTeacherReceivers_ReturnsInternalServerError_WhenExceptionOccurs()
    {
        var response = await _mockClient.GetAsync("/api/users/teacherreceivers");

        Assert.Equal(500, (int)response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Hiba történt: Mock exception for testing", content);
    }

    
    
    [Fact]
    public async Task GetParentReceivers_ReturnsEmptyList_WhenNoParentsExist()
    {
        await ClearDatabaseAsync();

        var response = await _client.GetAsync("/api/users/parentreceivers");

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
       
        Assert.Equal("[]", content);
    }

    
    
    [Fact]
    public async Task GetParentReceivers_ReturnsParents_WhenParentsExist()
    {
        await ClearDatabaseAsync();
        await SeedStudentsAndParentsAsync();
        
        var response = await _client.GetAsync("/api/users/parentreceivers");
        
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        
        var receivers = JsonConvert.DeserializeObject<List<ReceiverResponse>>(content);
        
        Assert.Equal(2, receivers.Count);
        
        var emil = receivers.FirstOrDefault(r => r.Name == "Emil Kerekes");
        Assert.NotNull(emil);
        Assert.NotNull(emil.Id);
        Assert.Equal("Parent", emil.Role);

  
        var eszter = receivers.FirstOrDefault(r => r.Name == "Eszter Tóthné Varga");
        Assert.NotNull(eszter);
        Assert.NotNull(eszter.Id);
        Assert.Equal("Parent", eszter.Role);
    }


    [Fact]
    public async Task GetParentReceivers_ReturnsInternalServerError_WhenExceptionOccurs()
    {
      
        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository
            .Setup(repo => repo.GetParentsAsReceivers())
            .Throws(new Exception("Mock exception for testing."));

        var response = await _mockClient.GetAsync("/api/users/parentreceivers");

        Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("Hiba történt", content);
        Assert.Contains("Mock exception for testing", content);
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


    
    private async Task SeedTeachersAsync()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();

            if (!context.Teachers.Any())
            {
                var teachers = new List<Teacher>
                {
                    new Teacher
                    {
                        Id = "teacher1Id",
                        UserName = "teacher1",
                        FirstName = "John",
                        FamilyName = "Doe",
                        Email = "john.doe@example.com",
                        Role = "Teacher"
                    },
                    new Teacher
                    {
                        Id = "teacher2Id",
                        UserName = "teacher2",
                        FirstName = "Jane",
                        FamilyName = "Smith",
                        Email = "jane.smith@example.com",
                        Role = "Teacher"
                    }
                };

                context.Teachers.AddRange(teachers);
                await context.SaveChangesAsync();
            }
        }
    }
    
    
    private async Task SeedStudentsAndParentsAsync()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();

            var student1 = new Student
            {
                Id = "Student1Id",
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
                Id = "Student2Id",
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

        
            var parent1 = new Parent
            { Id = "Parent1Id",
                FirstName = "Emil",
                FamilyName = "Kerekes",
                UserName = "kerekesemil",
                Email = "emil@gmail.com",
                StudentId = student1.Id,
                Role = "Parent"
            };

            var parent2 = new Parent
            {
                Id = "Parent2Id",
                FirstName = "Eszter",
                FamilyName = "Tóthné Varga",
                UserName = "tothnevargaeszter",
                Email = "eszter@gmail.com",
                StudentId = student2.Id, Role = "Parent"
            };

            await context.Parents.AddRangeAsync(parent1, parent2);
            await context.SaveChangesAsync();
        }
    }

}