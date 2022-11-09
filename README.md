
# BROMODS
Thank you [Gorzon](https://github.com/Gorzon38)!

1. [How do i use the mod engine?](#usage)
2. [What does this project depend on?](#dependencies)
3. [Gimme' a breakdown doc.](#disclaimer)
4. [Tell me more!](#extras)

# Usage
- _Note: always be carefull when downloading executable files over the internet - they could be malicious - it's best to build the project manually and read any `.csproj` files located in the project to understand what is being packaged in the executable file_
- _Before usage please make sure to read the [disclaimer](#disclaimer) featured below._
### Clone source
To clone the source download this github repositry, unzip it to your desired location, then run `BROMODS.exe` which can be located at `.\BroforceModSoftware\bin\Debug\netcoreapp3.1\`
### Use recent release
To use the recent release head over to the releases tab and find a zip - then unzip it and run the exe!
### Build manually
_The following instructions use "Visual Studio Code" (Not to be confused with "Visual Studio" which is more commonly used (i like to do everything manually so i can understand what's going on))_

1. `MSBUILD` and `dotnet` will be needed (`.NET` version `3.5` and `3.1` may be needed)
2. Delete all the files located at `\BroMods\BroforceModSoftware\bin\Debug\netcoreapp3.1\` (This step isn't necessary because the files are overwritten when building)
3. Set your local path in a new Vscode terminal to the engine's `.csproj` file (`cd <YourPath>\BroMods\BroforceModEngine\BroforceModEngine\`)
4. Build the engine using `MSBUILD` (`MSBuild /p:Configuration=Release /p:Platform="AnyCPU"`)
5. Once completed set your local path to the software's `.csproj` file (`cd <YourPath>\BroMods\BroforceModSoftware\`)
6. Build the software using `dotnet build`
7. Run `BROMODS.exe` which has been generated at `\BroMods\BroforceModSoftware\bin\Debug\netcoreapp3.1\`

# Dependencies
- https://harmony.pardeike.net
- https://www.newtonsoft.com/json
- https://www.mono-project.com

# Disclaimer
- _Content from [Broforce](https://www.broforcegame.com/) is used such as audio and code - I and my contributors in no way claim to own any of it._ 
- _The intellectual property goes to that of the original author and their publisher ([Broforce](https://www.broforcegame.com/) ~ [Free Lives](https://freelives.net/) ~ [Devolver Digital](https://www.devolverdigital.com/))._ 
- _Any metadata or text detailing false information in terms of ownership of assets is not intentional and is an accident - please contact me if this is the case._ 
- _Content from [Broforce](https://www.broforcegame.com/) used in this repositry is to be used in terms of modding and use of the application itself exclusively - i and my contributors do not endorse the copying of [Broforce](https://www.broforcegame.com/)'s assets for incorrect use._
- _This software is under the implication that it (itself) is user created content for [Broforce](https://www.broforcegame.com/)._
- _Me and my contributors do not hold any liability for damage to your machine caused by this software._
- _Use of this software implies that you have understood these terms._
- _The usage of the term "Software" in this disclaimer defines the entire contents of this repositry and not just the executable file for the application itself._

# Extras
### Doorstop Alternative
- Originally we were using [Doorstop](https://github.com/NeighTools/UnityDoorstop) to load our mod engine however i decided to replace it since i like to remove as many dependencies as i can from a project.
- If you'd like to alter my immplementation or just learn a bit more about how it works i reccommend checking out the following links
  - https://code.visualstudio.com/docs/languages/cpp
  - https://www.msys2.org/
  - https://www.unknowncheats.me
  - https://www.mono-project.com/docs/advanced/embedding/
  - https://www.youtube.com/watch?v=KCtLiBnlpk4 
  - https://www.codeproject.com/Articles/9407/Introduction-to-Mono-Your-first-Mono-app
