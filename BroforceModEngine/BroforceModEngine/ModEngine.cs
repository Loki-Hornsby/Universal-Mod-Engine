using System;
using System.IO;
using HarmonyLib;
using System.Reflection;
using System.Text;
using Unity = UnityEngine;

using BroforceModEngine.Logging;

/// <summary>
/// The engine - Mainly used for testing at the moment (needs tidying)
/// </summary>

namespace BroforceModEngine
{
    /// <summary>
    /// 
    /// </summary>
    public static class ModEngine
    {
        /// BroforceModEngine main directory
        public static string EngineDirectoryPath { get; private set; }

        /// BroforceModEngine dependencies directory
        public static string DependenciesDirectoryPath { get; private set; }

        /// BroforceModEngine mods directory
        public static string ModsDirectoryPath { get; private set; }

        internal static Harmony harmony;

        // Loaded boolean
        public static bool _loaded = false;

        /// <summary>
        /// Load Engine
        /// </summary>
        internal static void Load() {
            Logger.Log("Loading Mod Engine...", Logger.LogType.None);

            try {
                // Directories
                CheckDirectories();

                // Load all assemblies
                foreach (string file in Directory.GetFiles(DependenciesDirectoryPath, "*.dll")){
                    Assembly.LoadFile(file);
                }

                // Enable Harmony and logging
                Harmony.DEBUG = true;

                harmony = new Harmony("com.example.patch");
                var assembly = Assembly.GetExecutingAssembly();
                harmony.PatchAll(assembly);

                // Finished Loading
                Logger.Log("Loaded Mod Engine...");

                _loaded = true;
            } catch(Exception ex){
                Logger.Log(ex.ToString());

                _loaded = true;
            }

            // Load recursively until success or fail
            if (!_loaded){
                Load();
            } else {
                Logger.Log("Mod Engine Started.");
            }
        }

        /// <summary>
        /// Check if directories are present, otherwise create the directories. //Unused: To be replaced by GUI
        /// </summary>
        
        private static void CheckDirectories(){
            EngineDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "BroforceModEngine");
            DependenciesDirectoryPath = EngineDirectoryPath;
            ModsDirectoryPath = Path.Combine(EngineDirectoryPath, "Mods");

            if (!Directory.Exists(EngineDirectoryPath)){
                Directory.CreateDirectory(EngineDirectoryPath);
            }

            if (!Directory.Exists(DependenciesDirectoryPath)){
                Directory.CreateDirectory(DependenciesDirectoryPath);
            }

            if (!Directory.Exists(ModsDirectoryPath)){
                Directory.CreateDirectory(ModsDirectoryPath);
            }
        }

        /*
        /// <summary>
        /// Load Mods
        /// </summary>
        /// https://github.com/Gorzon38/BF-CODE/blob/main/BF-1131/Assembly-CSharp/Menu.cs
        [HarmonyPatch(typeof(Menu), "Awake")] // typeof(MainMenu), "Awake" or "Start"
        class LoadEverything_Patch
        {
            public static void Postfix()
            {
                // Screen logger
                ScreenLogger.Load();
            }
        }*/

        static void CauseError(){
            
        }

        /// <summary>
        /// Having some fun
        /// </summary>
        /// https://github.com/Gorzon38/BF-CODE/blob/main/BF-1131/Assembly-CSharp/Menu.cs
        [HarmonyPatch(typeof(MainMenu), "Awake")] // typeof(MainMenu), "Awake" or "Start"
        class LoadEverything_Patch
        {
            public static void Postfix()
            {
                CauseError();
            }
        }
    }
}
