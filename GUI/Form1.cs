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

        public void OpenFileExplorerAsAdmin(){
            Process proc = new Process();
            proc.StartInfo.FileName = "Explorer.exe";
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.Start();
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
                Text.Text = "Drop Broforce.exe here bro!";

                // Drag and Drop functionality
                Text.AllowDrop = true;
                Text.DragEnter += new DragEventHandler(GUI_Helpers.Form_DragEnter);
                Text.DragDrop += new DragEventHandler(GUI_Helpers.Form_DragDrop);

                // Effects
                string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                SoundPlayer simpleSound = new SoundPlayer(@"DragDropSound.wav"); // The "." is vital
                simpleSound.Play();

                Text.DragDrop += (sender, e) => ThreadHandling.QueueTask(GUI_Helpers.Shake(this, 100, 10));
                Text.DragDrop += (sender, e) => simpleSound.Play();
                Text.DragDrop += (send, e) => ThreadHandling.ExecuteTasks();

                // This has to be admin to match drag and drop permissions
                OpenFileExplorerAsAdmin();
            } else {
                // Text
                Text.Text = "Please run this application without administrator bro!";
            }

            // Add text to form
            this.Controls.Add (Text);

            // Form Sizing
            this.MinimumSize = new System.Drawing.Size(
                Screen.PrimaryScreen.Bounds.Width / 4, 
                Screen.PrimaryScreen.Bounds.Height / 4
            );

            this.MaximumSize = new System.Drawing.Size(
                Screen.PrimaryScreen.Bounds.Width / 4,
                Screen.PrimaryScreen.Bounds.Height / 4
            );

            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }

        public Form1(){
            InitializeComponent();
            InitializeUI();
        }
    }
}
