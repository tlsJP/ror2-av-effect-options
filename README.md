
# Risk Of Rain 2 - AV Effects Options
This is a pseudo-fork of https://gitlab.com/lexxyfox/ror2-av-effect-options

## About
The goal is to allow players to selectively disable in-game audio-visual effects that aren't strictly necessary to enjoy the game without changing game mechanics or balance. This may assist people who, among others; have photosensitivity, are sensitive to sensory overstimulation, or have limited computing resources. Effects can [usually] be toggled mid-stage.

If you know the asset address of an effect, or how to disable something else annoying then please feel free to [open an issue](https://github.com/tlsJP/ror2-av-effect-options/issues) or even submit a PR.

Intended for headed client-side installations. [Should] have no effect on headless installations. 

### Currently configurable
* Blast Shower's effects
* Brainstalks's screen effect
* Frost Relic particles and FOV change
* Gasoline's explosion
* Interstellar Desk Plant's indicator ward particle effects
* Kjaro's Band tornado
* Molten Perferators
* Runald's Band explosion
* Shatterspleen explosion
* Spinel Tonic's screen effect
* Sticky Bomb's drops & explosion
* Stone Titan's death effect
* Wandering Vagrant's death explosion
* Weeping Fungus' effects
* Will-o'-the-Wisp explosion
* Plasma Shrimp AV effects


## Configuring
"Enabling" an effect (default) plays the effect as it does normally. "Disabling" makes it silent and invisible.
* Use [Rune580's Risk of Options](https://thunderstore.io/package/Rune580/Risk_Of_Options) (recommended)!
    ![Risk of Options](https://github.com/tlsJP/ror2-av-effect-options/blob/main/screenshot.png)
* OR: modify the .cfg file manually...
    * You aren't required to run the game before doing this!
    * In BepInEx's config directory, create or modify the file named `com.thejpaproject.AVFX_Options.cfg`
    * Here's the default configuration to get you started...
    ```
    [Character Effects]
    # Enables Stone Titan's on-death explosion.
    Enable Titan Death Effect = true
    # Enables Wandering Vagrant's on-death explosion. 
    Enable Vagrant Death Explosion = true

    [Item Effects]
    # Enables Blast Shower's effects.
    Enable Blast Shower = true
    # Enables the temporary FOV change that Frost Relic's on-kill proc gives.
    Enable Frost Relic FOV = false
    # Enables the chunk and ring effects of Frost Relic.
    Enable Frost Relic Particles = false
    # Enables the sound effects of Frost Relic's on-kill proc.
    Enable Frost Relic Sound = true
    # Enables the spore, plus sign, and mushroom visual effects from Interstellar Desk Plant's healing ward indicator.
    Enable Desk Plant Ward Particles = false
    # Enables Brainstalks' screen effect.
    Enable Brainstalks = true
    # Enables Gasoline's explosion
    Enable Gasoline = true
    # Enables Kjaro's Band's tornado
    Enable Kjaros Band = true
    # Enables the Molten Perforator visuals
    Enable Molten Perforator = true
    # Enables Runald's Band's explosion
    Enable Runalds Band = true
    # Enables Shatterspleen's explosion
    Enable Shatterspleen = true
    # Enables Spinel Tonic's screen effect
    Enable Spinel Tonic = true
    # Enables Sticky Bomb's drops
    Enable Sticky Bomb Drops = true
    # Enables Sticky Bomb's explosion
    Enable Sticky Bomb Explosion = true
    # Enables Will o' the Wisp's explosion
    Enable Will-o-the-Wisp = true
    
    [SOTV Item Effects]
    # Enables Weeping Fungus' sound effect. 
    Enable Weeping Fungus Sound = true
    # Enables Weeping Fungus' visual particle effects. 
    Enable Weeping Fungus Visuals = true
    ```

## Installing

### Thunderstore

* Visit mod page at [Thunderstore.io](https://thunderstore.io/package/TeamNinjaDSM/JPs_AV_Effect_Options/)
    1. Click on "Install With Mod Manager" 

* OR manually (download)...
    1. Ensure you have a current version of [BepInExPack](https://thunderstore.io/package/bbepis/BepInExPack) installed and configured.
    2. Download .zip from [Thunderstore.io](https://thunderstore.io/package/TeamNinjaDSM/JPs_AV_Effect_Options/)
    2. Extract the `AVOptions.dll` file from this mod to BepInEx's `plugins` folder.

* OR manually (build)...
    1. Ensure you have a current version of [BepInExPack](https://thunderstore.io/package/bbepis/BepInExPack) installed and configured.
    2. Build this project
    3. Copy the `AVOptions.dll` file from this mod to BepInEx's `plugins` folder.

## Compiling

### Environment
* VS 2019
* .NET 2.1


### Requirements
* [BepInEx](https://github.com/BepInEx/BepInEx) Installed in `(Risk Of Rain 2 game folder)` 
* [Rune580's Risk of Options](https://thunderstore.io/package/Rune580/Risk_Of_Options) Installed in `(Risk Of Rain 2 game folder)/BepInEx/plugins`

### Build

1. Open `AvOptions.csproj` in VS
2. Update entry for `<BepInExPath>` in the above file to match your install location
3. Press `F6` to build the solution


### Versioning
Update the following files:
* `./README.md`
* `./avoptions/AvOptions.cs`
* `./manifest.json`

## Credits 
These good people/beings did me a help one way or another, but don't necessarily endorse or condone myself or this mod.
* Hopoo Games
* [The Mono Project](https://www.mono-project.com)
* [The BepInEx developers](https://github.com/BepInEx/BepInEx/graphs/contributors)
* [ebkr](https://github.com/ebkr) [et alia](https://github.com/ebkr/r2modmanPlus/graphs/contributors) for [r2modman Plus](https://thunderstore.io/package/ebkr/r2modman)
* The Thunderstore
* [Rune580](https://github.com/Rune580) for [Risk of Options](https://thunderstore.io/package/Rune580/Risk_Of_Options)
* [Vl4dimyr](https://github.com/Vl4dimyr) for their [mod](https://thunderstore.io/package/Vl4dimyr/CaptainShotgunModes) (pictured)
* Bon Yok
* [RoR2 Modding Wiki](https://github.com/risk-of-thunder/R2Wiki)
* YOU! For existing today :3

## Notes
* Xamarin's xbuild is not compatible with this mod, and will not be supported by me. (msbuild > xbuild)
* This mod is **NOT** guaranteed to come with a virus.
* I'm not a .NET/C# developer, so there's a good argument to be made that I have no idea what I'm doing

# Change Log

## 2022-08-25 : 1.13.6
* Void Fiend sprint/jump sfx toggle added

## 2022-08-09 : 1.13.5
* Runalds properly moved to separate configuration class
* Add UNLICENSE
* Repay technical debt (code cleanup / conventions)

## 2022-08-09 : 1.13.4

* Create abstractions for configuration options

## 2022-07-31 : 1.13.3

* Simplify Plasma Shrimp options to only those that seem to have any real effect

## 2022-07-31 : 1.13.2

* Comment bindings for VoidMegaCrab
* Remove invalid attempt to bind to MissileVoid asset

## 2022-07-31 : 1.13.1

* Namespace config file
* Update Installation instructions

## 2022-07-31 : 1.13.0

* Add Plasma Shrimp AV Options
