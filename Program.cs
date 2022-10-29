using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

// .Net needs to be 3.5

namespace BroMods{
    static class Program{
        // A test to check HarmonyX is working
        static void PerformTest(){
            TestingClass x = new TestingClass();

            Console.WriteLine("Started!");

            x.DoSomething();
            MyPatcher.DoPatching();
            x.DoSomething();
        }
   
        // The main entry point for the application.
        [STAThread]
        static void Main(){
            PerformTest();

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}