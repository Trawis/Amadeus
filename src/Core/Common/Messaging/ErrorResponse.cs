using System;
using Common.Messaging;

namespace Core.Common.Messaging
{
  public class ErrorResponse : BaseResponse
  {
    public ErrorResponse()
    {
    }

    public ErrorResponse(Guid correlationId) : base(correlationId)
    {
    }
  }
}
