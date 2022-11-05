using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;

using HarmonyLib;

/// <summary>
/// The entry point for the engine
/// MSBuild /p:Configuration=Release /p:Platform="AnyCPU"
/// </summary>

namespace BroforceModEngine {
    public static class Logger {
        static Func<string, string> EngineLowLogFunction;
        static Func<string, string> EngineMediumLogFunction;
        static Func<string, string> EngineHighLogFunction;

        public static void Initialize(Func<string, string> LogFuncLow, Func<string, string> LogFuncMedium, Func<string, string> LogFuncHigh){
            EngineLowLogFunction = LogFuncLow;
            EngineMediumLogFunction = LogFuncMedium;
            EngineHighLogFunction = LogFuncHigh;
        }

        public static void Log(string message, int verbosity){
            switch (verbosity){
                default:
                    EngineLowLogFunction(message);
                    break;
                case 2:
                    EngineMediumLogFunction(message);
                    break;
                case 3:
                    EngineHighLogFunction(message);
                    break;
            }
        }
    }

    public static class Loader {
        public static string Load(Func<string, string> LogFuncLow, Func<string, string> LogFuncMedium, Func<string, string> LogFuncHigh) {
            // Begin logging
            Logger.Initialize(LogFuncLow, LogFuncMedium, LogFuncHigh);
            Logger.Log("Logging was activated...", 3);

            // Begin execution of mod engine
            try {
                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

                Logger.Log("Passed 1st stage load...", 3);

                ModEngine.Load();
            } catch(Exception ex) {
                Logger.Log(ex.ToString(), 3);
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