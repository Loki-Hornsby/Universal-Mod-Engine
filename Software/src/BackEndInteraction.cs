using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Security.Permissions;

using Software.Interaction.Front;
//using Engine;

/// <summary>
/// Handles back end interaction for the GUI
/// This is the entry point for where the software begins communication to the engine
/// </summary>

namespace Software.Interaction.Back {
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
        
        /// <summary>
        /// Check if file at path is running
        /// </summary>
        public static bool InstanceIsRunning(string path){
            if (EXE.GetLocation() == null){
                //Logger.log("EXE Location is invalid!", Logger.LogType.Warning, Logger.VerboseType.Medium);

                return false;
            } else {
                try {
                    foreach (Process p in Process.GetProcesses()){
                        // Bug: could cause errors if someone has a weird path --> how can i fix this?
                        if (path.ToLower().Contains(p.ProcessName.ToLower())){ 
                            FI.Visuals.ExitWithMessageBox(
                                "An instance of " + Path.GetFileName(path) + " named "
                                + "'" + p.ProcessName + "'" +
                                " is already running! Please close it before trying to launch the engine."
                            );

                            return true;
                        }
                    }

                    return false;
                } catch (Exception e) {
                    //Logger.log(e.ToString(), Logger.LogType.Error, Logger.VerboseType.Low);

                    return false;
                }
            }
        }

        /// <summary>
        /// Pass the GUI logging over to the Engine
        /// </summary>
        public static string PassEngineLogLow(string message){
            //Logger.log(message, Logger.LogType.Engine, Logger.VerboseType.Low);

            return "";
        }

        /// <summary>
        /// Pass the GUI logging over to the Engine
        /// </summary>
        public static string PassEngineLogMedium(string message){
            //Logger.log(message, Logger.LogType.Engine, Logger.VerboseType.Medium);

            return "";
        }

        /// <summary>
        /// Pass the GUI logging over to the Engine
        /// </summary>
        public static string PassEngineLogHigh(string message){
            //Logger.log(message, Logger.LogType.Engine, Logger.VerboseType.High);

            return "";
        }

        /// <summary>
        /// Begins loading the Engine as well as .exe
        /// https://stackoverflow.com/questions/2237628/c-sharp-process-killing/2237689#2237689
        /// </summary>
        public static void BeginLoad(){
            // Load Engine
            /*System.Console.WriteLine(
                Loader.Load(
                    PassEngineLogLow, PassEngineLogMedium, PassEngineLogHigh,
                    Path.Combine(Path.GetDirectoryName(EXE.GetLocation()), @"Broforce_beta_Data\Managed\Assembly-CSharp.dll") 
                    // Bug: this path isn't the same for every game
                )
            );*/

            // Open // Bug: this causes read/write issues // Todo: when GUI is closed assemblyCSharp.dll should return to default
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
                        //Logger.log("The stored path to the executable is empty.", Logger.LogType.Warning, Logger.VerboseType.Medium);
                    } else {
                        //Logger.log(s + " is an invalid path for the executable.", Logger.LogType.Warning, Logger.VerboseType.Medium);
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