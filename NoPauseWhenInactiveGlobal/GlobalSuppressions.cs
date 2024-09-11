// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

// Mod Entry Ignores
[assembly: SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>", Scope = "member", Target = "~P:NoPauseWhenInactiveGlobal.ModEntry._instance")]
[assembly: SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>", Scope = "member", Target = "~F:NoPauseWhenInactiveGlobal.ModEntry.config")]

// InstanceGame_IsActive_Patch Ignores
[assembly: SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "<Pending>", Scope = "type",   Target = "~T:NoPauseWhenInactiveGlobal.Framework.InstanceGame_IsActive_Patch")]
[assembly: SuppressMessage("ReSharper",        "InconsistentNaming",                       Justification = "<Pending>", Scope = "type",   Target = "~T:NoPauseWhenInactiveGlobal.Framework.InstanceGame_IsActive_Patch")]
[assembly: SuppressMessage("ReSharper",        "UnusedMember.Global",                      Justification = "<Pending>", Scope = "type",   Target = "~T:NoPauseWhenInactiveGlobal.Framework.InstanceGame_IsActive_Patch")]

// InstanceGame_IsActive_Patch.Prefix Ignores
[assembly: SuppressMessage("Roslynator",       "RCS1163:Unused parameter",                 Justification = "<Pending>", Scope = "member", Target = "~M:NoPauseWhenInactiveGlobal.Framework.InstanceGame_IsActive_Patch.Prefix(StardewValley.InstanceGame,System.Boolean@)~System.Boolean")]

// Game1_IsActiveNoOverlay_Patch Ignores
[assembly: SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "<Pending>", Scope = "type", Target = "~T:NoPauseWhenInactiveGlobal.Framework.Game1_IsActiveNoOverlay_Patch")]
[assembly: SuppressMessage("ReSharper",        "InconsistentNaming",                       Justification = "<Pending>", Scope = "type", Target = "~T:NoPauseWhenInactiveGlobal.Framework.Game1_IsActiveNoOverlay_Patch")]
[assembly: SuppressMessage("ReSharper",        "UnusedMember.Global",                      Justification = "<Pending>", Scope = "type", Target = "~T:NoPauseWhenInactiveGlobal.Framework.Game1_IsActiveNoOverlay_Patch")]

// Game1_IsActiveNoOverlay_Patch.Prefix Ignores
[assembly: SuppressMessage("Roslynator", "RCS1163:Unused parameter", Justification = "<Pending>", Scope = "member", Target = "~M:NoPauseWhenInactiveGlobal.Framework.Game1_IsActiveNoOverlay_Patch.Prefix(StardewValley.Game1,System.Boolean@)~System.Boolean")]

// Game1_IsActiveNoOverlay_Patch.GetProgramSDK Ignores
[assembly: SuppressMessage("Major Code Smell", "S2589:Boolean expressions should not be gratuitous", Justification = "<Pending>", Scope = "member", Target = "~M:NoPauseWhenInactiveGlobal.Framework.Game1_IsActiveNoOverlay_Patch.GetProgramSDK~StardewValley.SDKs.SDKHelper")]
