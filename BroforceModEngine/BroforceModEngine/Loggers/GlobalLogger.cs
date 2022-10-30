using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using HarmonyLib;

namespace BroforceModEngine.Loggers
{
    /// <summary>
    /// Logger
    /// </summary>
    public static class GlobalLogger
    {
        /// <summary>
        /// 
        /// </summary>
        internal const string ENGINE_PREFIX = "[BroforceModEngine]";

        internal static List<string> logs = new List<string>();
        /// <summary>
        /// Path of the log file
        /// </summary>
        internal static string LogFilePath { get; private set; }

        private static bool _loaded;

        /// <summary>
        /// Logger
        /// </summary>
        /// <param name="prefix">Prefix of the mod</param>
        /// <param name="message">Message to send</param>
        public static void Log(string prefix, string message)
        {
            Log(prefix, message, BroforceModEngine.Loggers.LogType.Log);
        }
        /// <summary>
        /// Logger
        /// </summary>
        /// <param name="prefix">Prefix of the mod</param>
        /// <param name="message">Message to send</param>
        /// <param name="logType">Type of the message</param>
        public static void Log(string prefix, string message, BroforceModEngine.Loggers.LogType logType)
        {
            try
            {
                if(!_loaded)
                {
                    Load();
                }

                string newMessage = "[" + DateTime.Now.ToString("HH:mm:ss") + "] " + prefix + (logType == BroforceModEngine.Loggers.LogType.Log ? " " : "[" + logType.ToString() + "]") + message;
                logs.Add(newMessage);
                WriteLogFile(newMessage);
                // ScreenLogger.Instance?.AddLogOnScreen(message);
                if (ScreenLogger.Instance != null) ScreenLogger.Instance.AddLogOnScreen(message); // Changed due to errors on my side (no clue why!) - Bobby
            }
            catch(Exception ex)
            {
                NoEchecLog(ex.ToString());
            }
        }

        /// <summary>
        /// Load the logger
        /// </summary>
        internal static void Load()
        {
            try
            {
                LogFilePath = Path.Combine(ModEngine.EngineDirectoryPath, "BroforceModsEngine_Log.txt");


                // Delete Log file
                if (File.Exists(GlobalLogger.LogFilePath))
                {
                    File.Delete(GlobalLogger.LogFilePath);
                    File.Create(LogFilePath);
                }

                _loaded = true;
            }
            catch(Exception ex)
            {
                NoEchecLog(ex.ToString());
            }
        }

        /// <summary>
        /// In case the other log methods don't work.
        /// </summary>
        /// <param name="msg"></param>
        /// 
        internal static void NoEchecLog(string msg)
        {
            if (!_loaded)
            {
                Load();
            }
            logs.Add(msg);
            WriteLogFile(msg);
        }

        /// <summary>
        /// Write all the log in a file
        /// </summary>
        private static void WriteLogFile(string message)
        {
            /*if (!File.Exists(LogFilePath))
            {
                File.Create(LogFilePath);
            }*/
             /*using (StreamWriter writer = File.AppendText(LogFilePath))
             {
                 writer.WriteLine(message);
                writer.Close();
             }*/
            File.WriteAllText(LogFilePath, message);
        }
    }

    /// <summary>
    /// Type of log for the log. They each have a "custom" color.
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// White
        /// </summary>
        Log,

        /// <summary>
        /// Yellow
        /// </summary>
        Warning,

        /// <summary>
        /// Red
        /// </summary>
        Error,

        /// <summary>
        /// Red
        /// </summary>
        Exception
    }
}
