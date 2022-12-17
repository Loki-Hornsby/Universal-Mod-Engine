using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

using Software.Interaction.Back;
using Software.Interaction.Front;

/// <summary>
/// Build Order: Injector, Engine, GUI
/// Launches the GUI
/// </summary>

namespace Software {
    class Program {
        // The main entry point for the application.
        [STAThread]
        static void Main() {   
            bool exists = (System.Diagnostics.Process.GetProcessesByName(
                System.IO.Path.GetFileNameWithoutExtension(
                    System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1);

            if (!BI.InstanceIsRunning(BI.EXE.GetLocation())){
                if (!exists){
                    Application.SetHighDpiMode(HighDpiMode.SystemAware);
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new GUI());
                } else {
                    // FI.Visuals.ExitWithMessageBox("There is already another instance of this application running!");
                }
            }
        }
    }
}