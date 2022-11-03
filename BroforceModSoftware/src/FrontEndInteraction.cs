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
using BroforceModSoftware.Threading;
using BroforceModSoftware.Interaction.Back;

/// <summary>
/// Handles front end logic for the GUI
/// </summary>

namespace BroforceModSoftware.Interaction.Front {
    public static class FI {
        public static class Media {
            public static class Sounds {
                // Paths
                public const string SoundsPath = @"..\..\..\Sounds";

                public static void PlayBroforceFoundSound(){
                    ThreadHandling.QueueTask(() => {
                        SoundPlayer sound = new SoundPlayer(SoundsPath + @"\BroforceExeFound" + ".wav");
                        sound.Play();
                    });
                }

                public static void PlaySuccessSound(){
                    ThreadHandling.QueueTask(() => {
                        Random rnd = new Random();
                        int num = rnd.Next(4);

                        SoundPlayer sound = new SoundPlayer(SoundsPath + @"\Success" + (num + 1).ToString() + ".wav");
                        sound.Play();
                    });
                }

                public static void PlayFailSound(){
                    ThreadHandling.QueueTask(() => {
                        Random rnd = new Random();
                        int num = rnd.Next(2);

                        SoundPlayer sound = new SoundPlayer(SoundsPath + @"\Fail" + (num + 1).ToString() + ".wav");
                        sound.Play();
                    });
                }
            }

            public static class Effects {
                // RAINBOW
                private static Color[] rainbow = new Color[] {
                    Color.Red,
                    Color.Orange,
                    Color.Yellow,
                    Color.Green,
                    Color.Blue,
                    Color.Indigo,
                    Color.Violet
                };

                // Some funky constants
                const int multiply_divide_fail = 50;
                const int fail_avgtime = 2;

                const int multiply_divide_success = 25;
                const int success_avgtime = 2;

                const int lengthNormal = fail_avgtime * multiply_divide_fail;
                const int sleepNormal = 1000 / multiply_divide_fail;
                const int amplitudeNormal = 50;

                const int lengthInsane = success_avgtime * multiply_divide_success;
                const int sleepInsane = 1000 / multiply_divide_success;
                const int amplitudeInsane = 50;

                public static void ShakeNormal(Form form, TextBox txtbox){
                    ThreadHandling.QueueTask(Shake(form, txtbox, lengthNormal, sleepNormal, amplitudeNormal));
                }

                public static void ShakeIntense(Form form, TextBox txtbox){
                    ThreadHandling.QueueTask(Shake(form, txtbox, lengthInsane, sleepInsane, amplitudeInsane, true));
                }

                /// <summary>
                /// Shake Form
                /// </summary>
                static Action Shake(Form form, TextBox txtbox, int length, int sleep, int shake_amplitude, bool RainbowOn = false){
                    return () => {
                        Point originalloc = form.Location;
                        Color originalcol = txtbox.BackColor;
                        Random rnd = new Random(1337);
                        float divisoramp = shake_amplitude / length;

                        for (int i = 0; i < length; i++){
                            form.Location = new Point(
                                originalloc.X + rnd.Next(-shake_amplitude, shake_amplitude), 
                                originalloc.Y + rnd.Next(-shake_amplitude, shake_amplitude)
                            );

                            shake_amplitude = (int)System.Math.Ceiling(shake_amplitude - divisoramp);
                            
                            if (RainbowOn) txtbox.BackColor = rainbow[rnd.Next(rainbow.Length)];
                            System.Threading.Thread.Sleep(sleep);
                        }

                        txtbox.BackColor = originalcol;
                        form.Location = originalloc;
                    };
                }
            }
        }

        public static class DragAndDrop {
            /// <summary>
            /// Perform an effect dependant on current state
            /// </summary>
            static string PerformResponse(Form form, TextBox txtbox){
                string text = "";
                string id = BI.Files.GetId();

                switch (BI.FileState){
                    // Too little or too much
                    case BI.FileStates.Dearth:
                        Media.Sounds.PlayFailSound();
                        Media.Effects.ShakeNormal(form, txtbox);
                        text = "FATAL ERROR BRO! (No files were found - Have they been deleted?)";
                        break;

                    case BI.FileStates.Excess:
                        Media.Sounds.PlayFailSound();
                        Media.Effects.ShakeNormal(form, txtbox);
                        text = "THAT'S TOO MANY FILES BRO!";
                        break;

                    // Invalid
                    case BI.FileStates.Invalid:
                        Media.Sounds.PlayFailSound();
                        Media.Effects.ShakeNormal(form, txtbox);
                        text = "INVALID INPUT BRO!";
                        break;

                    // Exe
                    case BI.FileStates.SuccessOnExe:
                        Media.Sounds.PlayBroforceFoundSound();
                        Media.Effects.ShakeIntense(form, txtbox);
                        break;

                    case BI.FileStates.FailOnExe:
                        Media.Sounds.PlayFailSound();
                        Media.Effects.ShakeNormal(form, txtbox);
                        break;

                    // Mod
                    case BI.FileStates.SuccessOnMod:
                        Media.Sounds.PlaySuccessSound();
                        Media.Effects.ShakeIntense(form, txtbox);
                        text = id + " WAS UPLOADED SUCCESSFULLY BRO!";
                        break;

                    case BI.FileStates.FailOnMod:
                        Media.Sounds.PlayFailSound();
                        Media.Effects.ShakeNormal(form, txtbox);
                        text = id + " FAILED TO UPLOAD BRO!";
                        break;
                }

                ThreadHandling.ExecuteTasks();

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
                } else if (BI.FileState == BI.FileStates.SuccessOnExe || BI.FileState == BI.FileStates.SuccessOnMod){
                    col = Color.Green;
                } else {
                    col = Color.Orange;
                }

                // Disable Drag and Drop
                if (BI.FileState == BI.FileStates.SuccessOnExe) Logger.AllowDragAndDrop(false);

                // Log Output from response
                if (text != "") Logger.Log(text, Logger.LogType.Custom, Logger.VerboseType.Low, col);
            }

            /// <summary>
            /// Hover event
            /// </summary>
            public static void Enter(object sender, DragEventArgs e) {
                if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
            }
        }

        public static class Visuals {
            /// <summary>
            /// Bring the window to the front
            /// </summary>
            public static void ForceShow(object sender, EventArgs e) {
                Form f = sender as Form;
                f.TopMost = true;
                f.Focus();
                f.BringToFront();
                f.Activate();
            }
        }
    }
}