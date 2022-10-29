using RocketLib.Loggers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static ScreenDebug;

namespace BroforceModEngine.Loggers
{
    public static class GlobalLogger
    {
        internal static List<string> allLogs = new List<string>();

        internal const string ENGINE_PREFIX = "[BroforceModEngine]";

        private static string LogFilePath
        {
            get
            {
                return Path.Combine(Directory.GetCurrentDirectory(), "Log.txt");
            }
        }

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
                //string newMessage = "[" + DateTime.Now.ToString("HH:mm:ss") + "] " + prefix + (logType == BroforceModEngine.Loggers.LogType.Log ? " " : "[" + logType.ToString() + "]") + message;
                string newMessage = message;
                allLogs.Add(newMessage);
                WriteLogFile(newMessage);
                //ScreenLogger.Instance?.AddLogOnScreen(message);
            }
            catch
            {

            }
            
        }

        /// <summary>
        /// Write all the log in a file
        /// </summary>
        private static void WriteLogFile(string message)
        {
            try
            {
                if (!File.Exists(LogFilePath))
                {
                    File.Create(LogFilePath);
                }
                //File.write(LogFilePath, allLogs.ToArray());
                using (StreamWriter writer = File.AppendText(LogFilePath))
                {
                    writer.WriteLine(message);
                }
            }
            catch (Exception ex)
            {

            }
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
