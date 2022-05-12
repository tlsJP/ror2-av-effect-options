Hewwos!

## OwO, wat's dis?
The goal is to allow players to selectively disable in-game audio-visual effects that aren't strictly necessary to enjoy the game without changing game mechanics or balance. This may assist people who, among others; have photosensitivity, are sensitive to sensory overstimulation, or have limited computing resources. Effects can [usually] be toggled mid-stage.

Originally intended just my furiend group, I am now advertising it here on the Thunderstore because I believe everyone should be able to have some fun. If you don't like it/me being here, just remove it!

Currently only the following effects are configurable, but I'm always looking to add more options. If you know the asset address of an effect, or how to disable something else annoying then please feel free to [poke me](https://gitlab.com/lexxyfox/ror2-av-effect-options/issues) or even submit a PR.

Intended for headed client-side installations. [Should] have no effect on headless installations. 

### Currently configurable
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

### Future options
* Literally everything else
* Singularity Band
* Voidsent Flame
* Underwater screen effect
* Teleporter initialisation effect
* Imp boss effect
* etc.
* l10n support
* A new icon?

## Configuring
"Enabling" an effect (default) plays the effect as it does normally. "Disabling" makes it silent and invisible.
* Use [Rune580's Risk of Options](https://thunderstore.io/package/Rune580/Risk_Of_Options) (recommended)!
    ![Risk of Options](https://gitlab.com/lexxyfox/ror2-av-effect-options/wikis/roo.png)
* OR: modify the .cfg file manually...
    * You aren't required to run the game before doing this!
    * In BepInEx's config directory, create or modify the file named `.AVFX_Options...cfg`
    * Here's the default configuration to get you started...
    ```
    [Character Effects]
    # Enables Stone Titan's on-death explosion.
    Enable Titan Death Effect = true
    # Enables Wandering Vagrant's on-death explosion. 
    Enable Vagrant Death Explosion = true

    [Item Effects]
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
    ```

## Installing
* Using [ebkr's r2modman Plus](https://thunderstore.io/package/ebkr/r2modman) and the Thunderstore
    1. In r2modman, go to "🌐 Online" and Search for "AV Effect Options"
    1. Click on "AV Effect Options by Higgs1" → "Download" → "Download with dependencies".
    1. Call your mom/dad/parental figure/loved one/consumer of worlds
* OR using [ebkr's r2modman Plus](https://thunderstore.io/package/ebkr/r2modman) (local install)
    1. Obtain a release .zip of this mod.
    1. In r2modman, go to "⚙ Settings" → "Import local mod" (under "Profile") → "Select file"
    1. Browse to and select the .zip file.
    1. "Import local mod"
* OR manually...
    1. Ensure you have a current version of [BepInExPack](https://thunderstore.io/package/bbepis/BepInExPack) installed and configured.
    1. Extract the `..dll` file from this mod to BepInEx's `plugins` folder.

## Compiling

The binary release of this mod is built on a Debian-like but any [Portable Operating System](https://en.wikipedia.org/wiki/POSIX) that supports all of the prerequisite thingies should be fine.

1. Get all the prerequisite thingies
    * The following from [The Mono Project](https://www.mono-project.com/download/stable).
        * mono-devel
        * msbuild
    * The `convert` command from [ImageMagick](https://imagemagick.org/script/download.php) (`imagemagick` on Debians)
        * Make sure it's compiled with ico support! (all major distributions do)
    * [Rune580's Risk of Options](https://thunderstore.io/package/Rune580/Risk_Of_Options) (either manually or using r2modman)
1. Obtain this mod's source code (using git, or curl, wget, email, etc.)
1. Compile!
    * TODO: configure path to BepInEx plugins (look at the `BepInExPath` property)
    * `msbuild -r` inside the root of the source tree. 
1. Compiled assembly is located at `./out/..dll`, distribution zip is located at `./..zip`

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
* YOU! For existing today :3

## Notes
* Xamarin's xbuild is not compatible with this mod, and will not be supported by me. (msbuild > xbuild)
* This mod is **NOT** guaranteed to come with a virus.
