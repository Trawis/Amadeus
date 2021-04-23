using System.Linq;
using System.Threading.Tasks;
using Common.Models;
using Core.Common.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api.Filters
{
  public class ValidateModelStateAttribute : IAsyncActionFilter
  {
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
      if (!context.ModelState.IsValid)
      {
        var errors = context.ModelState
          .Where(x => x.Value.ValidationState == ModelValidationState.Invalid)
          .SelectMany(x =>
          {
            return x.Value.Errors.Select(e => new Error
            {
              Source = x.Key,
              Message = e.ErrorMessage
            });
          }).ToList();

        var response = new EmptyResponse();
        response.AddErrors(errors);

        context.Result = new JsonResult(response)
        {
          StatusCode = StatusCodes.Status400BadRequest
        };
        return;
      }

      await next();
    }
  }
}
