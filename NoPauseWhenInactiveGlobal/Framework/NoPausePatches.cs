using System.Collections.Generic;
using System.Linq;

using HarmonyLib;

using StardewValley.SDKs;

namespace NoPauseWhenInactiveGlobal.Framework;
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
/// <summary>
/// Patch the <see cref="InstanceGame.IsActive"/> property getter method.
/// </summary>
[HarmonyPatch(typeof(InstanceGame), "get_" + nameof(InstanceGame.IsActive))]
[HarmonyDebug]
public static class InstanceGame_IsActive_Patch {
  /// <summary>
  /// A generic Prefix method.
  /// </summary>
  /// <param name="__instance">The instance of <see cref="InstanceGame"/></param>
  /// <param name="__result">The resulting boolean value of the getter.</param>
  /// <returns>A boolean whether or not the game is active.</returns>
  public static bool Prefix(InstanceGame __instance, ref bool __result)  {
    if (!ModEntry.Config.DisableGamePause || ModEntry.IsSaveLoaded) return true;

    __result = true;

    return false;
  }
}

/// <summary>
/// Patch the <see cref="Game1.IsActiveNoOverlay"/> property getter method.
/// </summary>
[HarmonyPatch(typeof(Game1), "get_" + nameof(Game1.IsActiveNoOverlay))]
[HarmonyDebug]
public static class Game1_IsActiveNoOverlay_Patch {
  /// <summary>
  /// The field name to fetch from the <see cref="StardewValley.Program"/> class
  /// </summary>
  private static readonly IReadOnlyList<string> _fieldName = ["_sdk", "sdk"];

  private static SDKHelper? GetProgramSDK() {
    SDKHelper? output = null;

    // Program.sdk or Program._sdk is an internal labeled function so we must use HarmonyLib.Traverse to get it's value.
    try {
      Traverse  program    = Traverse.Create(typeof(Program));
      string?   foundField = null;
      bool?     isField    = null;

      foreach (var name in _fieldName) {
        foundField = program.Fields().Find(x => x.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (foundField is not null) {
          isField = true;

          break;
        }

        foundField = program.Properties().Find(x => x.Equals(name, StringComparison.OrdinalIgnoreCase));

        // ReSharper disable once InvertIf
        if (foundField is not null) {
          isField = false;

          break;
        }
      }

      if (foundField is not null && isField is not null) {
        var fieldProp = isField.Value ? program.Field(foundField) : program.Property(foundField);
        output    = fieldProp.GetValue<SDKHelper>();
      }
    } catch (Exception exception) {
      ModEntry.Logger.ExceptionOnce(exception, $"Failed to find {nameof(SDKHelper)} field/property in class {nameof(Program)}");
    }

    return output;
  }

  /// <summary>
  /// A generic Prefix method.
  /// </summary>
  /// <param name="__instance">The instance of <see cref="Game1"/></param>
  /// <param name="__result">The resulting boolean value of the getter.</param>
  /// <returns>A boolean whether or not the game is active.</returns>
  public static bool Prefix(Game1 __instance, ref bool __result) {
    var Program_sdk = GetProgramSDK();

    if (Program_sdk is null) {
      ModEntry.Logger.WarnOnce($"Unable to find property with name \"{_fieldName}\" and type of {nameof(SDKHelper)} in class {nameof(Program)}");

      return true;
    }

    if (ModEntry.Config.DisableGamePause && !ModEntry.IsSaveLoaded) {
      __result = true;

      return false;
    }

    return true;
  }
}
// ReSharper enable InconsistentNaming
// ReSharper enable UnusedMember.Global
