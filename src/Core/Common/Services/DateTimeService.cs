using System;
using Common.Interfaces;

namespace Core.Common.Services
{
  public class DateTimeService : IDateTime
  {
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTime Now => DateTime.Now;
  }
}
