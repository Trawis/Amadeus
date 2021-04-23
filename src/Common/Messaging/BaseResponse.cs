using System;
using System.Collections.Generic;
using Common.Models;

namespace Common.Messaging
{
  /// <summary>
  /// Base class used by API responses
  /// </summary>
  public abstract class BaseResponse : BaseMessage
  {
    public bool Succeeded { get; set; } = false;
    public string Message { get; set; }
    public List<Error> Errors { get; set; }
    public DateTime ServerTime { get; set; }

    public new Guid CorrelationId => _correlationId;

    public BaseResponse()
    {
    }

    public BaseResponse(Guid correlationId) : base()
    {
      base._correlationId = correlationId;
    }

    public void AddError(Error error, string message = "One or more errors occurred!")
    {
      if (Errors == null)
      {
        Errors = new List<Error>();
        Failure(message);
      }

      Errors.Add(error);
    }

    public void AddErrors(List<Error> errors, string message = "One or more errors occurred!")
    {
      Failure(message);
      Errors = errors;
    }

    public void Success(string message = null)
    {
      Succeeded = true;
      Message = message;
      Errors = null;
      ServerTime = DateTime.Now;
    }

    public void Failure(string message = "One or more errors occurred!")
    {
      Succeeded = false;
      Message = message;
      ServerTime = DateTime.Now;
    }
  }
}
