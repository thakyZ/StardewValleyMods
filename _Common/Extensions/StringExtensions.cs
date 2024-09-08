using System.Diagnostics.CodeAnalysis;

namespace NekoBoiNick.Common.Extensions;

/// <summary>
/// Extensions for <see langword="string"/>s to simplify code.
/// </summary>
internal static class StringExtensions {
  /// <summary>
  /// Tests if the provided string in null or an empty string or white space.
  /// </summary>
  /// <param name="string">The input string.</param>
  /// <returns>True if not null or an empty string or white space.</returns>
  [SuppressMessage("ReSharper", "RedundantLinebreak")]
  [SuppressMessage("ReSharper", "ArrangeMethodOrOperatorBody")]
  internal static bool IsNullOrEmptyOrWhiteSpace([NotNullWhen(false)] this string? @string)
    => string.IsNullOrWhiteSpace(@string) || string.IsNullOrEmpty(@string);

  [SuppressMessage("ReSharper", "RedundantLinebreak")]
  [SuppressMessage("ReSharper", "ArrangeMethodOrOperatorBody")]
  internal static bool ContainsAny(this string? @string, string[] values)
    => @string.ContainsAny(values, StringComparison.CurrentCulture);

  // ReSharper disable once MemberCanBePrivate.Global # Should be accessible if necessary
  internal static bool ContainsAny(this string? @string, string[] values, StringComparison comparisonType) {
    if (@string is null) return false;

    return Array.Exists(values, (string value) => {
      // ReSharper disable once InvertIf # Don't invert if otherwise it'll contain too much redundant code.
      if (comparisonType.IsStringComparisonIgnoreCase()) {
        @string = @string.ToLower();
        value = value.ToLower();
      }

      return @string.Contains(value);
    });
  }
}
