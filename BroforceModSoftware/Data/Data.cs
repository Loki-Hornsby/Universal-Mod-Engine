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

namespace BROMODS {
    public enum FileStates{
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

    public static class Data {
        public static string debug; // Testing purposes only

        public const string ModPath = @"..\..\..\..\Mods"; // ..\ resembles 1 folder back
        public const string SoundsPath = @"..\..\..\Sounds";

        // LAST UPLOAD
        public static FileStates FileState;
        public static string[] LastFiles = null;
        public static string LastFile = null;

        // EXE
        public static string BroforceExe = null;
        
        // MODS
        public static string[] Mods = null;

        public static void RefreshModList(){
            Mods = Directory.GetFiles(ModPath, "*.dll", SearchOption.AllDirectories);
            for (int i = 0; i < Mods.Length; i++){
                Mods[i] = Path.GetFileName(Mods[i]);
            }

            //debug = ""; //Mods[0] + LastFile + " ";
        }

        public static FileStates RemoveMod(){
            if (LastFile.Contains(".dll")) {
                RefreshModList();

                if (!Mods.Contains(LastFile)){
                    FileState = FileStates.SuccessOnDelete;
                } else {
                    FileState = FileStates.Exists;
                }

                /*if (!failAfterLoad){
                    FileState = FileStates.SuccessOnMod;
                } else {
                    FileState = FileStates.FailOnMod;
                }*/
            } else {
                FileState = FileStates.Invalid;
            }
            
            return FileState;
        }

        public static FileStates AddMod(){
            if (LastFile.Contains(".dll")) {
                RefreshModList();

                if (!Mods.Contains(LastFile)){
                    FileState = FileStates.SuccessOnMod;
                } else {
                    FileState = FileStates.Exists;
                }

                /*if (!failAfterLoad){
                    FileState = FileStates.SuccessOnMod;
                } else {
                    FileState = FileStates.FailOnMod;
                }*/
            } else {
                FileState = FileStates.Invalid;
            }
            
            return FileState;
        }

        public static FileStates AddExe(){
            if (LastFile.Contains(".exe")) {
                // Todo: check file first
                BroforceExe = LastFile;

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

        public static FileStates SendFiles(string[] files, bool delete){
            LastFiles = files;
            LastFile = Path.GetFileName(LastFiles[0]);

            if (LastFiles.Length > 0){ // Empty?
                if (LastFiles.Length == 1){ // Only 1 file?
                    if (!delete){ // Delete mode?
                        if (BroforceExe == null){ // Exe or Mod stage?
                            return AddExe();
                        } else {
                            return AddMod();
                        }
                    } else {
                        return RemoveMod();
                    }
                } else {
                    FileState = FileStates.Excess;
                }
            } else {
                FileState = FileStates.Dearth;
            }

            return FileState;
        }
    }
}