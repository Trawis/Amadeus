using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Interfaces
{
  /// <summary>
  /// Caching Service
  /// </summary>
  public interface ICachingService
  {
    /// <summary>
    /// Gets and/or Creates a cached item.
    /// ONLY FOR INSTANCES OF A CLASS, EXCLUDING STRING!
    /// </summary>
    /// <typeparam name="T">Type of cached item value.</typeparam>
    /// <param name="key">Key for cached item.</param>
    /// <param name="factory">Factory method to create cached item value, if not in cache.</param>
    /// <param name="memoryCacheExpiration">Expiration of in-memory cached item. Recommended to be smaller TimeSpan to reduce memory pressure.</param>
    /// <param name="distributedCacheExpiration">Expiration of cached item on distributed cache. Can be some bigger TimeSpan.</param>
    /// <returns>Cached value for sent key.</returns>
    Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan memoryCacheExpiration, TimeSpan? distributedCacheExpiration = null, CancellationToken cancellationToken = default) where T : class;
    /// <summary>
    /// Gets and/or Creates a cached string item.
    /// </summary>        
    /// <param name="key">Key for cached item.</param>
    /// <param name="factory">Factory method to create cached item value, if not in cache.</param>
    /// <param name="memoryCacheExpiration">Expiration of in-memory cached item. Recommended to be smaller TimeSpan to reduce memory pressure.</param>
    /// <param name="distributedCacheExpiration">Expiration of cached item on distributed cache. Can be some bigger TimeSpan.</param>
    /// <returns>Cached value for sent key.</returns>
    Task<string> GetOrCreateStringAsync(string key, Func<Task<string>> factory, TimeSpan memoryCacheExpiration, TimeSpan? distributedCacheExpiration = null, CancellationToken cancellationToken = default);
    /// <summary>
    /// Invalidates cache.
    /// </summary>        
    /// <param name="key">Key for cached item.</param>
    Task InvalidateCacheAsync(string key, CancellationToken cancellationToken = default);
  }
}
