using BroforceModEngine.Loggers;
using RocketLib.Loggers;
using System;
using System.Collections.Generic;
using System.IO;
using HarmonyLib;
using System.Reflection;
using System.Text;

namespace BroforceModEngine
{
    public static class ModEngine
    {
        /// <summary>
        /// BroforceModEngine main directory
        /// </summary>
        public static string EngineDirectoryPath
        {
            get
            {
                return Path.Combine(Directory.GetCurrentDirectory(), "BroforceModEngine");
            }
        }
        /// <summary>
        /// BroforceModEngine dependencies directory
        /// </summary>
        public static string DependenciesDirectoryPath
        {
            get
            {
                return Path.Combine(EngineDirectoryPath, "Dependencies");
            }
        }
        /// <summary>
        /// BroforceModEngine mods directory
        /// </summary>
        public static string ModsDirectoryPath
        {
            get
            {
                return Path.Combine(EngineDirectoryPath, "Mods");
            }
        }

        internal static Harmony harmony;

        /// <summary>
        /// Load everything
        /// </summary>
        internal static void Load()
        {
            try
            {
                CheckDirectories();

                foreach (string file in Directory.GetFiles(DependenciesDirectoryPath, "*.dll"))
                {
                    Assembly.LoadFile(file);
                }
                ScreenLogger.Load();

                GlobalLogger.Log("TEST", "SUCCEED");
            }
            catch(Exception ex)
            {
                GlobalLogger.Log(GlobalLogger.ENGINE_PREFIX, ex.ToString());
            }
           
        }

        /// <summary>
        /// Check if directories are present, otherwise create the directories.
        /// </summary>
        private static void CheckDirectories()
        {
            if (!Directory.Exists(EngineDirectoryPath))
            {
                Directory.CreateDirectory(EngineDirectoryPath);
            }

            if (!Directory.Exists(DependenciesDirectoryPath))
            {
                Directory.CreateDirectory(DependenciesDirectoryPath);
            }

            if (!Directory.Exists(ModsDirectoryPath))
            {
                Directory.CreateDirectory(ModsDirectoryPath);
            }
        }

        [HarmonyPatch(typeof(MainMenu), "Awake")]
        static class LoadEverything_Patch
        {
            static void Postfix()
            {
                Load();
            }
        }
    }
}
