using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Common.Helpers;

namespace Common.Extensions
{
  public static class Enums
  {
    public static IEnumerable<T> All<T>() where T : struct
    {
      return Enum.GetValues(typeof(T)).Cast<T>();
    }

    public static Dictionary<string, T> ToDictionary<T>(this Enum enumeration)
    {
      Array values = Enum.GetValues(enumeration.GetType());
      var dictionary = new Dictionary<string, T>();
      string name;

      foreach (var val in values)
      {
        name = Enum.GetName(enumeration.GetType(), val);
        dictionary.Add(name, (T)val);
      }

      return dictionary;
    }

    public static string GetName(this Enum source)
    {
      return Enum.GetName(source.GetType(), source);
    }

    public static string GetDisplayName<T>(this T x) where T : struct { return GetNames<T>()[x]; }

    public static string GetDescription(this Enum source)
    {
      var enumType = source.GetType();
      var field = enumType.GetField(source.ToString());
      var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

      return attributes.Length == 0 ? source.ToString() : ((DescriptionAttribute)attributes[0]).Description;
    }

    private static readonly Memoizer _memoizer = new Memoizer();
    private static IDictionary<T, string> GetNames<T>()
    {
      return _memoizer.Memoize(typeof(T), _ =>
      {
        var mvs = from m in typeof(T).GetFields()
                  where m.IsStatic
                  let a = m.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() as DisplayAttribute
                  let name = a == null ? m.Name : a.Name
                  select new { name, v = (T)m.GetValue(null) };
        return mvs.ToDictionary(x => x.v, x => x.name);
      });
    }
  }
}
