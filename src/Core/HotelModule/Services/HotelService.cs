using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Common.Extensions;
using Common.Interfaces;
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
    private readonly IMapper _mapper;
    private readonly IAmadeusService _amadeusService;
    private readonly ICachingService _cachingService;
    private readonly IAppLogger<HotelService> _logger;
    private readonly AmadeusSettings _amadeusSettings;

    public HotelService(
      IMapper mapper,
      IAmadeusService amadeusService,
      ICachingService cachingService,
      IAppLogger<HotelService> logger,
      IOptions<AmadeusSettings> amadeusSettings)
    {
      _mapper = mapper;
      _amadeusService = amadeusService;
      _cachingService = cachingService;
      _logger = logger;
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

      try
      {
        var checkInDate = request.CheckInDate.ToString("yyyy-MM-dd");
        var checkOutDate = request.CheckOutDate.HasValue ? request.CheckOutDate.Value.ToString("yyyy-MM-dd") : null;
        var cacheKey = $"{request.CityCode}#{checkInDate}#{checkOutDate}";

        var hotelOffers = await _cachingService.GetOrCreateAsync(cacheKey, () =>
        {
          return _amadeusSettings.BaseUrl
            .AppendPathSegment(_amadeusSettings.HotelOffersRoute)
            .SetQueryParams(new
            {
              cityCode = request.CityCode,
              checkInDate,
              checkOutDate,
              adults = Constants.Amadeus.HOTEL_SEARCH_ADULTS,
              radius = Constants.Amadeus.HOTEL_SEARCH_RADIUS,
              includeClosed = Constants.Amadeus.HOTEL_SEARCH_INCLUDE_ALL,
              page = 1
            })
            .WithOAuthBearerToken(authResponse.Result.Access_Token)
            .GetAsync()
            .ReceiveJson<HotelData>();
        }, TimeSpan.Zero, TimeSpan.FromDays(Constants.Amadeus.HOTEL_CACHE_STORE_DAYS));

        if (hotelOffers.Data.NullOrEmpty())
        {
          response.AddError(ValidationMessages.HotelsNotFoundError.ToError());
          return response;
        }

        var result = _mapper.Map<List<HotelSearchResponse>>(hotelOffers.Data);
        response.SetResult(result);
      }
      catch (Exception ex)
      {
        response.AddError(ValidationMessages.HotelsSearchError.ToError());
        _logger.LogError(ex, ValidationMessages.HotelsSearchError);
      }

      return response;
    }
  }
}
