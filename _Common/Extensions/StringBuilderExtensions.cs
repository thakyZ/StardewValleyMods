using System.Text;

namespace NekoBoiNick.Common.Extensions;

/// <summary>
/// Extensions for <see cref="StringBuilder"/>s to simplify code.
/// </summary>
internal static class StringBuilderExtensions {
  /// <summary>
  /// An unused method to encode <see cref="Type"/>s and values into a <see langword="string"/>.
  /// </summary>
  /// <typeparam name="TSource">A generic type to encode.</typeparam>
  /// <param name="sb">The instance of a <see cref="StringBuilder"/>.</param>
  /// <param name="item">The value to encode.</param>
  /// <returns>Instance of the <see cref="StringBuilder"/> that contains the encoded value.</returns>
  internal static StringBuilder AppendSimple<TSource>(this StringBuilder sb, TSource? item) {
    if (item is null) {
      sb.Append("null");
    } else if (item is bool bValue) {
      sb.Append(bValue ? "1" : "0");
    } else if (item is int iValue) {
      sb.Append($"i{iValue}");
    } else if (item is long lValue) {
      sb.Append($"l{lValue}");
    } else if (item is double dValue) {
      sb.Append($"d{dValue}");
    } else if (item is float fValue) {
      sb.Append($"f{fValue}");
    } else if (item is string sType) {
      sb.Append($"s{sType}");
    } else if (item is Enum eType) {
      sb.Append($"e{Enum.GetName(typeof(TSource), eType)}");
    } else {
      sb.Append($"o{item}");
    }

    return sb;
  }
}
