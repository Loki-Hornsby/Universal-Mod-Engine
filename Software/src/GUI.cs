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

namespace Software {
    public partial class GUI : Form {
        /// <summary>
        /// Setup The GUI
        /// </summary>
        public GUI(){
            // Window
            Window.Setup(this);

            // Logger
            Logger.Setup(this);

            // Toolbar
            Toolbar.Setup(this); 

            // Start GUI
            Application.Run(this);
        }
    }
}