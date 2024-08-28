using Classroom.Model.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Classroom.Data;

public class ClassroomContext : DbContext
{
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Grade> Grades { get; set; }

    public ClassroomContext(DbContextOptions<ClassroomContext> options)
        : base(options)
    {
    }
}