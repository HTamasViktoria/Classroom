using System.Net;
using System.Net.Http.Json;
using System.Text;
using Classroom.Contracts;
using Classroom.Data;
using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Classroom.Model.ResponseModels;
using Classroom.Service;
using Classroom.Service.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ClassromIntegrationTests;

[Collection("IntegrationTests")]

    public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;
        private readonly HttpClient _mockClient;
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly Mock<IUserService> _mockUserService;

        public AuthControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
    
            _mockAuthService = new Mock<IAuthService>();
            _mockUserService = new Mock<IUserService>();
    
            var mockFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(_mockAuthService.Object);
                    services.AddSingleton(_mockUserService.Object);
                });
            });

            _mockClient = mockFactory.CreateClient();

  

        }

        
        [Fact]
        public async Task RegisterStudent_ValidStudentRequest_ReturnsCreatedResponse()
        {
            var request = new StudentRequest
            {
                FirstName = "John",
                FamilyName = "Doe",
                Email = "john.doe@example.com",
                Username = "johnny123",
                Password = "Password123!",
                BirthDate = "2000-01-01",
                BirthPlace = "Budapest",
                StudentNo = "S123456"
            };

            var response = await _client.PostAsJsonAsync("/api/auth/signup/student", request);

          
            var responseBody = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Test failed with status code: {response.StatusCode} and response body: {responseBody}");
            }

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var registrationResponse = JsonConvert.DeserializeObject<RegistrationResponse>(responseBody);

            Assert.NotNull(registrationResponse);
            Assert.Equal("john.doe@example.com", registrationResponse.Email);
            Assert.Equal("johnny123", registrationResponse.UserName);

            await ClearDatabaseAsync();
        }

        
        
        [Fact]
        public async Task RegisterStudent_InvalidStudentRequest_MissingStudentNo_ReturnsBadRequest()
        {
            var request = new StudentRequest
            {
                FirstName = "John",
                FamilyName = "Doe",
                Email = "john.doe@example.com",
                Username = "johnny123",
                Password = "Password123!",
                BirthDate = "2000-01-01",
                BirthPlace = "Budapest",
                StudentNo = null
            };

            var response = await _client.PostAsJsonAsync("/api/auth/signup/student", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            
            await ClearDatabaseAsync();
        }

        
        
        
        [Fact]
        public async Task RegisterStudent_InternalServerError_ReturnsStatusCode500()
        {
            var request = new StudentRequest
            {
                FirstName = "John",
                FamilyName = "Doe",
                Email = "john.doe@example.com",
                Username = "johnny123",
                Password = "Password123!",
                BirthDate = "2000-01-01",
                BirthPlace = "Budapest",
                StudentNo = "S12345"
            };

            var mockAuthenticationService = new Mock<IAuthService>();
            mockAuthenticationService
                .Setup(service => service.RegisterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>()))
                .ThrowsAsync(new Exception("Internal server error"));

            var mockFactory = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(mockAuthenticationService.Object);
                });
            });

            var mockClient = mockFactory.CreateClient();

            var response = await mockClient.PostAsJsonAsync("/api/auth/signup/student", request);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            
            await ClearDatabaseAsync();
        }

        
        
       
        
        
        
        
        [Fact]
        public async Task RegisterTeacher_BadRequest_WhenMissingRequiredField()
        {
            var request = new TeacherRequest
            {
                FirstName = "John",
                FamilyName = "Doe",
                Password = "Password123",
                Username = "johndoe"
            };

            var requestBody = JsonConvert.SerializeObject(request);
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/auth/signup/teacher", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseString = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<JObject>(responseString);

            Assert.True(errorResponse["errors"]["Email"].ToString().Contains("The Email field is required."));
            
            await ClearDatabaseAsync();
        }

        
        
        
        [Fact]
        public async Task RegisterTeacher_InternalServerError_WhenExceptionOccurs()
        {
            var request = new TeacherRequest
            {
                FirstName = "John",
                FamilyName = "Doe",
                Email = "john.doe@example.com",
                Username = "johndoe",
                Password = "Password123"
            };

            var requestBody = JsonConvert.SerializeObject(request);
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.RegisterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Something went wrong"));

            var mockFactory = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(mockAuthService.Object);
                });
            });

            var client = mockFactory.CreateClient();

            var response = await client.PostAsync("/api/auth/signup/teacher", content);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Internal server error", responseString);
            
            await ClearDatabaseAsync();
        }

       
        
        [Fact]
        public async Task ParentRegistration_HappyPath()
        {
            await ClearDatabaseAsync();
            await SeedStudentsAsync();

            var response = await _client.GetAsync("/api/students");
            response.EnsureSuccessStatusCode();
    
            var studentsJson = await response.Content.ReadAsStringAsync();
            var students = JsonConvert.DeserializeObject<List<Student>>(studentsJson);

            var firstStudent = students.First();

            var parentRequest = new ParentRequest
            {
                FirstName = "Ottó",
                FamilyName = "Tóth",
                Email = "otto@gmail.com",
                Username = "tothotto",
                Password = "TestPassword123!",
                ChildName = firstStudent.FamilyName + " " + firstStudent.FirstName,
                StudentId = firstStudent.Id
            };

            var responsePost = await _client.PostAsJsonAsync("/api/auth/signup/parent", parentRequest);

            responsePost.EnsureSuccessStatusCode();
            
            var responseBody = await responsePost.Content.ReadAsStringAsync();
            
            Assert.Contains(parentRequest.Email, responseBody);
            Assert.Contains(parentRequest.Username, responseBody);
            
            await ClearDatabaseAsync();
        }



        [Fact]
        public async Task ParentRegistration_ReturnsBadRequest_WhenStudentDoesNotExist()
        {
            var parentRequest = new ParentRequest
            {
                FirstName = "Ottó",
                FamilyName = "Tóth",
                Email = "otto@gmail.com",
                Username = "tothotto",
                Password = "TestPassword123!",
                ChildName = "NemLétezőGyerek",
                StudentId = "12345"
            };

    
            var response = await _client.PostAsJsonAsync("/api/auth/signup/parent", parentRequest);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            
            var responseBody = await response.Content.ReadAsStringAsync();
            
            Assert.Contains("ValidationError", responseBody);
            
            await ClearDatabaseAsync();
        }



        [Fact]
        public async Task ParentRegistration_ReturnsBadRequest_WithMissingParentProperty()
        {
            await ClearDatabaseAsync();
            await SeedStudentsAsync();

            var response1 = await _client.GetAsync("/api/students");
            response1.EnsureSuccessStatusCode();
    
            var studentsJson = await response1.Content.ReadAsStringAsync();
            var students = JsonConvert.DeserializeObject<List<Student>>(studentsJson);

            var firstStudent = students.First();
            
            var parentRequest = new ParentRequest
            {
                FirstName = "Ottó",
                FamilyName = "Tóth",
                Username = "tothotto",
                Password = "TestPassword123!",
                ChildName = firstStudent.FirstName + " " + firstStudent.FamilyName,
                StudentId = firstStudent.Id
            };
            
            var response = await _mockClient.PostAsJsonAsync("/api/auth/signup/parent", parentRequest);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();

            Assert.Contains("The Email field is required.", responseBody);
            
            await ClearDatabaseAsync();
        }

        
        
        [Fact]
        public async Task ParentRegistration_ReturnsInternalServerError_WhenAuthServiceThrowsException()
        {
            await ClearDatabaseAsync();
            await SeedStudentsAsync();

            var response1 = await _client.GetAsync("/api/students");
            response1.EnsureSuccessStatusCode();
    
            var studentsJson = await response1.Content.ReadAsStringAsync();
            var students = JsonConvert.DeserializeObject<List<Student>>(studentsJson);

            var firstStudent = students.First();
            var parentRequest = new ParentRequest
            {
                FirstName = "Ottó",
                FamilyName = "Tóth",
                Email = "otto@gmail.com",
                Username = "tothotto",
                Password = "TestPassword123!",
                ChildName = firstStudent.FirstName + " " + firstStudent.FamilyName,
                StudentId = firstStudent.Id
            };
            
            _mockAuthService
                .Setup(service => service.RegisterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Test exception"));
            
            var response = await _mockClient.PostAsJsonAsync("/api/auth/signup/parent", parentRequest);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            
            Assert.Contains("Internal server error: Test exception", responseBody);
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
        
        [Fact]
        public async Task Authenticate_ValidCredentials_ReturnsSuccessResponseWithToken()
        {
            await ClearDatabaseAsync();

            var teacherRequest = new TeacherRequest()
            {
                FirstName = "Vilmos",
                FamilyName = "Kovács",
                Email = "vilmos@gmail.com",
                Username = "kovacsvilmos",
                Password = "Vilmos123"
            };

            var responsePost = await _client.PostAsJsonAsync("/api/auth/signup/teacher", teacherRequest);

            var authRequest = new AuthRequest
            {
                Email = "vilmos@gmail.com",
                Password = "Vilmos123"
            };

            var response = await _client.PostAsJsonAsync("/api/auth/signin", authRequest);

            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<JObject>(responseBody);

            Assert.Equal("Login successful", responseObject["message"]);
            Assert.NotNull(responseObject["token"]);
            Assert.Equal("kovacsvilmos", responseObject["user"]["userName"]);
            
            await ClearDatabaseAsync();
        }

        
        
        [Fact]
        public async Task Authenticate_InvalidCredentials_ReturnsUnauthorizedResponse()
        {
            await ClearDatabaseAsync();

            var teacherRequest = new TeacherRequest()
            {
                FirstName = "Vilmos",
                FamilyName = "Kovács",
                Email = "vilmos@gmail.com",
                Username = "kovacsvilmos",
                Password = "Vilmos123"
            };

            var responsePost = await _client.PostAsJsonAsync("/api/auth/signup/teacher", teacherRequest);

            var authRequest = new AuthRequest
            {
                Email = "vilmos@gmail.com",
                Password = "WrongPassword123"
            };

            var response = await _client.PostAsJsonAsync("/api/auth/signin", authRequest);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            
            await ClearDatabaseAsync();
        }

        
        
        
        
        [Fact]
        public async Task Authenticate_UserNotFound_ReturnsUnauthorizedResponse()
        {
            var authRequest = new AuthRequest
            {
                Email = "nonexistent@gmail.com",
                Password = "SomePassword123"
            };

            var response = await _client.PostAsJsonAsync("/api/auth/signin", authRequest);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            
            await ClearDatabaseAsync();
        }

        
        [Fact]
        public async Task Logout_ValidRequest_DeletesCookieAndReturnsNoContent()
        {
            var teacherRequest = new TeacherRequest()
            {
                FirstName = "Vilmos",
                FamilyName = "Kovács",
                Email = "vilmos@gmail.com",
                Username = "kovacsvilmos",
                Password = "Vilmos123"
            };

            await _client.PostAsJsonAsync("/api/auth/signup/teacher", teacherRequest);
            
            var authRequest = new AuthRequest
            {
                Email = "vilmos@gmail.com",
                Password = "Vilmos123"
            };

            var responseLogin = await _client.PostAsJsonAsync("/api/auth/signin", authRequest);
            
            responseLogin.EnsureSuccessStatusCode();
            var responseBody = await responseLogin.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<JObject>(responseBody);
            Assert.NotNull(responseObject["token"]);

    
            var responseLogout = await _client.PostAsync("/api/auth/signout", null);
            responseLogout.EnsureSuccessStatusCode();
            Assert.Equal(System.Net.HttpStatusCode.NoContent, responseLogout.StatusCode);

           
            var cookies = responseLogout.Headers.GetValues("Set-Cookie");
            Assert.DoesNotContain("access_token", cookies);
            
            await ClearDatabaseAsync();
        }

        
       
        
        private async Task SeedTeacherAsync()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();

                var teacher = new Teacher
                {
                    FirstName = "János",
                    FamilyName = "Kovács",
                    UserName = "kovacsjanos",
                    Email = "janos.kovacs@gmail.com",
                    Role = "Teacher" 
                };

                teacher.PasswordHash = new PasswordHasher<Teacher>().HashPassword(teacher, "Password123!");

                await context.Teachers.AddAsync(teacher);
                await context.SaveChangesAsync();
            }
        }


        
        
        private async Task SeedStudentsAsync()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();

                var student1 = new Student
                {
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
    }
