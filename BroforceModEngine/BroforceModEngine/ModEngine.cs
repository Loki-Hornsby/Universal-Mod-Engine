using System;
using System.IO;
using BroforceModEngine.Loggers;
using HarmonyLib;
using HarmonyLib.Tools;
using System.Reflection;
using System.Text;

namespace BroforceModEngine
{
    /// <summary>
    /// 
    /// </summary>
    public static class ModEngine
    {
        /// <summary>
        /// BroforceModEngine main directory
        /// </summary>
        public static string EngineDirectoryPath { get; private set; }
       /* /// <summary>
        /// BroforceModEngine dependencies directory
        /// </summary>
        public static string DependenciesDirectoryPath { get; private set; }*/
        /// <summary>
        /// BroforceModEngine mods directory
        /// </summary>
        public static string ModsDirectoryPath { get; private set; }

        //internal static Harmony harmony;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logType"></param>
        internal static void EngineLog(object message, LogType logType = LogType.Log)
        {
            GlobalLogger.Log(GlobalLogger.ENGINE_PREFIX, message.ToString(), logType);
        }

        /// <summary>
        /// Load everything
        /// </summary>
        internal static void Load()
        {
            try
            {
                CheckDirectories();
                EngineLog("ModEngine loaded");
                /*foreach (string file in Directory.GetFiles(DependenciesDirectoryPath, "*.dll"))
                {
                    Assembly.LoadFile(file);
                }*/
                //ScreenLogger.Load();
               /* HarmonyFileLog.Enabled = true; 
                ModEngine.harmony = new Harmony("BroforceModEngine");
                ModEngine.harmony.PatchAll();*/

                EngineLog("ModEngine loaded");
            }
            catch(Exception ex)
            {
                EngineLog(ex.ToString(), LogType.Error);
            }
        }
        

        /// <summary>
        /// Check if directories are present, otherwise create the directories.
        /// </summary>
        private static void CheckDirectories()
        {
            EngineDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "BroforceModEngine");
            //DependenciesDirectoryPath = EngineDirectoryPath;
            ModsDirectoryPath = Path.Combine(EngineDirectoryPath, "Mods");

            if (!Directory.Exists(EngineDirectoryPath))
            {
                Directory.CreateDirectory(EngineDirectoryPath);
            }

           /* if (!Directory.Exists(DependenciesDirectoryPath))
            {
                Directory.CreateDirectory(DependenciesDirectoryPath);
            }*/

            if (!Directory.Exists(ModsDirectoryPath))
            {
                Directory.CreateDirectory(ModsDirectoryPath);
            }

        }

       /* [HarmonyPatch(typeof(MainMenu), "Awake")]
        static class LoadEverything_Patch
        {
            static void Postfix()
            {
                EngineLog("HARMONY!");
            }
        }*/
    }
}
