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

using BroforceModEngine.Handling;
using BroforceModSoftware.Threading;
using BroforceModSoftware;

/// <summary>
/// Handles logic for the GUI
/// </summary>

namespace BroforceModSoftware.Helpers {
    public static class GUI_Helpers {
        public static class Sounds {
            // Paths
            public const string SoundsPath = @"..\..\..\Sounds";

            public static Action PlayBroforceFoundSound(){
                return () => {
                    SoundPlayer sound = new SoundPlayer(SoundsPath + @"\BroforceExeFound" + ".wav");
                    sound.Play();
                };
            }

            public static Action PlaySuccessSound(){
                return () => {
                    Random rnd = new Random();
                    int num = rnd.Next(4);

                    SoundPlayer sound = new SoundPlayer(SoundsPath + @"\Success" + (num + 1).ToString() + ".wav");
                    sound.Play();
                };
            }

            public static Action PlayFailSound(){
                return () => {
                    Random rnd = new Random();
                    int num = rnd.Next(2);

                    SoundPlayer sound = new SoundPlayer(SoundsPath + @"\Fail" + (num + 1).ToString() + ".wav");
                    sound.Play();
                };
            }

            public static Action PlayDeleteModSound(){
                return () => {
                    SoundPlayer sound = new SoundPlayer(SoundsPath + @"\DeleteMod.wav");
                    sound.Play();
                };
            }

            public static Action PlayModSound(string PathToMod){ // Unused
                return () => {
                    SoundPlayer sound = new SoundPlayer(PathToMod + @"\ModSound.wav");
                    sound.Play();
                };
            }
        }

        public static class Effects {
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

            // Messages
            public static string EXEON = "THIS WILL LOG ANYTHING YOU NEED TO SEE WHILE PLAYING BRO!" + 
                                        Environment.NewLine + 
                                        "REMEMBER TO LAUNCH THIS APP EACH TIME YOU WANT TO USE MODS BRO!" +
                                        Environment.NewLine + 
                                        "NOW PLAY SOME BROFORCE BRO!";

            public static string EXEOFF = "COULDN'T ADD EXE BRO, TRY AGAIN BRO!";

            public static string GetEXEON(){
                return "ADDED " + Data.Files.GetId() + Environment.NewLine + Environment.NewLine + EXEON;
            }

            public static string GetEXEOFF(){
                return "FAILED TO ADD " + Data.Files.GetId() + Environment.NewLine + EXEOFF;
            }

            /// <summary>
            /// Shake Form
            /// </summary>
            private static Action Shake(Form form, TextBox txtbox, int length, int sleep, int shake_amplitude, bool RainbowOn = false){
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

            /// <summary>
            /// Perform an effect dependant on current state
            /// </summary>
            public static string PerformEffect(Form form, TextBox txtbox, FileStates e, string id){
                //ThreadHandling.QueueTask(GUI_Helpers.Shake(form, 100, 10));
                //ThreadHandling.QueueTask(GUI_Helpers.Shake(form, 50, 1));

                string text = "";

                switch (e){
                    // Exists
                    case FileStates.Exists:
                        
                        ThreadHandling.QueueTask(Sounds.PlayFailSound());
                        ThreadHandling.QueueTask(
                            GUI_Helpers.Effects.Shake(form, txtbox, lengthNormal, sleepNormal, amplitudeNormal));

                        text = "THAT MOD ALREADY EXISTS BRO!";
                        break;

                    // Too little or too much
                    case FileStates.Dearth:
                        
                        ThreadHandling.QueueTask(Sounds.PlayFailSound());
                        ThreadHandling.QueueTask(
                            GUI_Helpers.Effects.Shake(form, txtbox, lengthNormal, sleepNormal, amplitudeNormal));

                        text = "NO FILES HERE BRO! I THINK SOMETHINGS GONE SERIOUSLY WRONG BRO!";
                        break;
                    case FileStates.Excess:
                        
                        ThreadHandling.QueueTask(Sounds.PlayFailSound());
                        ThreadHandling.QueueTask(
                            GUI_Helpers.Effects.Shake(form, txtbox, lengthNormal, sleepNormal, amplitudeNormal));

                        text = "THAT'S TOO MANY FILES BRO!";
                        break;

                    // Invalid
                    case FileStates.Invalid:
                        
                        ThreadHandling.QueueTask(Sounds.PlayFailSound());
                        ThreadHandling.QueueTask(
                            GUI_Helpers.Effects.Shake(form, txtbox, lengthNormal, sleepNormal, amplitudeNormal));

                        text = "INVALID INPUT BRO!";
                        break;

                    // Exe
                    case FileStates.SuccessOnExe:
                        
                        ThreadHandling.QueueTask(Sounds.PlayBroforceFoundSound());
                        ThreadHandling.QueueTask(
                            GUI_Helpers.Effects.Shake(form, txtbox, lengthInsane, sleepInsane, amplitudeInsane, true));

                        text = GetEXEON();
                        break;
                    case FileStates.FailOnExe:
                        
                        ThreadHandling.QueueTask(Sounds.PlayFailSound());
                        ThreadHandling.QueueTask(
                            GUI_Helpers.Effects.Shake(form, txtbox, lengthNormal, sleepNormal, amplitudeNormal));

                        text = GetEXEOFF();
                        break;

                    // Mod
                    case FileStates.SuccessOnMod:
                        
                        ThreadHandling.QueueTask(Sounds.PlaySuccessSound());
                        ThreadHandling.QueueTask(
                            GUI_Helpers.Effects.Shake(form, txtbox, lengthInsane, sleepInsane, amplitudeInsane, true));

                        text = id + Environment.NewLine + " WAS UPLOADED SUCCESSFULLY BRO!";
                        break;
                    case FileStates.FailOnMod:

                        ThreadHandling.QueueTask(Sounds.PlayFailSound());
                        ThreadHandling.QueueTask(
                            GUI_Helpers.Effects.Shake(form, txtbox, lengthNormal, sleepNormal, amplitudeNormal));

                        text = id + Environment.NewLine + " FAILED TO UPLOAD BRO!";
                        break;
                }

                ThreadHandling.ExecuteTasks();

                return text;
            }
        }

        public static class Files {
            /// <summary>
            /// Drag and drop event
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            /// <param name="form"></param>
            public static void DragDrop(object sender, DragEventArgs e, Form form) {
                FileStates state = Data.Files.SendFiles((string[])e.Data.GetData(DataFormats.FileDrop));

                string id = "";

                if (state == FileStates.SuccessOnMod || state == FileStates.FailOnMod){
                    id = Data.Files.GetId();
                }

                TextBox txtbox = sender as TextBox;
                Logger.Log(Effects.PerformEffect(form, txtbox, state, id), Color.Green);

                if (state == FileStates.SuccessOnExe) txtbox.AllowDrop = false;

                if (state == FileStates.FailOnExe || state == FileStates.FailOnMod){
                    txtbox.BackColor = Color.Red;
                } else if (state == FileStates.SuccessOnExe || state == FileStates.SuccessOnMod){
                    txtbox.BackColor = Color.Green;
                } else {
                    txtbox.BackColor = Color.Orange;
                }
            }

            /// <summary>
            /// Hover event
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public static void DragEnter(object sender, DragEventArgs e) {
                if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
            }
        }

        public static class Visuals {
            /// <summary>
            /// Open file explorer (preferably on top - hence the admin)
            /// </summary>
            public static void OpenFileExplorerAsAdmin(){
                try{
                    Process proc = new Process();
                    proc.StartInfo.FileName = "Explorer.exe";
                    proc.StartInfo.UseShellExecute = true;
                    proc.StartInfo.Verb = "runas";
                    proc.Start();
                } catch (Exception e){
                    System.Console.WriteLine(e.ToString());
                }
            }

            /// <summary>
            /// Bring the window to the front
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
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