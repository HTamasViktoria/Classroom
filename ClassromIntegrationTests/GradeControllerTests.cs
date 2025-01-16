using System.Net;
using System.Net.Http.Json;
using Classroom.Data;
using Classroom.Model.DataModels;
using Classroom.Service.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ClassromIntegrationTests.MockRepos;
using Classroom.Model.DataModels.Enums;
using Classroom.Model.RequestModels;
using Classroom.Model.ResponseModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClassromIntegrationTests;

[Collection("IntegrationTests")]
public class GradeControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly HttpClient _mockClient;
    

    public GradeControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();

        var mockFactory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddTransient<IGradeRepository, MockGradeRepository>();
            });
        });

        _mockClient = mockFactory.CreateClient();
     
    }
    

    
    
    
    [Fact]
    public async Task GetAll_ReturnsEmptyList_WhenNoGradesExist()
    {
  
        await ClearDatabaseAsync();
        
        var response = await _client.GetAsync("/api/grades");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var grades = JsonConvert.DeserializeObject<List<Grade>>(content);
        
        Assert.NotNull(grades);
        Assert.Empty(grades);

        await ClearDatabaseAsync();
    }

    
    [Fact]
    public async Task GetAll_ReturnsGrades_WhenGradesExist()
    {
        await ClearDatabaseAsync();
        await AddStudentsAndClassesAndGradesAsync();

        var response = await _client.GetAsync("/api/grades");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var grades = JsonConvert.DeserializeObject<List<Grade>>(content);
        
        Assert.NotNull(grades);
        Assert.NotEmpty(grades);
        
        var gradeValues = grades.Select(g => g.Value).ToList();
        Assert.Contains(GradeValues.Elégtelen, gradeValues);
        Assert.Contains(GradeValues.Elégséges, gradeValues);
        Assert.Contains(GradeValues.Jeles, gradeValues);
        Assert.Contains(GradeValues.Jó, gradeValues);
        await ClearDatabaseAsync();
    }

    
    
    [Fact]
    public async Task GetAll_ReturnsInternalServerError_WhenExceptionIsThrown()
    {

        await ClearDatabaseAsync();
        await AddStudentsAndClassesAndGradesAsync();
        var response = await _mockClient.GetAsync("/api/grades");

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Internal server error", content);
        await ClearDatabaseAsync();
    }

    
    [Fact]
    public async Task GetAllValues_ReturnsGradeValues_WhenCalled()
    {
        await ClearDatabaseAsync();
        await AddStudentsAndClassesAndGradesAsync();
        var response = await _client.GetAsync("/api/grades/gradevalues");

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var gradeValues = JsonConvert.DeserializeObject<List<string>>(content);
        
        Assert.NotNull(gradeValues);
        Assert.NotEmpty(gradeValues);
        
        var expectedValues = new List<string>
        {
            "Jeles = 5",
            "Jó = 4",
            "Közepes = 3",
            "Elégséges = 2",
            "Elégtelen = 1"
        };

        Assert.Equal(expectedValues.Count, gradeValues.Count);
        foreach (var expectedValue in expectedValues)
        {
            Assert.Contains(expectedValue, gradeValues);
        }
        
        await ClearDatabaseAsync();
    }

    
    
    
    [Fact]
public async Task Post_GradeRequest_ThrowsException_ReturnsInternalServerError()
{
    await ClearDatabaseAsync();
    
   
    var student = new Student
    {
        Id = "student1",
        UserName = "student1",
        FirstName = "Béla",
        FamilyName = "Kovács",
        BirthDate = new DateTime(2005, 5, 5),
        BirthPlace = "Budapest",
        StudentNo = "S12345",
        Role = "Student"
       
    };

    var teacher = new Teacher
    {
        Id = "teacher1",
        UserName = "teacher1",
        FirstName = "John",
        FamilyName = "Doe",
        Email = "teacher1@example.com",
        Role = "Teacher"
    };

    var subject = "Matematika";

    var gradeRequest = new GradeRequest
    {
        TeacherId = teacher.Id,
        StudentId = student.Id,
        Subject = subject,
        ForWhat = "Témazáró",
        Read = true,
        Value = "Jó",
        Date = "2024-12-17"
    };
    
    using (var scope = _factory.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();
        await context.Students.AddAsync(student);
        await context.Teachers.AddAsync(teacher);
        await context.SaveChangesAsync();
    }


    var response = await _mockClient.PostAsJsonAsync("/api/grades", gradeRequest);
    
    Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

    var responseBody = await response.Content.ReadAsStringAsync();
    Assert.Contains("Internal server error", responseBody);
    await ClearDatabaseAsync();
}

  
    
    [Fact]
public async Task Post_ReturnsCreatedResponse_WhenRequestIsValid()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassesAndGradesAsync();
    using (var scope = _factory.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();
        
        var student = new Student
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "kovacsbela",
            FirstName = "Béla",
            FamilyName = "Kovács",
            BirthDate = new DateTime(2005, 5, 5),
            BirthPlace = "Budapest",
            StudentNo = "S12345",
            Role = "Student"
          
        };

        context.Students.Add(student);
        await context.SaveChangesAsync();
        
        var classOfStudent = new ClassOfStudents
        {
            Name = "Math 101",
            Grade = "10",
            Section = "A",
            Students = new List<Student> { student }
        };

        context.ClassesOfStudents.Add(classOfStudent);
        await context.SaveChangesAsync();
        
        var teacher = new Teacher
        {
            FirstName = "John",
            FamilyName = "Doe",
            UserName = "johndoe",
            Email = "johndoe@example.com",
            Role = "Teacher"
        };

        context.Teachers.Add(teacher);
        await context.SaveChangesAsync();
        
        var teacherSubject = new TeacherSubject
        {
            Subject = "Matematika",
            TeacherId = teacher.Id,
            Teacher = teacher,
            ClassOfStudentsId = classOfStudent.Id,
            ClassOfStudents = classOfStudent,
            ClassName = classOfStudent.Name
        };

        context.TeacherSubjects.Add(teacherSubject);
        await context.SaveChangesAsync();
        
        var gradeRequest = new GradeRequest
        {
            TeacherId = teacher.Id,
            StudentId = student.Id,
            Subject = "Matematika",
            ForWhat = "Témazáró",
            Read = true,
            Value = "Jeles",
            Date = DateTime.Now.ToString("yyyy-MM-dd")
        };


        var response = await _client.PostAsJsonAsync("/api/grades", gradeRequest);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Értesítés sikeresen elmentve az adatbázisba", content);
    }
    
    await ClearDatabaseAsync();
}



[Fact]
public async Task Post_GradeRequest_InvalidGradeValue_ReturnsBadRequest()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassesAndGradesAsync();
    using (var scope = _factory.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();
        
        var student = new Student
        {
            
            UserName = "student1",
            FirstName = "Béla",
            FamilyName = "Kovács",
            BirthDate = new DateTime(2005, 5, 5),
            BirthPlace = "Budapest",
            StudentNo = "S12345",
            Role = "Student"
          
        };

        var teacher = new Teacher
        {
          
            UserName = "teacher1",
            FirstName = "John",
            FamilyName = "Doe",
            Email = "teacher1@example.com",
            Role = "Teacher"
        };

        var subject = "Matematika";
        
        await context.Students.AddAsync(student);
        await context.Teachers.AddAsync(teacher);
        await context.SaveChangesAsync();
        
        var gradeRequest = new GradeRequest
        {
            TeacherId = teacher.Id,
            StudentId = student.Id,
            Subject = subject,
            ForWhat = "Témazáró",
            Read = true,
            Value = "Elégségessss",
            Date = "2024-12-17"
        };
        
        var response = await _client.PostAsJsonAsync("/api/grades", gradeRequest);
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("Invalid grade value", responseBody);
    }
    
    await ClearDatabaseAsync();
}


[Fact]
public async Task Post_ReturnsInternalServerError_WhenExceptionOccurs()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassesAndGradesAsync();
    using (var scope = _factory.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();

        var student = new Student
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "kovacsbela",
            FirstName = "Béla",
            FamilyName = "Kovács",
            BirthDate = new DateTime(2005, 5, 5),
            BirthPlace = "Budapest",
            StudentNo = "S12345",
            Role = "Student"
          
        };

        context.Students.Add(student);
        await context.SaveChangesAsync();

        var classOfStudent = new ClassOfStudents
        {
            Name = "Math 101",
            Grade = "10",
            Section = "A",
            Students = new List<Student> { student }
        };

        context.ClassesOfStudents.Add(classOfStudent);
        await context.SaveChangesAsync();

        var teacher = new Teacher
        {
            FirstName = "John",
            FamilyName = "Doe",
            UserName = "johndoe",
            Email = "johndoe@example.com",
            Role = "Teacher"
        };

        context.Teachers.Add(teacher);
        await context.SaveChangesAsync();

        var teacherSubject = new TeacherSubject
        {
            Subject = "Matematika",
            TeacherId = teacher.Id,
            Teacher = teacher,
            ClassOfStudentsId = classOfStudent.Id,
            ClassOfStudents = classOfStudent,
            ClassName = classOfStudent.Name
        };

        context.TeacherSubjects.Add(teacherSubject);
        await context.SaveChangesAsync();

        var gradeRequest = new GradeRequest
        {
            TeacherId = teacher.Id,
            StudentId = student.Id,
            Subject = "Matematika",
            ForWhat = "Témazáró",
            Read = true,
            Value = "Jeles",
            Date = DateTime.Now.ToString("yyyy-MM-dd")
        };
        
        var response = await _mockClient.PostAsJsonAsync("/api/grades", gradeRequest);

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Internal server error", content);
    }
    await ClearDatabaseAsync();
}


[Fact]
public async Task Put_UpdatesGrade_WhenValidRequestIsSent()
{
   await ClearDatabaseAsync();
    await AddStudentsAndClassesAndGradesAsync();
    
    var response = await _client.GetAsync("/api/grades");
    response.EnsureSuccessStatusCode();
    var content = await response.Content.ReadAsStringAsync();
    var grades = JsonConvert.DeserializeObject<List<Grade>>(content);
    
    var gradeToUpdate = grades.First();

  
    var gradeRequest = new GradeRequest
    {
        TeacherId = gradeToUpdate.TeacherId,
        StudentId = gradeToUpdate.StudentId,
        Subject = gradeToUpdate.Subject,
        ForWhat = gradeToUpdate.ForWhat,
        Read = true,
        Value = "Jó",
        Date = DateTime.Now.ToString("yyyy-MM-dd")
    };
    
    var putResponse = await _client.PutAsJsonAsync($"/api/grades/{gradeToUpdate.Id}", gradeRequest);
    putResponse.EnsureSuccessStatusCode();

   
    var putContent = await putResponse.Content.ReadAsStringAsync();
    Assert.Contains("Successfully updated grade", putContent);
    
    var getResponse = await _client.GetAsync("/api/grades");
    getResponse.EnsureSuccessStatusCode();
    var updatedContent = await getResponse.Content.ReadAsStringAsync();
    var updatedGrades = JsonConvert.DeserializeObject<List<Grade>>(updatedContent);
    
    var updatedGrade = updatedGrades.FirstOrDefault(g => g.Id == gradeToUpdate.Id);
    Assert.NotNull(updatedGrade);

    Assert.Equal("Jó", updatedGrade?.Value.ToString());
    await ClearDatabaseAsync();
}

[Fact]
public async Task Put_ReturnsBadRequest_WhenInvalidDateIsSent()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassesAndGradesAsync();
    
    var response = await _client.GetAsync("/api/grades");
    response.EnsureSuccessStatusCode();
    var content = await response.Content.ReadAsStringAsync();
    var grades = JsonConvert.DeserializeObject<List<Grade>>(content);
    
    var gradeToUpdate = grades.First();

    var gradeRequest = new GradeRequest
    {
        TeacherId = gradeToUpdate.TeacherId,
        StudentId = gradeToUpdate.StudentId,
        Subject = gradeToUpdate.Subject,
        ForWhat = gradeToUpdate.ForWhat,
        Read = true,
        Value = "Jó",
        Date = "InvalidDate" 
    };
    
    var putResponse = await _client.PutAsJsonAsync($"/api/grades/{gradeToUpdate.Id}", gradeRequest);
   
    Assert.Equal(400, (int)putResponse.StatusCode);
    
    var putContent = await putResponse.Content.ReadAsStringAsync();
    Assert.Contains("Bad request", putContent);
    await ClearDatabaseAsync();
}



[Fact]
public async Task Put_ReturnsBadRequest_WhenInvalidGradeValueIsSent()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassesAndGradesAsync();
    
    var response = await _client.GetAsync("/api/grades");
    response.EnsureSuccessStatusCode();
    var content = await response.Content.ReadAsStringAsync();
    var grades = JsonConvert.DeserializeObject<List<Grade>>(content);
    
    var gradeToUpdate = grades.First();
    
    var gradeRequest = new GradeRequest
    {
        TeacherId = gradeToUpdate.TeacherId,
        StudentId = gradeToUpdate.StudentId,
        Subject = gradeToUpdate.Subject,
        ForWhat = gradeToUpdate.ForWhat,
        Read = true,
        Value = "",
        Date = DateTime.Now.ToString("yyyy-MM-dd")
    };
    
    var putResponse = await _client.PutAsJsonAsync($"/api/grades/{gradeToUpdate.Id}", gradeRequest);
    
    Assert.Equal(400, (int)putResponse.StatusCode);
   
    var putContent = await putResponse.Content.ReadAsStringAsync();
    Assert.Contains("Bad request", putContent);
    await ClearDatabaseAsync();
}



[Fact]
public async Task Put_ReturnsBadRequest_WhenInvalidIdIsProvided()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassesAndGradesAsync();
    var gradeRequest = new GradeRequest
    {
        TeacherId = "dfasfd",
        StudentId = "sdfasdf",
        Subject = "Matematika",
        ForWhat = "Zárthelyi",
        Value = "Jó",
        Date = DateTime.Now.ToString("yyyy-MM-dd")
    };

    int invalidId = 1234;

    var putResponse = await _client.PutAsJsonAsync($"/api/grades/{invalidId}", gradeRequest);

    Assert.Equal(404, (int)putResponse.StatusCode);

    var putContent = await putResponse.Content.ReadAsStringAsync();

    Assert.Contains("not found", putContent);
    await ClearDatabaseAsync();
}


[Fact]
public async Task Put_ReturnsInternalServerError_WhenExceptionOccurs()
{

    await ClearDatabaseAsync();
    await AddStudentsAndClassesAndGradesAsync();

    var response = await _client.GetAsync("/api/grades");
    response.EnsureSuccessStatusCode();
    var content = await response.Content.ReadAsStringAsync();
    var grades = JsonConvert.DeserializeObject<List<Grade>>(content);
    
    var gradeToUpdate = grades.First();
    
    var gradeRequest = new GradeRequest
    {
        TeacherId = gradeToUpdate.TeacherId,
        StudentId = gradeToUpdate.StudentId,
        Subject = gradeToUpdate.Subject,
        ForWhat = gradeToUpdate.ForWhat,
        Read = true,
        Value = "Jó",
        Date = DateTime.Now.ToString("yyyy-MM-dd")
    };
    
    var putResponse = await _mockClient.PutAsJsonAsync($"/api/grades/{gradeToUpdate.Id}", gradeRequest);

    Assert.Equal(500, (int)putResponse.StatusCode);

    var putContent = await putResponse.Content.ReadAsStringAsync();
    Assert.Contains("Internal server error", putContent);
    Assert.Contains("Mock exception for testing", putContent);
    await ClearDatabaseAsync();
}


[Fact]
public async Task Delete_ReturnsSuccess_WhenValidIdIsProvided()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassesAndGradesAsync();
    
    var response = await _client.GetAsync("/api/grades");
    response.EnsureSuccessStatusCode();
    var content = await response.Content.ReadAsStringAsync();
    var grades = JsonConvert.DeserializeObject<List<Grade>>(content);
    
    var gradeToDelete = grades.First();
    
    var deleteResponse = await _client.DeleteAsync($"/api/grades/{gradeToDelete.Id}");

    Assert.Equal(200, (int)deleteResponse.StatusCode);

    var deleteContent = await deleteResponse.Content.ReadAsStringAsync();
    Assert.Contains("Osztályzat sikeresen törölve.", deleteContent);
    
    var getResponse = await _client.GetAsync("/api/grades");
    getResponse.EnsureSuccessStatusCode();
    var updatedContent = await getResponse.Content.ReadAsStringAsync();
    var updatedGrades = JsonConvert.DeserializeObject<List<Grade>>(updatedContent);
    
    var deletedGrade = updatedGrades.FirstOrDefault(g => g.Id == gradeToDelete.Id);
    Assert.Null(deletedGrade);
    await ClearDatabaseAsync();
}

    

[Fact]
public async Task Delete_ReturnsNotFound_WhenInvalidIdIsProvided()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassesAndGradesAsync();

    var invalidId = 99999;

    var deleteResponse = await _client.DeleteAsync($"/api/grades/{invalidId}");
    
    Assert.Equal(404, (int)deleteResponse.StatusCode);
    
    var deleteContent = await deleteResponse.Content.ReadAsStringAsync();
    Assert.Contains($"Grade with Id {invalidId} not found.", deleteContent);
    await ClearDatabaseAsync();
}


[Fact]
public async Task Delete_ReturnsInternalServerError_WhenExceptionOccurs()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassesAndGradesAsync();

    var response = await _client.GetAsync("/api/grades");
    response.EnsureSuccessStatusCode();
    var content = await response.Content.ReadAsStringAsync();
    var grades = JsonConvert.DeserializeObject<List<Grade>>(content);
    
    var gradeToDelete = grades.First();
    
    var deleteResponse = await _mockClient.DeleteAsync($"/api/grades/{gradeToDelete.Id}");

    Assert.Equal(500, (int)deleteResponse.StatusCode);

    var deleteContent = await deleteResponse.Content.ReadAsStringAsync();
    Assert.Contains("Internal server error", deleteContent);
    await ClearDatabaseAsync();
}


[Fact]
public async Task GetGradesByStudentId_ReturnsGrades_WhenValidStudentIdIsProvided()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassesAndGradesAsync();
    
    var response = await _client.GetAsync("/api/grades");
    response.EnsureSuccessStatusCode();
    var content = await response.Content.ReadAsStringAsync();
    var grades = JsonConvert.DeserializeObject<List<Grade>>(content);

    var studentId = grades.First().StudentId;
    
    var gradesResponse = await _client.GetAsync($"/api/grades/{studentId}");

    gradesResponse.EnsureSuccessStatusCode();

    var gradesContent = await gradesResponse.Content.ReadAsStringAsync();
    var returnedGrades = JsonConvert.DeserializeObject<List<Grade>>(gradesContent);
    Assert.NotEmpty(returnedGrades);
    await ClearDatabaseAsync();
}



[Fact]
public async Task GetGradesByStudentId_ReturnsEmptyList_WhenNoGradesAreAdded()
{
 
    await ClearDatabaseAsync();
    await AddStudentsAndClassesAsync();
    
    var getStudentsResponse = await _client.GetAsync("/api/students");
    getStudentsResponse.EnsureSuccessStatusCode();
    var studentContent = await getStudentsResponse.Content.ReadAsStringAsync();
    var students = JsonConvert.DeserializeObject<List<Student>>(studentContent);
    
    var studentId = students.First().Id;
    
    var getGradesResponse = await _client.GetAsync($"/api/grades/{studentId}");
    getGradesResponse.EnsureSuccessStatusCode();
    var gradesContent = await getGradesResponse.Content.ReadAsStringAsync();
    var grades = JsonConvert.DeserializeObject<List<Grade>>(gradesContent);
    
    Assert.Empty(grades);
    await ClearDatabaseAsync();
}



[Fact]
public async Task GetGradesByStudentId_ReturnsBadRequest_WhenInvalidStudentIdIsProvided()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassesAsync();
    
    var invalidStudentId = "invalid_student_id";
    
    var getGradesResponse = await _client.GetAsync($"/api/grades/{invalidStudentId}");
    
    Assert.Equal(400, (int)getGradesResponse.StatusCode);
    
    var errorMessage = await getGradesResponse.Content.ReadAsStringAsync();
    Assert.Contains("Bad request", errorMessage);
    await ClearDatabaseAsync();
    
}



[Fact]
public async Task GetGradesByStudentId_ReturnsInternalServerError_WhenExceptionOccurs()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassesAndGradesAsync();
    
    var response = await _client.GetAsync("/api/grades");
    response.EnsureSuccessStatusCode();
    var content = await response.Content.ReadAsStringAsync();
    var grades = JsonConvert.DeserializeObject<List<Grade>>(content);
    
    var studentId = grades.First().StudentId;
    
    var gradesResponse = await _mockClient.GetAsync($"/api/grades/{studentId}");
    
    Assert.Equal(500, (int)gradesResponse.StatusCode);
    
    var errorMessage = await gradesResponse.Content.ReadAsStringAsync();
    Assert.Contains("Internal server error", errorMessage);
    await ClearDatabaseAsync();
}

[Fact]
public async Task ByClassBySubject_ShouldReturnCorrectClassAndSubject()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassWithSingleSubjectAsync();

    var response = await _client.GetAsync("api/classes");
    response.EnsureSuccessStatusCode();

    var classes = await response.Content.ReadFromJsonAsync<IEnumerable<ClassOfStudents>>();

    var classId = classes.First().Id;
    var subject = "Matematika";
    
    var gradeResponse = await _client.GetAsync($"api/grades/byclass/{classId}/bysubject/{subject}");

    gradeResponse.EnsureSuccessStatusCode();

    var grades = await gradeResponse.Content.ReadFromJsonAsync<IEnumerable<Grade>>();

    Assert.NotNull(grades);
    Assert.Equal(4, grades.Count());
    Assert.All(grades, grade => Assert.Equal(subject, grade.Subject));
    await ClearDatabaseAsync();
    
}

[Fact]
public async Task ByClassBySubject_ShouldReturnEmptyListWhenNoGradesAdded()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassWithSingleSubjectNoGradesAsync();

    var response = await _client.GetAsync("api/classes/");
    response.EnsureSuccessStatusCode();

    var classes = await response.Content.ReadFromJsonAsync<IEnumerable<ClassOfStudents>>();
    
    var classId = classes.First().Id;
    var subject = "Matematika";

    var gradeResponse = await _client.GetAsync($"api/grades/byclass/{classId}/bysubject/{subject}");
    gradeResponse.EnsureSuccessStatusCode();

    var grades = await gradeResponse.Content.ReadFromJsonAsync<IEnumerable<Grade>>();

    Assert.NotNull(grades);
    Assert.Empty(grades);
    await ClearDatabaseAsync();
}


[Fact]
public async Task ByClassBySubject_ShouldReturnBadRequestWhenInvalidClassId()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassWithSingleSubjectAsync();

    var invalidClassId = 999;
    var subject = "Matematika";

    var gradeResponse = await _client.GetAsync($"api/grades/byclass/{invalidClassId}/bysubject/{subject}");

    Assert.Equal(System.Net.HttpStatusCode.BadRequest, gradeResponse.StatusCode);

    var errorMessage = await gradeResponse.Content.ReadAsStringAsync();
    Assert.Contains("Bad request", errorMessage);
    await ClearDatabaseAsync();
}



[Fact]
public async Task ByClassBySubject_ShouldReturnBadRequestWhenInvalidSubject()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassWithSingleSubjectAsync();

    var classId = 1;
    var invalidSubject = "InvalidSubject";

    var gradeResponse = await _client.GetAsync($"api/grades/byclass/{classId}/bysubject/{invalidSubject}");

    Assert.Equal(System.Net.HttpStatusCode.BadRequest, gradeResponse.StatusCode);

    var errorMessage = await gradeResponse.Content.ReadAsStringAsync();
    Assert.Contains("Bad request", errorMessage);
    await ClearDatabaseAsync();
}

[Fact]
public async Task ByClassBySubject_ShouldReturnInternalServerErrorWhenRepositoryFails()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassWithSingleSubjectAsync();

    var classId = 1;
    var subject = "Matematika";

    var gradeResponse = await _mockClient.GetAsync($"api/grades/byclass/{classId}/bysubject/{subject}");

    Assert.Equal(System.Net.HttpStatusCode.InternalServerError, gradeResponse.StatusCode);

    var errorMessage = await gradeResponse.Content.ReadAsStringAsync();
    Assert.Contains("Internal server error", errorMessage);
    await ClearDatabaseAsync();
}



[Fact]
public async Task GetGradesByClass_ShouldReturnGradesForValidClassId()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassWithSingleSubjectAsync();

    
    var response = await _client.GetAsync("api/classes/");
    response.EnsureSuccessStatusCode();

    var classes = await response.Content.ReadFromJsonAsync<IEnumerable<ClassOfStudents>>();
    var classId = classes.First().Id;

    var subject = "Matematika";
    
    var gradeResponse = await _client.GetAsync($"api/grades/byclass/{classId}");
    gradeResponse.EnsureSuccessStatusCode();

    var grades = await gradeResponse.Content.ReadFromJsonAsync<IEnumerable<Grade>>();

    Assert.NotNull(grades);
    Assert.Equal(4, grades.Count());
    Assert.All(grades, grade => Assert.Equal(subject, grade.Subject));
    await ClearDatabaseAsync();
}


[Fact]
public async Task GetGradesByClass_ShouldReturnEmptyList_WhenNoGradesExist()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassWithSingleSubjectNoGradesAsync();

    var response = await _client.GetAsync("api/classes/");
    response.EnsureSuccessStatusCode();

    var classes = await response.Content.ReadFromJsonAsync<IEnumerable<ClassOfStudents>>();
    var classId = classes.First().Id;

    var gradeResponse = await _client.GetAsync($"api/grades/byclass/{classId}");
    gradeResponse.EnsureSuccessStatusCode();

    var grades = await gradeResponse.Content.ReadFromJsonAsync<IEnumerable<Grade>>();

    Assert.NotNull(grades);
    Assert.Empty(grades);
    await ClearDatabaseAsync();
}


[Fact]
public async Task GetGradesByClass_ShouldReturnBadRequest_WhenClassIdIsInvalid()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassWithSingleSubjectAsync();

    var invalidClassId = -1;

    var response = await _client.GetAsync($"api/grades/byclass/{invalidClassId}");

  
    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    
    var content = await response.Content.ReadAsStringAsync();
    Assert.Contains("Bad request", content);

    await ClearDatabaseAsync();
}


[Fact]
public async Task GetGradesByClass_ShouldReturnInternalServerError_WhenExceptionOccurs()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassWithSingleSubjectAsync();

    var classId = (await _client.GetAsync("api/classes/")).Content.ReadFromJsonAsync<IEnumerable<ClassOfStudents>>()
        .Result.First().Id;

    var mockFactory = _factory.WithWebHostBuilder(builder =>
    {
        builder.ConfigureServices(services =>
        {
            services.AddTransient<IGradeRepository, MockGradeRepository>();
        });
    });

    var mockClient = mockFactory.CreateClient();

    var response = await mockClient.GetAsync($"api/grades/byclass/{classId}");
    
    Assert.Equal(500, (int)response.StatusCode);
    await ClearDatabaseAsync();
}


[Fact]
public async Task GetGradesBySubjectByStudent_ShouldReturnGradesForStudent()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassWithSingleSubjectAsync();

    var response = await _client.GetAsync("api/students");
    response.EnsureSuccessStatusCode();

    var students = await response.Content.ReadFromJsonAsync<IEnumerable<Student>>();
    var studentId = students.First().Id.ToString();
    var subject = "Matematika"; 

    var gradeResponse = await _client.GetAsync($"api/grades/bysubject/{subject}/bystudent/{studentId}");
    gradeResponse.EnsureSuccessStatusCode();

    var grades = await gradeResponse.Content.ReadFromJsonAsync<IEnumerable<Grade>>();

    Assert.NotNull(grades);
    Assert.Equal(1, grades.Count());
    Assert.Equal(subject, grades.First().Subject);
    await ClearDatabaseAsync();
}


[Fact]
public async Task GetGradesBySubjectByStudent_ShouldReturnEmptyList_WhenNoGradesExist()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassWithSingleSubjectNoGradesAsync();

    var response = await _client.GetAsync("api/students/");
    response.EnsureSuccessStatusCode();

    var students = await response.Content.ReadFromJsonAsync<IEnumerable<Student>>();
    var studentId = students.First().Id.ToString();
    var subject = "Matematika"; 

    var gradeResponse = await _client.GetAsync($"api/grades/bysubject/{subject}/bystudent/{studentId}");
    gradeResponse.EnsureSuccessStatusCode();

    var grades = await gradeResponse.Content.ReadFromJsonAsync<IEnumerable<Grade>>();

    Assert.NotNull(grades);
    Assert.Empty(grades);
    await ClearDatabaseAsync();
}



[Fact]
public async Task GetGradesBySubjectByStudent_ShouldReturnBadRequest_WhenInvalidStudentId()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassWithSingleSubjectAsync();

    var subject = "Matematika";
    var invalidStudentId = "invalid_student_id";

    var gradeResponse = await _client.GetAsync($"api/grades/bysubject/{subject}/bystudent/{invalidStudentId}");
    
    Assert.Equal(400, (int)gradeResponse.StatusCode);
    var errorMessage = await gradeResponse.Content.ReadAsStringAsync();
    Assert.Contains("Bad request", errorMessage);
    await ClearDatabaseAsync();
}



[Fact]
public async Task GetGradesBySubjectByStudent_ShouldReturnBadRequest_WhenInvalidSubject()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassWithSingleSubjectAsync();

    var validStudentId = "studentId1";
    var invalidSubject = "InvalidSubject";

    var gradeResponse = await _client.GetAsync($"api/grades/bysubject/{invalidSubject}/bystudent/{validStudentId}");

    Assert.Equal(400, (int)gradeResponse.StatusCode);
    var errorMessage = await gradeResponse.Content.ReadAsStringAsync();
    Assert.Contains("Bad request", errorMessage);
    await ClearDatabaseAsync();
}


[Fact]
public async Task GetGradesBySubjectByStudent_ShouldReturnInternalServerError_WhenExceptionOccurs()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassWithSingleSubjectAsync();

    var validStudentId = "studentId1";
    var validSubject = "Matematika";

    var gradeResponse = await _mockClient.GetAsync($"api/grades/bysubject/{validSubject}/bystudent/{validStudentId}");

    Assert.Equal(500, (int)gradeResponse.StatusCode);
    var errorMessage = await gradeResponse.Content.ReadAsStringAsync();
    Assert.Contains("Internal server error", errorMessage);
    await ClearDatabaseAsync();
}


private async Task AddStudentsAndClassWithSingleSubjectNoGradesAsync()
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
            StudentNo = "S12345",
            Role = "Student"
           
        };

        var student2 = new Student
        {
            Id = "34fcq234",
            UserName = "hajdukalman",
            FirstName = "Kálmán",
            FamilyName = "Hajdu",
            BirthDate = new DateTime(2006, 6, 6),
            BirthPlace = "Debrecen",
            StudentNo = "S67890",
            Role = "Student"
         
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
            Role = "Student"
          
        };

        var student4 = new Student
        {
            Id = "789d2x2",
            UserName = "janoslaszlo",
            FirstName = "László",
            FamilyName = "János",
            BirthDate = new DateTime(2008, 8, 8),
            BirthPlace = "Szeged",
            StudentNo = "S22222",
            Role = "Student"
          
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
            Students = new List<Student> { student1, student2, student3, student4 }
        };

        var classesOfStudents = new List<ClassOfStudents> { class1 };

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

        var teacher1 = new Teacher
        {
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

    
private async Task ClearDatabaseAsync()
{
    using (var scope = _factory.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();
        
        context.ClassesOfStudents.RemoveRange(context.ClassesOfStudents);
        context.Grades.RemoveRange(context.Grades);
        context.Messages.RemoveRange(context.Messages);
        context.Parents.RemoveRange(context.Parents);
        context.Students.RemoveRange(context.Students);
        context.Teachers.RemoveRange(context.Teachers);
        context.TeacherSubjects.RemoveRange(context.TeacherSubjects);
        
        var users = await context.Users
            .Where(u => u.Id != null)
            .ToListAsync<IdentityUser>();

        context.Users.RemoveRange(users);
        
        await context.SaveChangesAsync();
    }
}



[Fact]
public async Task GetTeachersLastGrade_ReturnsLatestGrade_WhenValidTeacherIdIsProvided()
{
   
    await ClearDatabaseAsync();
    await AddStudentsAndClassesAndGradesAsync();
    
    var response = await _client.GetAsync("/api/grades");
    response.EnsureSuccessStatusCode();
    var content = await response.Content.ReadAsStringAsync();
    var grades = JsonConvert.DeserializeObject<List<Grade>>(content);
    
    var teacherId = grades.First().TeacherId;
    
    var gradeResponse = await _client.GetAsync($"/api/grades/teacherslast/{teacherId}");

    gradeResponse.EnsureSuccessStatusCode();
    var gradeContent = await gradeResponse.Content.ReadAsStringAsync();
    var returnedGrade = JsonConvert.DeserializeObject<LatestGradeResponse>(gradeContent);
    
    var latestGrade = grades.Where(g => g.TeacherId == teacherId).OrderByDescending(g => g.Date).First();
    
    Assert.Equal(latestGrade.Value, returnedGrade.Value);
    Assert.Equal(latestGrade.Subject, returnedGrade.Subject);
    Assert.Equal(latestGrade.Date.ToString("yyyy-MM-dd"), returnedGrade.Date.ToString("yyyy-MM-dd"));
    Assert.Equal(latestGrade.StudentId, returnedGrade.StudentId);
    Assert.Equal(latestGrade.ForWhat, returnedGrade.ForWhat);
    Assert.Equal(latestGrade.Read, returnedGrade.Read);
    await ClearDatabaseAsync();
}



[Fact]
public async Task GetTeachersLastGrade_ReturnsBasicLatestGradeResponse_WhenTeacherHasNoGrades()
{
    await ClearDatabaseAsync();
    
    await AddTeachersAsync();
    
    var response = await _client.GetAsync("/api/teachers");
    response.EnsureSuccessStatusCode();

    var content = await response.Content.ReadAsStringAsync();
    var teachers = JsonConvert.DeserializeObject<List<Teacher>>(content);

    var teacherId = teachers.First().Id;
    
    var gradeResponse = await _client.GetAsync($"/api/grades/teacherslast/{teacherId}");
    gradeResponse.EnsureSuccessStatusCode();
    
    var gradeContent = await gradeResponse.Content.ReadAsStringAsync();
    var latestGradeResponse = JsonConvert.DeserializeObject<LatestGradeResponse>(gradeContent);
    
    Assert.NotNull(latestGradeResponse);
    Assert.Equal(0, latestGradeResponse.Id);
    Assert.Null(latestGradeResponse.TeacherId);
    Assert.Null(latestGradeResponse.StudentId);
    Assert.Null(latestGradeResponse.StudentName);
    Assert.Null(latestGradeResponse.Subject);
    Assert.Null(latestGradeResponse.ForWhat);
    Assert.False(latestGradeResponse.Read);

    Assert.Equal(DateTime.MinValue, latestGradeResponse.Date);
    await ClearDatabaseAsync();
    
}




[Fact]
public async Task GetTeachersLastGrade_ReturnsBadRequest_WhenInvalidTeacherIdIsProvided()
{
    await ClearDatabaseAsync();
    
    await AddTeachersAsync();
    
    var invalidTeacherId = "invalidTeacherId123";

    var gradeResponse = await _client.GetAsync($"/api/grades/teacherslast/{invalidTeacherId}");
    
    Assert.Equal(HttpStatusCode.BadRequest, gradeResponse.StatusCode);
    
    var errorContent = await gradeResponse.Content.ReadAsStringAsync();
    Assert.Contains("Bad request", errorContent);
    await ClearDatabaseAsync();
}



[Fact]
public async Task GetTeachersLastGrade_ReturnsInternalServerError_WhenUnexpectedErrorOccurs()
{
    await ClearDatabaseAsync();
    
    await AddTeachersAsync(); 
    
    var validTeacherId = "validTeacherId123";

    var mockFactory = _factory.WithWebHostBuilder(builder =>
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<IGradeRepository, MockGradeRepository>();
        });
    });

    var mockClient = mockFactory.CreateClient();
    
    var gradeResponse = await mockClient.GetAsync($"/api/grades/teacherslast/{validTeacherId}");
    
    Assert.Equal(HttpStatusCode.InternalServerError, gradeResponse.StatusCode);
    var errorContent = await gradeResponse.Content.ReadAsStringAsync();
    Assert.Contains("Internal server error", errorContent);
    await ClearDatabaseAsync();
}



[Fact]
public async Task GetNewGradesNumber_ReturnsCorrectCount_WhenValidStudentIdIsProvided()
{
    await ClearDatabaseAsync();
    
    await AddStudentsAndClassesAndGradesAsync();
    
    var studentResponse = await _client.GetAsync("/api/students");
    studentResponse.EnsureSuccessStatusCode();
    var studentContent = await studentResponse.Content.ReadAsStringAsync();
    var students = JsonConvert.DeserializeObject<List<Student>>(studentContent);
    
    var studentId = students.First().Id;
    var newGradesResponse = await _client.GetAsync($"/api/grades/newgradesnum/{studentId}");
    newGradesResponse.EnsureSuccessStatusCode();
    
    var newGradesContent = await newGradesResponse.Content.ReadAsStringAsync();
    var newGradesCount = int.Parse(newGradesContent);
    Assert.Equal(1, newGradesCount);
    await ClearDatabaseAsync();
}

[Fact]
public async Task GetNewGradesNumber_ReturnsInternalServerError_WhenRepositoryThrowsException()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassesAndGradesAsync();
    
    var studentResponse = await _client.GetAsync("/api/students");
    studentResponse.EnsureSuccessStatusCode();
    var studentContent = await studentResponse.Content.ReadAsStringAsync();
    var students = JsonConvert.DeserializeObject<List<Student>>(studentContent);

    var validStudentId = students.First().Id;

    var mockClientResponse = await _mockClient.GetAsync($"/api/grades/newgradesnum/{validStudentId}");

    Assert.Equal(System.Net.HttpStatusCode.InternalServerError, mockClientResponse.StatusCode);

    var mockClientContent = await mockClientResponse.Content.ReadAsStringAsync();
    Assert.Contains("Internal server error", mockClientContent);
    await ClearDatabaseAsync();
}



[Fact]
public async Task GetNewGradesNumber_ReturnsBadRequest_WhenInvalidStudentIdIsProvided()
{
    await ClearDatabaseAsync();
    
    await AddStudentsAndClassesAndGradesAsync();
    
    var studentResponse = await _client.GetAsync("/api/students");
    studentResponse.EnsureSuccessStatusCode();
    var studentContent = await studentResponse.Content.ReadAsStringAsync();
    var students = JsonConvert.DeserializeObject<List<Student>>(studentContent);
    
    var invalidStudentId = "invalid-student-id";
    
    var invalidStudentResponse = await _client.GetAsync($"/api/grades/newgradesnum/{invalidStudentId}");
    
    Assert.Equal(System.Net.HttpStatusCode.BadRequest, invalidStudentResponse.StatusCode);
    
    var invalidStudentContent = await invalidStudentResponse.Content.ReadAsStringAsync();
    Assert.Contains("Bad request", invalidStudentContent);
    await ClearDatabaseAsync();
    
}

[Fact]
public async Task GetNewGradesByStudentId_ReturnsEmptyList_WhenNoGradesExistForStudent()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassesAsync();
    
    var studentResponse = await _client.GetAsync("/api/students");
    studentResponse.EnsureSuccessStatusCode();
    var studentContent = await studentResponse.Content.ReadAsStringAsync();
    var students = JsonConvert.DeserializeObject<List<Student>>(studentContent);

    var validStudentId = students.First().Id;

    var response = await _client.GetAsync($"/api/grades/newgrades/{validStudentId}");
    
    response.EnsureSuccessStatusCode();
    
    var content = await response.Content.ReadAsStringAsync();
    var grades = JsonConvert.DeserializeObject<List<Grade>>(content);

    Assert.Empty(grades);
    await ClearDatabaseAsync();
}


[Fact]
public async Task GetNewGradesByStudentId_ReturnsNewGrades_WhenValidStudentIdIsProvided()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassesAndGradesAsync();
    
    var studentResponse = await _client.GetAsync("/api/students");
    studentResponse.EnsureSuccessStatusCode();
    var studentContent = await studentResponse.Content.ReadAsStringAsync();
    var students = JsonConvert.DeserializeObject<List<Student>>(studentContent);

    var validStudentId = students.First().Id;

    var response = await _client.GetAsync($"/api/grades/newgrades/{validStudentId}");
    
    response.EnsureSuccessStatusCode();
    
    var content = await response.Content.ReadAsStringAsync();
    var grades = JsonConvert.DeserializeObject<List<Grade>>(content);

    Assert.NotEmpty(grades);
    await ClearDatabaseAsync();
}

[Fact]
public async Task GetNewGradesByStudentId_ReturnsBadRequest_WhenInvalidStudentIdIsProvided()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassesAndGradesAsync();

    var invalidStudentId = "invalidStudentId";

    var response = await _client.GetAsync($"/api/grades/newgrades/{invalidStudentId}");

    Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);

    var content = await response.Content.ReadAsStringAsync();
    Assert.Contains("Bad request", content);
    await ClearDatabaseAsync();
}


[Fact]
public async Task GetNewGradesByStudentId_ReturnsInternalServerError_WhenRepositoryThrowsException()
{
    await ClearDatabaseAsync();
    await AddStudentsAndClassesAndGradesAsync();
    
    var studentResponse = await _client.GetAsync("/api/students");
    studentResponse.EnsureSuccessStatusCode();
    var studentContent = await studentResponse.Content.ReadAsStringAsync();
    var students = JsonConvert.DeserializeObject<List<Student>>(studentContent);

    var validStudentId = students.First().Id;

    var mockClientResponse = await _mockClient.GetAsync($"/api/grades/newgrades/{validStudentId}");

    Assert.Equal(System.Net.HttpStatusCode.InternalServerError, mockClientResponse.StatusCode);

    var mockClientContent = await mockClientResponse.Content.ReadAsStringAsync();
    Assert.Contains("Internal server error", mockClientContent);
    await ClearDatabaseAsync();
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
            StudentNo = "S12345",
            Role = "Student"
            
        };

       
        var students = new List<Student> { student1 };

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
            Students = new List<Student> { student1 }
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


        var teacher1 = new Teacher
        {
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
            ClassOfStudents = class1,
            ClassName = class1.Name
        };
        
        context.TeacherSubjects.Add(teacherSubject1);

        await context.SaveChangesAsync();
    }
}


private async Task AddTeachersAsync()
{
    using (var scope = _factory.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();

   
        var teacher1 = new Teacher
        {
            FirstName = "Emily",
            FamilyName = "Wood",
            UserName = "emilywood",
            Email = "emily@example.com",
            Role = "Teacher"
        };

        context.Teachers.Add(teacher1);

        await context.SaveChangesAsync();
    }
}






private async Task AddStudentsAndClassWithSingleSubjectAsync()
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
            StudentNo = "S12345",
            Role = "Student"
           
        };

        var student2 = new Student
        {
            Id = "34fcq234",
            UserName = "hajdukalman",
            FirstName = "Kálmán",
            FamilyName = "Hajdu",
            BirthDate = new DateTime(2006, 6, 6),
            BirthPlace = "Debrecen",
            StudentNo = "S67890",
            Role = "Student"
           
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
            Role = "Student"
            
        };

        var student4 = new Student
        {
            Id = "789d2x2",
            UserName = "janoslaszlo",
            FirstName = "László",
            FamilyName = "János",
            BirthDate = new DateTime(2008, 8, 8),
            BirthPlace = "Szeged",
            StudentNo = "S22222",
            Role = "Student"
           
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
            Students = students
        };


        var existingClass = await context.ClassesOfStudents
            .FirstOrDefaultAsync(c => c.Name == class1.Name && c.Grade == class1.Grade && c.Section == class1.Section);
        if (existingClass == null)
        {
            context.ClassesOfStudents.Add(class1);
        }
        else
        {
            context.Entry(existingClass).CurrentValues.SetValues(class1);
        }

        var teacher = new Teacher
        {
            FirstName = "John",
            FamilyName = "Doe",
            UserName = "johndoe",
            Email = "johndoe@example.com",
            Role = "Teacher"
        };


        context.Teachers.Add(teacher);


        var teacherSubject = new TeacherSubject
        {
            Subject = "Matematika",
            TeacherId = teacher.Id,
            Teacher = teacher,
            ClassOfStudentsId = class1.Id,
            ClassOfStudents = class1,
            ClassName = class1.Name
        };

        context.TeacherSubjects.Add(teacherSubject);

  
        var grades = new List<Grade>
        {
            new Grade
            {
                TeacherId = teacher.Id,
                StudentId = student1.Id,
                Subject = "Matematika",
                ForWhat = "Témazáró",
                Read = false,
                Value = GradeValues.Elégtelen,
                Date = DateTime.Now
            },
            new Grade
            {
                TeacherId = teacher.Id,
                StudentId = student2.Id,
                Subject = "Matematika",
                ForWhat = "Témazáró",
                Read = false,
                Value = GradeValues.Elégséges,
                Date = DateTime.Now
            },
            new Grade
            {
                TeacherId = teacher.Id,
                StudentId = student3.Id,
                Subject = "Matematika",
                ForWhat = "Témazáró",
                Read = false,
                Value = GradeValues.Jeles,
                Date = DateTime.Now
            },
            new Grade
            {
                TeacherId = teacher.Id,
                StudentId = student4.Id,
                Subject = "Matematika",
                ForWhat = "Témazáró",
                Read = false,
                Value = GradeValues.Jó,
                Date = DateTime.Now
            }
        };

        foreach (var grade in grades)
        {
            var existingGrade = await context.Grades
                .FirstOrDefaultAsync(g => g.StudentId == grade.StudentId && g.Subject == grade.Subject);
            if (existingGrade == null)
            {
                context.Grades.Add(grade);
            }
            else
            {
                context.Entry(existingGrade).CurrentValues.SetValues(grade);
            }
        }


        await context.SaveChangesAsync();
    }
}

    
   private async Task AddStudentsAndClassesAndGradesAsync()
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
            StudentNo = "S12345",
            Role = "Student"
           
        };

        var student2 = new Student
        {
            Id = "34fcq234",
            UserName = "hajdukalman",
            FirstName = "Kálmán",
            FamilyName = "Hajdu",
            BirthDate = new DateTime(2006, 6, 6),
            BirthPlace = "Debrecen",
            StudentNo = "S67890",
            Role = "Student"
         
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
            Role = "Student"
         
        };

        var student4 = new Student
        {
            Id = "789d2x2",
            UserName = "janoslaszlo",
            FirstName = "László",
            FamilyName = "János",
            BirthDate = new DateTime(2008, 8, 8),
            BirthPlace = "Szeged",
            StudentNo = "S22222",
            Role = "Student"
           
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
        
        var teacher1 = new Teacher
        {
            FirstName = "John",
            FamilyName = "Doe",
            UserName = "johndoe",
            Email = "johndoe@example.com",
            Role = "Teacher"
        };

        var teacher2 = new Teacher
        {
            FirstName = "Jane",
            FamilyName = "Smith",
            UserName = "janesmith",
            Email = "janesmith@example.com",
            Role = "Teacher"
        };
        

        context.Teachers.Add(teacher1);
        context.Teachers.Add(teacher2);


        var teacherSubject1 = new TeacherSubject
        {
            Subject = "Matematika",
            TeacherId = teacher1.Id,
            Teacher = teacher1,
            ClassOfStudentsId = class1.Id,
            ClassOfStudents = class1,
            ClassName = class1.Name
        };

        var teacherSubject2 = new TeacherSubject
        {
            Subject = "Fizika",
            TeacherId = teacher2.Id,
            Teacher = teacher2,
            ClassOfStudentsId = class2.Id,
            ClassOfStudents = class2,
            ClassName = class2.Name
        };

        context.TeacherSubjects.Add(teacherSubject1);
        context.TeacherSubjects.Add(teacherSubject2);

        await context.SaveChangesAsync();

     
        var grade1 = new Grade
        {
            TeacherId = teacher1.Id,
            StudentId = student1.Id,
            Subject = "Matematika",
            ForWhat = "Témazáró",
            Read = false,
            Value = GradeValues.Elégtelen,
            Date = DateTime.Now
        };

        var grade2 = new Grade
        {
            TeacherId = teacher2.Id,
            StudentId = student2.Id,
            Subject = "Fizika",
            ForWhat = "Röpdolgozat",
            Read = false,
            Value = GradeValues.Elégséges,
            Date = DateTime.Now
        };

        var grade3 = new Grade
        {
            TeacherId = teacher1.Id,
            StudentId = student3.Id,
            Subject = "Matematika",
            ForWhat = "Órai munka",
            Read = false,
            Value = GradeValues.Jeles,
            Date = DateTime.Now
        };

        var grade4 = new Grade
        {
            TeacherId = teacher2.Id,
            StudentId = student4.Id,
            Subject = "Fizika",
            ForWhat = "Beadandó",
            Read = false,
            Value = GradeValues.Jó,
            Date = DateTime.Now
        };

 
        student1.Grades.Add(grade1);
        student2.Grades.Add(grade2);
        student3.Grades.Add(grade3);
        student4.Grades.Add(grade4);

        await context.SaveChangesAsync();
    }
}
}