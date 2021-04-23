using System;
using Common.Messaging;

namespace Core.Common.Messaging
{
  public class Response<T> : BaseResponse
  {
    public T Result { get; set; }

    public Response()
    {
    }

    public Response(T result, string message = null)
    {
      Result = result;
      Success(message);
    }

    public Response(Guid correlationId) : base(correlationId)
    {
    }

    public Response(T result, Guid correlationId, string message = null) : base(correlationId)
    {
      Result = result;
      Success(message);
    }

    public void SetResult(T result, string message = null)
    {
      Result = result;
      Success(message);
    }
  }
}
