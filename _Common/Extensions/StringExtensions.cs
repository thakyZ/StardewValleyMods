using System.Diagnostics.CodeAnalysis;

namespace NekoBoiNick.Common.Extensions;

/// <summary>
/// Extensions for <see langword="string"/>s to simplify code.
/// </summary>
internal static class StringExtensions {
  /// <summary>
  /// Tests if the provided string in null or an empty string or white space.
  /// </summary>
  /// <param name="value">The input string.</param>
  /// <returns>True if not null or an empty string or white space.</returns>
  internal static bool IsNullOrEmptyOrWhiteSpace([NotNullWhen(false)] this string? value)
    => string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(value);
}
