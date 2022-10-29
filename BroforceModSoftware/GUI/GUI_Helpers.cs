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
            Process proc = new Process();
            proc.StartInfo.FileName = "Explorer.exe";
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.Start();
        }

        public static void DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        public static void PlaySuccessSound(){
            SoundPlayer Success = new SoundPlayer(@"Success.wav");
            Success.Play();
        }

        private static Action Shake(Form form, int x, int y, int shake_amplitude = 10){
            return () => {
                var original = form.Location;
                var rnd = new Random(1337);

                for (int i = 0; i < x; i++){
                    form.Location = new Point(original.X + rnd.Next(-shake_amplitude, shake_amplitude), original.Y + rnd.Next(-shake_amplitude, shake_amplitude));
                    System.Threading.Thread.Sleep(y);
                }

                form.Location = original;
            };
        }

        public static string PerformEffect(Form form, FileStates e){
            //ThreadHandling.QueueTask(GUI_Helpers.Shake(form, 100, 10));
            //ThreadHandling.QueueTask(GUI_Helpers.Shake(form, 50, 1));

            string text = "";
       
            switch (e){
                case FileStates.Exists:
                    ThreadHandling.QueueTask(GUI_Helpers.Shake(form, 100, 10, 15));
                    text = "THAT MOD ALREADY EXISTS BRO!";
                    break;

                case FileStates.Dearth:
                    ThreadHandling.QueueTask(GUI_Helpers.Shake(form, 100, 10, 15));
                    text = "NO FILES HERE BRO! I THINK SOMETHINGS GONE SERIOUSLY WRONG BRO!";
                    break;
                case FileStates.Excess:
                    ThreadHandling.QueueTask(GUI_Helpers.Shake(form, 100, 10, 15));
                    text = "THAT'S TOO MANY FILES BRO!";
                    break;

                case FileStates.Invalid:
                    ThreadHandling.QueueTask(GUI_Helpers.Shake(form, 100, 10, 15));
                    text = "INVALID INPUT BRO!";
                    break;

                case FileStates.SuccessOnExe:
                    ThreadHandling.QueueTask(GUI_Helpers.Shake(form, 100, 10, 15));
                    PlaySuccessSound();

                    text = "NOW UPLOAD YOUR MODS BRO OR DON'T SEE WHAT I CARE!";
                    break;
                case FileStates.SuccessOnMod:
                    ThreadHandling.QueueTask(GUI_Helpers.Shake(form, 100, 10, 15));
                    PlaySuccessSound();

                    text = "WOOOOOOOOOOOOOOOOOOO! YEH BRO! NOTHING WENT WRONG!";
                    break;

                case FileStates.FailOnExe:
                    ThreadHandling.QueueTask(GUI_Helpers.Shake(form, 100, 10, 15));
                    PlaySuccessSound();

                    text = "REUPLOAD YOUR EXE BRO OR DON'T SEE WHAT I CARE!";
                    break;
                case FileStates.FailOnMod:
                    ThreadHandling.QueueTask(GUI_Helpers.Shake(form, 100, 10, 15));
                    PlaySuccessSound();

                    text = "NOOOOOOOOOOOOOOOOOOO! NO BRO! SOMETHING WENT WRONG!";
                    break;
            }

            ThreadHandling.ExecuteTasks();

            return text;
        }

        public static void DragDrop(object sender, DragEventArgs e, Form form) {
            FileStates state = Data.SendFiles((string[])e.Data.GetData(DataFormats.FileDrop));

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