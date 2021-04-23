using System;
using System.Collections.Generic;
using Common.Messaging;

namespace Core.Common.Messaging
{
  public class ListResponse<T> : BaseResponse
  {
    public IEnumerable<T> Result { get; set; }

    public ListResponse()
    {
    }

    public ListResponse(IEnumerable<T> result, string message = null)
    {
      Result = result;
      Success(message);
    }

    public ListResponse(Guid correlationId) : base(correlationId)
    {
    }

    public ListResponse(IEnumerable<T> result, Guid correlationId, string message = null) : base(correlationId)
    {
      Result = result;
      Success(message);
    }

    public void SetResult(IEnumerable<T> result, string message = null)
    {
      Result = result;
      Success(message);
    }
  }
}
