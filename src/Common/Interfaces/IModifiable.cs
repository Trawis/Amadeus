using System;

namespace Common.Interfaces
{
  public interface IModifiable<T>
  {
    public DateTime? ModifiedAt { get; set; }
  }
}
