using System.Net;
using System.Net.Http.Json;
using Classroom.Service.Repositories;
using Classroom.Data;
using Classroom.Model.DataModels;
using ClassromIntegrationTests.MockRepos;
using Classroom.Model.RequestModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace ClassromIntegrationTests;

public class TeacherSubjectControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly HttpClient _mockClient;

    public TeacherSubjectControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();

        var mockFactory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddTransient<ITeacherSubjectRepository, MockTeacherSubjectRepository>();
            });
        });

        _mockClient = mockFactory.CreateClient();
    }


    [Fact]
    public async Task GetAll_ReturnsOkResult_WithTeacherSubjects()
    {
        await ClearDatabaseAsync();
        await AddStudentsAndClassesAsync();
        await AddTeacher();
        await AddTeacherSubject();

        var response = await _client.GetAsync("/api/teachersubjects");

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var teacherSubjects = JsonConvert.DeserializeObject<List<TeacherSubject>>(content);

        Assert.NotNull(teacherSubjects);
        Assert.Equal(2, teacherSubjects.Count);
        var validSubjects = new HashSet<string> { "Matematika", "Irodalom" };

        foreach (var teacherSubject in teacherSubjects)
        {
            Assert.Contains(teacherSubject.Subject, validSubjects);
        }
    }
    
    
    [Fact]
    public async Task GetSubjectsByTeacherId_ReturnsOkResult_WithTeacherSubjects()
    {
        await ClearDatabaseAsync();
        await AddStudentsAndClassesAsync();
        await AddTeacher();
        await AddTeacherSubject();

        var responseGetAll = await _client.GetAsync("/api/teachersubjects");
        responseGetAll.EnsureSuccessStatusCode();
    
        var contentGetAll = await responseGetAll.Content.ReadAsStringAsync();
        var teacherSubjects = JsonConvert.DeserializeObject<List<TeacherSubject>>(contentGetAll);

        var teachChosen = teacherSubjects.First();
        var teacherId = teachChosen.TeacherId;

        Assert.NotNull(teacherId);

        var responseByTeacherId = await _client.GetAsync($"/api/teachersubjects/byteacher/{teacherId}");
        responseByTeacherId.EnsureSuccessStatusCode();
    
        var contentByTeacherId = await responseByTeacherId.Content.ReadAsStringAsync();
        var teacherSubjectsByTeacherId = JsonConvert.DeserializeObject<List<TeacherSubject>>(contentByTeacherId);

        Assert.NotNull(teacherSubjectsByTeacherId);
  
        Assert.True(teacherSubjectsByTeacherId.All(ts => ts.TeacherId == teacherId));

        var expectedSubject = teachChosen.Teacher.UserName == "janesmith" ? "Matematika" : "Irodalom";
    
        Assert.True(teacherSubjectsByTeacherId.All(ts => ts.Subject == expectedSubject));

        Assert.True(teacherSubjectsByTeacherId.Count > 0);
    }

    
    
    [Fact]
    public async Task GetSubjectsByTeacherId_ReturnsEmptyList_WhenNoSubjectsExist()
    {
        
        await ClearDatabaseAsync();
        await AddStudentsAndClassesAsync();
        await AddTeacher();
        await AddTeacherSubject();

        var responseGetAll = await _client.GetAsync("/api/teachersubjects");
        responseGetAll.EnsureSuccessStatusCode();
    
        var contentGetAll = await responseGetAll.Content.ReadAsStringAsync();
        var teacherSubjects = JsonConvert.DeserializeObject<List<TeacherSubject>>(contentGetAll);
        
        var teachChosen = teacherSubjects.FirstOrDefault();
        var teacherId = teachChosen?.TeacherId;

        Assert.NotNull(teacherId);

        await ClearTeacherSubjectsAsync();

        var responseByTeacherId = await _client.GetAsync($"/api/teachersubjects/byteacher/{teacherId}");
        responseByTeacherId.EnsureSuccessStatusCode();

        var contentByTeacherId = await responseByTeacherId.Content.ReadAsStringAsync();
        var teacherSubjectsByTeacherId = JsonConvert.DeserializeObject<List<TeacherSubject>>(contentByTeacherId);
        
        Assert.NotNull(teacherSubjectsByTeacherId);
        Assert.Empty(teacherSubjectsByTeacherId);
    }


    
    [Fact]
    public async Task GetSubjectsByTeacherId_ReturnsBadRequest_WhenTeacherIdIsInvalid()
    {
        await ClearDatabaseAsync();
        await AddStudentsAndClassesAsync();
        await AddTeacher();
        var invalidTeacherId = "ewrlacwrpeoc3ajoip4 aj";
        
        var responseByTeacherId = await _client.GetAsync($"/api/teachersubjects/byteacher/{invalidTeacherId}");
    
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, responseByTeacherId.StatusCode);

        var contentByTeacherId = await responseByTeacherId.Content.ReadAsStringAsync();
        Assert.Contains("Bad request", contentByTeacherId);
    }


    
    [Fact]
    public async Task GetSubjectsByTeacherId_ReturnsInternalServerError_WhenRepositoryThrowsException()
    {
        await ClearDatabaseAsync();
        await AddStudentsAndClassesAsync();
        await AddTeacher();
        await AddTeacherSubject();
        
        var responseGetAll = await _client.GetAsync("/api/teachersubjects");
        responseGetAll.EnsureSuccessStatusCode();
    
        var contentGetAll = await responseGetAll.Content.ReadAsStringAsync();
        var teacherSubjects = JsonConvert.DeserializeObject<List<TeacherSubject>>(contentGetAll);
        
        var teacherId = teacherSubjects.FirstOrDefault()?.TeacherId;
    
        Assert.NotNull(teacherId);

        var mockFactory = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddTransient<ITeacherSubjectRepository, MockTeacherSubjectRepository>();
            });
        });

        var mockClient = mockFactory.CreateClient();

        var responseByTeacherId = await mockClient.GetAsync($"/api/teachersubjects/byteacher/{teacherId}");
        Assert.Equal(HttpStatusCode.InternalServerError, responseByTeacherId.StatusCode);

        var content = await responseByTeacherId.Content.ReadAsStringAsync();
        Assert.Contains("Internal server error", content);
    }

    
    
   


    
    
    [Fact]
    public async Task Post_ReturnsCreatedResponse_WhenRequestIsValid()
    {
       
        await ClearDatabaseAsync();
        await AddStudentsAndClassesAsync();
        await AddTeacher();
        await AddTeacherSubject();
        
        var responseGetAll = await _client.GetAsync("/api/teachersubjects");
        responseGetAll.EnsureSuccessStatusCode();
    
        var contentGetAll = await responseGetAll.Content.ReadAsStringAsync();
        var teacherSubjects = JsonConvert.DeserializeObject<List<TeacherSubject>>(contentGetAll);
    
      
        var teacherSubject = teacherSubjects.First();
        var teacherId = teacherSubject.TeacherId;
        var classId = teacherSubject.ClassOfStudentsId;
        var className = teacherSubject.ClassName;

       
        var request = new TeacherSubjectRequest
        {
            Subject = "Matematika",
            TeacherId = teacherId,
            ClassOfStudentsId = classId,
            ClassName = className
        };

        var response = await _client.PostAsJsonAsync("/api/teachersubjects", request);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Successfully added new teacherSubject", content);
        
        Assert.True(response.Headers.Location.AbsoluteUri.Contains(teacherId));
    }

    
    [Fact]
    public async Task Post_ReturnsBadRequest_WhenTeacherSubjectAlreadyExists()
    {
        await ClearDatabaseAsync();
        await AddStudentsAndClassesAsync();
        await AddTeacher();
        await AddTeacherSubject();
        
        var responseGetAll = await _client.GetAsync("/api/teachersubjects");
        responseGetAll.EnsureSuccessStatusCode();
    
        var contentGetAll = await responseGetAll.Content.ReadAsStringAsync();
        var teacherSubjects = JsonConvert.DeserializeObject<List<TeacherSubject>>(contentGetAll);
    
        var teacherSubject = teacherSubjects.First();
        var teacherId = teacherSubject.TeacherId;
        var classId = teacherSubject.ClassOfStudentsId;

    
        var request = new TeacherSubjectRequest
        {
            Subject = teacherSubject.Subject,
            TeacherId = teacherId,
            ClassOfStudentsId = classId,
            ClassName = teacherSubject.ClassName
        };

        var response = await _client.PostAsJsonAsync("/api/teachersubjects", request);

 
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Bad request: Already existing teachersubject", content);
    }

    
    
    
    [Fact]
    public async Task Post_ReturnsInternalServerError_WhenRepositoryThrowsException()
    {
        await ClearDatabaseAsync();
        await AddStudentsAndClassesAsync();
        await AddTeacher();
        await AddTeacherSubject();
        
        var responseGetAll = await _client.GetAsync("/api/teachersubjects");
        responseGetAll.EnsureSuccessStatusCode();
    
        var contentGetAll = await responseGetAll.Content.ReadAsStringAsync();
        var teacherSubjects = JsonConvert.DeserializeObject<List<TeacherSubject>>(contentGetAll);
    
        var teacherId = teacherSubjects.FirstOrDefault()?.TeacherId;
        
        Assert.NotNull(teacherId);

        var mockFactory = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddTransient<ITeacherSubjectRepository, MockTeacherSubjectRepository>();
            });
        });

        var mockClient = mockFactory.CreateClient();
        
        var request = new TeacherSubjectRequest
        {
            Subject = "Matematika",
            TeacherId = teacherId,
            ClassOfStudentsId = 1,
            ClassName = "A osztály"
        };
        
        var response = await mockClient.PostAsJsonAsync("/api/teachersubjects", request);
        
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Internal server error", content);
    }

    
   

    
    [Fact]
    public async Task GetStudentsByTeacherSubjectId_ReturnsInternalServerError()
    {
    
        await ClearDatabaseAsync();
        await AddStudentsAndClassesAsync();
        await AddTeacher();
        await AddTeacherSubject();

        var responseGetAll = await _client.GetAsync("/api/teachersubjects");
        responseGetAll.EnsureSuccessStatusCode();

        var contentGetAll = await responseGetAll.Content.ReadAsStringAsync();
        var teacherSubjects = JsonConvert.DeserializeObject<List<TeacherSubject>>(contentGetAll);

        var teacherSubjectId = teacherSubjects.FirstOrDefault()?.Id;
        Assert.NotNull(teacherSubjectId);

        var mockFactory = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddTransient<ITeacherSubjectRepository, MockTeacherSubjectRepository>();
            });
        });

        var mockClient = mockFactory.CreateClient();

        var response = await mockClient.GetAsync($"/api/teachersubjects/studentsof/{teacherSubjectId}");

     
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Internal server error", content);
    }

    
    
    
    
    
    [Fact]
    public async Task GetStudentsByTeacherSubjectId_ReturnsOkResult_WithStudents()
    {
        await ClearDatabaseAsync();
        await AddStudentsAndClassesAsync();
        await AddTeacher();
        await AddTeacherSubject();
        
        var responseGetAll = await _client.GetAsync("/api/teachersubjects");
        responseGetAll.EnsureSuccessStatusCode();

        var contentGetAll = await responseGetAll.Content.ReadAsStringAsync();
        var teacherSubjects = JsonConvert.DeserializeObject<List<TeacherSubject>>(contentGetAll);

        var teacherSubjectId = teacherSubjects.FirstOrDefault()?.Id;
        Assert.NotNull(teacherSubjectId);

        var response = await _client.GetAsync($"/api/teachersubjects/studentsof/{teacherSubjectId}");
        response.EnsureSuccessStatusCode();
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      
        var content = await response.Content.ReadAsStringAsync();
        var students = JsonConvert.DeserializeObject<List<Student>>(content);

        Assert.NotEmpty(students);

        
        var studentIds = students.Select(s => s.Id).ToList();
        Assert.True((studentIds.Contains("123d1xd3") && studentIds.Contains("34fcq234")) ||
                    (studentIds.Contains("567d1x1") && studentIds.Contains("789d2x2")));
    }


    [Fact]
    public async Task GetStudentsByTeacherSubjectId_ReturnsOkResult_WithEmptyStudentList()
    {
        await ClearDatabaseAsync();
        await AddEmptyClassesAsync();
        await AddTeacher();
        await AddTeacherSubjectWithEmptyClassAsync();

        var responseGetAll = await _client.GetAsync("/api/teachersubjects");
        responseGetAll.EnsureSuccessStatusCode();

        var contentGetAll = await responseGetAll.Content.ReadAsStringAsync();
        var teacherSubjects = JsonConvert.DeserializeObject<List<TeacherSubject>>(contentGetAll);

        var teacherSubjectId = teacherSubjects.FirstOrDefault()?.Id;

        Assert.NotNull(teacherSubjectId);
        
        var response = await _client.GetAsync($"/api/teachersubjects/studentsof/{teacherSubjectId}");
        response.EnsureSuccessStatusCode();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var students = JsonConvert.DeserializeObject<List<Student>>(content);
        
        Assert.Empty(students);
    }

    

    private async Task AddEmptyClassesAsync()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();

            var class1 = new ClassOfStudents
            {
                Name = "Math 101",
                Grade = "10",
                Section = "A",
                Students = new List<Student>()
            };
            

            var classesOfStudents = new List<ClassOfStudents> { class1};
            foreach (var classOfStudent in classesOfStudents)
            {
                var existingClass = await context.ClassesOfStudents
                    .FirstOrDefaultAsync(c =>
                        c.Name == classOfStudent.Name && c.Grade == classOfStudent.Grade &&
                        c.Section == classOfStudent.Section);

                if (existingClass == null)
                {
                    context.ClassesOfStudents.Add(classOfStudent);
                }
                else
                {
                    context.Entry(existingClass).CurrentValues.SetValues(classOfStudent);
                }
            }

            await context.SaveChangesAsync();
        }
    }

    
    private async Task AddTeacherSubjectWithEmptyClassAsync()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();

            var teacher = await context.Teachers.FirstOrDefaultAsync(t => t.UserName == "johndoe");

            var classOfStudents = await context.ClassesOfStudents.FirstOrDefaultAsync(c => c.Name == "Math 101" && !c.Students.Any());

            if (teacher != null && classOfStudents != null)
            {
                var teacherSubject = new TeacherSubject
                {
                    Subject = "Irodalom",
                    TeacherId = teacher.Id,
                    Teacher = teacher,
                    ClassOfStudentsId = classOfStudents.Id,
                    ClassOfStudents = classOfStudents,
                    ClassName = classOfStudents.Name
                };

                context.TeacherSubjects.Add(teacherSubject);
            }

            await context.SaveChangesAsync();
        }
    }



    private async Task ClearTeacherSubjectsAsync()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();

            context.TeacherSubjects.RemoveRange(context.TeacherSubjects);

            await context.SaveChangesAsync();
        }
    }


  private async Task AddStudentsAndClassesAsync()
{
    using (var scope = _factory.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();

        var student1 = new Student
        {
            Id = "123d1xd3",
            UserName = "kovacsbela",
            FirstName = "Béla",
            FamilyName = "Kovács",
            BirthDate = new DateTime(2005, 5, 5),
            BirthPlace = "Budapest",
            StudentNo = "S12345"
        };

        var student2 = new Student
        {
            Id = "34fcq234",
            UserName = "hajdukalman",
            FirstName = "Kálmán",
            FamilyName = "Hajdu",
            BirthDate = new DateTime(2006, 6, 6),
            BirthPlace = "Debrecen",
            StudentNo = "S67890"
        };

        var student3 = new Student
        {
            Id = "567d1x1",
            UserName = "peterkarl",
            FirstName = "Károly",
            FamilyName = "Péter",
            BirthDate = new DateTime(2007, 7, 7),
            BirthPlace = "Pécs",
            StudentNo = "S11111",
        };

        var student4 = new Student
        {
            Id = "789d2x2",
            UserName = "janoslaszlo",
            FirstName = "László",
            FamilyName = "János",
            BirthDate = new DateTime(2008, 8, 8),
            BirthPlace = "Szeged",
            StudentNo = "S22222"
        };

        var students = new List<Student> { student1, student2, student3, student4 };
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

        var class1 = new ClassOfStudents
        {
         
            Name = "Math 101",
            Grade = "10",
            Section = "A",
            Students = new List<Student> { student1, student2 }
        };

        var class2 = new ClassOfStudents
        {
            
            Name = "Science 101",
            Grade = "10",
            Section = "B",
            Students = new List<Student> { student3, student4 }
        };

        var classesOfStudents = new List<ClassOfStudents> { class1, class2 };
        foreach (var classOfStudent in classesOfStudents)
        {
            var existingClass = await context.ClassesOfStudents
                .FirstOrDefaultAsync(c =>
                    c.Name == classOfStudent.Name && c.Grade == classOfStudent.Grade &&
                    c.Section == classOfStudent.Section);

            if (existingClass == null)
            {
                context.ClassesOfStudents.Add(classOfStudent);
            }
            else
            {
                context.Entry(existingClass).CurrentValues.SetValues(classOfStudent);
            }
        }

        await context.SaveChangesAsync();
    }
}




    private async Task AddTeacher()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();


            var teacher1 = new Teacher
            {
                Id = "Teacher1Id",
                FirstName = "John",
                FamilyName = "Doe",
                UserName = "johndoe",
                Email = "johndoe@example.com",
                Role = "Teacher"
            };

            var teacher2 = new Teacher
            {
                Id = "Teacher2Id",
                FirstName = "Jane",
                FamilyName = "Smith",
                UserName = "janesmith",
                Email = "janesmith@example.com",
                Role = "Teacher"
            };


            context.Teachers.Add(teacher1);
            context.Teachers.Add(teacher2);

            await context.SaveChangesAsync();
        }
    }


    private async Task AddTeacherSubject()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();

            var teacher1 = await context.Teachers.FirstOrDefaultAsync(t => t.UserName == "johndoe");
            var teacher2 = await context.Teachers.FirstOrDefaultAsync(t => t.UserName == "janesmith");

            var class1 = await context.ClassesOfStudents.FirstOrDefaultAsync(c => c.Name == "Math 101");
            var class2 = await context.ClassesOfStudents.FirstOrDefaultAsync(c => c.Name == "Science 101");

            if (teacher1 != null && class1 != null)
            {
                var teacherSubject1 = new TeacherSubject
                {
                    Subject = "Irodalom",
                    TeacherId = teacher1.Id,
                    Teacher = teacher1,
                    ClassOfStudentsId = class1.Id,
                    ClassOfStudents = class1,
                    ClassName = class1.Name
                };

                context.TeacherSubjects.Add(teacherSubject1);
            }

            if (teacher2 != null && class2 != null)
            {
                var teacherSubject2 = new TeacherSubject
                {
                    Subject = "Matematika",
                    TeacherId = teacher2.Id,
                    Teacher = teacher2,
                    ClassOfStudentsId = class2.Id,
                    ClassOfStudents = class2,
                    ClassName = class2.Name
                };

                context.TeacherSubjects.Add(teacherSubject2);
            }

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
