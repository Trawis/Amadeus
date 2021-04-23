using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Common.Extensions;
using Core.AmadeusModule.Interfaces;
using Core.Common.Configuration;
using Core.Common.Constants;
using Core.Common.Messaging;
using Core.HotelModule.Interfaces;
using Core.HotelModule.Messaging;
using Core.HotelModule.Models;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Options;

namespace Core.HotelModule.Services
{
  public class HotelService : IHotelService
  {
    private readonly IAmadeusService _amadeusService;
    private readonly IMapper _mapper;
    private readonly AmadeusSettings _amadeusSettings;

    public HotelService(
      IAmadeusService amadeusService,
      IOptions<AmadeusSettings> amadeusSettings,
      IMapper mapper)
    {
      _amadeusService = amadeusService;
      _mapper = mapper;
      _amadeusSettings = amadeusSettings.Value;
    }

    public async Task<ListResponse<HotelSearchResponse>> SearchAsync(HotelSearchRequest request)
    {
      var response = new ListResponse<HotelSearchResponse>(request.CorrelationId());
      var authResponse = await _amadeusService.AuthenticateAsync();

      if (!authResponse.Succeeded)
      {
        response.AddErrors(authResponse.Errors);
        return response;
      }

      var hotelOffers = await _amadeusSettings.BaseUrl
      .AppendPathSegment(_amadeusSettings.HotelOffersRoute)
      .SetQueryParams(new
      {
        cityCode = request.CityCode,
        checkInDate = request.CheckInDate.ToString("yyyy-MM-dd"),
        checkOutDate = request.CheckOutDate.HasValue ? request.CheckOutDate.Value.ToString("yyyy-MM-dd") : null,
        adults = Constants.Amadeus.HOTEL_SEARCH_ADULTS,
        radius = Constants.Amadeus.HOTEL_SEARCH_RADIUS,
        includeClosed = Constants.Amadeus.HOTEL_SEARCH_INCLUDE_ALL,
        page = 1
      })
      .WithOAuthBearerToken(authResponse.Result.Access_Token)
      .GetAsync()
      .ReceiveJson<HotelData>();

      if (hotelOffers.Data.NullOrEmpty())
      {
        response.AddError(ValidationMessages.HotelsNotFoundError.ToError());
        return response;
      }

      var result = _mapper.Map<List<HotelSearchResponse>>(hotelOffers.Data);

      response.SetResult(result);
      return response;
    }
  }
}
