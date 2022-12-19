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
using System.Runtime.InteropServices;

using Software;

/// <summary>
/// The mod engine
/// </summary>

namespace Engine {
    public static class Engine {
        // GUI
        public static GUI gui;

        // DEBUG
        public static bool debug;

        // Console
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        /// <summary>
        /// Queries wether the user wants to enable debug mode
        /// </summary>
        static bool QueryDebugMode(){
            DialogResult dr = MessageBox.Show("Hi! Do you want to run this application in debug mode?", 
                      "Query", MessageBoxButtons.YesNo);

            return ((dr == DialogResult.Yes) ? true : false);
        }

        /// <summary>
        /// Creates a console
        /// </summary>
        static void SetupGUI(){
            // Setup Application
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();

            // Create GUI
            gui = new GUI();
            bool setup = gui.Setup();

            // Launch
            Application.Run(gui);

            //System.Threading.Thread.Sleep(5000);

            Logger.Log("Starting...", Logger.LogType.Error, Logger.VerboseType.Low);
        }

        /// <summary>
        /// Entry point
        /// </summary>
        static void Main(string[] args){
            // Debug
            debug = QueryDebugMode();
            if (debug) AllocConsole();

            // Instance of GUI already exists
            bool exists = (System.Diagnostics.Process.GetProcessesByName(
                System.IO.Path.GetFileNameWithoutExtension(
                    System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1);

            // GUI
            if (!exists){
                SetupGUI();
            }
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
            System.Console.WriteLine("£dsds");
            Logger.Log("Starting...", Logger.LogType.Error, Logger.VerboseType.Low);
            Logger.SetLogColor(Color.Red);

            try {
                // Interface
                //InterfaceLoader.Setup();

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
