using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BROMODS {
    public static class GUI_Helpers {
        public static void Form_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        public static void Form_DragDrop(object sender, DragEventArgs e) {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string test = "";

            foreach (string file in files) test = test + file;

            Label lbl = sender as Label;
            lbl.Text = test;
        }

        public static Action Shake(Form form, int x, int y){
            return () => {
                var original = form.Location;
                var rnd = new Random(1337);
                const int shake_amplitude = 10;

                for (int i = 0; i < x; i++){
                    form.Location = new Point(original.X + rnd.Next(-shake_amplitude, shake_amplitude), original.Y + rnd.Next(-shake_amplitude, shake_amplitude));
                    System.Threading.Thread.Sleep(y);
                }

                form.Location = original;
            };
        }
    }
}