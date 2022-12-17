using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;  
using System.Text;
using System.Windows.Forms;
using System.Media;
using System;

/// <summary>
/// Handles the software's GUI ~ Front end and Back end
/// </summary>

namespace Software {
    public partial class GUI : Form {
        /// <summary>
        /// Start The GUI
        /// </summary>
        public void Start(){
            
        }

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
            // Just a precautionary step
                // We don't want any nasty bugs to go unnoticed

            bool setup = Setup();

            if (setup){
                Start();
            }
        }
    }
}