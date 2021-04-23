using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Common.Extensions;
using Core.Common.Messaging;
using Core.HotelModule.Interfaces;
using Core.HotelModule.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.HotelModule
{
  [ApiVersion("1.0")]
  [Route("api/v{version:apiVersion}")]
  public class HotelSearch : BaseAsyncEndpoint<HotelSearchRequest, ListResponse<HotelSearchResponse>>
  {
    private readonly IHotelService _hotelService;

    public HotelSearch(IHotelService hotelService)
    {
      _hotelService = hotelService;
    }

    [HttpPost("hotel-search")]
    [ProducesResponseType(typeof(ListResponse<HotelSearchResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EmptyResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Search hotels",
        Description = "Search hotels by paramteres",
        OperationId = "hotel.search",
        Tags = new[] { "HotelEndpoints" })
    ]
    public override async Task<ActionResult<ListResponse<HotelSearchResponse>>> HandleAsync([FromBody] HotelSearchRequest request, CancellationToken cancellationToken)
    {
      var response = await _hotelService.SearchAsync(request);

      if (response.Result.NullOrEmpty())
      {
        return NotFound(response);
      }

      return Ok(response);
    }
  }
}
