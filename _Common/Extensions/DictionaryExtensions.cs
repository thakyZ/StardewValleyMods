using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace NekoBoiNick.Common.Extensions;

/// <summary>
/// Extensions for <see cref="System.Collections.IDictionary"/>-like types to simplify code.
/// </summary>
internal static class DictionaryExtensions {
  [SuppressMessage("ReSharper", "RedundantLinebreak")]
  public static IEnumerable<(TKey Key, TValue Value)> Iterate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary) where TKey : notnull
    => dictionary.Select((KeyValuePair<TKey, TValue> keyValuePair) => (keyValuePair.Key, keyValuePair.Value));

  [SuppressMessage("ReSharper", "RedundantLinebreak")]
  public static IEnumerable<(TKey Key, TValue Value)> Iterate<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> dictionary) where TKey : notnull
    => dictionary.Select((KeyValuePair<TKey, TValue> keyValuePair) => (keyValuePair.Key, keyValuePair.Value));
}
