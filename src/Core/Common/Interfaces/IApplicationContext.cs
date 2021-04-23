using System.Threading;
using System.Threading.Tasks;

namespace Core.Common.Interfaces
{
  public interface IApplicationContext
  {
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
  }
}
