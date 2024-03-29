using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Security.Permissions;

using BroforceModSoftware;
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

        /// <summary>
        /// Checks wether a file is in use or not
        /// </summary>
        // https://stackoverflow.com/questions/876473/is-there-a-way-to-check-if-a-file-is-in-use
        public static bool IsFileLocked(string s){
            try {
                FileInfo f = new FileInfo(s);
                
                using (FileStream stream = f.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None)){
                    stream.Close();
                }
            } catch (IOException ex) {
                return true;

                // We don't need a log here since an intentional error is meant to occur here
            }

            return false;
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
        /// </summary>
        public static Action BeginLoad(){
            return () => {
                if (!BI.EXE.IsInUse()){
                    // Do absolutely nothing while waiting...
                    while (!File.Exists(EXE.GetLocation())){}
                    while (!EXE.IsInUse()){}

                    // Begin load
                    Logger.AddNewLine();
                    Logger.Log("Starting Mod Engine Via GUI...", Logger.LogType.Custom, Logger.VerboseType.Low, Color.Purple);
                    System.Console.WriteLine(Loader.Load(PassEngineLogLow, PassEngineLogMedium, PassEngineLogHigh));

                    while (EXE.IsInUse()){} // Wait to close
                    System.Windows.Forms.Application.Exit();
                } else {
                    Logger.Log("Error occurred when trying to launch mod engine...", Logger.LogType.Error, Logger.VerboseType.Low);
                }
            };
        }
        
        // LATEST UPLOAD
        public static FileStates FileState;

        public static string[] LastFiles = null;
        public static string LastFile = null;
        public static string LastPath = null;

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

                    // We don't need a log here since an intentional error is meant to occur here
                }

                return s;
            }

        
            /// <summary>
            /// Checks wether EXE (Broforce.exe) is in use
            /// </summary>
            public static bool IsInUse(){
                if (String.IsNullOrEmpty(EXE.GetLocation())){
                    return false;
                } else {
                    return IsFileLocked(EXE.GetLocation());
                }
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
            public static void AddExe(){
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
                LastFiles = files;
                LastFile = Path.GetFileName(LastFiles[0]);
                LastPath = Path.GetDirectoryName(LastFiles[0]);

                if (LastFiles.Length > 0){ // Empty?
                    if (LastFiles.Length == 1){ // Only 1 file?
                        EXE.AddExe();
                    } else {
                        FileState = FileStates.Excess;
                    }
                } else {
                    FileState = FileStates.Dearth;
                }
            }

            /// <summary>
            /// Returns the last file stored
            /// </summary>
            public static string GetId(){
                string pfull = ((LastFile == null) ? Path.GetFullPath(EXE.StorageFilePath) : Path.GetFullPath(LastFile));
                return pfull;
            }
        }
    }
}