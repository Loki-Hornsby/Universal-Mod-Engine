using System;
using System.Runtime.InteropServices;
using System.Drawing;

/// <summary>
/// Logging for the mod engine - Communicates to a spawned console and GUI
/// </summary>

namespace BroforceModEngine.Logging {
    public static class Logger {
        public enum LogType {
            Error,
            Warning,
            Success,
            None
        }

        [DllImport("kernel32")]
        static extern bool AllocConsole();

        public static bool _initialized;

        public static void Initialize(bool console){
            _initialized = true;

            if (console){ 
                AllocConsole();
            }

            Log("Debug Console Started!", LogType.None);
        }

        public static void Log(string message, LogType type = LogType.None){
            if (!_initialized) { try { Initialize(true); } catch (Exception e) { ; } } // The ";" acts like a pass statement in python

            if (type == LogType.None){
                System.Console.WriteLine(message);
                //Logger.Log(message) // Todo: need to communicate to gui somehow
            } else {
                Color col;

                switch (type){
                    case LogType.Error:
                        col = Color.Red;
                        break;
                    case LogType.Warning:
                        col =  Color.Yellow;
                        break;
                    case LogType.Success:
                        col = Color.Green;
                        break;
                }

                System.Console.WriteLine(message);
                //Logger.Log(message, (Color)type) // Todo: need to communicate to gui somehow
            }
        }
    }
}