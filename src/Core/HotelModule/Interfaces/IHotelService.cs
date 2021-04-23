using System.Threading.Tasks;
using Core.Common.Messaging;
using Core.HotelModule.Messaging;

namespace Core.HotelModule.Interfaces
{
  public interface IHotelService
  {
    Task<ListResponse<HotelSearchResponse>> SearchAsync(HotelSearchRequest request);
  }
}
