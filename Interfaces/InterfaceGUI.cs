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
    public partial class InterfaceGUI : Form {
        public TextBox textBox;

        /// <summary>
        /// Setup The GUI
        /// </summary>
        public InterfaceGUI(){
            this.textBox = new System.Windows.Forms.TextBox();
        }
    }
}