using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;  
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.Diagnostics;
using System.Security.Principal;
using System.IO;
using System.Runtime.CompilerServices;
using System.Reflection;

//using Injection;
using ModInterface;
using Software;

/// <summary>
/// The mod engine
/// </summary>

namespace Engine {
    public static class Engine {
        /// <summary>
        /// Entry point
        /// </summary>
        public static int Main(string[] Args){
            return 0;
        }

        /// <summary>
        /// Resolve DLLs
        /// </summary>
        private static Assembly ResolveDLL(object sender, ResolveEventArgs args){
            //Logger.log("TRYING TO RESOLVE " + args.Name, 3);

            if (args.Name.Contains("Injector")) {
                return Assembly.LoadFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Injector.dll"));
            } else {
                return null;
            }
        }

        /// <summary>
        /// Load Engine
        /// </summary>
        public static void Load() {
            Logger.Log("Starting...", Logger.LogType.Error, Logger.VerboseType.Low);

            try {
                // Interface
                InterfaceLoader.Setup();

                // Resolve DLLs
                AppDomain.CurrentDomain.AssemblyResolve += (x, y) => ResolveDLL(x, y); 

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
