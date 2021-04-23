using System;
using System.Threading.Tasks;
using Common.Extensions;
using Common.Interfaces;
using Core.AmadeusModule.Interfaces;
using Core.AmadeusModule.Messaging;
using Core.Common.Configuration;
using Core.Common.Constants;
using Core.Common.Messaging;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
  public class AmadeusService : IAmadeusService
  {
    private readonly AmadeusSettings _amadeusSettings;
    private readonly ICachingService _cachingService;
    private readonly IAppLogger<AmadeusService> _logger;

    public AmadeusService(
      ICachingService cachingService,
      IOptions<AmadeusSettings> amadeusSettings,
      IAppLogger<AmadeusService> logger)
    {
      _amadeusSettings = amadeusSettings.Value;
      _cachingService = cachingService;
      _logger = logger;
    }

    public async Task<Response<AuthResponse>> AuthenticateAsync()
    {
      var response = new Response<AuthResponse>();

      try
      {
        var cacheKey = nameof(AuthenticateAsync);
        var result = await _cachingService.GetOrCreateAsync(cacheKey, () =>
        {
          return _amadeusSettings.BaseAuthUrl
          .AppendPathSegment(_amadeusSettings.AuthRoute)
          .SetQueryParams()
          .PostUrlEncodedAsync(new
          {
            grant_type = _amadeusSettings.AuthGrantType,
            client_id = _amadeusSettings.AuthClientId,
            client_secret = _amadeusSettings.AuthClientSecret
          })
          .ReceiveJson<AuthResponse>();
        }, TimeSpan.FromSeconds(_amadeusSettings.AuthAccessTokenTTL), TimeSpan.FromSeconds(_amadeusSettings.AuthAccessTokenTTL));

        if (result.Access_Token.IsNullOrWhiteSpace())
        {
          response.AddError(ValidationMessages.AmadeusApiAuthError.ToError());
          return response;
        }

        var expireDate = DateTime.UtcNow.AddSeconds(int.Parse(result.Expires_In));
        if (DateTime.UtcNow > expireDate)
        {
          await AuthenticateAsync();
        }

        response.SetResult(result);
      }
      catch (Exception ex)
      {
        response.AddError(ValidationMessages.AmadeusApiAuthError.ToError());
        _logger.LogError(ex, ValidationMessages.AmadeusApiAuthError);
      }

      return response;
    }
  }
}
