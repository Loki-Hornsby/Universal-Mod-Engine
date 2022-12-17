using System;
using System.IO;
using System.Reflection;
using System.Text;

using Injection;

/// <summary>
/// The engine - Mainly used for testing at the moment (needs tidying)
/// </summary>

namespace Engine {
    public static class Engine {
        /// Engine mods directory
        public const string ModsDirectoryPath = @"bin\"; // Unknown? yet to be implemented

        public static int Main(string[] args){
            System.Console.WriteLine("Engine Compiled.");

            return 0;
        }

        /// <summary>
        /// Load Engine
        /// </summary>
        internal static void Load(string BroDLL) {
            //Logger.log("Starting...", 1);

            try {
                // Load all assemblies
                foreach (string file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")){
                    try {
                        Assembly.LoadFile(file);
                    } catch (Exception ex) {
                        //Logger.log("Assembly Load Fail: " + ex.ToString(), 3);
                    }
                }

                // Inject
                //Logger.log(Injector.Inject(BroDLL).ToString(), 3);
            } catch(Exception ex){
                //Logger.log("Internal Engine Load Fail: " + ex.ToString(), 3);
            }
        }
    }
}
