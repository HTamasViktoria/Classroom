using Classroom.Model.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Classroom.Data;

public class ClassroomContext : DbContext
{
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Grade> Grades { get; set; }   
    public DbSet<NotificationBase> Notifications { get; set; }
  
    
    public ClassroomContext(DbContextOptions<ClassroomContext> options)
        : base(options)
    {
    }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NotificationBase>()
            .HasMany(n => n.Students)
            .WithMany(s => s.Notifications)
            .UsingEntity(j => j.ToTable("NotificationStudents"));
    }

   
}