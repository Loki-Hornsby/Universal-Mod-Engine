using System;
using System.IO;
using System.Reflection;
using BroforceModEngine.Loggers;
using HarmonyLib;

namespace BroforceModEngine
{
    static class Loader
    {
        public static void Main()
        {
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