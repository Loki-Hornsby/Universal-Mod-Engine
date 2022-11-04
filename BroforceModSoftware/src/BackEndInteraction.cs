using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Security.Permissions;

using BroforceModSoftware;

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
        static bool IsFileLocked(FileInfo f){
            try {
                using (FileStream stream = f.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None)){
                    stream.Close();
                }
            } catch (Exception ex) {
                return true;

                // We don't need a log here since an intentional error is meant to occur here
            }

            return false;
        }

        // LATEST UPLOAD
        public static FileStates FileState;

        public static string[] LastFiles = null;
        public static string LastFile = null;
        public static string LastPath = null;

        public static class EXE {
            public const string StorageFilePath = @".\Storage\STORE.txt";
            public const string EnginePath = @"..\..\..\..\BroforceModEngine\BroforceModEngine\bin\Release";
            public const string Doorstop = @"..\..\..\..\BroforceModEngine\lib\Doorstop";
            public const string EngineFolderName = "BROMODS";

            /// <summary>
            /// Get location of exe
            /// </summary>
            public static string GetExeLocation(){
                string s;

                try{
                    s = File.ReadAllText(StorageFilePath);
                } catch (Exception ex){
                    s = null;

                    // We don't need a log here since an intentional error is meant to occur here
                }

                return s;
            }

            /// <summary>
            /// Checks wether EXE (Broforce.exe) is in use
            /// </summary>
            public static bool IsInUse(){
                return IsFileLocked(new FileInfo(GetExeLocation()));
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
            /// Copy the engine to the exe
            /// </summary>
            public static void CopyEngineToExe(){
                string[][] files = new string[][] { 
                    new string[] { EnginePath, Path.Combine(GetExeLocation(), EngineFolderName) }, 
                    new string[] { Doorstop, GetExeLocation() }
                };

                // files[i] ~ Array containing source and destination
                // files[i][0] ~ Source
                // files[i][1] ~ Destination
                
                for (int i = 0; i < files.Length; i++){
                    string source = files[i][0];
                    string destination = files[i][1];

                    //Logger.Log("Directory Copying Is Failing");

                    //zDirectoryInfo dirInfo = new DirectoryInfo(source);

                    /*
                    DirectoryInfo[] dirs = dirInfo.GetDirectories();
                    foreach(DirectoryInfo dir in dirs){
                        //f.CopyTo(Path.Combine(destination, f.Name));
                        //Directory.CreateDirectory(Path.Combine(destination, dir.Name));
                        Console.WriteLine(Path.Combine(destination, dir.Name));
                        //Logger.Log("CHEESE", Color.Success);
                    }*/
                }
            }  

            /// <summary>
            /// Add the exe location to a text file
            /// </summary>
            public static void AddExe(){
                if (LastFile.Contains(".exe")) {
                    CreateExeStorage(LastPath, LastFile);
                    CopyEngineToExe();

                    FileState = FileStates.SuccessOnExe;

                    /*if (!failAfterLoad){
                        FileState = FileStates.SuccessOnExe;
                    } else {
                        FileState = FileStates.FailOnExe;
                    }*/
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