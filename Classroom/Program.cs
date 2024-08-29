using System.Text;
//using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Classroom.Data;
using Classroom.Service.Repositories;
using Microsoft.OpenApi.Models;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure services
        ConfigureServices(builder.Services, builder.Configuration);
        ConfigureSwagger(builder.Services);
        ConfigureDatabaseContexts(builder.Services, builder.Configuration);
        ConfigureCustomServices(builder.Services);

        var app = builder.Build();

        // Configure middleware
        ConfigureMiddleware(app);
        
        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
    }

    private static void ConfigureSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
        });
    }

    private static void ConfigureDatabaseContexts(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MSSQL_CONNECTION");
        
        services.AddDbContext<ClassroomContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
    }

    private static void ConfigureCustomServices(IServiceCollection services)
    {
        services.AddScoped<ITeacherRepository, TeacherRepository>();
        services.AddScoped<IGradeRepository, GradeRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
    }

    private static void ConfigureMiddleware(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo API V1");
                c.RoutePrefix = string.Empty; // Swagger UI elérhető a gyökér URL-en
            });
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
    }
}
