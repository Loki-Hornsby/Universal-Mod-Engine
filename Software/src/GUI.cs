using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;  
using System.Text;
using System.Windows.Forms;
using System.Media;
using System;

using Engine;

/// <summary>
/// Handles the software's GUI ~ Front end and Back end
/// </summary>

namespace Software {
    public partial class GUI : Form {
        /// <summary>
        /// Setup The GUI
        /// </summary>
        public bool Setup(){
            try {
                // Window
                Window.Setup(this);

                // Logger
                Logger.Setup(this);

                // Toolbar
                Toolbar.Setup(this); 

                return true;
            } catch (Exception ex) {
                Logger.Log(ex.ToString(), Logger.LogType.Error, Logger.VerboseType.Low);

                return false;
            }
        }

        /// <summary>
        /// GUI constructor
        /// </summary>
        public GUI(){
            // If the GUI launches then start the engine
            if (Setup()){
                Engine.Engine.Load();
            } else {
                Logger.Log("The engine failed to start!", Logger.LogType.Error, Logger.VerboseType.Low);
            }
        }
    }
}