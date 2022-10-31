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
    public static class GUI_Helpers {
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

        public static void DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        public static Action PlayBroforceFoundSound(){
            return () => {
                SoundPlayer sound = new SoundPlayer(Data.SoundsPath + @"\BroforceExeFound" + ".wav");
                sound.Play();
            };
        }

        public static Action PlaySuccessSound(){
            return () => {
                Random rnd = new Random();
                int num = rnd.Next(4);

                SoundPlayer sound = new SoundPlayer(Data.SoundsPath + @"\Success" + (num + 1).ToString() + ".wav");
                sound.Play();
            };
        }

        public static Action PlayFailSound(){
            return () => {
                Random rnd = new Random();
                int num = rnd.Next(2);

                SoundPlayer sound = new SoundPlayer(Data.SoundsPath + @"\Fail" + (num + 1).ToString() + ".wav");
                sound.Play();
            };
        }

        public static Action PlayDeleteModSound(){
            return () => {
                SoundPlayer sound = new SoundPlayer(Data.SoundsPath + @"\DeleteMod.wav");
                sound.Play();
            };
        }

        public static Action PlayModSound(string PathToMod){
            return () => {
                SoundPlayer sound = new SoundPlayer(PathToMod + @"\ModSound.wav");
                sound.Play();
            };
        }

        private static Action Shake(Form form, int length, int sleep, int shake_amplitude = 10){
            return () => {
                var original = form.Location;
                var rnd = new Random(1337);
                float divisor = shake_amplitude / length;

                for (int i = 0; i < length; i++){
                    form.Location = new Point(original.X + rnd.Next(-shake_amplitude, shake_amplitude), original.Y + rnd.Next(-shake_amplitude, shake_amplitude));
                    shake_amplitude = (int)Math.Ceiling(shake_amplitude - divisor);
                    System.Threading.Thread.Sleep(sleep);
                }

                form.Location = original;
            };
        }

        public static string PerformEffect(Form form, FileStates e){
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
                    
                    ThreadHandling.QueueTask(PlayFailSound());
                    ThreadHandling.QueueTask(GUI_Helpers.Shake(form, fail_avgtime * multiply_divide_fail, 1000 / multiply_divide_fail, 15));

                    text = "THAT MOD ALREADY EXISTS BRO!";
                    break;

                // Too little or too much
                case FileStates.Dearth:
                    
                    ThreadHandling.QueueTask(PlayFailSound());
                    ThreadHandling.QueueTask(GUI_Helpers.Shake(form, fail_avgtime * multiply_divide_fail, 1000 / multiply_divide_fail, 15));

                    text = "NO FILES HERE BRO! I THINK SOMETHINGS GONE SERIOUSLY WRONG BRO!";
                    break;
                case FileStates.Excess:
                    
                    ThreadHandling.QueueTask(PlayFailSound());
                    ThreadHandling.QueueTask(GUI_Helpers.Shake(form, fail_avgtime * multiply_divide_fail, 1000 / multiply_divide_fail, 15));

                    text = "THAT'S TOO MANY FILES BRO!";
                    break;

                // Invalid
                case FileStates.Invalid:
                    
                    ThreadHandling.QueueTask(PlayFailSound());
                    ThreadHandling.QueueTask(GUI_Helpers.Shake(form, fail_avgtime * multiply_divide_fail, 1000 / multiply_divide_fail, 15));

                    text = "INVALID INPUT BRO!";
                    break;

                // Exe
                case FileStates.SuccessOnExe:
                    
                    ThreadHandling.QueueTask(PlayBroforceFoundSound());
                    ThreadHandling.QueueTask(GUI_Helpers.Shake(form, success_avgtime * multiply_divide_success, 1000 / multiply_divide_success, 50));

                    text = "UPLOAD MODS HERE BRO!";
                    break;
                case FileStates.FailOnExe:
                    
                    ThreadHandling.QueueTask(PlayFailSound());
                    ThreadHandling.QueueTask(GUI_Helpers.Shake(form, fail_avgtime * multiply_divide_fail, 1000 / multiply_divide_fail, 15));

                    text = "COULDN'T ADD EXE BRO!";
                    break;

                // Mod
                case FileStates.SuccessOnMod:
                    
                    ThreadHandling.QueueTask(PlaySuccessSound());
                    ThreadHandling.QueueTask(GUI_Helpers.Shake(form, success_avgtime * multiply_divide_success, 1000 / multiply_divide_success, 50));

                    text = "UPLOAD MODS HERE BRO!";
                    break;
                case FileStates.FailOnMod:

                    ThreadHandling.QueueTask(PlayFailSound());
                    ThreadHandling.QueueTask(GUI_Helpers.Shake(form, fail_avgtime * multiply_divide_fail, 1000 / multiply_divide_fail, 15));

                    text = "COULDN'T ADD MOD BRO!";
                    break;

                // Delete
                case FileStates.FailOnDelete:

                    ThreadHandling.QueueTask(PlayFailSound());
                    ThreadHandling.QueueTask(GUI_Helpers.Shake(form, fail_avgtime * multiply_divide_fail, 1000 / multiply_divide_fail, 15));

                    text = "COULDN'T DELETE FILE BRO!";
                    break;
                case FileStates.SuccessOnDelete:
                
                    ThreadHandling.QueueTask(PlayDeleteModSound());

                    text = "DELETE MODS HERE BRO!";
                    break;
            }

            ThreadHandling.ExecuteTasks();

            return text;
        }

        public static void DragDrop(object sender, DragEventArgs e, Form form, bool delete) {
            FileStates state = Data.SendFiles((string[])e.Data.GetData(DataFormats.FileDrop), delete);

            Label lbl = sender as Label;
            lbl.Text = Data.debug + PerformEffect(form, state);
        }

        public static void ForceShow(object sender, EventArgs e) {
            Form f = sender as Form;
            f.TopMost = true;
            f.Focus();
            f.BringToFront();
            f.Activate();
        }
    }
}