using Microsoft.Xna.Framework;

using System.Runtime.CompilerServices;
using System.Text;

namespace ClearFarmDebris.Framework;
internal class FailedAction {
  public string Method { get; private set; }
  public string Action { get; private set; }
  private static Vector2 DefaultPosition = Vector2.Negate(Vector2.One);
  public Vector2 Position { get; private set; } = DefaultPosition;
  public bool HardFail { get; private set; } = false;
  public Exception? Exception { get; private set; } = null;
  public string ID => $"{this.Method} {this.Position} {this.HardFail}";
  public FailedAction(string action, [CallerMemberName] string method = "", Vector2? position = null, bool hard = false, Exception? exception = null) {
    this.Method = method;
    this.Action = action;

    if (position is not null && position.Value != DefaultPosition)
      this.Position = position.Value;

    this.HardFail = hard;

    if (exception is not null)
      this.Exception = exception;
  }
  public override string ToString() {
    var sb = new StringBuilder();

    if (this.HardFail) {
      sb.Append("[hard] ");
    }

    sb.Append(this.Method)
      .Append(' ')
      .Append(this.Action)
      .Append(' ');

    if (this.Position != DefaultPosition) {
      sb.Append(this.Position.ToString());
    }

    return sb.ToString();
  }
  public bool Equals(FailedAction? failedAction) => failedAction is not null && this.ID.Equals(failedAction.ID, StringComparison.OrdinalIgnoreCase);
  public override bool Equals(object? obj) => obj is not null && obj is FailedAction failedAction && this.Equals(failedAction: failedAction);
  public override int GetHashCode() => HashCode.Combine(this.ID.GetHashCode(StringComparison.OrdinalIgnoreCase));
}
