using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using BroforceModSoftware.Interaction.Back;

/// <summary>
/// Launches the GUI
/// Taskkill /IM BroMods.exe /F 
/// </summary>

namespace BroforceModSoftware {
    class Program {
        // The main entry point for the application.
        [STAThread]
        static void Main() {   
            bool exists = (System.Diagnostics.Process.GetProcessesByName(
                System.IO.Path.GetFileNameWithoutExtension(
                    System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1);

            if (!exists){
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new GUI());
            } else {
                MessageBox.Show("There is already another instance of this application running!", "Warning!", MessageBoxButtons.OK);
                System.Windows.Forms.Application.Exit();
            }
        }
    }
}