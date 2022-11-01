using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Diagnostics;
using System.Security.Principal;
using System.IO;

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

    public static class Data { // Nicknamed data because it modifies data - namely files
        // LATEST UPLOAD
        public static FileStates FileState;

        public static string[] LastFiles = null;
        public static string LastFile = null;
        public static string LastPath = null;

        public static class EXE {
            public const string StorageFilePath = @".\Storage\STORE.txt";

            /// <summary>
            /// Check if exe storage location exists
            /// </summary>
            public static bool GetExeStorage(){
                return File.Exists(StorageFilePath);
            }

            /// <summary>
            /// Create the file
            /// </summary>
            static void CreateExeStorage(){
                string dir = Path.GetDirectoryName(StorageFilePath);
                string name = StorageFilePath;

                // If directory does not exist, create it
                if (!Directory.Exists(dir)){
                    Directory.CreateDirectory(dir);

                    File.WriteAllText(name, name);
                }
            }

            /// <summary>
            /// Add the exe location to a text file
            /// </summary>
            /// <returns></returns>
            public static FileStates AddExe(){
                if (LastFile.Contains(".exe") && LastFile.Contains("force", StringComparison.OrdinalIgnoreCase) && LastFile.Contains("bro", StringComparison.OrdinalIgnoreCase)) {
                    CreateExeStorage();

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