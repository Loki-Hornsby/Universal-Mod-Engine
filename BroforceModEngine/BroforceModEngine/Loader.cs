using System;
using System.IO;
using System.Reflection;
using BroforceModEngine.Loggers;
using HarmonyLib;
using System.Runtime.InteropServices;

// i'll try my best to maintain your code style throughout your files 
    // Gorzon but if you don't mind more comments will popup because it helps me to understand what's going on here - Bobby :)

namespace BroforceModEngine
{
    static class Loader
    {
        [DllImport("kernel32")]
        static extern bool AllocConsole();

        public static void Load()
        {   
            // A console is displayed for debugging purposes
            AllocConsole();
            SimpleLogger.Log("", "Hello! - Console was started!");

            // Begin execution of mod engine
            try
            {
                //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

                ModEngine.Load();
            }
            catch(Exception ex)
            {
                GlobalLogger.NoEchecLog(ex.ToString());
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
                ModEngine.EngineLog(ex.ToString(), LogType.Error);
            }
            return null;
        }

    }
}