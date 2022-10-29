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
            // Text
            Label Text = new Label();

            // Text Properties
            Text.AutoSize = false;
            Text.TextAlign = ContentAlignment.MiddleCenter;
            Text.Dock = DockStyle.Fill;
           
            if (!IsAdministrator()){
                // Text
                Text.Text = "DROP BROFORCE.EXE HERE BRO!";

                // Drag and Drop functionality
                Text.AllowDrop = true;
                Text.DragEnter += (sender, e) => GUI_Helpers.DragEnter(sender, e);
                Text.DragDrop += (sender, e) => GUI_Helpers.DragDrop(sender, e, this);

                // Open File Browser
                GUI_Helpers.OpenFileExplorerAsAdmin();
            } else {
                // Text
                Text.Text = "PLEASE RUN THIS APPLICATION WITHOUT ADMINISTRATOR BRO!";
            }

            // Add text to form
            this.Controls.Add(Text);

            // Form Sizing
            this.MinimumSize = new System.Drawing.Size(
                1, 
                1
            );

            this.MaximumSize = new System.Drawing.Size(
                Screen.PrimaryScreen.Bounds.Width / 4,
                Screen.PrimaryScreen.Bounds.Height / 4
            );

            this.AutoSize = false;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Width = Screen.PrimaryScreen.Bounds.Width / 4;
            this.Height = Screen.PrimaryScreen.Bounds.Height / 4;

            // Show form
            this.Shown += (sender, e) => GUI_Helpers.ForceShow(sender, e);
        }

        public Form1(){
            InitializeComponent();
            InitializeUI();
        }
    }
}
