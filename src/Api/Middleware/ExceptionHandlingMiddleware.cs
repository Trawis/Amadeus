using System;
using System.Text.Json;
using System.Threading.Tasks;
using Common.Extensions;
using Common.Interfaces;
using Core.Common.Messaging;
using Microsoft.AspNetCore.Http;

namespace Api.Middleware
{
  public class ExceptionHandlingMiddleware
  {
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task Invoke(HttpContext context, IAppLogger<ExceptionHandlingMiddleware> logger)
    {
      try
      {
        await _next(context);
      }
      catch (Exception ex)
      {
        await HandleExceptionAsync(context, ex, logger);
      }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex, IAppLogger<ExceptionHandlingMiddleware> logger)
    {
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = StatusCodes.Status500InternalServerError;

      var response = new ErrorResponse();
      var errorMessage = "The application has encountered an unknown error.";
      response.AddError(errorMessage.ToError().AddSource(ex.Source));
      logger.LogError(ex, $"{response.CorrelationId} - {errorMessage}");

      await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions { IgnoreNullValues = true }));
    }
  }
}
