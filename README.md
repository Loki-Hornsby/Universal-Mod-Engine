
# Universal Mod Engine
Thank you [Gorzon](https://github.com/Gorzon38), [Atom0s](https://forum.exetools.com/showthread.php?t=16470), [Cecilifier](https://cecilifier.me/) and [Joe Best-Rotheray](https://www.codersblock.org/blog//2014/06/integrating-monocecil-with-unity.html)!

1. [I want mods **NOW!**](#usage)
2. [Dude, i've like totally ripped off your code but i don't know how to test it works!](#building)
3. [So how much code have you ~~stolen~~ repurposed anyway?](#dependencies)
4. [Gimme' a breakdown doc.](#disclaimer)

# Usage
### Clone source
To clone the source download this github repository, unzip it to your desired location, then run `Gui.exe` which can be located at `./Software/bin/Release/netcoreapp3.1/`
### Use recent release
To use the recent release head over to the releases tab and find a zip - then unzip it and run the exe!

# Building
_The following instructions use "Visual Studio Code" (Not to be confused with "Visual Studio" which is more commonly used (i like to do everything manually so i can understand what's going on))_

### Setup (WIP ~ steps below may not work)
1. `MSBUILD` and `dotnet` **will** be needed (Installing [`Mono`](#dependencies) and `.NET` version `3.5` and `3.1` **may** be needed)
2. Create a new vscode terminal
3. Ensure you follow these steps in order

### 1. Building the injector
1. Set your local path to the injector's `.csproj` file (`cd <YourPath>\UniversalModEngine\Engine\Injector\`)
2. Run `dotnet build -c:Release`

### 2. Building the engine
1. Set your local path to the engine's `.csproj` file (`cd <YourPath>\UniversalModEngine\Engine\`)
2. Run `MSBuild /p:Configuration=Release /p:Platform="AnyCPU"`

### 3. Building the software
1. Set your local path to the software's `.csproj` file (`cd <YourPath>\UniversalModEngine\Software\`)
2. Run `dotnet build -c:Release`

### 4. Final Step
- Run `UniversalModLoader.exe` which has been generated at `UniversalModEngine/Software/bin/Release/netcoreapp3.1/`

```lua
if Errors then
  print("Create an issue and keep in touch.")
elseif not Errors then
  print("Happy forking ;D")
else
  error("Fatal Error!");
end
```

# Dependencies
- https://www.mono-project.com (`Mono.cecil`)
