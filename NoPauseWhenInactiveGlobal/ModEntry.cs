using System.Diagnostics.CodeAnalysis;

using HarmonyLib;

using StardewModdingAPI.Events;
using NoPauseWhenInactiveGlobal.Framework;

namespace NoPauseWhenInactiveGlobal;

/// <inheritdoc />
public class ModEntry : Mod {
  #region Properties
  /// <summary>
  /// Instance of this mod for static methods.
  /// </summary>
  private static ModEntry _instance { get; set; } = null!;

  /// <summary>
  /// This mod's config instance.
  /// </summary>
  private ModConfig config = null!;

  /// <summary>
  /// This mod's config instance.
  /// </summary>
  internal static ModConfig Config => _instance.config;

  /// <summary>
  /// The Logging function for the mod.
  /// </summary>
  internal static Logger Logger => new(_instance.Monitor);

  /// <summary>
  /// The SMAPI mod helper instance.
  /// </summary>
  internal static IModHelper ModHelper => _instance.Helper;

  /// <summary>
  /// A bool to check if a save game has been loaded or not.
  /// </summary>
  internal static bool IsSaveLoaded { get; private set; } = false;
#endregion

  public ModEntry() {
    _instance = this;
  }

#region Public methods
  /// <inheritdoc />
  public override void Entry(IModHelper helper) {
    I18n.Init(helper.Translation);
    this.config                            =  ModHelper.ReadConfig<ModConfig>();
    helper.Events.GameLoop.GameLaunched    += this.OnGameLaunched;
    helper.Events.GameLoop.SaveLoaded      += this.OnSaveLoaded;
    helper.Events.GameLoop.ReturnedToTitle += this.OnReturnedToTitle;

    // Patch all harmony methods via attributes found in the same namespace or descendants.
    new Harmony("NekoBoiNick.NoPauseWhenInactiveGlobal").PatchAll();
  }
#endregion

#region Private methods
#region Event handlers
  /// <summary>
  /// Sets the property <see cref="IsSaveLoaded"/> to <see langword="false"/>, when returning back to the main menu.
  /// </summary>
  /// <param name="sender">The event sender.</param>
  /// <param name="e">The event arguments.</param>
  private void OnReturnedToTitle(object? sender, ReturnedToTitleEventArgs e) => IsSaveLoaded = false;

  /// <summary>
  /// Sets the property <see cref="IsSaveLoaded"/> to <see langword="true"/>, when loading a save game.
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
    // ReSharper disable once ArrangeMethodOrOperatorBody
    GenericModConfigMenuIntegration.Register(
      this.ModManifest, this.Helper.ModRegistry, this.Monitor,
      getConfig: () => this.config,
      reset: () => this.config = new(),
      save: () => this.Helper.WriteConfig(this.config),
      titleScreenOnly: true);
  }
#endregion
#endregion
}
