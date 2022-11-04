using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;

using HarmonyLib;
using BroforceModEngine.Logging;

/// <summary>
/// The entry point for the engine
/// MSBuild /p:Configuration=Release /p:Platform="AnyCPU"
/// </summary>

namespace BroforceModEngine {
    public static class Loader {
        public static string Load() {
            // Initialize the logger
            if (!Logger._initialized) Logger.Initialize();

            Logger.Log("HI! FROM LAUNCHED ENGINE");

            // Begin execution of mod engine
            try {
                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

                ModEngine.Load();
            } catch(Exception ex) {
                Logger.Log(ex.ToString());
            }

            return Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args){
            try {
                return Assembly.LoadFile(Path.Combine(ModEngine.EngineDirectoryPath, "0Harmony.dll"));
            } catch(Exception ex){
                
            }
            
            return null;
        }
    }
}