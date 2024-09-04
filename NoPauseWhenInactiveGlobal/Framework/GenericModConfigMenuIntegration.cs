namespace NoPauseWhenInactiveGlobal.Framework;

/// <summary>
/// Configures the integration with Generic Mod Config Menu.
/// </summary>
internal static class GenericModConfigMenuIntegration
{
#region Public methods
  /// <summary>
  /// Add a config UI to Generic Mod Config Menu if it's installed.
  /// </summary>
  /// <param name="manifest">The mod manifest.</param>
  /// <param name="modRegistry">The mod registry from which to get the API.</param>
  /// <param name="monitor">The monitor with which to log errors.</param>
  /// <param name="getConfig">Get the current mod configuration.</param>
  /// <param name="reset">Reset the config to its default values.</param>
  /// <param name="save">Save the current config to the <c>config.json</c> file.</param>
  public static void Register(IManifest manifest, IModRegistry modRegistry, IMonitor monitor, Func<ModConfig> getConfig, Action reset, Action save, bool titleScreenOnly = false)
  {
    // Get API
    IGenericModConfigMenuApi? api = IntegrationHelper.GetGenericModConfigMenu(modRegistry, monitor);
    if (api is null) return;

    // Register config UI
    api.Register(manifest, reset, save, titleScreenOnly);

    // General options
    // Add the config option to disable pausing of the game outside of a save.
    api.AddBoolOption(
      mod: manifest,
      name: I18n.Config_Global_Name,
      tooltip: I18n.Config_Global_Desc,
      getValue: () => getConfig().DisableGamePause,
      setValue: value => getConfig().DisableGamePause = value
    );
  }
#endregion
}
