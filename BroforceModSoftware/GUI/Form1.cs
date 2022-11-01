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

using BroforceModSoftware.Helpers;
using BroforceModEngine.Handling;

namespace BroforceModSoftware.GUI {
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
            TextBox Text = new TextBox();

            // Text Properties
            Text.AcceptsReturn = true;
            Text.Multiline = true;
            Text.ScrollBars = ScrollBars.Vertical;
            Text.AutoSize = false; 
            Text.Anchor = AnchorStyles.None;
            Text.Dock = DockStyle.Fill;
            Text.Width = x;
            Text.Height = y;
            Text.ReadOnly = true;
            Text.BackColor = Color.Green;
           
            if (!IsAdministrator()){
                if (!Data.EXE.GetExeStorage()){
                    // Text
                    Text.AppendText("DRAG AND DROP BROFORCE.EXE HERE BRO!");

                    // Drag and drop
                    Text.AllowDrop = true;

                    // Open File Browser
                    GUI_Helpers.Visuals.OpenFileExplorerAsAdmin();
                } else {
                    // Text
                    Text.AppendText(GUI_Helpers.Effects.GetEXEON());

                    // Drag and drop
                    Text.AllowDrop = false;
                }

                // Drag and Drop functionality
                Text.DragEnter += (sender, e) => GUI_Helpers.Files.DragEnter(sender, e);
                Text.DragDrop += (sender, e) => GUI_Helpers.Files.DragDrop(sender, e, this);
            } else {
                // Text
                Text.AppendText("PLEASE RUN THIS APPLICATION WITHOUT ADMINISTRATOR BRO!");
            }

            // Add text to form
            this.Controls.Add(Text);

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
            this.Shown += (sender, e) => GUI_Helpers.Visuals.ForceShow(sender, e);
        }

        public Form1(){
            InitializeComponent();
            InitializeUI();
        }
    }
}
