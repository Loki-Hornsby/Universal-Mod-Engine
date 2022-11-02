using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Diagnostics;
using System.IO;

/// <summary>
/// Handles the Addition and Removal of mods as well as Hooking to Broforce.exe
/// Referenced By: GUI.cs (BroforceModSoftware), GUI.cs (BroforceModEngine, GUI.cs)
/// </summary>

namespace BroforceModEngine.Handling {
    public enum FileStates {
        Exists, // Already exists
        Dearth, // Too little files (None were found)
        Excess, // Too many files
        Invalid, // Invalid file type

        SuccessOnExe, // YES! SUCCESS!
        FailOnExe, // NO! FAILURE!

        SuccessOnMod, // YES! MORE SUCCESS!
        FailOnMod, // NO! MORE FAILURE!

        SuccessOnDelete, // YES! AWESOME!
        FailOnDelete, // NO! TERRIBLE!
    }

    public static class Processes {
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

    public static class Data { // Nicknamed data because it modifies data - namely files
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
                } catch (Exception e){
                    s = "";
                }

                return s;
            }

            /// <summary>
            /// Create the file
            /// </summary>
            static void CreateExeStorage(string exe){
                string dir = Path.GetDirectoryName(StorageFilePath);
                string name = StorageFilePath;

                // If directory does not exist, create it
                if (!Directory.Exists(dir)){
                    Directory.CreateDirectory(dir);

                    File.WriteAllText(name, exe);
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

                    //Logger.Log("Directory Copying Is Failing", Color.Red);

                    DirectoryInfo dirInfo = new DirectoryInfo(source);

                    /*
                    DirectoryInfo[] dirs = dirInfo.GetDirectories();
                    foreach(DirectoryInfo dir in dirs){
                        //f.CopyTo(Path.Combine(destination, f.Name));
                        //Directory.CreateDirectory(Path.Combine(destination, dir.Name));
                        Console.WriteLine(Path.Combine(destination, dir.Name));
                        //Logger.Log("CHEESE", Color.Green);
                    }*/
                }
            }  

            /// <summary>
            /// Add the exe location to a text file
            /// </summary>
            /// <returns></returns>
            public static FileStates AddExe(){
                if (LastFile.Contains(".exe")) {
                    CreateExeStorage(LastPath);
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

                return FileState;
            }
        }

        public static class Files{
            /// <summary>
            /// Send the file uploaded to be added as an exe or if reenabled by a mod sent to it's reciever
            /// </summary>
            /// <param name="files"></param>
            /// <returns></returns>
            public static FileStates SendFiles(string[] files){
                LastFiles = files;
                LastFile = Path.GetFileName(LastFiles[0]);
                LastPath = Path.GetDirectoryName(LastFiles[0]);

                if (LastFiles.Length > 0){ // Empty?
                    if (LastFiles.Length == 1){ // Only 1 file?
                        return EXE.AddExe();
                    } else {
                        FileState = FileStates.Excess;
                    }
                } else {
                    FileState = FileStates.Dearth;
                }

                return FileState;
            }

            /// <summary>
            /// Returns the last file stored
            /// </summary>
            /// <returns></returns>
            public static string GetId(){
                string pfull = ((LastFile == null) ? Path.GetFullPath(Data.EXE.StorageFilePath) : Path.GetFullPath(LastFile));
                return pfull;
            }
        }
    }
}