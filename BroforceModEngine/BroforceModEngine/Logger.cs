using System;
using System.Runtime.InteropServices;
using System.Drawing;

/// <summary>
/// Logging for the mod engine - Communicates to a spawned console
/// </summary>

namespace BroforceModEngine.Logging {
    public static class Logger {
        [DllImport("kernel32")]
        static extern bool AllocConsole();

        public static bool _initialized;

        public static void Initialize(){
            _initialized = true;
            AllocConsole();
            Log("Debug Console Started!");
        }

        public static void Log(string message){
            if (!_initialized) { 
                try { 
                    Initialize(); 
                } catch (Exception ex) {} 
            }
                                                
            System.Console.WriteLine(message);
        }
    }
}