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

using BroforceModSoftware;
using BroforceModSoftware.Helpers;
using BroforceModEngine.Handling;

/// <summary>
/// Handles the software's GUI
/// </summary>

namespace BroforceModSoftware {
    // https://stackoverflow.com/questions/18726852/redirecting-console-writeline-to-textbox
    public class ConsoleLogger : TextWriter {
        private Control textbox;

        public ConsoleLogger(Control textbox){
            this.textbox = textbox;
        }

        public override void Write(char value){
            Logger.Log(value.ToString(), Logger.TxtBox.BackColor);
        }

        public override void Write(string value){
            Logger.Log(value, Logger.TxtBox.BackColor);
        }

        public override Encoding Encoding {
            get { return Encoding.ASCII; }
        }
    }

    public static class Logger {
        public static TextBox TxtBox;
        static Form form;

        /// <summary>
        /// Initialises Logger
        /// </summary>
        /// <param name="f"></param>
        public static void Init(Form f){
            // Form
            form = f;

            // Text Box
            TxtBox = new TextBox();

            // Text Properties
            TxtBox.AcceptsReturn = true;
            TxtBox.Multiline = true;
            TxtBox.ScrollBars = ScrollBars.Vertical;
            TxtBox.AutoSize = false; 
            TxtBox.Anchor = AnchorStyles.None;
            TxtBox.Dock = DockStyle.Fill;
            TxtBox.ReadOnly = true;
            TxtBox.BackColor = Color.Green;

            // Drag and Drop functionality
            TxtBox.DragEnter += (sender, e) => GUI_Helpers.Files.DragEnter(sender, e);
            TxtBox.DragDrop += (sender, e) => GUI_Helpers.Files.DragDrop(sender, e, form);

            // Set output to textbox
            Console.SetOut(new ConsoleLogger(TxtBox));

            // Add text to form
            form.Controls.Add(TxtBox);
        }

        /// <summary>
        /// Outputs to log
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="col"></param>
        public static void Log(string txt, Color col){
            TxtBox.AppendText(txt + Environment.NewLine);
            TxtBox.BackColor = col;
        }
        
        /// <summary>
        /// Changes color of log
        /// </summary>
        /// <param name="col"></param>
        public static void ChangeLogColor(Color col){
            TxtBox.BackColor = col;
        }

        /// <summary>
        /// Adds new empty line to log
        /// </summary>
        public static void AddNewLine(){
            TxtBox.AppendText(Environment.NewLine);
        }

        /// <summary>
        /// Allows upload of files to log
        /// </summary>
        /// <param name="x"></param>
        public static void AllowDragAndDrop(bool x){
            TxtBox.AllowDrop = x;
        }
    }
}

namespace BroforceModSoftware {
    public partial class GUI : Form {
        // Check if program is admin
        public static bool IsAdministrator(){
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public void InitializeUI(){
            int x = Screen.PrimaryScreen.Bounds.Width / 4;
            int y = Screen.PrimaryScreen.Bounds.Height / 4;
           
            if (!IsAdministrator()){
                if (Data.EXE.GetExeLocation() == ""){
                    // Text
                    Logger.Log("DRAG AND DROP BROFORCE.EXE HERE BRO!", Color.Green);

                    // Drag and drop
                    Logger.AllowDragAndDrop(true);

                    // Open File Browser
                    GUI_Helpers.Visuals.OpenFileExplorerAsAdmin();
                } else {
                    // Text
                    Logger.Log(GUI_Helpers.Effects.GetEXEON(), Color.Green);

                    // Drag and drop
                    Logger.AllowDragAndDrop(false);
                }
            } else {
                // Text
                Logger.Log("PLEASE RUN THIS APPLICATION WITHOUT ADMINISTRATOR BRO!", Color.Red);
            }

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

        public GUI(){
            Logger.Init(this);
            InitializeComponent();
            InitializeUI();
        }
    }
}
