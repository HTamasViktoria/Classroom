using Classroom.Service.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using ClassromIntegrationTests.MockRepos;
using Classroom.Data;
using Classroom.Model.DataModels;
using Classroom.Model.RequestModels;
using Classroom.Model.ResponseModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ClassromIntegrationTests
{
    public class ClassOfStudentsControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;
        private readonly HttpClient _mockClient;

        public ClassOfStudentsControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();


            var mockFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices((context, services) =>
                {
                    services.AddTransient<IClassOfStudentsRepository, MockClassOfStudentsRepository>();
                });
            });

            _mockClient = mockFactory.CreateClient();
        }


        [Fact]
        public async Task GetAll_ShouldReturnEmptyList_WhenNoClassesExist()
        {
            await ClearDatabaseAsync();
            var response = await _client.GetAsync("/api/classes");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.Equal("[]", responseBody);
        }


        [Fact]
        public async Task GetAll_ShouldReturnClasses_WhenClassesExist()
        {
            await ClearDatabaseAsync();
            await AddStudentsClassesTeachersAndSubjectsAsync();

            var response = await _client.GetAsync("/api/classes");

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var classes = JsonConvert.DeserializeObject<List<ClassOfStudents>>(responseContent);

            Assert.NotEmpty(classes);

            Assert.Contains(classes, c => c.Name == "Math 101" && c.Grade == "10" && c.Section == "A");
            Assert.Contains(classes, c => c.Name == "Science 101" && c.Grade == "10" && c.Section == "B");
        }





        [Fact]
        public async Task GetAll_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            await ClearDatabaseAsync();

            var mockFactory = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddTransient<IClassOfStudentsRepository, MockClassOfStudentsRepository>();
                });
            });

            var mockClient = mockFactory.CreateClient();

            var response = await mockClient.GetAsync("/api/classes");

            Assert.Equal(500, (int)response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("Internal server error", responseContent);
        }



        [Fact]
        public async Task GetAllStudentsWithClasses_ShouldReturnStudentsWithClasses_WhenClassesExist()
        {
            await ClearDatabaseAsync();
            await AddStudentsClassesTeachersAndSubjectsAsync();

            var response = await _client.GetAsync("/api/classes/students");
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var studentsWithClasses = JsonConvert.DeserializeObject<List<StudentWithClassResponse>>(responseContent);

            Assert.NotEmpty(studentsWithClasses);
            
            foreach (var student in studentsWithClasses)
            {
                Console.WriteLine(
                    $"Id: {student.Id}, FirstName: {student.FirstName}, FamilyName: {student.FamilyName}, NameOfClass: {student.NameOfClass}");
            }

           
            Assert.Contains(studentsWithClasses, s =>
                s.NameOfClass == "Math 101" && s.FirstName == "Béla" && s.FamilyName == "Kovács");
            Assert.Contains(studentsWithClasses, s =>
                s.NameOfClass == "Math 101" && s.FirstName == "Kálmán" && s.FamilyName == "Hajdu");

            Assert.Contains(studentsWithClasses, s =>
                s.NameOfClass == "Science 101" && s.FirstName == "Károly" && s.FamilyName == "Péter");
            Assert.Contains(studentsWithClasses, s =>
                s.NameOfClass == "Science 101" && s.FirstName == "László" && s.FamilyName == "János");
        }



        [Fact]
        public async Task GetAllStudentsWithClasses_ShouldReturnInternalServerError_WhenExceptionThrown()
        {
            var mockFactory = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddTransient<IClassOfStudentsRepository, MockClassOfStudentsRepository>();
                });
            });

            var mockClient = mockFactory.CreateClient();

            var response = await mockClient.GetAsync("/api/classes/students");

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.Contains("Mock exception for testing.", responseBody);
        }



        
        [Fact]
        public async Task GetStudents_ShouldReturnStudents_WhenClassExists()
        {
            await ClearDatabaseAsync();
            await AddStudentsClassesTeachersAndSubjectsAsync();

            var classesResponse = await _client.GetAsync("/api/classes");
            classesResponse.EnsureSuccessStatusCode();
            var classesContent = await classesResponse.Content.ReadAsStringAsync();
            var classes = JsonConvert.DeserializeObject<List<ClassOfStudents>>(classesContent);

            Assert.NotEmpty(classes);

            var mathClass = classes.FirstOrDefault(c => c.Name == "Math 101");
            var scienceClass = classes.FirstOrDefault(c => c.Name == "Science 101");

            Assert.NotNull(mathClass);
            Assert.NotNull(scienceClass);

            var responseMath = await _client.GetAsync($"/api/classes/students-of-a-class/{mathClass.Id}");
            responseMath.EnsureSuccessStatusCode();
            var responseContentMath = await responseMath.Content.ReadAsStringAsync();
            var studentsInMathClass = JsonConvert.DeserializeObject<List<Student>>(responseContentMath);

            Assert.NotEmpty(studentsInMathClass);
            Assert.Contains(studentsInMathClass, s => s.Id == "123d1xd3" && s.FirstName == "Béla" && s.FamilyName == "Kovács");
            Assert.Contains(studentsInMathClass, s => s.Id == "34fcq234" && s.FirstName == "Kálmán" && s.FamilyName == "Hajdu");

            var responseScience = await _client.GetAsync($"/api/classes/students-of-a-class/{scienceClass.Id}");
            responseScience.EnsureSuccessStatusCode();
            var responseContentScience = await responseScience.Content.ReadAsStringAsync();
            var studentsInScienceClass = JsonConvert.DeserializeObject<List<Student>>(responseContentScience);

            Assert.NotEmpty(studentsInScienceClass);
            Assert.Contains(studentsInScienceClass, s => s.Id == "567d1x1" && s.FirstName == "Károly" && s.FamilyName == "Péter");
            Assert.Contains(studentsInScienceClass, s => s.Id == "789d2x2" && s.FirstName == "László" && s.FamilyName == "János");
        }


        


        [Fact]
        public async Task GetStudents_ShouldReturnBadRequest_WhenClassNotFound()
        {
            var nonExistentClassId = 9999;
            
            var response = await _client.GetAsync($"/api/classes/students-of-a-class/{nonExistentClassId}");
            
            Assert.Equal(400, (int)response.StatusCode);
    
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("Bad request", responseContent);
        }


        
        [Fact]
        public async Task GetStudents_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            await ClearDatabaseAsync();

            var mockFactory = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddTransient<IClassOfStudentsRepository, MockClassOfStudentsRepository>();
                });
            });

            var mockClient = mockFactory.CreateClient();

            var response = await mockClient.GetAsync("/api/classes/students-of-a-class/1");
            
            Assert.Equal(500, (int)response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("Internal server error", responseContent);
        }

        
        /*[Fact]
        public async Task GetClassesBySubject_ShouldReturnClasses_WhenSubjectExists()
        {
            // Arrange
            await ClearDatabaseAsync();
            await AddStudentsClassesTeachersAndSubjectsAsync();

            // Act
            var response = await _client.GetAsync("/api/classes/bysubject/Matematika"); 

            // Assert
            response.EnsureSuccessStatusCode();
    
            var responseContent = await response.Content.ReadAsStringAsync();
            var classes = JsonConvert.DeserializeObject<List<ClassOfStudents>>(responseContent);
    
            Assert.NotEmpty(classes);
    
            // Módosított Assert
            Assert.Contains(classes, c => c.Name == "Math 101" && c.Grade == "10" && c.Section == "A");
            Assert.DoesNotContain(classes, c => c.Name == "Science 101" && c.Grade == "10" && c.Section == "B");
        }
        */




        [Fact]
        public async Task GetClassesBySubject_ShouldReturnEmptyList_WhenNoClassesExistForSubject()
        {
            await ClearDatabaseAsync();
            
            var response = await _client.GetAsync("/api/classes/bysubject/Matematika");
            
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.Equal("[]", responseContent);
        }
        

        [Fact]
        public async Task GetClassesBySubject_ShouldReturnBadRequest_WhenSubjectInvalid()
        {
            var invalidSubject = "nemlétezőtantárgy";
            
            var response = await _client.GetAsync($"/api/classes/bysubject/{invalidSubject}");
            
            Assert.Equal(400, (int)response.StatusCode);
    
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("Bad request", responseContent);
        }
    

        
        
        [Fact]
        public async Task GetClassesBySubject_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            await ClearDatabaseAsync();

            var mockFactory = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddTransient<IClassOfStudentsRepository, MockClassOfStudentsRepository>();
                });
            });

            var mockClient = mockFactory.CreateClient();

            var response = await mockClient.GetAsync("/api/classes/bysubject/Matematika");
            
            Assert.Equal(500, (int)response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("Internal server error", responseContent);
        }

        
        
        [Fact]
        public async Task Post_ShouldReturnCreated_WhenClassIsSuccessfullyAdded()
        {
            await ClearDatabaseAsync();
            var student1 = new Student
            {
                Id = "1",
                FirstName = "John",
                FamilyName = "Doe",
                Role = "Student",
                StudentNo = "S12345",
                BirthDate = DateTime.Now.AddYears(-16),
                BirthPlace = "Budapest"
            };

            var student2 = new Student
            {
                Id = "2",
                FirstName = "Jane",
                FamilyName = "Smith",
                Role = "Student",
                StudentNo = "S12346",
                BirthDate = DateTime.Now.AddYears(-15),
                BirthPlace = "Budapest"
            };

            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();

                context.Students.AddRange(student1, student2);
                await context.SaveChangesAsync();
            }

            var request = new ClassOfStudentsRequest
            {
                Grade = "10",
                Section = "A",
                Students = new List<Student> { student1, student2 }
            };

  
            var response = await _client.PostAsJsonAsync("/api/classes", request);


            Assert.Equal(201, (int)response.StatusCode);
    
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("Osztály sikeresen elmentve az adatbázisba", responseContent);
        }

        
       
        
        
        [Fact]
public async Task Post_ShouldReturnBadRequest_WhenClassAlreadyExists()
{
    var existingClass = new ClassOfStudentsRequest
    {
        Id = 1,
        Grade = "10",
        Section = "A",
        Students = new List<Student> 
        { 
            new Student 
            { 
                Id = "5r6j7z5", 
                FirstName = "John", 
                FamilyName = "Doe", 
                StudentNo = "12345", 
                Role = "Student",
                BirthPlace = "Budapest",
                BirthDate = new DateTime(2005, 5, 15)
            } 
        }
    };

    
    await _client.PostAsJsonAsync("/api/classes", existingClass);
    
    var request = new ClassOfStudentsRequest
    {
        Id = 2,
        Grade = "10",
        Section = "A",
        Students = new List<Student> 
        { 
            new Student 
            { 
                Id = "2", 
                FirstName = "Jane", 
                FamilyName = "Smith", 
                StudentNo = "67890", 
                Role = "Student",
                BirthPlace = "Debrecen",
                BirthDate = new DateTime(2005, 6, 20)
            } 
        }
    };
    
    var response = await _client.PostAsJsonAsync("/api/classes", request);
    
    Assert.Equal(400, (int)response.StatusCode);

    var responseContent = await response.Content.ReadAsStringAsync();
    Assert.Contains("already exists", responseContent);
}


        
        


        [Fact]
        public async Task Post_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            await ClearDatabaseAsync();
            
            var mockFactory = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddTransient<IClassOfStudentsRepository, MockClassOfStudentsRepository>();
                });
            });

            var mockClient = mockFactory.CreateClient();
            
            var request = new ClassOfStudentsRequest
            {
                Id = 1,
                Grade = "10",
                Section = "A",
                Students = new List<Student>
                {
                    new Student
                    {
                        Id = "v4wr",
                        FirstName = "John",
                        FamilyName = "Doe",
                        StudentNo = "12345",
                        Role = "Student",
                        BirthPlace = "Budapest",
                        BirthDate = new DateTime(2005, 5, 15)
                    }
                }
            };
            
            var response = await mockClient.PostAsJsonAsync("/api/classes", request);
            
            Assert.Equal(500, (int)response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("Internal server error", responseContent);
        }

        
       
        
        
     [Fact]
public async Task AddingStudentToClass_ShouldReturnOk_WhenStudentSuccessfullyAddedToClass()
{
 
    await ClearDatabaseAsync();
    await AddStudentsClassesTeachersAndSubjectsAsync();
    
  
    var newStudent = new Student
    {
        Id = "3243gutjuz",
        UserName = "janedoe",
        FirstName = "Jane",
        FamilyName = "Doe",
        BirthDate = new DateTime(2005, 1, 1),
        BirthPlace = "Budapest",
        StudentNo = "S10002",
        Role = "Student"
    };

    await AddStudentToDatabaseAsync(newStudent);
    
    var classesResponse = await _client.GetAsync("/api/classes");
    Assert.True(classesResponse.IsSuccessStatusCode, "Failed to fetch classes.");

    var classesContent = await classesResponse.Content.ReadAsStringAsync();
    
    var classes = JsonConvert.DeserializeObject<List<ClassOfStudents>>(classesContent);
    Assert.NotEmpty(classes);


    var classId = classes.First().Id;
    
    var request = new AddingStudentToClassRequest
    {
        StudentId = newStudent.Id,
        ClassId = classId
    };


    var response = await _client.PostAsJsonAsync("/api/classes/addStudent", request);
 
    Assert.Equal(200, (int)response.StatusCode);

    var responseContent = await response.Content.ReadAsStringAsync();

    Assert.Contains("Student added to class", responseContent);
}




[Fact]
public async Task AddingStudentToClass_ShouldReturnBadRequest_WhenStudentIsAlreadyInClass()
{

    await ClearDatabaseAsync();
    await AddStudentsClassesTeachersAndSubjectsAsync();


    var classesResponse = await _client.GetAsync("/api/classes");
    Assert.True(classesResponse.IsSuccessStatusCode, "Failed to fetch classes.");

    var classesContent = await classesResponse.Content.ReadAsStringAsync();
    var classes = JsonConvert.DeserializeObject<List<ClassOfStudents>>(classesContent);
    Assert.NotEmpty(classes);


    var classId = classes.First().Id;
    
    var studentsResponse = await _client.GetAsync($"/api/classes/students-of-a-class/{classId}");
    Assert.True(studentsResponse.IsSuccessStatusCode, "Failed to fetch students for the class.");

    var studentsContent = await studentsResponse.Content.ReadAsStringAsync();
    var studentsInClass = JsonConvert.DeserializeObject<List<Student>>(studentsContent);
    Assert.NotEmpty(studentsInClass);

    var studentId = studentsInClass.First().Id;
    
    var request = new AddingStudentToClassRequest
    {
        StudentId = studentId,
        ClassId = classId
    };
    
    var response = await _client.PostAsJsonAsync("/api/classes/addStudent", request);
    
    Assert.Equal(400, (int)response.StatusCode);

    var responseContent = await response.Content.ReadAsStringAsync();
    
    Assert.Contains($"Student with ID {studentId} is already in the class", responseContent);
}



[Fact]
public async Task AddingStudentToClass_ShouldReturnNotFound_WhenClassIdIsInvalid()
{
 
    await ClearDatabaseAsync();

    await AddStudentsClassesTeachersAndSubjectsAsync();
    
  
    var newStudent = new Student
    {
        Id = "3243gutjuz",
        UserName = "janedoe",
        FirstName = "Jane",
        FamilyName = "Doe",
        BirthDate = new DateTime(2005, 1, 1),
        BirthPlace = "Budapest",
        StudentNo = "S10002",
        Role = "Student"
    };

    await AddStudentToDatabaseAsync(newStudent);

    var request = new AddingStudentToClassRequest
    {
        StudentId = newStudent.Id,
        ClassId = 3
    };


    var response = await _client.PostAsJsonAsync("/api/classes/addStudent", request);
 
    Assert.Equal(404, (int)response.StatusCode);

    var responseContent = await response.Content.ReadAsStringAsync();

    Assert.Contains($"Class with ID {request.ClassId} not found.", responseContent);
}


[Fact]
public async Task AddStudent_ShouldReturnInternalServerError_WhenExceptionOccurs()
{
    await ClearDatabaseAsync();
    await AddStudentsClassesTeachersAndSubjectsAsync();
  
    var newStudent = new Student
    {
        Id = "3243gutjuz",
        UserName = "janedoe",
        FirstName = "Jane",
        FamilyName = "Doe",
        BirthDate = new DateTime(2005, 1, 1),
        BirthPlace = "Budapest",
        StudentNo = "S10002",
        Role = "Student"
    };


    await AddStudentToDatabaseAsync(newStudent);


    var classesResponse = await _client.GetAsync("/api/classes");
    Assert.True(classesResponse.IsSuccessStatusCode, "Failed to fetch classes.");

    var classesContent = await classesResponse.Content.ReadAsStringAsync();
    var classes = JsonConvert.DeserializeObject<List<ClassOfStudents>>(classesContent);
    Assert.NotEmpty(classes);


    var classId = classes.First().Id;

 
    var request = new AddingStudentToClassRequest
    {
        StudentId = newStudent.Id,
        ClassId = classId
    };

 
    var mockFactory = _factory.WithWebHostBuilder(builder =>
    {
        builder.ConfigureServices(services =>
        {
        
            services.AddTransient<IClassOfStudentsRepository, MockClassOfStudentsRepository>();
        });
    });

    var mockClient = mockFactory.CreateClient();

    var response = await mockClient.PostAsJsonAsync("/api/classes/addStudent", request);
    
    Assert.Equal(500, (int)response.StatusCode);

    var responseContent = await response.Content.ReadAsStringAsync();
    Assert.Contains("Internal Server error", responseContent);
}




        
        private async Task AddStudentToDatabaseAsync(Student student)
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();
                
                var existingStudent = await context.Students.FindAsync(student.Id);
                if (existingStudent == null)
                {
                    context.Students.Add(student);
                }
                else
                {
                    context.Entry(existingStudent).CurrentValues.SetValues(student);
                }
                
                await context.SaveChangesAsync();
            }
        }

        
        
        

        private async Task ClearDatabaseAsync()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();
                
                context.TeacherSubjects.RemoveRange(context.TeacherSubjects);
                
                context.ClassesOfStudents.RemoveRange(context.ClassesOfStudents);
                context.Students.RemoveRange(context.Students);
                context.Parents.RemoveRange(context.Parents);

                await context.SaveChangesAsync();
            }
        }
        
        
      

        
  private async Task AddStudentsClassesTeachersAndSubjectsAsync()
{
    using (var scope = _factory.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();


        var students = new List<Student>
        {
            new Student
            {
                Id = "123d1xd3", UserName = "kovacsbela", FirstName = "Béla", FamilyName = "Kovács", 
                BirthDate = new DateTime(2005, 5, 5), BirthPlace = "Budapest", StudentNo = "S12345", Role = "Student"
            },
            new Student
            {
                Id = "34fcq234", UserName = "hajdukalman", FirstName = "Kálmán", FamilyName = "Hajdu", 
                BirthDate = new DateTime(2006, 6, 6), BirthPlace = "Debrecen", StudentNo = "S67890", Role = "Student"
            },
            new Student
            {
                Id = "567d1x1", UserName = "peterkarl", FirstName = "Károly", FamilyName = "Péter", 
                BirthDate = new DateTime(2007, 7, 7), BirthPlace = "Pécs", StudentNo = "S11111", Role = "Student"
            },
            new Student
            {
                Id = "789d2x2", UserName = "janoslaszlo", FirstName = "László", FamilyName = "János", 
                BirthDate = new DateTime(2008, 8, 8), BirthPlace = "Szeged", StudentNo = "S22222", Role = "Student"
            }
        };

    
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

      
        var classesOfStudents = new List<ClassOfStudents>
        {
            new ClassOfStudents
            {
                Name = "Math 101", Grade = "10", Section = "A", 
                Students = new List<Student> { students[0], students[1] }
            },
            new ClassOfStudents
            {
                Name = "Science 101", Grade = "10", Section = "B", 
                Students = new List<Student> { students[2], students[3] }
            }
        };

 
        foreach (var classOfStudent in classesOfStudents)
        {
            var existingClass = await context.ClassesOfStudents
                .FirstOrDefaultAsync(c => c.Name == classOfStudent.Name && c.Grade == classOfStudent.Grade && c.Section == classOfStudent.Section);

            if (existingClass == null)
            {
                context.ClassesOfStudents.Add(classOfStudent);
            }
            else
            {
                context.Entry(existingClass).CurrentValues.SetValues(classOfStudent);
            }
        }

        var teachers = new List<Teacher>
        {
            new Teacher
            {
                FirstName = "John", FamilyName = "Doe", UserName = "johndoe", Email = "johndoe@example.com", Role = "Teacher"
            },
            new Teacher
            {
                FirstName = "Jane", FamilyName = "Smith", UserName = "janesmith", Email = "janesmith@example.com", Role = "Teacher"
            }
        };

     
        foreach (var teacher in teachers)
        {
            context.Teachers.Add(teacher);
        }

   
        var teacher1 = await context.Teachers.FirstOrDefaultAsync(t => t.UserName == "johndoe");
        var teacher2 = await context.Teachers.FirstOrDefaultAsync(t => t.UserName == "janesmith");

        var class1 = await context.ClassesOfStudents.FirstOrDefaultAsync(c => c.Name == "Math 101");
        var class2 = await context.ClassesOfStudents.FirstOrDefaultAsync(c => c.Name == "Science 101");

   
        if (teacher1 != null && class1 != null)
        {
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
        }

        if (teacher2 != null && class2 != null)
        {
            var teacherSubject2 = new TeacherSubject
            {
                Subject = "Irodalom",
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


    }
}