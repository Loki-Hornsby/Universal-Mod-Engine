using System;

/// <summary>
/// Handles communication to and detection of processes
/// </summary>

namespace ModEngine.Processes {
    public static class Processes {
        /// </summary>
        /// Tries to find a process by name
        /// </summary>
        //public static Process[] FindByName(string name){
            
        //}

        /// <summary>
        /// Check if a process is still running
        /// </summary>
        public static bool IsRunning(this Process process){
            if (process == null) 
                throw new ArgumentNullException("process");

            try {
                Process.GetProcessById(process.Id);
            } catch (ArgumentException) {
                return false;
            }

            return true;
        }
    }
}