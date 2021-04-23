namespace Core.Common.Configuration
{
  public class AmadeusSettings
  {
    public string BaseAuthUrl { get; set; }
    public string AuthRoute { get; set; }
    public string AuthGrantType { get; set; }
    public string AuthClientId { get; set; }
    public string AuthClientSecret { get; set; }
    public int AuthAccessTokenTTL { get; set; }

    public string BaseUrl { get; set; }
    public string HotelOffersRoute { get; set; }
  }
}
