using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Microsoft.Xna.Framework;

using Object = StardewValley.Object;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;

using StardewModdingAPI.Events;

using ClearFarmDebris.Extensions;
using ClearFarmDebris.Framework;
using System.Globalization;
using System.Text;

namespace ClearFarmDebris;

// ReSharper disable once UnusedMember.Global
public class ClearFarmMod : Mod {
  private ModConfig Config { get; set; } = null!;
  private static Pickaxe Pickaxe => new() {
    additionalPower        = new(int.MaxValue),
    AnimationSpeedModifier = float.MaxValue,
    UpgradeLevel           = int.MaxValue,
  };

  private static Axe Axe = new() {
    additionalPower        = new(int.MaxValue),
    AnimationSpeedModifier = float.MaxValue,
    UpgradeLevel           = int.MaxValue,
  };

  private static MeleeWeapon Scythe = new() {
    ItemId = "66",
  };

  private List<Command> Commands = [];

  private FailedActionCollection Failed = [];

  public override void Entry(IModHelper helper) {
    this.Config = helper.ReadConfig<ModConfig>();
    I18n.Init(helper.Translation);
    helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
    helper.Events.Input.ButtonPressed   += this.OnButtonPressed;

    this.Commands.Add(new("Clear Farm", "clear_farm", "Clears the farm of trees, bushes, rocks, weeds, stumps, logs, boulders, grass, and random event objects.", ["trees","bushes","rocks","weeds","stumps","logs","boulders","grass","and random event objects"], this.ChopTreesCommand));
    this.Commands.Add(new("Clear Farm Trees", "clear_farm_trees", "Clears the farm of trees.", ["trees"], this.ChopTreesCommand));
    this.Commands.Add(new("Clear Farm Bushes", "clear_farm_bushes", "Clears the farm of bushes.", ["bushes"], this.ChopBushCommand));
    this.Commands.Add(new("Clear Farm Clumps", "clear_farm_clumps", "Clears the farm of stumps, logs, boulders, and random event objects.", ["stumps","logs","boulders","grass","and random event objects"], this.DestroyClumpCommand));
    this.Commands.Add(new("Clear Farm Grass Till", "clear_farm_grass_till", "Clears the farm of grass, and random event objects.", ["grass","and random event objects"], this.FlattenHoeAndCutGrassCommand));
    this.Commands.Add(new("Clear Farm Others", "clear_farm_others", "Clears the farm of rocks, weeds, stumps, branches, and random event objects.", ["rocks","weeds","stumps","branches","and random event objects"], this.BreakEverythingElseCommand));
    this.Commands.Add(new("Collect Items", "collect_items", "Clears the farm of dropped items.", ["dropped items"], this.PickupItemsCommand));

    foreach (var command in this.Commands) {
      command.Init(helper.ConsoleCommands);
    }
  }

  // ReSharper disable once ArrangeMethodOrOperatorBody
  private void OnGameLaunched(object? sender, GameLaunchedEventArgs e) {
    ClearFarmMod.Register(this.ModManifest, this.Helper.ModRegistry, this.Monitor,
                          getConfig: () => this.Config,
                          reset: () => this.Config = new(),
                          save: () => this.Helper.WriteConfig(this.Config));
  }

  private void OnButtonPressed(object? sender, ButtonPressedEventArgs e) {
    if (e.Button == this.Config.ClearFarmKey)
      this.Commands.FirstOrDefault(x => x.Name == "Clear Farm")?.Run("clear_farm", null);
  }

  public static void Register(IManifest manifest, IModRegistry modRegistry, IMonitor monitor, Func<ModConfig> getConfig, Action reset, Action save, bool titleScreenOnly = false) {
    // Get API
    IGenericModConfigMenuApi? api = IntegrationHelper.GetGenericModConfigMenu(modRegistry, monitor);

    if (api is null) return;

    // Register config UI
    api.Register(manifest, reset, save, titleScreenOnly);

    // General options
    // Add the config option to disable pausing of the game outside of a save.
    api.AddKeybind(mod: manifest,
                   name: I18n.Config_ClearFarmKey_Name,
                   tooltip: I18n.Config_ClearFarmKey_Desc,
                   getValue: () => getConfig().ClearFarmKey,
                   setValue: value => getConfig().ClearFarmKey = value);
  }

  private void ChopTrees(Farm farm, Tool axe, Tool scythe) {
    foreach ((Vector2 position, TerrainFeature feature) in farm.terrainFeatures.Pairs.Where(p => p.Value is Tree).Iterate()) {
      if (feature is Tree tree && tree.isActionable() && !tree.tapped.Get() && !tree.falling.Get()) {
        try {
        if (tree.hasMoss.Get() || tree.hasSeed.Get()) {
            if (!tree.performToolAction(scythe, 0, position)) {
              this.Failed.Add(new("scythe action", position: position));
            }
          }
        } catch (Exception exception) {
          this.Failed.Add(new("scythe", position: position, hard: true, exception: exception));
        }
        try {
          if (!tree.instantDestroy(position)) {
            var initialHealth = tree.health.Get();
            while (tree.health.Get() > 0f) {
                if (!tree.performToolAction(scythe, 0, position)
                  && !tree.performToolAction(axe, 0, position)
                  && !tree.instantDestroy(position)) {
                  this.Failed.Add(new("chop action", position: position));
                }

                if (initialHealth == tree.health.Get()) {
                this.Failed.Add(new("loop-break", position: position));
                  break;
                }
              }
          }
        } catch (Exception exception) {
          this.Failed.Add(new("chop", position: position, hard: true, exception: exception));
        }
      }
    }
  }

  private void ChopTreesCommand(string command, string[] args, string[] sets) {
    Farmer farmer         = Game1.player;
    float  originalStamina = farmer.Stamina;
    this.ChopTrees(Game1.getFarm(), Axe, Scythe);
    this.ChopTrees(Game1.getFarm(), Axe, Scythe);
    this.Monitor.Log($"Farm cleared of {string.Join(", ", sets)}.", LogLevel.Info);
    farmer.Stamina = originalStamina;
    this.Failed.Log(this.Monitor);
  }

  private void ChopBush(Farm farm, Tool axe, Tool scythe) {
    foreach (LargeTerrainFeature largeFeature in farm.largeTerrainFeatures.Where(f => f is Bush)) {
      try {
        if (largeFeature is Bush bush && bush.isDestroyable()) {
          var initialHealth = bush.health;
          while (bush.health > 0f) {
            if (bush.performToolAction(axe, 0, bush.netTilePosition.Get())
              && !bush.performToolAction(scythe, 0, bush.netTilePosition.Get())) {
              this.Failed.Add(new("toolAction", position: bush.Tile));
              if (!bush.instantDestroy(bush.netTilePosition.Get())) {
                this.Failed.Add(new("destroy", position: bush.Tile));
              }
            }

            if (initialHealth == bush.health) {
              break;
            }
          }
        }
      } catch (Exception exception) {
        this.Failed.Add(new("chop", position: largeFeature.Tile, hard: true, exception: exception));
      }
    }
  }

  private void ChopBushCommand(string command, string[] args, string[] sets) {
    Farmer farmer         = Game1.player;
    float  originalStamina = farmer.Stamina;
    this.ChopBush(Game1.getFarm(), Axe, Scythe);
    this.Monitor.Log($"Farm cleared of {string.Join(", ", sets)}.", LogLevel.Info);
    farmer.Stamina = originalStamina;
    this.Failed.Log(this.Monitor);
  }

  private void DestroyClump(Farm farm, Tool pickaxe, Tool axe, Tool scythe) {
    // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
    foreach (ResourceClump clump in farm.resourceClumps) {
      try {
        var position = clump.netTile.Get();
        if (clump.isActionable()) {
          var initialHealth = clump.health.Get();
          while (clump.health.Get() > 0f) {
            if (!clump.performToolAction(pickaxe, int.MaxValue, position)
              && !clump.performToolAction(axe,int.MaxValue, position)
              && !clump.performToolAction(scythe, int.MaxValue, position)) {
              this.Failed.Add(new("toolAction", position: position));
              if (!clump.destroy(null, clump.Location, position)) {
                this.Failed.Add(new("destroy", position: position));
              }
            }

            if (initialHealth == clump.health.Get()) {
              break;
            }
          }
        }
      } catch (Exception exception) {
        this.Failed.Add(new("chop", position: clump.Tile, hard: true, exception: exception));
      }
    }
  }

  private void DestroyClumpCommand(string command, string[] args, string[] sets) {
    Farmer farmer         = Game1.player;
    float  originalStamina = farmer.Stamina;
    this.DestroyClump(Game1.getFarm(), Pickaxe, Axe, Scythe);
    this.Monitor.Log($"Farm cleared of {string.Join(", ", sets)}.", LogLevel.Info);
    farmer.Stamina = originalStamina;
    this.Failed.Log(this.Monitor);
  }

  private void FlattenHoeAndCutGrass(Farm farm, Tool pickaxe, Tool axe, Tool scythe) {
    foreach ((Vector2 position, TerrainFeature feature) in farm.terrainFeatures.Pairs.Where(p => p.Value is HoeDirt or Grass).Iterate()) {
      if (feature is HoeDirt hoeDirt && hoeDirt.isActionable() && !hoeDirt.hasPaddyCrop() && !hoeDirt.HasFertilizer() && !hoeDirt.isWatered() && hoeDirt.crop is null && !hoeDirt.readyForHarvest()) {
        try {
          if (!hoeDirt.performToolAction(pickaxe, int.MaxValue, hoeDirt.Tile)
            && !hoeDirt.performToolAction(axe, int.MaxValue, hoeDirt.Tile)
            && !hoeDirt.performToolAction(scythe, int.MaxValue, hoeDirt.Tile)) {
            this.Failed.Add(new("toolAction", position: position));
          }
        } catch (Exception exception) {
          this.Failed.Add(new("flatten", position: hoeDirt.Tile, hard: true, exception: exception));
        }
      } else if (feature is Grass grass && grass.isActionable()) {
        try {
          if (!grass.performToolAction(pickaxe, int.MaxValue, grass.Tile)
            && !grass.performToolAction(axe, int.MaxValue, grass.Tile)
            && !grass.performToolAction(scythe, int.MaxValue, grass.Tile)) {
            this.Failed.Add(new("grass", position: position));
          }
        } catch (Exception exception) {
          this.Failed.Add(new("cut", position: grass.Tile, hard: true, exception: exception));
        }
      }
    }
  }

  private void FlattenHoeAndCutGrassCommand(string command, string[] args, string[] sets) {
    Farmer farmer         = Game1.player;
    float  originalStamina = farmer.Stamina;
    this.FlattenHoeAndCutGrass(Game1.getFarm(), Pickaxe, Axe, Scythe);
    this.Monitor.Log($"Farm cleared of {string.Join(", ", sets)}.", LogLevel.Info);
    farmer.Stamina = originalStamina;
    this.Failed.Log(this.Monitor);
  }

  private void BreakEverythingElse(Farm farm, Farmer farmer, Tool pickaxe, Tool axe) {
    var objects = from p in farm.objects.Pairs
                  where p.Value.Name.ContainsAny(["Rock", "Stone", "Twig", "Bush", "Weed",]) || p.Value.Name is "Meteorite" or "Iridium Ore"
                  select (p.Key, p.Value);
    foreach ((Vector2 position, Object @object) in objects) {
      try {
        if ((@object.IsTwig() || @object.IsBreakableStone() || @object.IsWeeds() || @object.IsWildTreeSapling())
          && !@object.isSapling() && !@object.IsTapper()) {
            //var initialHealth = @object.getHealth();
            @object.setHealth(-1);
        }
      } catch (Exception exception) {
          this.Failed.Add(new("setHealth", position: position, hard: true, exception: exception));
      }
      /*} else if () {
      try {
        @object.cutWeed(farmer);
      } catch (Exception exception) {
        this.Monitor.LogOnce(exception.GetFullyQualifiedExceptionText(), LogLevel.Error);
        this.Failed.Add(new("weeds", position: position));
      }*/
    }
  }

  private void BreakEverythingElseCommand(string command, string[] args, string[] sets) {
    Farmer farmer         = Game1.player;
    float  originalStamina = farmer.Stamina;
    this.BreakEverythingElse(Game1.getFarm(), farmer, Pickaxe, Axe);
    this.Monitor.Log($"Farm cleared of {string.Join(", ", sets)}.", LogLevel.Info);
    farmer.Stamina = originalStamina;
    this.Failed.Log(this.Monitor);
  }

  private void PickupEverything(Farm farm, Farmer farmer) {
    // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
    foreach (Debris debris in farm.debris) {
      try {
        if (debris.item is not null && !debris.collect(farmer)) {
            this.Failed.Add(new($"collect {debris.itemId}"));
        }
      } catch (Exception exception) {
          this.Failed.Add(new($"collect {debris.itemId}", hard: true, exception: exception));
      }
    }
  }

  private void PickupItemsCommand(string command, string[] args, string[] sets) {
    Farmer farmer         = Game1.player;
    float  originalStamina = farmer.Stamina;

    this.PickupEverything(Game1.getFarm(), farmer);

    this.Monitor.Log($"Farm cleared of {string.Join(", ", sets)}.", LogLevel.Info);

    farmer.Stamina = originalStamina;

    this.Failed.Log(this.Monitor);
  }

  private void ClearFarm(string command, string[] args, string[] sets) {
    Farmer  farmer         = Game1.player;
    float   originalStamina = farmer.Stamina;

    this.ChopTrees(Game1.getFarm(), Axe, Scythe);
    this.ChopTrees(Game1.getFarm(), Axe, Scythe);
    this.ChopBush(Game1.getFarm(), Axe, Scythe);
    this.DestroyClump(Game1.getFarm(), Pickaxe, Axe, Scythe);
    this.FlattenHoeAndCutGrass(Game1.getFarm(), Pickaxe, Axe, Scythe);
    this.BreakEverythingElse(Game1.getFarm(), farmer, Pickaxe, Axe);
    this.PickupEverything(Game1.getFarm(), farmer);

    this.Monitor.Log($"Farm cleared of {string.Join(", ", sets)}.", LogLevel.Info);

    farmer.Stamina = originalStamina;

    this.Failed.Log(this.Monitor);
  }
}
