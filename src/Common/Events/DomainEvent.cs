using System;
using System.Collections.Generic;

namespace Common.Events
{
  public interface IHasDomainEvent
  {
    public List<DomainEvent> DomainEvents { get; set; }
  }

  public abstract class DomainEvent
  {
    protected DomainEvent()
    {
      DateOccurred = DateTimeOffset.UtcNow;
    }

    public DateTimeOffset DateOccurred { get; protected set; } = DateTime.UtcNow;
  }
}
