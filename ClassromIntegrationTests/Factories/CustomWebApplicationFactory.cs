using Classroom.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace ClassromIntegrationTests.Factories;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
           
            config.AddJsonFile("appsettings.Test.json", optional: false);
        });

        builder.ConfigureServices(services =>
        {
            services.AddDbContext<ClassroomContext>(options =>
                options.UseSqlite("DataSource=:memory:"));

            services.AddScoped<ClassroomContext>(sp =>
            {
                var context = sp.GetRequiredService<ClassroomContext>();
                context.Database.EnsureCreated();
                return context;
            });
        });
    }
}
