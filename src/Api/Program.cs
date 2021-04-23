using System;
using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Api
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var host = CreateHostBuilder(args).Build();

      using (var scope = host.Services.CreateScope())
      {
        var services = scope.ServiceProvider;
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger<Program>();
        var configuration = services.GetRequiredService<IConfiguration>();
        var migrate = configuration.GetValue<bool?>("Database:Migrate");
        var seed = configuration.GetValue<bool?>("Database:Seed");
        try
        {
          var appContext = services.GetRequiredService<ApplicationContext>();

          if (migrate.GetValueOrDefault(false))
          {
            logger.LogInformation("Migrating the database");
            appContext.Database.Migrate();
          }

          if (seed.GetValueOrDefault(false))
          {
            logger.LogInformation("Seeding the database");
          }
        }
        catch (Exception ex)
        {
          logger.LogError(ex, "An error occurred while migrating or seeding the database.");
        }
      }

      host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            });
  }
}
