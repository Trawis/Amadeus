using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Common.Extensions
{
  public static class EnumerableExtensions
  {
    /// <summary>
    /// Determines if the sequence is null or empty.
    /// </summary>
    [Pure]
    public static bool NullOrEmpty<T>(this IEnumerable<T> source)
    {
      return source == null || !source.Any();
    }

    /// <summary>
    /// When the argument is null, returns an empty sequence of the same type. Otherwise, returns the argument itself.
    /// </summary>
    [Pure]
    public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
    {
      return source ?? Enumerable.Empty<T>();
    }

    /// <summary>
    /// When the argument is null, returns an empty sequence of the same type. Otherwise, returns the argument itself.
    /// </summary>
    [Pure]
    public static IDictionary<T, U> EmptyIfNull<T, U>(this IDictionary<T, U> source)
    {
      return source ?? new Dictionary<T, U>();
    }

    /// <summary>
		/// Casts every element of the given sequence to its corresponding nullable type.
		/// </summary>
		[Pure]
    public static IEnumerable<T?> AsNullable<T>(this IEnumerable<T> source) where T : struct
    {
      Contract.Requires(source != null);
      return source.Select(x => (T?)x);
    }

    /// <summary>
    /// If the dictionary contains the given key, returns the value corresponding to that key. Otherwise, returns default value.
    /// </summary>
    [Pure]
    public static TValue ValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> d, TKey key, TValue defaultValue = default)
    {
      Contract.Requires(d != null);
      return d.TryGetValue(key, out TValue v) ? v : defaultValue;
    }
  }
}
