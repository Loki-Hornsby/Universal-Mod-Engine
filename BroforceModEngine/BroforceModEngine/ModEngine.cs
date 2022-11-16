using System;
using System.IO;
using HarmonyLib;
using System.Reflection;
using System.Text;

using Injection;

/// <summary>
/// The engine - Mainly used for testing at the moment (needs tidying)
/// This is going to be our replacement for doorstop
/// https://www.unknowncheats.me/forum/general-programming-and-reversing/209134-injecting-dll-unity3d-game.html
/// https://www.unknowncheats.me/forum/general-programming-and-reversing/176942-accessing-mono-loading-assemblies.html
/// </summary>

namespace BroforceModEngine {
    public static class ModEngine {
        /// BroforceModEngine mods directory
        public const string ModsDirectoryPath = @"bin\"; // Unknown? yet to be implemented

        internal static Harmony harmony;

        public static int Main(string[] args){
            System.Console.WriteLine("Mod Engine Compiled.");

            return 0;
        }

        /// <summary>
        /// Load Engine
        /// </summary>
        internal static void Load(string InjDLL, string BroDLL) {
            Logger.Log("Starting...", 1);

            try {
                // Load all assemblies
                foreach (string file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")){
                    try {
                        Assembly.LoadFile(file);
                    } catch (Exception ex) {
                        Logger.Log("Assembly Load Fail: " + ex.ToString(), 3);
                    }
                }

                // Enable Harmony and logging
                //Harmony.DEBUG = true;

                //harmony = new Harmony("com.example.patch");
                //var assembly = Assembly.GetExecutingAssembly();
                //harmony.PatchAll(assembly);

                // Inject
                Logger.Log(Injector.Inject(InjDLL, BroDLL).ToString(), 3);
            } catch(Exception ex){
                Logger.Log("Internal Mod Engine Load Fail: " + ex.ToString(), 3);
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
        /*[HarmonyPatch(typeof(MainMenu), "Awake")] // typeof(MainMenu), "Awake" or "Start"
        class LoadEverything_Patch {
            public static void Postfix() {
                CauseError();
            }
        }*/
    }
}
