using BroforceModEngine.Loggers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace BroforceModEngine.Loggers
{
    /// <summary>
    /// On screen logger.
    /// </summary>
    public class ScreenLogger : MonoBehaviour
    {
        /// <summary>
        /// Instance of ScreenLogger
        /// </summary>
        public static ScreenLogger Instance
        {
            get
            {
                return instance;
            }
        }

        private float timeOnScreen = 3f;
        private float timeRemaining = 3f;
        private List<string> logsOnScreen = new List<string>();
        private static ScreenLogger instance;

        /// <summary>
        /// Add a log on the screen
        /// </summary>
        /// <param name="message"></param>
        internal void AddLogOnScreen(string message)
        {
            logsOnScreen.Add(message);
        }

        /// <summary>
        /// Create the GameObject
        /// </summary>
        internal static void Load()
        {
            try
            {
                new GameObject(typeof(ScreenLogger).FullName, typeof(ScreenLogger));
            }
            catch (Exception ex)
            {
                GlobalLogger.Log(GlobalLogger.ENGINE_PREFIX, "ScreenLogger Failed: " + ex.ToString());
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
            instance = this;
        }

        /// <summary>
        /// Clear the log on screen.
        /// </summary>
        private void ClearLog()
        {
            logsOnScreen = new List<string>();
        }

        private void OnGUI()
        {
            if (logsOnScreen.Count > 0)
            {
                GUILayout.BeginVertical("box");
                var LogStyle = new GUIStyle();
                foreach (string log in logsOnScreen)
                {
                    LogStyle.normal.textColor = WhichColor(log);
                    GUILayout.Label(log, LogStyle);
                }
                GUILayout.EndVertical();
            } 
        }

        private void Start()
        {
            GlobalLogger.Log("", "TEST Log", BroforceModEngine.Loggers.LogType.Log);
            GlobalLogger.Log("", "TEST Warning", BroforceModEngine.Loggers.LogType.Warning);
            GlobalLogger.Log("", "TEST ERROR", BroforceModEngine.Loggers.LogType.Error);
            GlobalLogger.Log("", "TEST Exception", BroforceModEngine.Loggers.LogType.Exception);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F7))
            {
                ClearLog();
                GlobalLogger.Log("", "Cleared Log", BroforceModEngine.Loggers.LogType.Log);
            }

            if (logsOnScreen.Count > 0)
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                }
                else
                {
                    logsOnScreen.Remove(logsOnScreen.First());
                    timeRemaining = timeOnScreen;
                }
            }
            if (logsOnScreen.Count > 30)
            {
                logsOnScreen.Remove(logsOnScreen.First());
            }

            GUILayout.BeginVertical("box");

            GUILayout.Button("I'm the top button");
            GUILayout.Button("I'm the bottom button");

            GUILayout.EndVertical();
        }

        /// <summary>
        /// Which color will be shown for the log.
        /// </summary>
        /// <param name="logMsg"></param>
        /// <returns></returns>
        private static Color WhichColor(string logMsg)
        {
            logMsg = logMsg.ToLower();
            if (logMsg.Contains("error") || logMsg.Contains("exception"))
            {
                return Color.red;
            }
            else if (logMsg.Contains("warning"))
            {
                return Color.yellow;
            }
            else
            {
                return Color.white;
            }
        }

    }
}
