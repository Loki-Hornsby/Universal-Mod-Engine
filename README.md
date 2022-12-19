
# Universal Mod Engine (UME)
Thank you [Gorzon](https://github.com/Gorzon38), [Atom0s](https://forum.exetools.com/showthread.php?t=16470), [Cecilifier](https://cecilifier.me/) and [Joe Best-Rotheray](https://www.codersblock.org/blog//2014/06/integrating-monocecil-with-unity.html)!

- [I want mods **NOW!**](#usage)
- [Hay! i can do it myself y'know!](#build)
- [Dependencies](#dependencies)

# Usage
Use the releases tab to get the latest standalone EXE or [build from source](#build)

# Build
- Build using `dotnet build -c:Release`
- `.csproj` files (in build order) are;
  - `..\UUniversalModEngine\Software\Gui.csproj` (Software)
  - `..\UUniversalModEngine\Injector\Injector.csproj` (Injector)
  - `..\UniversalModEngine\Engine\Engine.csproj` (Engine)
- Run `Engine.exe` which is located at `..\UUniversalModEngine\Engine\bin\Release`

# Dependencies
- https://www.mono-project.com (`Mono.cecil`)
