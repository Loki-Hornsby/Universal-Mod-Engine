using System;
using System.IO;
using System.Reflection;

using HarmonyLib;
using BroforceModEngine.Logging;

/// <summary>
/// The entry point for the engine
/// </summary>

namespace BroforceModEngine {
    static class Loader {
        public static void Main(){   
            // Initialize the logger
            if (!Logger._initialized) Logger.Initialize(true);

            // Begin execution of mod engine
            try {
                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

                ModEngine.Load();
            } catch(Exception ex) {
                Logger.Log(ex.ToString(), Logger.LogType.None);
            }
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                return Assembly.LoadFile(Path.Combine(ModEngine.EngineDirectoryPath, "0Harmony.dll"));
            }
            catch(Exception ex)
            {
                
            }
            return null;
        }
    }
}