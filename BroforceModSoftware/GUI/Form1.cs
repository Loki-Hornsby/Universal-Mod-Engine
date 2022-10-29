using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Diagnostics;
using System.Security.Principal;
using System.IO;

namespace BROMODS {
    public partial class Form1 : Form {
        // Check if program is admin
        public static bool IsAdministrator(){
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public void InitializeUI(){
            int x = Screen.PrimaryScreen.Bounds.Width / 4;
            int y = Screen.PrimaryScreen.Bounds.Height / 4;

            // Text
            Label TextLeft = new Label();
            Label TextRight = new Label();

            // Text Properties
            TextLeft.AutoSize = false; // false
            TextLeft.TextAlign = ContentAlignment.MiddleCenter;
            TextLeft.Anchor = AnchorStyles.None;
            TextLeft.Dock = DockStyle.Left;
            TextLeft.Width = x / 2;
            TextLeft.Height = y;

            TextRight.AutoSize = false; // false
            TextRight.TextAlign = ContentAlignment.MiddleCenter;
            TextRight.Anchor = AnchorStyles.None;
            TextRight.Dock = DockStyle.Right;
            TextRight.Width = x / 2;
            TextRight.Height = y;
           
            if (!IsAdministrator()){
                // Text
                TextLeft.Text =  "DRAG AND DROP BROFORCE.EXE HERE BRO!";
                TextRight.Text = "DELETE MODS HERE BRO!";

                // Drag and Drop functionality
                TextLeft.AllowDrop = true;
                TextLeft.DragEnter += (sender, e) => GUI_Helpers.DragEnter(sender, e);
                TextLeft.DragDrop += (sender, e) => GUI_Helpers.DragDrop(sender, e, this);

                //TextRight.AllowDrop = true;
                //TextRight.DragEnter += (sender, e) => GUI_Helpers.DragEnter(sender, e);
                //TextRight.DragDrop += (sender, e) => GUI_Helpers.DragDrop(sender, e, this);

                // Open File Browser
                //GUI_Helpers.OpenFileExplorerAsAdmin();
            } else {
                // Text
                TextLeft.Text = "PLEASE RUN THIS APPLICATION WITHOUT ADMINISTRATOR BRO!";
                TextRight.Text = "OTHERWISE STUFF BREAKS BRO!";
            }

            // Add text to form
            this.Controls.Add(TextLeft);
            this.Controls.Add(TextRight);

            // Form Sizing
            this.MinimumSize = new System.Drawing.Size(
                x,
                y
            );

            this.MaximumSize = new System.Drawing.Size(
                x,
                y
            );

            this.AutoSize = false;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            // Show form
            this.Shown += (sender, e) => GUI_Helpers.ForceShow(sender, e);
        }

        public Form1(){
            InitializeComponent();
            InitializeUI();
        }
    }
}
