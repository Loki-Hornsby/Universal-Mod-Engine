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

using Software;
using Software.Interaction.Front;
using Software.Interaction.Back;

namespace Software {
    public static class Window {
        /// <summary>
        /// Bring the window to the front
        /// </summary>
        static void ForceShow(object sender, EventArgs e) {
            Form form = sender as Form;

            form.Focus();
            form.BringToFront();
            form.Activate();
        }

        /// <summary>
        /// Window Setup
        /// </summary>
        public static void Setup(Form form){
            // Text
            form.Text = "Universal Mod Loader";

            // Form Sizing
            int x = Screen.PrimaryScreen.Bounds.Width;
            int y = Screen.PrimaryScreen.Bounds.Height;

            form.MinimumSize = new System.Drawing.Size(
                x/4,
                y/4
            );

            form.MaximumSize = new System.Drawing.Size(
                x,
                y
            );

            form.Size = new System.Drawing.Size(
                x/4,
                y/4
            );

            form.AutoSize = false;

            // Show form
            form.Shown += (sender, e) => ForceShow(sender, e);
        }
    }
}