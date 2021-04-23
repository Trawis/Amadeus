using System;
using Common.Messaging;

namespace Core.Common.Messaging
{
  public class EmptyResponse : BaseResponse
  {
    public EmptyResponse()
    {
    }

    public EmptyResponse(Guid correlationId) : base(correlationId)
    {
    }
  }
}
