using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

// Notes;
// https://www.mono-project.com/docs/tools+libraries/libraries/Mono.Cecil/faq/
// https://www.codersblock.org/blog//2014/06/integrating-monocecil-with-unity.html
// https://forum.exetools.com/showthread.php?t=16470

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
        static LoadedAssembly ChosenAssembly = null;

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

            // Move File to Backup
            if (!File.Exists(BackupFilePath)){
                File.Move(file, BackupFilePath);
            }

            // Delete last file written to by injector
            if (File.Exists(file)){
                File.Delete(file);
            }
        }

        public static int Inject(string BroDLL) {
            // Create backup of file (so long as a backup doesn't already exists) and also delete the previous injector file
            CreateBackup(BroDLL);

            // Load our chosen assembly (Assembly-CSharp.dll)
            // We use GetBackup since the backup file is now used as a template ~ this frees up BroDLL to be used
            ChosenAssembly = new LoadedAssembly(GetBackup(BroDLL));

            // Get the method we want to inject into
            var method = InjectionUtils.GetMethodDefinition(InjectionUtils.GetTypeDefinition(ChosenAssembly.GetTypes(), "Menu"), "Awake");

            // Get the IL processor
            // CIL and IL are synonymous https://stackoverflow.com/questions/293800/what-is-the-difference-between-cil-and-msil-il 
            // https://en.wikipedia.org/wiki/Common_Intermediate_Language
            var il = method.Body.GetILProcessor();

            // Write our code into the chosen method
            /*Instruction first = method.Body.Instructions[0];
            il.InsertBefore(first, Instruction.Create(OpCodes.Ldstr, "Enter " + method.FullName + "." + method.Name ));
            il.InsertBefore(first, Instruction.Create(OpCodes.Call, method));

            Instruction last = method.Body.Instructions[method.Body.Instructions.Count - 1];
            il.InsertBefore(last, Instruction.Create(OpCodes.Ldstr, "Exit " + method.FullName + "." + method.Name ) );
            il.InsertBefore(last, Instruction.Create(OpCodes.Call, method));*/

            method.Body.Instructions.Clear(); 
            method.Body.ExceptionHandlers.Clear(); 

            // Write to the assembly
            // We use BroDLL and thus replace Broforce's DLL with ours
            ChosenAssembly.GetDefinition().Write(BroDLL);
            
            return 0;
        }
    }
}