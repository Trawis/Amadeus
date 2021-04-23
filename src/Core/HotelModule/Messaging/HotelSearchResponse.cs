using System.Linq;
using AutoMapper;
using Core.Common.Mappings;
using Core.HotelModule.Models;

namespace Core.HotelModule.Messaging
{
  public class HotelSearchResponse : IMapFrom<HotelOffer>
  {
    public string HotelId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Rating { get; set; }
    public string Currency { get; set; }
    public string Total { get; set; }
    public bool Avaliable { get; set; }

    public void Mapping(Profile profile)
    {
      profile.CreateMap<HotelOffer, HotelSearchResponse>()
        .ForMember(x => x.HotelId, opt => opt.MapFrom(y => y.Hotel.HotelId))
        .ForMember(x => x.Name, opt => opt.MapFrom(y => y.Hotel.Name))
        .ForMember(x => x.Description, opt => opt.MapFrom(y => y.Hotel.Description != null ? y.Hotel.Description.Text : string.Empty))
        .ForMember(x => x.Rating, opt => opt.MapFrom(y => y.Hotel.Rating))
        .ForMember(x => x.Currency, opt => opt.MapFrom(y => y.Rooms != null ? y.Rooms.OrderBy(x => x.Price.Total).FirstOrDefault().Price.Currency : string.Empty))
        .ForMember(x => x.Total, opt => opt.MapFrom(y => y.Rooms != null ? y.Rooms.OrderBy(x => x.Price.Total).FirstOrDefault().Price.Total : string.Empty))
        .ForMember(x => x.Avaliable, opt => opt.MapFrom(y => y.Avaliable));
    }
  }
}
