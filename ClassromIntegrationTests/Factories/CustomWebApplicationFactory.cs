using Classroom.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.AspNetCore.Hosting;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public void ConfigureTestServices(IServiceCollection services)
    {
        var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ClassroomContext>));
        if (dbContextDescriptor != null)
        {
            services.Remove(dbContextDescriptor);
        }

        services.AddDbContext<ClassroomContext>(options =>
            options.UseInMemoryDatabase("TestDatabase"));
        
        var serviceProvider = services.BuildServiceProvider();
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ClassroomContext>();
            context.Database.EnsureCreated();
        }
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            ConfigureTestServices(services);
        });
    }
}