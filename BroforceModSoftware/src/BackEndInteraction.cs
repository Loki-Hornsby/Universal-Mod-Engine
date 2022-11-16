using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Security.Permissions;

using BroforceModSoftware;
using BroforceModSoftware.Interaction.Front;
using BroforceModEngine;

/// <summary>
/// Handles back end interaction for the GUI
/// This is the entry point for where the software begins communication to the engine
/// </summary>

namespace BroforceModSoftware.Interaction.Back {
    public static class BI { 
        public enum FileStates {
            Dearth, // Too little files (None were found)
            Excess, // Too many files
            Invalid, // Invalid file type

            SuccessOnExe, // YES! SUCCESS!
            FailOnExe, // NO! FAILURE!

            SuccessOnMod, // YES! MORE SUCCESS!
            FailOnMod, // NO! MORE FAILURE!
        }

        public static string InjectionDLL = 
            @"C:\Program Files (x86)\Steam\steamapps\common\Broforce\Broforce_beta_Data\Managed\Assembly-CSharp.dll";
        public static string BroforceDLL = 
            @"C:\Program Files (x86)\Steam\steamapps\common\Broforce\Broforce_beta_Data\Managed\Assembly-CSharp.dll";
        
        /// <summary>
        /// Check if file at path is running
        /// </summary>
        public static bool InstanceIsRunning(string path, string PseudoName){
            if (EXE.GetLocation() == null){
                Logger.Log("EXE Location is invalid!", Logger.LogType.Warning, Logger.VerboseType.Medium);

                return false;
            } else {
                try {
                    foreach (Process p in Process.GetProcesses()){
                        // Bug: could cause errors if someone has a weird path --> how can i fix this?
                        if (path.ToLower().Contains(p.ProcessName.ToLower())){ 
                            FI.Visuals.ExitWithMessageBox(
                                "An instance of " + PseudoName + " named "
                                + "'" + p.ProcessName + "'" +
                                " is already running! Please close it before trying to launch the engine."
                            );

                            return true;
                        }
                    }

                    return false;
                } catch (Exception e) {
                    Logger.Log(e.ToString(), Logger.LogType.Error, Logger.VerboseType.Low);

                    return false;
                }
            }
        }

        /// <summary>
        /// Pass the GUI logging over to the mod engine
        /// </summary>
        public static string PassEngineLogLow(string message){
            Logger.Log(message, Logger.LogType.Engine, Logger.VerboseType.Low);

            return "";
        }

        /// <summary>
        /// Pass the GUI logging over to the mod engine
        /// </summary>
        public static string PassEngineLogMedium(string message){
            Logger.Log(message, Logger.LogType.Engine, Logger.VerboseType.Medium);

            return "";
        }

        /// <summary>
        /// Pass the GUI logging over to the mod engine
        /// </summary>
        public static string PassEngineLogHigh(string message){
            Logger.Log(message, Logger.LogType.Engine, Logger.VerboseType.High);

            return "";
        }

        /// <summary>
        /// Begins loading the mod engine as well as broforce.exe
        /// https://stackoverflow.com/questions/2237628/c-sharp-process-killing/2237689#2237689
        /// </summary>
        public static void BeginLoad(){
            // Raise error if assembly-csharp is being accessed
            BI.InstanceIsRunning(BroforceDLL, "Assembly-Csharp.dll");

            // Load Engine
            System.Console.WriteLine(
                Loader.Load(
                    PassEngineLogLow, PassEngineLogMedium, PassEngineLogHigh,
                    InjectionDLL, BroforceDLL
                )
            );

            // Open Broforce
            //Process.Start(EXE.GetLocation());
        }
        
        // LATEST UPLOAD
        public static FileStates FileState;

        public static class EXE {
            public const string StorageFilePath = @".\Storage\STORE.txt";

            /// <summary>
            /// Get location of exe
            /// </summary>
            public static string GetLocation(){
                string s;

                try{
                    s = File.ReadAllText(StorageFilePath);
                } catch (IOException ex){
                    s = null;
                }

                if (!File.Exists(s)){
                    if (String.IsNullOrEmpty(s)){
                        Logger.Log("The stored path to the Broforce executable is empty.", Logger.LogType.Warning, Logger.VerboseType.Medium);
                    } else {
                        Logger.Log(s + " is an invalid path for the Broforce executable.", Logger.LogType.Warning, Logger.VerboseType.Medium);
                    }

                    s = null;
                }

                return s;
            }

            /// <summary>
            /// Create the file
            /// </summary>
            static void CreateExeStorage(string path, string name){
                string StorageDir = Path.GetDirectoryName(StorageFilePath);
                string StorageFull = StorageFilePath;

                // If directory does not exist, create it
                if (!Directory.Exists(StorageDir)){
                    Directory.CreateDirectory(StorageDir);
                }

                // Write EXE path and name to file
                if (!File.Exists(StorageFull)){
                    File.WriteAllText(StorageFull, Path.Combine(path, name));
                }
            }

            /// <summary>
            /// Add the exe location to a text file
            /// </summary>
            public static void AddExe(string LastPath, string LastFile){
                if (LastFile.Contains(".exe")) {
                    // Create exe storage
                    CreateExeStorage(LastPath, LastFile);

                    FileState = FileStates.SuccessOnExe;
                } else {
                    FileState = FileStates.Invalid;
                }
            }
        }

        public static class Files {
            /// <summary>
            /// Send the file uploaded to be added as an exe or if reenabled by a mod sent to it's reciever
            /// </summary>
            public static void SendFiles(string[] files){
                string[] LastFiles = files;

                if (LastFiles.Length > 0){ // Empty?
                    if (LastFiles.Length == 1){ // Only 1 file?
                        EXE.AddExe(Path.GetDirectoryName(LastFiles[0]), Path.GetFileName(LastFiles[0]));
                    } else {
                        FileState = FileStates.Excess;
                    }
                } else {
                    FileState = FileStates.Dearth;
                }
            }
        }
    }
}