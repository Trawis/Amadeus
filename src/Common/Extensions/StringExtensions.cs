using System;
using System.Globalization;

namespace Common.Extensions
{
  public static class StringExtensions
  {
    public static int? ToIntOrNull(this string s)
    {
      return s == null ? null :
          int.TryParse(s, out int parsed) ? (int?)parsed : null;
    }

    public static long? ToLongOrNull(this string s)
    {
      return s == null
        ? null
        : long.TryParse(s, out long parsed)
          ? (long?)parsed
          : null;
    }

    public static short? ToShortOrNull(this string s)
    {
      return s == null ? null :
        short.TryParse(s, out short parsed) ? (short?)parsed : null;
    }

    public static byte? ToByteOrNull(this string s)
    {
      return s == null ? null :
        byte.TryParse(s, out byte parsed) ? (byte?)parsed : null;
    }

    public static Guid? ToGuidOrNull(this string s)
    {
      return s == null ? null :
        Guid.TryParse(s, out Guid parsed) ? parsed : (Guid?)null;
    }

    public static double? ToDoubleOrNull(this string s)
    {
      return s == null ? null :
        double.TryParse(s, out double parsed) ? (double?)parsed : null;
    }

    public static double ToDoubleOrZero(this string s)
    {
      return s == null ? 0 : double.TryParse(s, out double parsed) ? (double)parsed : 0;
    }

    public static decimal? ToDecimalOrNull(this string s)
    {
      return s == null ? null :
        decimal.TryParse(s, out decimal parsed) ? (decimal?)parsed : null;
    }

    public static DateTime? ToDateTimeOrNull(this string s, CultureInfo cultureInfo = null)
    {
      var style = DateTimeStyles.AssumeLocal;
      return s == null ? null :
        DateTime.TryParse(s, cultureInfo ?? CultureInfo.InvariantCulture, style, out DateTime parsed) ? (DateTime?)parsed : null;
    }

    public static bool IsNullOrWhiteSpace(this string s) => string.IsNullOrWhiteSpace(s);
  }
}
