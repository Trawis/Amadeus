using System;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Common.Helpers
{
  /// <summary>
  /// Memoization helper.
  /// If we have a function (f(x) -> y) it caches y so that f(x) gets evaluated only on the first call and all other calls return just the cached value y.  
  /// It basically caches the function results in a dictionary.  
  /// 
  /// Keep in mind that the memoizer cache lives only as long the instance lives, there are no static variables (i.e. no cache sharing between instances).  
  /// If you MUST have a static cache, make the memoizer instance static (memoizer implementation is thread safe, no need to do any external thread synchronization).  
  /// 
  /// For more details about memoization look at http://www.cs.cornell.edu/courses/cs3110/2011sp/lectures/lec22-memoization/memo.htm
  /// Wikipedia also has a decent enough explanation: https://en.wikipedia.org/wiki/Memoization
  /// </summary>
  [DebuggerStepThrough]
  public class Memoizer
  {
    /// <summary>
    /// <see cref="Memoize{T, U}(T, Func{T, U})"/> cache differs by input/output types (T and U) so we first need a map between (typeof(T),typeof(U)) and the actual values store.  
    /// i.e. a different storage is needed for Memoize&lt;string, int&gt; than it is for Memoize&lt;DateTime, bool&gt;
    /// </summary>
    private ImmutableDictionary<Tuple<Type, Type>, IMemoizedValues> _maps = ImmutableDictionary<Tuple<Type, Type>, IMemoizedValues>.Empty;
    /// <summary>
    /// Memoize a function result. 
    /// Cache key is actually a combination of three things: typeof(T), typeof(U) and the <paramref name="key" /> argument.
    /// Example:
    /// 
    /// var toLower = Func.Create(x => x.ToLower())
    /// var value = memoizer.Memoize("string value", toLower)
    /// var thisComesFromCache = memoizer.Memoize("string value", toLower) /* the same T,U and key */
    /// var thisIsNotAlreadyCached = memoizer.Memoize("another string value", toLower) /* the same T,U and but a different key */
    /// var alsoNotAlreadyCached = memoizer.Memoize("string value", key => key.ToCharArray()) /* U is char[], thus the result is cached separately */
    /// </summary>
    /// <param name="key">A cache key. It gets passed to the <paramref name="computeIfNotInCache"/> function if a cached value cannot be found.</param>
    /// <param name="computeIfNotInCache">The function used to compute the value for a supplied <paramref name="key"/>. x in compute(x) is actually the <paramref name="key"/> param</param>
    public U Memoize<T, U>(T key, Func<T, U> computeIfNotInCache)
    {
      var map = ImmutableInterlocked.GetOrAdd(ref _maps, Tuple.Create(typeof(T), typeof(U)), _ => new MemoizedValues<T, U>()) as MemoizedValues<T, U>;
      return map.Memoize(key, computeIfNotInCache);
    }

    private interface IMemoizedValues { }
    /// <summary>
    /// Actual key->value map.
    /// </summary>
    private class MemoizedValues<T, U> : IMemoizedValues
    {
      private ImmutableDictionary<T, U> _maps = ImmutableDictionary<T, U>.Empty;
      public U Memoize(T key, Func<T, U> computeIfNotInCache)
      {
        return ImmutableInterlocked.GetOrAdd(ref _maps, key, computeIfNotInCache);
      }
    }
  }
}
