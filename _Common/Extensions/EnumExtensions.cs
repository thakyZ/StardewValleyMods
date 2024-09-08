using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace NekoBoiNick.Common.Extensions;

/// <summary>
/// Extensions for various or all <see langword="Enum"/>s to simplify code.
/// </summary>
internal static class EnumExtensions {
  [SuppressMessage("ReSharper", "RedundantLinebreak")]
  [SuppressMessage("ReSharper", "ArrangeMethodOrOperatorBody")]
  public static bool IsStringComparisonIgnoreCase(this StringComparison comparisonType)
    => comparisonType is StringComparison.CurrentCultureIgnoreCase or StringComparison.InvariantCultureIgnoreCase or StringComparison.OrdinalIgnoreCase;
}
