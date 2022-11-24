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

using Software;
using Software.Interaction.Back;

/// <summary>
/// Handles front end logic for the GUI
/// </summary>

namespace Software.Interaction.Front {
    public static class FI {
        public static class DragAndDrop {
            /// <summary>
            /// Perform an effect dependant on current state
            /// </summary>
            static string PerformResponse(Form form, TextBox txtbox){
                string text = "";

                switch (BI.FileState){
                    // Too little or too much
                    case BI.FileStates.Dearth:
                        text = "No files were found?";
                        break;

                    case BI.FileStates.Excess:
                        text = "Too many files uploaded.";
                        break;

                    // Invalid
                    case BI.FileStates.Invalid:
                        text = "Incorrect input. Is the file corrupted?";
                        break;

                    // Exe
                    case BI.FileStates.SuccessOnExe:
                        text = "Exe upload succeeded!";
                        break;

                    case BI.FileStates.FailOnExe:
                        text = "Exe upload failed!";
                        break;

                    // Mod
                    case BI.FileStates.SuccessOnMod:
                        text = "Mod was uploaded.";
                        break;

                    case BI.FileStates.FailOnMod:
                        text = "Mod failed to upload.";
                        break;
                }

                return text;
            }
    
            /// <summary>
            /// Drag and drop event
            /// </summary>
            public static void Drop(object sender, DragEventArgs e, Form form) {
                // Send files to Back End
                BI.Files.SendFiles((string[])e.Data.GetData(DataFormats.FileDrop));

                // Define Text Box
                TextBox txtbox = sender as TextBox;

                // Perform response
                string text = PerformResponse(form, txtbox);

                // Handle Log Color
                Color col;

                if (BI.FileState == BI.FileStates.FailOnExe || BI.FileState == BI.FileStates.FailOnMod){
                    col = Color.Red;
                } else {
                    col = txtbox.BackColor;
                }

                // Disable Drag and Drop
                if (BI.FileState == BI.FileStates.SuccessOnExe) Logger.AllowDragAndDrop(false);

                // Log Output from response
                if (!String.IsNullOrEmpty(text)) Logger.Log(text, Logger.LogType.Custom, Logger.VerboseType.Low, col);
            }

            /// <summary>
            /// Hover event
            /// </summary>
            public static void Enter(object sender, DragEventArgs e) {
                e.Effect = DragDropEffects.Copy;
            }
        }

        public static class Visuals {
            public static void ExitWithMessageBox(string s){
                MessageBox.Show(
                    s, 
                    "Warning!", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                System.Windows.Forms.Application.Exit();
            }

            /// <summary>
            /// Bring the window to the front
            /// </summary>
            public static void ForceShow(object sender, EventArgs e) {
                Form f = sender as Form;
                f.Focus();
                f.BringToFront();
                f.Activate();
            }
        }
    }
}