/// <summary>
/// Copyright 2022, Loki Alexander Button Hornsby (Loki Hornsby), All rights reserved.
/// Licensed under the BSD 3-Clause "New" or "Revised" License
/// </summary>

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
using System.Threading;

using Software;
using Injection;
using Interfaces;

/// <summary>
/// The mod engine
/// </summary>

namespace Engine {
    public static class Engine {
        // GUI
        public static GUI gui;

        // Debug
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
        /// Start the GUI
        /// </summary>
        static void StartGUI(){
            // Setup Application
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();

            // Create GUI on a separate thread
            new Thread(
                delegate(){
                    gui = new GUI();
                }
            ).Start();

            // Subscribe to interface event
            Toolbar.InterfaceChanged += (x) => StartEngine(x.GetDefs());
        }

        /// <summary>
        /// Resolve DLLs
        /// </summary>
        private static Assembly ResolveDLL(object sender, ResolveEventArgs args){
            Logger.Log("TRYING TO RESOLVE " + args.Name, Logger.LogType.Warning, Logger.VerboseType.Low);

            if (args.Name.Contains("Injector")) {
                return Assembly.LoadFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Injector.dll"));
            } else {
                Logger.Log("Skipped resolving a dll...", Logger.LogType.Warning, Logger.VerboseType.Low);
                return null;
            }
        }

        /// <summary>
        /// Start the Engine
        /// </summary>
        public static void StartEngine(DLLDefinitions Defs) {
            try {
                // Resolve DLLs
                AppDomain.CurrentDomain.AssemblyResolve += (x, y) => ResolveDLL(x, y); 

                // Load all assemblies
                foreach (string file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")){
                    try {
                        Assembly.LoadFile(file);
                    } catch (Exception ex) {
                        Logger.Log("Assembly Load Fail: " + ex.ToString(), Logger.LogType.Error, Logger.VerboseType.Low);
                    }
                }

                // Load Injector
                Injector.Inject(Defs);

                // Cowboy bebop baby!
                Logger.Log("I think it's time we blow this scene,", Logger.LogType.Warning, Logger.VerboseType.Low);
                Logger.Log("Get everybody and the stuff together,", Logger.LogType.Warning, Logger.VerboseType.Low);
                Logger.Log("Okay, three, two, one, let's jam...", Logger.LogType.Warning, Logger.VerboseType.Low);
                Logger.Log("Engine and interface loaded successfully!", Logger.LogType.Success, Logger.VerboseType.Low);
            } catch(Exception ex){
                Logger.Log("Engine Failed to Load!", Logger.LogType.Error, Logger.VerboseType.Low);
                Logger.Log(ex.ToString(), Logger.LogType.Error, Logger.VerboseType.Low);
            }
        }

        /// <summary>
        /// Entry point
        /// </summary>
        [STAThread] // [STAThread] allows the GUI to run parallel to the program when starting a new thread
        static void Main(string[] args){
            // Instance of GUI already exists
            bool exists = (System.Diagnostics.Process.GetProcessesByName(
                System.IO.Path.GetFileNameWithoutExtension(
                    System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1);

            // If an instance doesn't already exist
            if (!exists){
                // Debug (Creates a console and enables debug boolean if user selects yes)
                debug = QueryDebugMode();
                if (debug) AllocConsole();

                // Start the GUI
                StartGUI();
            } else {
                MessageBox.Show(
                    "An instance of the UME is already running! Please close it and try again.", 
                    "Warning!", 
                    MessageBoxButtons.OK
                );
            }
        }
    }
}
