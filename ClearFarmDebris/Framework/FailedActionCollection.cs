using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ClearFarmDebris.Framework;
internal class FailedActionCollection : IEnumerable<FailedAction> {
  public HashSet<FailedAction> Values = new(EqualityComparer<FailedAction>.Comparer);

  public int Count => this.Values.Count;
  public bool IsNotEmpty => this.Values.Count > 0;

  public bool Add(FailedAction entity) => !this.Values.Contains(entity) && this.Values.Add(entity);

  public void Clear() => this.Values.Clear();

  public IEnumerator<FailedAction> GetEnumerator() => this.Values.GetEnumerator();
  IEnumerator IEnumerable.GetEnumerator() => this.Values.GetEnumerator();
  public void Log(IMonitor monitor) {
    foreach (FailedAction failedAction in this.Values) {
      monitor.LogOnce(failedAction.ToString(), LogLevel.Alert);
      if (failedAction.Exception is not null) {
        monitor.LogOnce(failedAction.Exception.GetFullyQualifiedExceptionText(), LogLevel.Error);
      }
    }
  }
}
