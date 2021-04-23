using System;

namespace Common.Interfaces
{
  public interface IDateTime
  {
    DateTime UtcNow { get; }
    DateTime Now { get; }
  }
}
