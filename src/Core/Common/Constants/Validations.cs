namespace Core.Common.Constants
{
  public static class ValidationMessages
  {
    public const string Success = "Success.";

    // Amadeus service
    public const string AmadeusApiAuthError = "An error has occurred on authenticating amadeus api user.";

    // Hotel service
    public const string HotelsNotFoundError = "No hotels found by requested search parameters.";
    public const string HotelsSearchError = "An error has occurred on hotel search.";
  }
}
