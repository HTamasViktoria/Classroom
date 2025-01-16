using System.Net.Http.Json;
using Classroom.Data;
using ClassromIntegrationTests.MockRepos;
using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Classroom.Service.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ClassromIntegrationTests;

[Collection("IntegrationTests")]
    public class TeacherControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;
        private readonly HttpClient _mockClient;

        public TeacherControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();

            var mockFactory = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices((context, services) =>
                {
                    services.AddTransient<ITeacherRepository, MockTeacherRepository>();
                });
            });

            _mockClient = mockFactory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ReturnsEmptyList_WhenNoTeachers()
        {
            await ClearDatabaseAsync();

            var response = await _client.GetAsync("/api/teachers");

            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.Equal("[]", responseBody);
            await ClearDatabaseAsync();
        }



        [Fact]
        public async Task GetAll_ReturnsTeachers_WhenTeachersExist()
        {
            await ClearDatabaseAsync();
            await SeedTeachersAsync();

            var response = await _client.GetAsync("/api/teachers");

            response.EnsureSuccessStatusCode();
            var teachers = await response.Content.ReadFromJsonAsync<IEnumerable<Teacher>>();

            Assert.NotEmpty(teachers);
            Assert.Equal(2, teachers.Count());

            var teacherList = teachers.ToList();

            Assert.Contains(teacherList, t => t.FirstName == "John" && t.FamilyName == "Doe" && t.Email == "john.doe@example.com");
            Assert.Contains(teacherList, t => t.FirstName == "Jane" && t.FamilyName == "Smith" && t.Email == "jane.smith@example.com");
            await ClearDatabaseAsync(); }




        [Fact]
        public async Task GetAll_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            await ClearDatabaseAsync();

            var response = await _mockClient.GetAsync("/api/teachers");

            Assert.Equal(500, (int)response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("Internal server error", responseContent);
            await ClearDatabaseAsync();
        }




        [Fact]
        public async Task GetByTeacherId_ReturnsTeacher_WhenTeacherExists()
        {
            await ClearDatabaseAsync();
            await SeedTeachersAsync();

            var teacherId = "JohnId";
            var getByIdResponse = await _client.GetAsync($"/teacher-by-id/{teacherId}");
            getByIdResponse.EnsureSuccessStatusCode();
            var teacherById = await getByIdResponse.Content.ReadFromJsonAsync<Teacher>();

            Assert.NotNull(teacherById);
            Assert.Equal("John", teacherById.FirstName);
            Assert.Equal("Doe", teacherById.FamilyName);
            await ClearDatabaseAsync();
        }



        [Fact]
        public async Task GetByTeacherId_ReturnsNotFound_WhenTeacherDoesNotExist()
        {
            await ClearDatabaseAsync();
            await SeedTeachersAsync();

            var nonExistentId = "-1";
            var getByIdResponse = await _client.GetAsync($"/api/teachers/{nonExistentId}");
            Assert.Equal(404, (int)getByIdResponse.StatusCode);
            await ClearDatabaseAsync();
        }



        [Fact]
        public async Task GetByTeacherId_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            await ClearDatabaseAsync();

            await SeedTeachersAsync();
            var teacherId = "JaneId";
            var response = await _mockClient.GetAsync($"/teacher-by-id/{teacherId}");

            Assert.Equal(500, (int)response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("Internal server error", responseContent);
            await ClearDatabaseAsync();
        }



        [Fact]
        public async Task Post_ReturnsCreated_AndTeacherIsInGetAllList()
        {
            await ClearDatabaseAsync();
            var newTeacher = new TeacherRequest
            {
                FirstName = "Alice",
                FamilyName = "Smith",
                Email = "alice.smith@example.com",
                Username = "alicesmith",
                Password = "StrongPassword123",
                
            };

            var response = await _client.PostAsJsonAsync("/api/teachers", newTeacher);

            Assert.Equal(201, (int)response.StatusCode);

            var getAllResponse = await _client.GetAsync("/api/teachers");

            getAllResponse.EnsureSuccessStatusCode();

            var teachers = await getAllResponse.Content.ReadFromJsonAsync<IEnumerable<Teacher>>();
            var addedTeacher = teachers.FirstOrDefault(t => t.FirstName == "Alice" && t.FamilyName == "Smith");

            Assert.NotNull(addedTeacher);
            await ClearDatabaseAsync();

        }




        [Fact]
        public async Task Post_ReturnsBadRequest_WhenTeacherAlreadyExists()
        {
            await ClearDatabaseAsync();

            await SeedTeachersAsync();

            var duplicateTeacher = new TeacherRequest
            {
                FirstName = "John",
                FamilyName = "Doe",
                Email = "john.doe@example.com",
                Username = "teacher1",
                Password = "NewPassword123"
            };

            var response = await _client.PostAsJsonAsync("/api/teachers", duplicateTeacher);

            Assert.Equal(400, (int)response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.Contains("Bad request:", responseBody);
            Assert.Contains("Teacher already added", responseBody);
            await ClearDatabaseAsync();
        }



        [Fact]
        public async Task Post_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            await ClearDatabaseAsync();

            await SeedTeachersAsync();

            var teacherRequest = new TeacherRequest
            {
                FirstName = "John",
                FamilyName = "Doe",
                Email = "john.doe@example.com",
                Username = "teacher1",
                Password = "Password123"
            };

        
            var response = await _mockClient.PostAsJsonAsync("/api/teachers", teacherRequest);

            Assert.Equal(500, (int)response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("Internal server error", responseContent);
            await ClearDatabaseAsync();
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
                            Id = "JohnId",
                            UserName = "teacher1",
                            FirstName = "John",
                            FamilyName = "Doe",
                            Email = "john.doe@example.com",
                            Role = "Teacher"
                        },
                        new Teacher
                        {
                            UserName = "JaneId",
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
