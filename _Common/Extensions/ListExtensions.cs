using System.Collections.Generic;

using Microsoft.CodeAnalysis.Operations;

namespace NekoBoiNick.Common.Extensions;

internal static class ListExtensions {
  public static bool AddUniqueNullable<T>(this List<T?> list, T? item) where T : class {
    if (!list.Contains(item) && !list.Exists((T? x) => (x is null && item is null) || x?.Equals(item) == true)) {
      list.Add(item);
      return true;
    }

    return false;
  }

  public static bool AddUnique<T>(this List<T> list, T item) where T : notnull {
    if (!list.Contains(item) && !list.Exists((T x) => x.Equals(item))) {
      list.Add(item);
      return true;
    }

    return false;
  }
}
