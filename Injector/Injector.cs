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

        /// <summary>
        /// Gets the IL processor
        /// </summary>
        public Mono.Cecil.Cil.ILProcessor GetIL(MethodDefinition method){
            // Get the IL processor (Only needed for methods)
            // CIL and IL are synonymous https://stackoverflow.com/questions/293800/what-is-the-difference-between-cil-and-msil-il 
            // https://en.wikipedia.org/wiki/Common_Intermediate_Language
                // https://en.wikipedia.org/wiki/List_of_CIL_instructions 
            return method.Body.GetILProcessor();
        }

        /// <summary>
        /// Gets a type definition from the new [Game] executable.
        /// Self Explainer: We access the public types available from our loaded assembly and then we return the one matching [typeName]
        /// Linkie: https://github.com/jbevain/cecil/blob/master/Mono.Cecil/TypeDefinition.cs
        /// </summary>
        private TypeDefinition GetTypeDefinition(Mono.Collections.Generic.Collection<Mono.Cecil.TypeDefinition> types, string typeName){
            return (from TypeDefinition t in types
                    where t.Name == typeName
                    select t).FirstOrDefault();
        }

        /// <summary>
        /// Gets a method definition from the new [Game] executable.
        /// Self Explainer: We look through the methods available in our chosen type and return the one matching [methodName]
        /// Linkie: https://github.com/jbevain/cecil/blob/master/Mono.Cecil/MethodDefinition.cs
        /// </summary>
        public MethodDefinition GetMethodDefinition(string className, string methodName){
            TypeDefinition t = this.GetTypeDefinition(
                this.GetTypes(), 
                className
            );

            return (from MethodDefinition m in t.Methods
                    where m.Name == methodName
                    select m).FirstOrDefault();
        }

        /// <summary>
        /// Gets a field definition from the new [Game] executable.
        /// Self Explainer: Gets a variable of any type
        /// Linkie: https://github.com/jbevain/cecil/blob/master/Mono.Cecil/FieldDefinition.cs 
        /// </summary>
        public FieldDefinition GetFieldDefinition(string className, string fieldName){
            TypeDefinition t = this.GetTypeDefinition(
                this.GetTypes(), 
                className 
            );
            
            return (from FieldDefinition f in t.Fields
                    where f.Name == fieldName
                    select f).FirstOrDefault();
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
        static bool InitializeBackup(string origin){
            string backup = GetBackup(origin);

            // If a backup of our original (origin) file exists
            if (File.Exists(backup)){
                // Delete the origin
                File.Delete(origin);

                // Replace the origin with the backup file
                File.Copy(backup, origin);

                return true;

            // Create a backup if it doesn't exist
            } else {
                if (File.Exists(origin)){
                    CreateBackup(origin);

                    return true;
                } else {
                    return false;
                }
            }
        }

        public static int Inject(string DLL) {
            // Initialize Backup Procedure for the file
            if (InitializeBackup(DLL)) {
                // Load our chosen assembly (Assembly-CSharp.dll)
                ChosenAssembly = new LoadedAssembly(GetBackup(DLL));

                // ~~@ Testing
                Library.ChangeField<bool>(
                    ChosenAssembly, 
                    "TestVanDammeAnim", 
                    "Awake", 
                    "canAirdash", 
                    true
                );

                // Write to the assembly
                ChosenAssembly.GetDefinition().Write(DLL);
            } else {
                Logger.Log("DLL DOESN'T EXIST!", Logger.LogType.Error, Logger.VerboseType.Low);
            }

            return 0;
        }
    }
}