using System.Text.Json.Serialization;
using Api.Filters;
using Api.Middleware;
using AutoMapper;
using Common.Interfaces;
using Core.AmadeusModule.Interfaces;
using Core.Common.Configuration;
using Core.Common.Constants;
using Core.Common.Interfaces;
using Core.Common.Mappings;
using Core.Common.Services;
using Core.HotelModule.Interfaces;
using Core.HotelModule.Messaging;
using Core.HotelModule.Services;
using FluentValidation.AspNetCore;
using Infrastructure.Data;
using Infrastructure.Logging;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Api
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureDevelopmentServices(IServiceCollection services)
    {
      if (Configuration.GetValue<bool>("Database:UseInMemoryDatabase"))
      {
        ConfigureInMemoryDatabases(services);
      }
      else
      {
        ConfigureDatabases(services);
      }
    }

    public void ConfigureDockerServices(IServiceCollection services)
    {
      ConfigureDevelopmentServices(services);
    }

    public void ConfigureProductionServices(IServiceCollection services)
    {
      if (Configuration.GetValue<bool>("Database:UseInMemoryDatabase"))
      {
        ConfigureInMemoryDatabases(services);
      }
      else
      {
        ConfigureDatabases(services);
      }
    }

    public void ConfigureTestingServices(IServiceCollection services)
    {
      ConfigureInMemoryDatabases(services);
    }

    private void ConfigureInMemoryDatabases(IServiceCollection services)
    {
      services.AddDbContext<ApplicationContext>(options =>
          options.UseInMemoryDatabase("amadeus"));

      ConfigureServices(services);
    }

    private void ConfigureDatabases(IServiceCollection services)
    {
      services.AddDbContext<ApplicationContext>(options =>
          options.UseLazyLoadingProxies().UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
          x => x.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Constants.Database.DEFAULT_SCHEMA)));

      ConfigureServices(services);
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.Configure<BaseUrlSettings>(Configuration.GetSection("BaseUrl"));
      services.Configure<AmadeusSettings>(Configuration.GetSection("Amadeus"));

      services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
      services.AddScoped<IApplicationContext, ApplicationContext>();
      services.AddScoped<IAmadeusService, AmadeusService>();
      services.AddScoped<IHotelService, HotelService>();
      services.AddScoped<ICachingService, CachingService>();
      services.AddTransient<IDateTime, DateTimeService>();

      services.AddMemoryCache();

      services.AddStackExchangeRedisCache(options =>
      {
        options.InstanceName = Configuration["Redis:InstanceName"];
        options.Configuration = Configuration.GetConnectionString("RedisConnection");
      });

      services.AddHealthChecks()
              .AddDbContextCheck<ApplicationContext>();

      services.AddCors(options =>
      {
        options.AddPolicy(Constants.Startup.CORS_POLICY, builder =>
        {
          builder.AllowAnyOrigin();
          builder.AllowAnyMethod();
          builder.AllowAnyHeader();
        });
      });

      services.Configure<ApiBehaviorOptions>(options =>
      {
        options.SuppressModelStateInvalidFilter = true;
      });

      services.AddControllers(options =>
              {
                options.Filters.Add(typeof(ValidateModelStateAttribute));
              })
              .AddFluentValidation(options =>
              {
                options.RegisterValidatorsFromAssemblyContaining<HotelSearchRequest>();
              })
              .AddJsonOptions(options =>
              {
                options.JsonSerializerOptions.IgnoreNullValues = true;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
              });

      services.AddApiVersioning(options =>
      {
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);
      });

      services.AddAutoMapper(typeof(MappingProfile).Assembly);

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Amadeus API", Version = "v1" });
        c.EnableAnnotations();
        c.SchemaFilter<CustomSchemaFilters>();
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseMiddleware<ExceptionHandlingMiddleware>();

      //app.UseHttpsRedirection();

      app.UseRouting();

      app.UseCors(Constants.Startup.CORS_POLICY);

      // Enable middleware to serve generated Swagger as a JSON endpoint.
      app.UseSwagger();

      // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
      // specifying the Swagger JSON endpoint.
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.yaml", "My API V1");
      });

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapHealthChecks("/");
      });
    }
  }
}
