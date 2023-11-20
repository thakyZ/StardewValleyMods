namespace NoPauseWhenInactiveGlobal;

/// <summary>
/// The mod's config class.
/// </summary>
internal class ModConfig
{
    /// <summary>
    /// An option whether or not to disable the game from pausing when outside a save game.
    /// Enabled by default as it is normally wanted if one has this mod.
    /// </summary>
    public bool DisableGamePause { get; set; } = true;
}
