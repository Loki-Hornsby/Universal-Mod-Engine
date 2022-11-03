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
using System.Runtime.CompilerServices;

using BroforceModSoftware;
using BroforceModSoftware.Interaction.Front;
using BroforceModSoftware.Interaction.Back;

/// <summary>
/// Handles the software's GUI ~ Front end and Back end
/// </summary>

namespace BroforceModSoftware {
    // https://stackoverflow.com/questions/18726852/redirecting-console-writeline-to-textbox
    public class ConsoleLogger : TextWriter {
        private Control textbox;

        public ConsoleLogger(Control textbox){
            this.textbox = textbox;
        }

        public override void Write(char value){
            Logger.Log("[Message from System] " + value.ToString(), Logger.LogType.Default, Logger.VerboseType.High);
        }

        public override void Write(string value){
            Logger.Log("[Message from System] " + value, Logger.LogType.Default, Logger.VerboseType.High);
        }

        public override Encoding Encoding {
            get { return Encoding.ASCII; }
        }
    }

    public static class Logger {
        public enum LogType {
            Error,
            Warning,
            Success,
            Custom,
            Default
        }

        public enum VerboseType {
            High = 3,
            Medium = 2,
            Low = 1
        }

        public static TextBox TxtBox;
        public static Form form;

        static VerboseType verbosity;

        /// <summary>
        /// Initialises Logger
        /// </summary>
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
            TxtBox.DragEnter += (sender, e) => FI.DragAndDrop.Enter(sender, e);
            TxtBox.DragDrop += (sender, e) => FI.DragAndDrop.Drop(sender, e, form);

            // Set output to textbox
            Console.SetOut(new ConsoleLogger(TxtBox));

            // Add text to form
            form.Controls.Add(TxtBox);
        }

        /// <summary>
        /// Outputs to log
        /// </summary>
        public static void Log(
                string txt, 
                LogType type, 
                VerboseType v,
                Color? col = null, 
                [CallerFilePath] string file = "", 
                [CallerMemberName] string member = "", 
                [CallerLineNumber] int line = 0
            ){
            
            if ((int)v <= (int)verbosity){
                switch (type){
                    case LogType.Error:
                        col = Color.Red;
                        break;
                    case LogType.Warning:
                        col = Color.Yellow;
                        break;
                    case LogType.Success:
                        col = Color.Green;
                        break;
                    case LogType.Custom:
                        col = col;
                        break;
                    case LogType.Default:
                        col = Logger.TxtBox.BackColor;
                        break;
                }

                string RebuiltMessage = String.Format("[{0}]@[{1}/{2}/{3}]: {4}",
                    type.ToString(), Path.GetFileName(file), member, line, txt) + Environment.NewLine;          

                TxtBox.AppendText(RebuiltMessage);
                TxtBox.BackColor = col ?? Color.Black;
            }
        }
        
        /// <summary>
        /// Changes color of log
        /// </summary>
        public static void SetLogColor(Color col){
            TxtBox.BackColor = col;
        }

        /// <summary>
        /// Change verbosity of logger
        /// </summary>
        public static void SetVerbosity(VerboseType v){
            verbosity = v;
            string s = "";

            switch (v){
                case VerboseType.High:
                    s = "[High]";
                    break;
                case VerboseType.Medium:
                    s = "[Medium]";
                    break;
                case VerboseType.Low:
                    s = "[Low]";
                    break;
            }

            Logger.Log("Set Verbose Level to " + s, Logger.LogType.Default, Logger.VerboseType.Low);
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
        public static void AllowDragAndDrop(bool x){
            TxtBox.AllowDrop = x;
        }
    }
}

namespace BroforceModSoftware {
    public partial class GUI : Form {
        /// <summary>
        /// Check if program is admin
        /// </summary>
        public static bool IsAdministrator(){
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// Log the launch info
        /// </summary>
        public void GenerateLaunchInfo(object? sender, DragEventArgs? e){
            bool fail = false;
            bool retry = false;
            string s = "";

            if (File.Exists(BI.EXE.StorageFilePath)){ // STORE.txt exists?
                s += "LOCATED STORE.TXT (" + Path.GetFullPath(BI.EXE.StorageFilePath) + ")";
            } else {
                fail = true;
                s += "STORE.TXT IS MISSING";
            }

            Logger.Log(s, (fail) ? Logger.LogType.Error : Logger.LogType.Success, Logger.VerboseType.Medium);
            s = "";

            if (File.Exists(BI.EXE.GetExeLocation())){ // STORE.txt contents valid?
                s += 
                "LOCATED BROFORCE.EXE (" + 
                BI.EXE.GetExeLocation() + ")" +
                Environment.NewLine;
            } else {
                fail = true;

                s += BI.EXE.GetExeLocation() + " IS AN INVALID PATH FOR BROFORCE.EXE";
            }

            Logger.Log(s, (fail) ? Logger.LogType.Error : Logger.LogType.Success, Logger.VerboseType.Medium);
            s = "";

            if (!fail){ // no fails occurred?
                s += 
                Environment.NewLine + 
                "THIS WILL LOG ANYTHING YOU NEED TO SEE WHILE PLAYING BRO!" + 
                Environment.NewLine + 
                "REMEMBER TO LAUNCH THIS APP EACH TIME YOU WANT TO USE MODS BRO!" +
                Environment.NewLine + 
                "NOW PLAY SOME BROFORCE BRO!";
            } else {
                if (File.Exists(BI.EXE.StorageFilePath)){ 
                    // Todo - create this functionality below (don't close the application)
                    s += "FATAL ERRORS OCCURED BRO! DELETING STORE.TXT AND RELOADING...";

                    File.Delete(BI.EXE.StorageFilePath);
                } else {
                    retry = true;

                    s += "ASKING USER FOR EXE LOCATION...";
                }
            }

            Logger.Log(s, (fail) ? Logger.LogType.Error : Logger.LogType.Success, Logger.VerboseType.Medium);
            if (retry) InitializeUI();

            if (!fail && !retry && BI.EXE.IsInUse()){ // Broforce is launched or incorrect exe was chosen?
                Logger.Log("EITHER BROFORCE IS CURRENTLY RUNNING OR YOU CHOSE THE WRONG EXE BRO! (TRY USING OPTIONS/RESET BRO!)", Logger.LogType.Error, Logger.VerboseType.Low);
            }
        }

        /// <summary>
        /// Reset STORE.txt
        /// </summary>
        void Reset(object sender, EventArgs e){
            if (File.Exists(BI.EXE.StorageFilePath)){ 
                Logger.Log("RESETTING STORE.TXT!", Logger.LogType.Success, Logger.VerboseType.Medium);

                File.Delete(BI.EXE.StorageFilePath);
            } else {
                Logger.Log("ALREADY RESET STORE.TXT!", Logger.LogType.Warning, Logger.VerboseType.Medium);
            }

            Logger.Log("RESETTING...", Logger.LogType.Success, Logger.VerboseType.Low);

            InitializeUI();
        }

        /// <summary>
        /// Initialize UI Controls
        /// </summary>
        public void InitializeUI(){
            // Starting Message
            if (!IsAdministrator()){ // Administrator enabled?
                if (BI.EXE.GetExeLocation() == null){ // Exe location found?
                    // Text
                    Logger.Log("DRAG AND DROP BROFORCE.EXE HERE BRO!", Logger.LogType.Success, Logger.VerboseType.Low);

                    // Drag and drop
                    Logger.AllowDragAndDrop(true);
                    Logger.TxtBox.DragDrop += (sender, e) => GenerateLaunchInfo(sender, e);
                } else {
                    // Show launch info
                    GenerateLaunchInfo(null, null);

                    // Drag and drop
                    Logger.AllowDragAndDrop(false);
                }
            } else {
                // Text
                Logger.Log("PLEASE RUN THIS APPLICATION WITHOUT ADMINISTRATOR BRO!", Logger.LogType.Error, Logger.VerboseType.Low);
            }
        }

        // https://stackoverflow.com/questions/13603654/check-only-one-toolstripmenuitem
        public void UncheckOtherToolStripMenuItems(ToolStripMenuItem selectedMenuItem) {
            selectedMenuItem.Checked = true;

            // Select the other MenuItens from the ParentMenu(OwnerItens) and unchecked this,
            // The current Linq Expression verify if the item is a real ToolStripMenuItem
            // and if the item is a another ToolStripMenuItem to uncheck this.
            foreach (var ltoolStripMenuItem in (from object 
                                                    item in selectedMenuItem.Owner.Items 
                                                let ltoolStripMenuItem = item as ToolStripMenuItem 
                                                where ltoolStripMenuItem != null 
                                                where !item.Equals(selectedMenuItem) 
                                                select ltoolStripMenuItem))
                    (ltoolStripMenuItem).Checked = false;

            // This line is optional, for show the mainMenu after click
            //selectedMenuItem.Owner.Show();
        }

        private void SetVerboseLevel(object? sender, EventArgs? e, Logger.VerboseType type) {
            Logger.SetVerbosity(type);

            UncheckOtherToolStripMenuItems((ToolStripMenuItem)sender);
        }

        public void InitializeControls(){
            // Tool Menu
            this.IsMdiContainer = true;

            // Create a MenuStrip control with a new window.
            MenuStrip ms = new MenuStrip();

            ToolStripMenuItem options = new ToolStripMenuItem("Options");
            ToolStripMenuItem reset = new ToolStripMenuItem("Reset", null, new EventHandler(Reset));
            options.DropDownItems.Add(reset);

            ms.Items.Add(options);

            ToolStripMenuItem verbosity = new ToolStripMenuItem("Verbosity");
            ToolStripMenuItem low_verbose = new ToolStripMenuItem(
                "Low", null, new EventHandler((sender, e) => SetVerboseLevel(sender, e, Logger.VerboseType.Low)));
                verbosity.DropDownItems.Add(low_verbose);
            ToolStripMenuItem medium_verbose = new ToolStripMenuItem(
                "Medium", null, new EventHandler((sender, e) => SetVerboseLevel(sender, e, Logger.VerboseType.Medium)));
                verbosity.DropDownItems.Add(medium_verbose);
            ToolStripMenuItem high_verbose = new ToolStripMenuItem(
                "High", null, new EventHandler((sender, e) => SetVerboseLevel(sender, e, Logger.VerboseType.High)));
                verbosity.DropDownItems.Add(high_verbose);

            SetVerboseLevel(low_verbose, null, Logger.VerboseType.Low);

            ms.Items.Add(verbosity);

            ms.MdiWindowListItem = verbosity;
           
            ms.Dock = DockStyle.Top;
            this.MainMenuStrip = ms;
            this.Controls.Add(ms);

            // Form Sizing
            int x = Screen.PrimaryScreen.Bounds.Width;
            int y = Screen.PrimaryScreen.Bounds.Height;

            this.MinimumSize = new System.Drawing.Size(
                x/4,
                y/4
            );

            this.MaximumSize = new System.Drawing.Size(
                x,
                y
            );

            this.Size = new System.Drawing.Size(
                x/2,
                y/4
            );

            this.AutoSize = false;

            // Show form
            this.Shown += (sender, e) => FI.Visuals.ForceShow(sender, e);
        }

        public GUI(){
            Logger.Init(this);
            InitializeControls();
            InitializeComponent();
            InitializeUI();
        }
    }
}