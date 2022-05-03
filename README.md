Hewwos!

## OwO, wat's dis?
The goal is to allow players to selectively disable in-game audio-visual effects that aren't strictly necessary to enjoy the game without changing game mechanics or balance. This may assist people who- among others; have photosensitivity, are sensitive to sensory overstimulation, or have limited computing resources. Effects can [usually] be toggled mid-stage.

Originally intended just my furiend group, I am now advertising it here on the ThunderStore because I believe everyone should be able to have some fun. If you don't like it/me being here, just remove it!

Currently only the following effects are configurable, but I'm always looking to add more options. If you know the asset address of an effect, or how to disable something else annoying then please feel free to poke me or even submit a PR.

Intended for headed client-side installations. [Should] have no effect on headless installations. 

### Currently configurable
* Brainstalks's screen effect
* Molten Perferators
* Spinel Tonic's screen effect
* Sticky Bombs

### Future options
* Literally everything else
* Will-o'-the-Wisp
* The Rings
* The generic explosion
* Teleporter initialisation effect
* etc.
* A new icon?

## Configuring
"Enabling" an effect (default) plays the effect as it does normally. "Disabling" makes it silent and invisible.
* Use [Rune580's Risk of Options](https://thunderstore.io/package/Rune580/Risk_Of_Options) (recommended)!
    ![Risk of Options](https://gitlab.com/lexxyfox/ror2-av-effect-options/-/wikis/roo.png)
* OR: modify the .cfg file manually...
    * You aren't required to run the game before doing this!
    * In BepInEx's config directory, create or modify the file named `.AVFX_Options...cfg`
    * Here's the default configuration to get you started...
    ```
    [Item Effects]
    
    ## Enables the Sticky Bomb visuals
    Enable Sticky Bomb = true
    
    ## Enables the Molten Perforator visuals
    Enable Molten Perforator = true
    
    ## Enables the Spinel Tonic screen effect
    Enable Spinel Tonic = true
    
    ## Enables the Brainstalks screen effect.
    Enable Brainstalks = true
    ```

## Installing
* Using [ebkr's r2modman Plus](https://thunderstore.io/package/ebkr/r2modman)
    1. Obtain a release .zip of this mod.
    1. In r2modman, go to "⚙ Settings" → "Import local mod" (under "Profile") → "Select file"
    1. Browse to and select the .zip file.
    1. "Import local mod"
* OR manually...
    1. Ensure you have a current version of [BepInExPack](https://thunderstore.io/package/bbepis/BepInExPack) installed and configured.
    1. Extract the `..dll` file from this mod to BepInEx's `plugins` folder.

## Compiling

The binary release of this mod is built on some sort of Debian but any Portable Operating System that supports all of the prerequisite thingies should be fine.

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
You know who you are ;3 (TODO)

## Notes
* Xamarin's xbuild is not compatible with this mod, and will not be supported by me. (msbuild > xbuild)
* This mod is **NOT** guaranteed to come with a virus.
