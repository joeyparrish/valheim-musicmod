# MusicMod - A Valheim Mod by Joey Parrish

Source: https://github.com/joeyparrish/valheim-musicmod


## About

MusicMod is a base mod that you can depend on to add custom music to your own
mod.


## Installation

MusicMod is only useful as a dependency for other mods.  Mod developers should
install it, build against it, and depend on it.  End users will only ever
install it to satisfy dependencies for other mods.

MusicMod is published on both ThunderStore and Nexus Mods.  Install using your
favorite mod manager.


## Sample Usage

```csharp
using System.IO;
using System.Reflection;
using MusicMod;

public class MyMod : BaseUnityPlugin {
  private void Awake() {
    // When your mod wakes up, build an absolute path
    // to your music file, then tell MusicMod about it.
    string assemblyFolder = Path.GetDirectoryName(
        Assembly.GetExecutingAssembly().Location);
    string absolutePath = Path.Combine(
        assemblyFolder, "Main-Menu.mp3");
    MusicMod.Mod.OverrideMusic["menu"] = absolutePath;
  }
}
```


## Override Keys

Each piece of music in Valheim is identified by a string "key".  MusicMod will
discover all music at runtime and print debug logs showing all music currently
in the game.  For an up-to-date list, check the log.

These are the current keys showing up in the log as of August 2022:

 - `respawn`
 - `intro`
 - `menu`
 - `combat`
 - `CombatEventL1`
 - `CombatEventL2`
 - `CombatEventL3`
 - `CombatEventL4`
 - `boss_eikthyr`
 - `boss_gdking`
 - `boss_bonemass`
 - `boss_moder`
 - `boss_goblinking`
 - `morning`
 - `evening`
 - `sailing`
 - `blackforest`
 - `meadows`
 - `swamp`
 - `mountain`
 - `plains`
 - `forestcrypt`
 - `frostcaves`
 - `home`
 - `location_forest`


## Dependencies

Just [BepInEx](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/).


## Incompatibilities

None that we know of!


## Links

 - Nexus Mods: https://www.nexusmods.com/valheim/mods/1975/
 - Thunderstore: https://valheim.thunderstore.io/package/joeyparrish/MusicMod/


## Credits

MusicMod was created by [Joey Parrish](https://joeyparrish.github.io/), and
spun out of [Pokéheim](https://github.com/joeyparrish/pokeheim).
