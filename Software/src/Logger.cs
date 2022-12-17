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

/// <summary>
/// https://stackoverflow.com/questions/18726852/redirecting-console-writeline-to-textbox
/// </summary>

namespace Software {
    public class ConsoleLogger : TextWriter {
        Control textbox;

        public ConsoleLogger(Control _textbox){
            textbox = _textbox;
        }

        public override void Write(char value){
            if (!String.IsNullOrEmpty(value.ToString())) {
                Logger.Log(value.ToString(), Logger.LogType.System, Logger.VerboseType.High);
            }
        }

        public override void Write(string value){ // public override void Write(string? value){
            if (!String.IsNullOrEmpty(value)) {
                Logger.Log(value, Logger.LogType.System, Logger.VerboseType.High);
            }
        }

        public override Encoding Encoding {
            get { return Encoding.ASCII; }
        }
    }

    public static class Logger {
        // Type of log
        public enum LogType {
            Error,
            Warning,
            Success,
            
            Engine,
            System,

            Custom,
            Default,
        }

        // Verbosity
        public enum VerboseType {
            High = 3,
            Medium = 2,
            Low = 1
        }

        static VerboseType verbosity;

        // Components
        static TextBox textbox;
        static Form form;

        /// <summary>
        /// Logger Setup
        /// </summary>
        public static void Setup(Form f){
            // Form
            form = f;

            // Text Box
            textbox = new TextBox();

            // Text Properties
            textbox.AcceptsReturn = true;
            textbox.Multiline = true;
            textbox.ScrollBars = ScrollBars.Vertical;
            textbox.AutoSize = false; 
            textbox.Anchor = AnchorStyles.None;
            textbox.Dock = DockStyle.Fill;
            textbox.ReadOnly = true;
            textbox.BackColor = Color.Green;

            // Set output to textbox
            Console.SetOut(new ConsoleLogger(textbox));

            // Add text to form
            form.Controls.Add(textbox);
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
                            col = textbox.BackColor;
                            break;
                        case LogType.System:
                            col = textbox.BackColor;
                            break;

                        case LogType.Custom: // Allows for custom color select
                            // col = col; 
                            break;
                        case LogType.Default:
                            col = textbox.BackColor; 
                            break;
                    }

                    string RebuiltMessage = "You shouldn't be seeing this message. Try using Options/Reset, Rebuilding from source, Or contacting the developer.";      

                    switch (verbosity){
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
                    
                    textbox.AppendText(RebuiltMessage + txt + Environment.NewLine);
                    textbox.BackColor = col ?? textbox.BackColor;
                }
            }
        }
        
        /// <summary>
        /// Changes color of log
        /// </summary>
        public static void SetLogColor(Color col){
            textbox.BackColor = col;
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

            Log("Set Verbose Level to " + s, LogType.Default, v);
        }

        /// <summary>
        /// Adds new empty line to log
        /// </summary>
        public static void AddNewLine(){
            textbox.AppendText(Environment.NewLine);
        }

        /// <summary>
        /// Allows upload of files to log
        /// </summary>
        public static void AllowDragAndDrop(bool x){
            textbox.AllowDrop = x;
        }

        /// <summary>
        /// Empty Log
        /// </summary>
        public static void Clear(){
            textbox.Text = "Log was emptied..." + Environment.NewLine;
        }
    }
}