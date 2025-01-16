using Classroom.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.AspNetCore.Hosting;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = Guid.NewGuid().ToString();
   
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Keressük meg és távolítsuk el a meglévő ClassroomContext konfigurációját
            var classroomDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ClassroomContext>));
            if (classroomDescriptor != null)
            {
                services.Remove(classroomDescriptor);
            }
            
            // Hozzáadjuk a ClassroomContext-et egy in-memory adatbázissal
            services.AddDbContext<ClassroomContext>(options =>
            {
                options.UseInMemoryDatabase(_dbName);
            });
            
            // Adatbázis inicializálása
            using var scope = services.BuildServiceProvider().CreateScope();
            var classroomContext = scope.ServiceProvider.GetRequiredService<ClassroomContext>();
            
            // Adatbázis törlése és létrehozása
            classroomContext.Database.EnsureDeleted();
            classroomContext.Database.EnsureCreated();
            
            // Ide jöhet további inicializálás (pl. alapadatok betöltése)
            // classroomContext.SeedData();
        });
    }
}