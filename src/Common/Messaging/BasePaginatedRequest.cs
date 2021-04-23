namespace Common.Messaging
{
  /// <summary>
  /// Base class used by API requests that have pagination
  /// </summary>
  public abstract class BasePaginatedRequest : BaseMessage
  {
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
  }
}
