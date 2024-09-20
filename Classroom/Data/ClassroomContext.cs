using Classroom.Model.DataModels;
using Classroom.Model.DataModels.Enums;
using Microsoft.EntityFrameworkCore;

namespace Classroom.Data;

public class ClassroomContext : DbContext
{
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<ClassOfStudents> ClassesOfStudents{get;set;}
    public DbSet<TeacherSubject> TeacherSubjects { get; set; }

    public DbSet<Grade> Grades { get; set; }   
    public DbSet<NotificationBase> Notifications { get; set; }
  
    
    public ClassroomContext(DbContextOptions<ClassroomContext> options)
        : base(options)
    {
    }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
            .HasMany(s => s.Grades)
            .WithOne()
            .HasForeignKey("StudentId")
            .OnDelete(DeleteBehavior.Restrict);

        
        modelBuilder.Entity<Student>()
            .HasMany(s => s.Notifications)
            .WithMany(n => n.Students)
            .UsingEntity(j => j.ToTable("StudentNotifications"));

     
        modelBuilder.Entity<ClassOfStudents>()
            .HasMany(c => c.Students)
            .WithOne()
            .HasForeignKey("ClassOfStudentsId")
            .OnDelete(DeleteBehavior.Restrict);

    
        modelBuilder.Entity<TeacherSubject>()
            .HasOne(ts => ts.ClassOfStudents)
            .WithMany()
            .HasForeignKey(ts => ts.ClassOfStudentsId)
            .OnDelete(DeleteBehavior.Restrict);

       
        modelBuilder.Entity<TeacherSubject>()
            .HasOne(ts => ts.Teacher)
            .WithMany()
            .HasForeignKey(ts => ts.TeacherId)
            .OnDelete(DeleteBehavior.Cascade);

        
    }
   
}