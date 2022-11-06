# Usage
_Note: always be carefull when downloading executable files over the internet - they could be malicious - it's best to build the project manually and read any `.csproj` files located in the project to understand what is being packaged in the executable file_

### Clone source
To clone the source download this github repositry then run `BROMODS.exe` which can be located at `.\BroforceModSoftware\bin\Debug\netcoreapp3.1\`
### Use recent release
To use the recent release head over to the releases tab and find a zip - then unzip it and run the exe!
### Build manually
_The following instructions use "Visual Studio Code" (Not to be confused with "Visual Studio" which is more commonly used (i like to do everything manually so i can understand what's going on))_

1. `MSBUILD` and `dotnet` will be needed (`.NET` version `3.5` and `3.1` may be needed)
2. Set your local path in the terminal to the engine's `.csproj` file (`cd <YourPath>\BroMods\BroforceModEngine\BroforceModEngine\`)
3. Build the engine using `MSBUILD` (`MSBuild /p:Configuration=Release /p:Platform="AnyCPU"`)
4. Once completed set your local path to the software's `.csproj` file (`cd <YourPath>\BroMods\BroforceModSoftware\`)
5. Build the software using `dotnet build`
6. Run `BROMODS.exe` which can be located at `.\BroforceModSoftware\bin\Debug\netcoreapp3.1\`

# Dependencies
- https://github.com/pardeike/Harmony
- https://www.newtonsoft.com/json

# Developer (Wip section)
- Use Vscode or Visual Studio
- When using vscode a global.json file will be required with the sdk version set to "3.5.0"
- dotnet run ~ runs application in current working directory
- dotnet build ~ builds application in current working directory
- MSBuild /p:Configuration=Release /p:Platform="AnyCPU" - Builds to bin/release where contents can then be copied into broforce/BroforceModEngine folder
- cd {path} ~ changes working directory
