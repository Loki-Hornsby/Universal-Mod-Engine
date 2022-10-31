using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

// .Net needs to be 3.5
// Taskkill /IM BroMods.exe /F 

namespace BROMODS 
{
    static class Program
    {
        [DllImport("kernel32")]
        static extern bool AllocConsole();

        // A test to check HarmonyX is working
        static void PerformTest()
        {
            TestingClass x = new TestingClass();

            System.WriteLine("Test!");

            x.DoSomething();
            MyPatcher.DoPatching();
            x.DoSomething();
        }
   
        // The main entry point for the application.
        [STAThread]
        static void Main()
        {
            AllocConsole();
            System.Console.WriteLine("Hello! - Console was started!");

            //PerformTest();

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}