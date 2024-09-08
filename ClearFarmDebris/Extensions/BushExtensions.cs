using System.Diagnostics.CodeAnalysis;

using Microsoft.Xna.Framework;

using StardewValley.TerrainFeatures;

namespace ClearFarmDebris.Extensions;
/// <summary>
/// Extensions for <see cref="Bush"/> to simplify code.
/// </summary>
internal static class BushExtensions {
  [SuppressMessage("Style",     "IDE1006:Naming Styles", Justification = "Constancy with StardewValley")]
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  public static bool instantDestroy(this Bush bush, Vector2 tileLocation, Tool? tool = null, int explosion = 0) => bush.performToolAction(tool, explosion, tileLocation);
}
