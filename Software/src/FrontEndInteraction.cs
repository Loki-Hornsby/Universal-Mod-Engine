/// <summary>
/// Copyright 2022, Loki Alexander Button Hornsby (Loki Hornsby), All rights reserved.
/// Licensed under the BSD 3-Clause "New" or "Revised" License
/// </summary>

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
            /// Return text dependent on file state
            /// </summary>
            static string PerformResponse(Form form, TextBox txtbox){
                switch (BI.FileState){
                    // Too little or too much
                    case BI.FileStates.Dearth:
                        return "No files were found?";

                    case BI.FileStates.Excess:
                        return "Too many files uploaded.";

                    // Invalid
                    case BI.FileStates.Invalid:
                        return "Incorrect input. Is the file corrupted?";

                    // Exe
                    case BI.FileStates.SuccessOnExe:
                        return "Exe upload succeeded!";

                    case BI.FileStates.FailOnExe:
                        return "Exe upload failed!";

                    // Mod
                    case BI.FileStates.SuccessOnMod:
                        return "Mod was uploaded.";

                    case BI.FileStates.FailOnMod:
                        return "Mod failed to upload.";
                    
                    // Fatal error
                    default:
                        return "FATAL ERROR!";
                }
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
                //if (BI.FileState == BI.FileStates.SuccessOnExe) Logger.AllowDragAndDrop(false);

                // Log Output from response
                //if (!String.IsNullOrEmpty(text)) //Logger.log(text, Logger.LogType.Custom, Logger.VerboseType.Low, col);
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
        }
    }
}