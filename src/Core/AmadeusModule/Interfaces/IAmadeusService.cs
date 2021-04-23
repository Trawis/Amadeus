using System.Threading.Tasks;
using Core.AmadeusModule.Messaging;
using Core.Common.Messaging;

namespace Core.AmadeusModule.Interfaces
{
  public interface IAmadeusService
  {
    Task<Response<AuthResponse>> AuthenticateAsync();
  }
}
