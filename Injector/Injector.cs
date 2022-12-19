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

            bool backup = false;
            string BackupFilePath = GetBackup(file);
            string BackupFolder = Path.GetDirectoryName(BackupFilePath);

            // Create Backup Folder
            if (!Directory.Exists(BackupFolder)){
                Directory.CreateDirectory(BackupFolder);
            }

            // Move File to Backup
            if (!File.Exists(BackupFilePath)){
                File.Move(file, BackupFilePath);

                backup = true;
            }

            // Delete last file written to by injector
            if (File.Exists(file)){
                File.Delete(file);
            }
        }

        public static int Inject(string DLL) {
            // Create backup of file
            CreateBackup(DLL);

            // Load our chosen assembly (Assembly-CSharp.dll)
            ChosenAssembly = new LoadedAssembly(GetBackup(DLL));

            // ~~@ Testing
            Player.canWallClimb(false);

            // Write to the assembly
            ChosenAssembly.GetDefinition().Write(DLL);
            
            return 0;
        }
    }
}