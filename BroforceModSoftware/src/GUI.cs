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
using BroforceModSoftware.Threading;

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
            if (!String.IsNullOrEmpty(value.ToString())) {
                Logger.Log(value.ToString(), Logger.LogType.System, Logger.VerboseType.High);
            }
        }

        public override void Write(string? value){
            if (!String.IsNullOrEmpty(value)) {
                Logger.Log(value, Logger.LogType.System, Logger.VerboseType.High);
            }
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
            
            Engine,
            System,

            Custom,
            Default,
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
            
            if (!String.IsNullOrEmpty(txt)){
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

                        case LogType.Engine:
                            col = TxtBox.BackColor;
                            break;
                        case LogType.System:
                            col = TxtBox.BackColor;
                            break;

                        case LogType.Custom: // Allows for custom color select
                            // col = col; 
                            break;
                        case LogType.Default:
                            col = TxtBox.BackColor; 
                            break;
                    }

                    string RebuiltMessage = "You shouldn't be seeing this message. Try using Options/Reset, Rebuilding from source, Or contacting the developer.";      

                    switch (Logger.verbosity){
                        case VerboseType.High:
                            RebuiltMessage = String.Format("[{0}, {1}]@[{2}/{3}/{4}]: ", 
                                v.ToString(), type.ToString(), Path.GetFileName(file), member, line);          
                            break;
                        case VerboseType.Medium:
                            RebuiltMessage = String.Format("[{0}, {1}]: ", 
                                v.ToString(), type.ToString());      
                            break;
                        case VerboseType.Low:
                            RebuiltMessage = "~ ";
                            break;
                    }
                    
                    TxtBox.AppendText(RebuiltMessage + txt + Environment.NewLine);
                    TxtBox.BackColor = col ?? Logger.TxtBox.BackColor;
                }
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

            Logger.Log("Set Verbose Level to " + s, Logger.LogType.Default, v);
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
    
        /// <summary>
        /// Empty Log
        /// </summary>
        public static void Clear(){
            TxtBox.Text = "Log was emptied..." + Environment.NewLine;
        }
    }
}

namespace BroforceModSoftware {
    public partial class GUI : Form {
        /// <summary>
        /// Reset STORE.txt
        /// </summary>
        void Reset(object sender, EventArgs e){
            Logger.Clear();

            if (File.Exists(BI.EXE.StorageFilePath)){ 
                Logger.Log("Resetting STORE.txt.", Logger.LogType.Success, Logger.VerboseType.Medium);

                File.Delete(BI.EXE.StorageFilePath);
            } else {
                Logger.Log("Already reset STORE.txt!", Logger.LogType.Warning, Logger.VerboseType.Medium);
            }

            Logger.Log("Resetting...", Logger.LogType.Success, Logger.VerboseType.Low);

            StartGUI();
        }

        /// <summary>
        /// Log the launch info
        /// </summary>
        void GenerateLaunchInfo(object sender, DragEventArgs e){
            bool fail = false;
            bool retry = false;
            string s = "";

            /// <summary>
            /// Checks wether STORE.txt exists
            /// </summary>
            if (File.Exists(BI.EXE.StorageFilePath)){
                s += "Found STORE.txt @ (" + Path.GetFullPath(BI.EXE.StorageFilePath) + ")";
            } else {
                fail = true;
                s += "STORE.txt was not found.";
            }

            Logger.Log(s, (fail) ? Logger.LogType.Error : Logger.LogType.Success, Logger.VerboseType.Medium);
            s = "";
            
            /// <summary>
            /// Checks wether the path inside STORE.txt exists
            /// </summary>
            if (File.Exists(BI.EXE.GetLocation())){
                s += 
                "Found Broforce executable @ (" + 
                BI.EXE.GetLocation() + ")" +
                Environment.NewLine;
            } else {
                fail = true;

                if (String.IsNullOrEmpty(BI.EXE.GetLocation())){
                    s += "The stored path to the Broforce executable is empty.";
                } else {
                    s += BI.EXE.GetLocation() + " is an invalid path for the Broforce executable.";
                }
            }

            Logger.Log(s, (fail) ? Logger.LogType.Error : Logger.LogType.Success, Logger.VerboseType.Medium);
            s = "";
            
            /// <summary>
            /// Final output - decides wether reset is needed or if operation was successfull
            /// </summary>
            if (!fail){ 
                s += 
                Environment.NewLine + 
                "You can now open Broforce!" +
                Environment.NewLine + 
                "~ This will log anything you need to see whilst playing." + 
                Environment.NewLine + 
                "~ When you want to use mods open this application first then open Broforce." +
                Environment.NewLine + 
                "~ If you don't want to use mods then don't open this application! (Duh ;P)";
            } else {
                retry = true;

                if (File.Exists(BI.EXE.StorageFilePath)){ 
                    s += "The path in STORE.txt was incorrect. Resetting...";
    
                    File.Delete(BI.EXE.StorageFilePath);
                } else {
                    s += "Asking user to upload executable file for Broforce...";
                }
            }

            /// <summary>
            /// Only logs above contents if selected EXE isn't currently running
            /// </summary>
            if (!fail && !retry && BI.EXE.IsInUse()){ 
                // Bug: this isn't changing the color
                Logger.Log("You need to open this application before launching Broforce. Use Options/Reset in the context menu above if you have chosen the wrong exe or close this application and reopen it after closing Broforce.", Logger.LogType.Error, Logger.VerboseType.Low);
            } else {
                Logger.Log(s, (fail) ? Logger.LogType.Error : Logger.LogType.Success, Logger.VerboseType.Low);
            }

            // Reset if needed
            if (retry) StartGUI();
        }

        /// <summary>
        /// Check if program is admin
        /// </summary>
        bool IsAdministrator(){
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// Initialize UI Controls
        /// </summary>
        void StartGUI(){
            // Tip
            if (!IsAdministrator()) Logger.Log(
                "If you would like to see a higher level of logging at startup then run this application as administrator. This will stop you from uploading files for security reasons.", 
                Logger.LogType.Success, Logger.VerboseType.Low
            );

            Logger.AddNewLine();

            // Starting Message
            if (String.IsNullOrEmpty(BI.EXE.GetLocation())){ // Exe location found?
                // info
                Logger.Log("STORE.txt contents are empty. Asking for exe...", Logger.LogType.Warning, Logger.VerboseType.High);

                // Text
                Logger.Log(@"Drag and Drop the executable file for Broforce into this window. (Usually this is named 'Broforce_beta.exe' or 'Broforce.exe' and can normally be found under 'Steam\steamapps\common\Broforce')", Logger.LogType.Success, Logger.VerboseType.Low);

                // Drag and drop
                Logger.AllowDragAndDrop(true);
            } else {
                // info
                Logger.Log("STORE.txt contents are not empty. No action needed...", Logger.LogType.Success, Logger.VerboseType.High);

                // Show launch info
                GenerateLaunchInfo(null, null);

                // Drag and drop
                Logger.AllowDragAndDrop(false);
            }
        }

        void SetVerboseLevel(object sender, EventArgs? e, Logger.VerboseType type) {
            Logger.SetVerbosity(type);

            UncheckOtherToolStripMenuItems((ToolStripMenuItem)sender);
        }

        // https://stackoverflow.com/questions/13603654/check-only-one-toolstripmenuitem
        void UncheckOtherToolStripMenuItems(ToolStripMenuItem selectedMenuItem) {
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

        void SetupToolbar(){
            // Tool Menu
            this.IsMdiContainer = true;

            // Menu strip stuff
            MenuStrip ms = new MenuStrip();

            // Options
            ToolStripMenuItem options = new ToolStripMenuItem("Options");
            ToolStripMenuItem reset = new ToolStripMenuItem("Reset", null, new EventHandler(Reset));
            options.DropDownItems.Add(reset);

            ms.Items.Add(options);

            // Verbosity
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

            ms.Items.Add(verbosity);

            // Override startup verbosity
            if (IsAdministrator()){
                SetVerboseLevel(high_verbose, null, Logger.VerboseType.High);

                Logger.Log("Reminder: Running this application as administrator will usually not allow for upload of files. This serves as a way to quickly debug startup.", 
                    Logger.LogType.Warning, Logger.VerboseType.Low);
            } else {
                SetVerboseLevel(low_verbose, null, Logger.VerboseType.Low);
            }

            // Assigning
            ms.MdiWindowListItem = verbosity; // What does this do?
           
            // Finishing Setup 
            ms.Dock = DockStyle.Top;
            this.MainMenuStrip = ms;
            this.Controls.Add(ms);
        }

        void SetupForm(){
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
            // Logger Setup
            Logger.Init(this);
            Logger.TxtBox.DragDrop += (sender, e) => GenerateLaunchInfo(sender, e);

            // General Setup
            SetupForm();
            SetupToolbar();
            InitializeComponent();

            // Show Start UI
            StartGUI();

            ThreadHandling.QueueTask(BI.BeginLoad());
        }

        // https://stackoverflow.com/questions/8367586/asking-for-confirmation-when-x-button-is-clicked
        protected override void OnFormClosing(FormClosingEventArgs e){
            try {
                if (BI.EXE.IsInUse()){
                    if (MessageBox.Show("Closing the engine whilst the game is open is not recommended.\nAre you sure you want to close the engine?", "Close Engine?", MessageBoxButtons.YesNo) == DialogResult.No){
                        e.Cancel = true;
                    }
                } else {
                    e.Cancel = false;
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString(), "Error!", MessageBoxButtons.OK);
                e.Cancel = false;
            }
        }
    }
}