using System;
using Common.Messaging;
using FluentValidation;

namespace Core.HotelModule.Messaging
{
  public class HotelSearchRequest : BaseRequest
  {
    public string CityCode { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime? CheckOutDate { get; set; }
  }

  public class HotelSearchRequestValidator : AbstractValidator<HotelSearchRequest>
  {
    public HotelSearchRequestValidator()
    {
      RuleFor(x => x.CityCode)
        .NotEmpty();
      RuleFor(x => x.CheckInDate)
        .NotEmpty()
        .GreaterThan(DateTime.UtcNow);
      When(x => x.CheckOutDate.HasValue, () =>
      {
        RuleFor(y => y.CheckOutDate)
        .GreaterThan(y => y.CheckInDate)
        .GreaterThan(DateTime.UtcNow);
      });
    }
  }
}
