// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

using StardewModdingAPI.Framework.Logging;

namespace NekoBoiNick.Common;
public class Logger {
  private readonly IMonitor _monitor;

  public Logger(IMonitor monitor) {
    this._monitor = monitor;
  }

  /// <inheritdoc cref="IMonitor.IsVerbose"/>
  public bool IsVerbose => this._monitor.IsVerbose;

  /// <inheritdoc cref="IMonitor.Log(string, LogLevel)"/>
  public void Alert(string message) => this._monitor.Log(message, LogLevel.Alert);

  /// <inheritdoc cref="IMonitor.LogOnce(string, LogLevel)"/>
  public void AlertOnce(string message) => this._monitor.LogOnce(message, LogLevel.Alert);

  /// <inheritdoc cref="IMonitor.Log(string, LogLevel)"/>
  public void Error(string message) => this._monitor.Log(message, LogLevel.Error);

  /// <inheritdoc cref="IMonitor.LogOnce(string, LogLevel)"/>
  public void ErrorOnce(string message) => this._monitor.LogOnce(message, LogLevel.Error);

  /// <inheritdoc cref="IMonitor.Log(string, LogLevel)"/>
  public void Exception(Exception exception, string? message) => this._monitor.Log((message is not null ? message + "\n" : "") + exception.GetFullyQualifiedExceptionText(), LogLevel.Error);

  /// <inheritdoc cref="IMonitor.LogOnce(string, LogLevel)"/>
  public void ExceptionOnce(Exception exception, string? message) => this._monitor.LogOnce((message is not null ? message + "\n" : "") + exception.GetFullyQualifiedExceptionText(), LogLevel.Error);

  /// <inheritdoc cref="IMonitor.Log(string, LogLevel)"/>
  public void Warn(string message) => this._monitor.Log(message, LogLevel.Warn);

  /// <inheritdoc cref="IMonitor.LogOnce(string, LogLevel)"/>
  public void WarnOnce(string message) => this._monitor.LogOnce(message, LogLevel.Warn);

  /// <inheritdoc cref="IMonitor.Log(string, LogLevel)"/>
  public void Info(string message) => this._monitor.Log(message, LogLevel.Info);

  /// <inheritdoc cref="IMonitor.LogOnce(string, LogLevel)"/>
  public void InfoOnce(string message) => this._monitor.LogOnce(message, LogLevel.Info);

  /// <inheritdoc cref="IMonitor.Log(string, LogLevel)"/>
  public void Debug(string message) => this._monitor.Log(message, LogLevel.Debug);

  /// <inheritdoc cref="IMonitor.LogOnce(string, LogLevel)"/>
  public void DebugOnce(string message) => this._monitor.LogOnce(message, LogLevel.Debug);

  // ReSharper disable once RedundantArgumentDefaultValue
  /// <inheritdoc cref="IMonitor.Log(string, LogLevel)"/>
  public void Trace(string message) => this._monitor.Log(message, LogLevel.Trace);

  // ReSharper disable once RedundantArgumentDefaultValue
  /// <inheritdoc cref="IMonitor.LogOnce(string, LogLevel)"/>
  public void TraceOnce(string message) => this._monitor.LogOnce(message, LogLevel.Trace);

  /// <inheritdoc cref="IMonitor.VerboseLog(string)"/>
  public void Verbose(string message) => this._monitor.VerboseLog(message);

  /// <inheritdoc cref="IMonitor.VerboseLog(ref VerboseLogStringHandler)"/>
  public void Verbose(ref VerboseLogStringHandler message) => this._monitor.VerboseLog(ref message);
}
