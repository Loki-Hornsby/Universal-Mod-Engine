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
        }
        
        /// <summary>
        /// Get temporary file naming convention
        /// </summary>
        static string GetTemp(string file){
            return Path.GetDirectoryName(file) + @"\" + "Temp_" + Path.GetFileName(file);
        }

        /// <summary>
        /// Create a temporary file from backup
        /// </summary>
        static string BackupAndTemp(string file){
            // We backup the original file first 
            CreateBackup(file);

            string temp = GetTemp(file);

            // Delete Previous temp file
            if (File.Exists(temp)){
                File.Delete(temp);
            }

            // Copy from backup
            if (!File.Exists(temp)){
                string backup = GetBackup(file);

                if (File.Exists(backup)) {
                    File.Copy(backup, temp, false);
                } else {
                    System.Console.WriteLine("Create Backup Failed!");
                }
            } else {
                System.Console.WriteLine("Delete Temp Failed!");
            }

            return temp;
        }

        static void ConvertTemp(string file){
            string backupName = Path.GetFileName(GetBackup(file));

            // Rename temp to match backup file - We actually copy it so a temp file is always present for debug reasons
            File.Copy(file, Path.GetDirectoryName(file) + @"\" + backupName);
        }

        public static int Inject(string BroDLL) {
            // Create a temporary file to modify and then eventually overwrite assembly-csharp.
            string temp = BackupAndTemp(BroDLL);

            // Load our chosen assembly (Assembly-CSharp.dll)
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
            ChosenAssembly.GetDefinition().Write(temp);

            // Convert temp to its backup equivalent
            ConvertTemp(temp);
            
            return 0;
        }
    }
}