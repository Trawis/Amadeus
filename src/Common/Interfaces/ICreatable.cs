using System;

namespace Common.Interfaces
{
  public interface ICreatable<T>
  {
    public DateTime CreatedAt { get; set; }
  }
}
