using Common.Models;

namespace Common.Extensions
{
  public static class ErrorExtensions
  {
    public static Error ToError(this string message) => new Error { Message = message };

    public static Error AddCode(this Error error, string code)
    {
      error.Code = code;
      return error;
    }

    public static Error AddType(this Error error, string type)
    {
      error.Type = type;
      return error;
    }

    public static Error AddSource(this Error error, string source)
    {
      error.Source = source;
      return error;
    }

    public static Error AddMessage(this Error error, string message)
    {
      error.Message = message;
      return error;
    }
  }
}
