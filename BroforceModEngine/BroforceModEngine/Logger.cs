using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Runtime.CompilerServices;

/// <summary>
/// Logging for the mod engine - Communicates to a spawned console and GUI
/// </summary>

namespace BroforceModEngine.Logging {
    public static class Logger {
        [DllImport("kernel32")]
        static extern bool AllocConsole();

        public static bool _initialized;

        public static void Initialize(bool console){
            _initialized = true;

            if (console){ 
                AllocConsole();
            }

            Log("Debug Console Started!");
        }

        public static void Log(
                string message, 
                //LogType type = LogType.None, 
                [CallerFilePath] string file = "", 
                [CallerMemberName] string member = "", 
                [CallerLineNumber] int line = 0
            ){

            if (!_initialized) { try { Initialize(true); } catch (Exception ex) { ; } } // The ";" acts like a pass statement in python

            string RebuiltMessage = ("{0}_{1}({2}): {3}", Path.GetFileName(file), member, line, text);                                                    
            System.Console.WriteLine(RebuiltMessage);
        }
    }
}