using Ardalis.ApiEndpoints;
using Core.Common.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.HealthCheck
{
  [AllowAnonymous]
  [ApiVersionNeutral]
  public class HealthCheck : BaseEndpoint<HealthStatus>
  {
    private readonly HealthCheckService _healthCheckService;

    public HealthCheck(HealthCheckService healthCheckService)
    {
      _healthCheckService = healthCheckService;
    }

    [HttpGet("api/health")]
    [ProducesResponseType(typeof(HealthStatus), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Returns health status",
        Description = "Runs all the health checks in the application and returns the aggregated status.",
        OperationId = "health.check",
        Tags = new[] { "HealthCheckEndpoints" })
    ]
    public override ActionResult<HealthStatus> Handle()
    {
      var healthReport = _healthCheckService.CheckHealthAsync();

      if (healthReport.Result.Status == HealthStatus.Healthy)
      {
        return Ok(healthReport.Result.Status);
      }

      return StatusCode(500);
    }
  }
}
