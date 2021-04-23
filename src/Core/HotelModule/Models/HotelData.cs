using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Core.HotelModule.Models
{
  public class HotelData
  {
    public List<HotelOffer> Data { get; set; }
  }

  public class HotelOffer
  {
    public Hotel Hotel { get; set; }
    public bool Avaliable { get; set; }

    [JsonPropertyName("offers")]
    public List<HotelRoom> Rooms { get; set; }
  }

  public class Hotel
  {
    public string HotelId { get; set; }
    public string Rating { get; set; }
    public string Name { get; set; }
    public HotelDescription Description { get; set; }
  }

  public class HotelDescription
  {
    public string Lang { get; set; }
    public string Text { get; set; }
  }

  public class HotelRoom
  {
    public string Id { get; set; }
    public HotelPrice Price { get; set; }
  }

  public class HotelPrice
  {
    public string Currency { get; set; }
    public string Total { get; set; }
  }
}
