/// <summary>
/// Copyright 2022, Loki Alexander Button Hornsby (Loki Hornsby), All rights reserved.
/// Licensed under the BSD 3-Clause "New" or "Revised" License
/// </summary>

using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using System.Linq;

using Software;
using Injection.Utilities;
using Injection.Library;

namespace Injection {
    // This is the class for the assembly we load
    public class LoadedAssembly {
        Mono.Cecil.AssemblyDefinition definition = null;

        /// <summary>
        /// Constructor for our assembly class
        /// </summary>
        public LoadedAssembly(string Input){
            // Load the dll
            try {
                definition = AssemblyDefinition.ReadAssembly(Input, new ReaderParameters { ReadWrite = true });
            } catch (System.BadImageFormatException e) {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Get our definition (Our assembly)
        /// </summary>
        public Mono.Cecil.AssemblyDefinition GetDefinition(){
            return definition;
        }

        /// <summary>
        /// Get our main module from our assembly
        /// </summary>
        public Mono.Cecil.ModuleDefinition GetMainModule(){
            return definition.MainModule;
        }

        /// <summary>
        /// Get all the types (classes) from our main module
        /// </summary>
        public Mono.Collections.Generic.Collection<Mono.Cecil.TypeDefinition> GetTypes(){
            return definition.MainModule.Types;
        }
    }

    // This is the INJECTOR2000.
    public static class Injector {
        public static LoadedAssembly ChosenAssembly = null;

        public static int Main(string[] Args){
            System.Console.WriteLine("Injector Compiled.");

            return 0;
        }

        /// <summary>
        /// Get backup file naming convention
        /// </summary>
        static string GetBackup(string file){
            return Path.GetDirectoryName(file) + @"\..\..\BACKUP\" + Path.GetFileName(file);
        }

        /// <summary>
        /// Move a file into a BACKUP folder
        /// </summary>
        static void CreateBackup(string file){
            /// Todo: Uninstaller should move original assembly-csharp back into correct place
            
            string BackupFilePath = GetBackup(file);
            string BackupFolder = Path.GetDirectoryName(BackupFilePath);

            // Create Backup Folder
            if (!Directory.Exists(BackupFolder)){
                Directory.CreateDirectory(BackupFolder);
            }

            // Move original file to generated backup path
            if (!File.Exists(BackupFilePath)){
                File.Move(file, BackupFilePath);

            // If a backup already exists then we can safely delete the original file
            } else {
                File.Delete(file);
            }
        }

        /// <summary>
        /// Initialize our file backup
        /// </summary>
        static void InitializeBackup(string origin){
            string backup = GetBackup(origin);

            // If a backup of our original (origin) file exists
            if (File.Exists(backup)){
                // Delete the origin
                File.Delete(origin);

                // Replace the origin with the backup file
                File.Copy(backup, origin);

            // Create a backup if it doesn't exist
            } else {
                CreateBackup(origin);
            }
        }

        public static int Inject(string DLL) {
            // If our DLL exists
            if (File.Exists(DLL)){
                // Initialize Backup Procedure for the file
                InitializeBackup(DLL);

                // Load our chosen assembly (Assembly-CSharp.dll)
                ChosenAssembly = new LoadedAssembly(GetBackup(DLL));

                // ~~@ Testing
                Tools.ChangeField<bool>("TestVanDammeAnim", "Awake", "canAirdash", true);

                // Write to the assembly
                ChosenAssembly.GetDefinition().Write(DLL);
            } else {
                Logger.Log("DLL DOESN'T EXIST!", Logger.LogType.Error, Logger.VerboseType.Low);
            }

            return 0;
        }
    }
}