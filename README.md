# BROMODS
**RAD MOD LOADER FOR BROFORCE WHICH INCLUDES: (WIP section)**
- GUI for adding and removing mods aswell as hooking broforce.exe at startup
- Built in GUI for toggling mods when playing broforce
- Entire mod loader built from scratch (WIP - Some dependencies are used as placeholders for now)

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
