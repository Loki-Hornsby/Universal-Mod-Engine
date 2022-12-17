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
    public static class Config {
        public static Window window;
        public static Logger logger;
        public static Toolbar toolbar;

        public static void InputData(Window _window, Logger _logger, Toolbar _toolbar){
            window = _window;
            logger = _logger;
            toolbar = _toolbar;
        }
    }

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
                Window window = new Window(this);

                // Logger
                Logger logger = new Logger(this);

                // Toolbar
                Toolbar toolbar = new Toolbar(this); 

                // Config
                Config.InputData(window, logger, toolbar);

                return true;
            } catch (Exception ex) {
                Config.logger.Log(ex.ToString(), Logger.LogType.Error, Logger.VerboseType.High);

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