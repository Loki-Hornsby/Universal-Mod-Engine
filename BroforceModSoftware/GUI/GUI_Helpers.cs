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
            public const string EXEON = "NOW PLAY SOME BROFORCE BRO!\n THIS WILL LOG ANYTHING YOU NEED TO SEE WHILE PLAYING BRO.";
            public const string EXEOFF = "COULDN'T ADD EXE BRO, TRY AGAIN BRO!";

            public static string GetEXEON(){
                return "ADDED " + Data.Files.GetId() + Environment.NewLine + Environment.NewLine + EXEON + Environment.NewLine;
            }

            public static string GetEXEOFF(){
                return "FAILED TO ADD " + Data.Files.GetId() + Environment.NewLine + EXEOFF;
            }

            private static Action Shake(Form form, TextBox txtbox, int length, int sleep, int shake_amplitude = 10, bool RainbowOn = false){
                return () => {
                    var originalloc = form.Location;
                    var originalcol = txtbox.BackColor;

                    var rnd = new Random();
                    float divisor = shake_amplitude / length;

                    for (int i = 0; i < length; i++){
                        form.Location = new Point(
                            originalloc.X + rnd.Next(-shake_amplitude, shake_amplitude), 
                            originalloc.Y + rnd.Next(-shake_amplitude, shake_amplitude)
                        );
                        shake_amplitude = (int)Math.Ceiling(shake_amplitude - divisor);
                        
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
            /// <param name="form"></param>
            /// <param name="txtbox"></param>
            /// <param name="e"></param>
            /// <param name="id"></param>
            /// <returns></returns>
            public static string PerformEffect(Form form, TextBox txtbox, FileStates e, string id){
                //ThreadHandling.QueueTask(GUI_Helpers.Shake(form, 100, 10));
                //ThreadHandling.QueueTask(GUI_Helpers.Shake(form, 50, 1));

                string text = "";

                const int multiply_divide_fail = 50;
                const int fail_avgtime = 2;

                const int multiply_divide_success = 25;
                const int success_avgtime = 2;
        
                switch (e){
                    // Exists
                    case FileStates.Exists:
                        
                        ThreadHandling.QueueTask(Sounds.PlayFailSound());
                        ThreadHandling.QueueTask(
                            GUI_Helpers.Effects.Shake(form, txtbox, fail_avgtime * multiply_divide_fail, 1000 / multiply_divide_fail, 15));

                        text = "THAT MOD ALREADY EXISTS BRO!";
                        break;

                    // Too little or too much
                    case FileStates.Dearth:
                        
                        ThreadHandling.QueueTask(Sounds.PlayFailSound());
                        ThreadHandling.QueueTask(
                            GUI_Helpers.Effects.Shake(form, txtbox, fail_avgtime * multiply_divide_fail, 1000 / multiply_divide_fail, 15));

                        text = "NO FILES HERE BRO! I THINK SOMETHINGS GONE SERIOUSLY WRONG BRO!";
                        break;
                    case FileStates.Excess:
                        
                        ThreadHandling.QueueTask(Sounds.PlayFailSound());
                        ThreadHandling.QueueTask(
                            GUI_Helpers.Effects.Shake(form, txtbox, fail_avgtime * multiply_divide_fail, 1000 / multiply_divide_fail, 15));

                        text = "THAT'S TOO MANY FILES BRO!";
                        break;

                    // Invalid
                    case FileStates.Invalid:
                        
                        ThreadHandling.QueueTask(Sounds.PlayFailSound());
                        ThreadHandling.QueueTask(
                            GUI_Helpers.Effects.Shake(form, txtbox, fail_avgtime * multiply_divide_fail, 1000 / multiply_divide_fail, 15));

                        text = "INVALID INPUT BRO!";
                        break;

                    // Exe
                    case FileStates.SuccessOnExe:
                        
                        ThreadHandling.QueueTask(Sounds.PlayBroforceFoundSound());
                        ThreadHandling.QueueTask(
                            GUI_Helpers.Effects.Shake(form, txtbox, success_avgtime * multiply_divide_success, 1000 / multiply_divide_success, 50, true));

                        text = GetEXEON();
                        break;
                    case FileStates.FailOnExe:
                        
                        ThreadHandling.QueueTask(Sounds.PlayFailSound());
                        ThreadHandling.QueueTask(
                            GUI_Helpers.Effects.Shake(form, txtbox, fail_avgtime * multiply_divide_fail, 1000 / multiply_divide_fail, 15));

                        text = GetEXEOFF();
                        break;

                    // Mod
                    case FileStates.SuccessOnMod:
                        
                        ThreadHandling.QueueTask(Sounds.PlaySuccessSound());
                        ThreadHandling.QueueTask(
                            GUI_Helpers.Effects.Shake(form, txtbox, success_avgtime * multiply_divide_success, 1000 / multiply_divide_success, 50, true));

                        text = id + Environment.NewLine + " WAS UPLOADED SUCCESSFULLY BRO!";
                        break;
                    case FileStates.FailOnMod:

                        ThreadHandling.QueueTask(Sounds.PlayFailSound());
                        ThreadHandling.QueueTask(
                            GUI_Helpers.Effects.Shake(form, txtbox, fail_avgtime * multiply_divide_fail, 1000 / multiply_divide_fail, 15));

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
                txtbox.AppendText(Environment.NewLine + Effects.PerformEffect(form, txtbox, state, id));

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