// I'm rewriting your loggers Gorzon - Why? 
    // Because i can! >:) 
    // no, but in all seriousness im using the console to show my outputs so your methods don't work for me
    // your methods will still have functionality on your side however (i've barely touched them ;D)

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using HarmonyLib;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Text;

namespace BroforceModEngine.Loggers {
    public static class SimpleLogger {
        public static void Log(string prefix, string message){
            System.Console.WriteLine("--\n" + prefix + ": " + message + "\n" + "--");
        }

        // Source: https://stackoverflow.com/questions/4272579/how-to-print-full-stack-trace-in-exception
        public static string GetAllFootprints(Exception x){
            var st = new StackTrace(x, true);
            var frames = st.GetFrames();
            var traceString = new StringBuilder();

            foreach (var frame in frames)
            {
                if (frame.GetFileLineNumber() < 1)
                    continue;

                traceString.Append("File: " + frame.GetFileName());
                traceString.Append(", Method:" + frame.GetMethod().Name);
                traceString.Append(", LineNumber: " + frame.GetFileLineNumber());
                traceString.Append("  -->  ");
            }

            return traceString.ToString();
        }
    }
}