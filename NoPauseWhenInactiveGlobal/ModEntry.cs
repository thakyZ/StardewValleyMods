using System.Diagnostics.CodeAnalysis;

using GenericModConfigMenu;
using NekoBoiNick.Common;
using NekoBoiNick.Common.Extensions;
using NekoBoiNick.Common.Integrations;

using HarmonyLib;

using StardewModdingAPI;
using StardewModdingAPI.Events;
using NoPauseWhenInactiveGlobal.Framework;

namespace NoPauseWhenInactiveGlobal;

/// <inheritdoc />
public class ModEntry : Mod {
#region  Properties
  /// <summary>
  /// This mod's config instance.
  /// </summary>
  internal static ModConfig Config { get; private set; } = null!;

  /// <summary>
  /// The Logging function for the mod.
  /// </summary>
  internal static IMonitor IMonitor { get; private set; } = null!;

  /// <summary>
  /// The SMAPI mod helper instance.
  /// </summary>
  internal static IModHelper ModHelper { get; private set; } = null!;

  /// <summary>
  /// A bool to check if a save game has been loaded or not.
  /// </summary>
  internal static bool IsSaveLoaded { get; private set; } = false;
#endregion

#region Public methods
  /// <inheritdoc />
  public override void Entry(IModHelper modHelper) {
      ModHelper = modHelper;
      IMonitor = this.Monitor;
      I18n.Init(modHelper.Translation);
      Config = ModHelper.ReadConfig<ModConfig>();
      modHelper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
      modHelper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
      modHelper.Events.GameLoop.ReturnedToTitle += this.OnReturnedToTitle;
      // Patch all harmony methods via attributes found in the same namespace or descendants.
      new Harmony("NekoBoiNick.NoPauseWhenInactiveGlobal").PatchAll();
  }
#endregion

#region Private methods
#region Event handlers
  /// <summary>
  /// Sets the property <see cref="IsSaveLoaded"/> to <see cref="false"/>, when returning back to the main menu.
  /// </summary>
  /// <param name="sender">The event sender.</param>
  /// <param name="e">The event arguments.</param>
  private void OnReturnedToTitle(object? sender, ReturnedToTitleEventArgs e) => IsSaveLoaded = false;

  /// <summary>
  /// Sets the property <see cref="IsSaveLoaded"/> to <see cref="true"/>, when loading a save game.
  /// </summary>
  /// <param name="sender">The event sender.</param>
  /// <param name="e">The event arguments.</param>
  private void OnSaveLoaded(object? sender, SaveLoadedEventArgs e) => IsSaveLoaded = true;
#endregion

#region Methods
  /// <summary>
  /// Implements the GenericModConfig options menu if it is loaded into the game.
  /// </summary>
  /// <param name="sender">The event sender.</param>
  /// <param name="e">The event arguments.</param>
  private void OnGameLaunched(object? sender, GameLaunchedEventArgs e) {
    GenericModConfigMenuIntegration.Register(this.ModManifest, this.Helper.ModRegistry, this.Monitor,
      getConfig: () => Config,
      reset: () => Config = new(),
      save: () => this.Helper.WriteConfig(Config),
      titleScreenOnly: true
    );
  }
#endregion
#endregion
}
