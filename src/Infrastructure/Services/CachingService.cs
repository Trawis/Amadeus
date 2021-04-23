using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Extensions;
using Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Services
{
  public class CachingService : ICachingService
  {
    private readonly IAppLogger<CachingService> _logger;
    private readonly IMemoryCache _memoryCache;
    private readonly IDistributedCache _distributedCache;

    public CachingService(
      IAppLogger<CachingService> logger,
      IMemoryCache memoryCache,
      IDistributedCache distributedCache
      )
    {
      _logger = logger;
      _memoryCache = memoryCache;
      _distributedCache = distributedCache;
    }

    public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan memoryCacheExpiration, TimeSpan? distributedCacheExpiration = null, CancellationToken cancellationToken = default) where T : class
    {
      if (typeof(T) == typeof(string))
      {
        throw new NotSupportedException("This method does not support 'string' type. Please use GetOrCreateStringAsync method instead.");
      }

      return await GetOrCreateAsync(new JsonConverter<T>(), key, factory, memoryCacheExpiration, distributedCacheExpiration, cancellationToken);
    }

    public async Task<string> GetOrCreateStringAsync(string key, Func<Task<string>> factory, TimeSpan memoryCacheExpiration, TimeSpan? distributedCacheExpiration = null, CancellationToken cancellationToken = default)
    {
      return await GetOrCreateAsync(new StringConverter(), key, factory, memoryCacheExpiration, distributedCacheExpiration, cancellationToken);
    }

    public async Task InvalidateCacheAsync(string key, CancellationToken cancellationToken = default)
    {
      _memoryCache.Remove(key);
      await _distributedCache.RemoveAsync(key, cancellationToken);

      // TODO: Notify all instances that the item was removed from cache so they can also drop local memory cache
    }

    private async Task<T> GetOrCreateAsync<T>(IConverter<T> converter, string key, Func<Task<T>> factory, TimeSpan memoryCacheExpiration, TimeSpan? distributedCacheExpiration, CancellationToken cancellationToken = default)
    {
      return await _memoryCache.GetOrCreateAsync(key, entry =>
      {
        TimeSpan calculatedDistributedCacheExpiration = distributedCacheExpiration ?? memoryCacheExpiration;
        entry.AbsoluteExpiration = DateTime.UtcNow.Add(memoryCacheExpiration);
        return GetOrCreateFromDistributedCacheAsync(converter, key, factory, calculatedDistributedCacheExpiration, cancellationToken);
      });
    }

    private async Task<T> GetOrCreateFromDistributedCacheAsync<T>(IConverter<T> converter, string key, Func<Task<T>> factory, TimeSpan distributedCacheExpiration, CancellationToken cancellationToken = default)
    {
      _logger.LogInformation("Getting cached value from distributed cache for key {Key}", key);

      var cachedItem = await _distributedCache.GetStringAsync(key, cancellationToken);
      if (cachedItem != null)
      {
        _logger.LogInformation("Read cached value from distributed cache for key {Key}", key);
        var value = converter.Deserialize(cachedItem);
        return value;
      }

      var item = await factory.Invoke();
      if (item != null)
      {
        var cacheEntryOptions = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = distributedCacheExpiration };
        var serializedValue = converter.Serialize(item);
        await _distributedCache.SetStringAsync(key, serializedValue, cacheEntryOptions, cancellationToken);
        _logger.LogInformation("Stored in distributed cache for key {Key}", key);
      }

      return item;
    }

    #region Helpers
    private interface IConverter<T>
    {
      string Serialize(object obj);

      T Deserialize(string value);
    }

    private class StringConverter : IConverter<string>
    {
      public string Deserialize(string value) => value;

      public string Serialize(object obj) => obj.ToString();
    }

    private class JsonConverter<T> : IConverter<T>
    {
      public T Deserialize(string value)
      {
        T result = default;

        if (!value.IsNullOrWhiteSpace())
        {
          result = value.FromJson<T>();
        }

        return result;
      }

      public string Serialize(object obj) => obj.ToJson();
    }
    #endregion
  }
}
