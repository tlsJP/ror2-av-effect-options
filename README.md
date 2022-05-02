Hewwos!

## Installing
* Using [ebkr's r2modman Plus](https://thunderstore.io/package/ebkr/r2modman)
    1. Obtain a release .zip of this mod.
    1. In r2modman, go to "⚙ Settings" → "Import local mod" (under "Profile") → "Select file"
    1. Browse to and select the .zip file.
    1. "Import local mod"
* OR manually...
    1. Ensure you have a current version of [BepInExPack](https://thunderstore.io/package/bbepis/BepInExPack) installed and configured.
    1. Extract the `..dll` file from this mod to your BepInEx's `plugins` folder.

## Compiling
1. Get all the prerequisite thingies
    * The following from [The Mono Project](https://www.mono-project.com/download/stable). Note: Xamarin's xbuild doesn't work!
        * mono-devel
        * msbuild
    * The `convert` command from [ImageMagick](https://imagemagick.org/script/download.php) (`imagemagick` on Debians)
        * Make sure it's compiled with ico support! (all major distributions do)
    * [Rune580's Risk of Options](https://thunderstore.io/package/Rune580/Risk_Of_Options) (either manually or using r2modman)
1. Obtain the source code (using git, or curl, wget, email, etc.)
1. Compile!
    * TODO: configure path to BepInEx plugins (look at the `BepInEx` property)
    * `msbuild -r` inside the root of the source tree
1. Compiled assembly is located at `./out/..dll`, distribution zip is located at `./..zip`
