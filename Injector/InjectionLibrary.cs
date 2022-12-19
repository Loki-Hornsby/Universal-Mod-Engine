using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using System.Linq;

using Injection.Utilities;

namespace Injection.Library {
    
    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Helpers ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ \\

    public static class Tools {
        /// <summary>
        /// Wipes a function clean
        /// </summary>
        public static void ClearFunction(MethodDefinition method){
            method.Body.Instructions.Clear(); 
            method.Body.ExceptionHandlers.Clear(); 
        }

        /// <summary>
        /// Gets the IL processor
        /// </summary>
        public static Mono.Cecil.Cil.ILProcessor GetIL(MethodDefinition method){
            // Get the IL processor (Only needed for methods)
            // CIL and IL are synonymous https://stackoverflow.com/questions/293800/what-is-the-difference-between-cil-and-msil-il 
            // https://en.wikipedia.org/wiki/Common_Intermediate_Language
                // https://en.wikipedia.org/wiki/List_of_CIL_instructions 
            return method.Body.GetILProcessor();
        }

        public static Mono.Cecil.Cil.OpCode? DetermineOpcode<T>(T x){
            if (x.GetType() == typeof(int)){
                return (Mono.Cecil.Cil.OpCode?) OpCodes.Ldc_I4;
            } else if (x.GetType() == typeof(float)){
                return (Mono.Cecil.Cil.OpCode?) OpCodes.Ldc_R4;
            } else if (x.GetType() == typeof(string)){
                return (Mono.Cecil.Cil.OpCode?) OpCodes.Ldstr;
            } else if (x.GetType() == typeof(bool)){
                return (Mono.Cecil.Cil.OpCode?) OpCodes.Ldc_I4;
            } else {
                return null;
            }
        }

        public static void ChangeField<T>(string className, string methodName, string fieldName, T x){
            Mono.Cecil.Cil.OpCode opcode = (Mono.Cecil.Cil.OpCode) DetermineOpcode<T>(x);

            var a = AssemblyDefinition.ReadAssembly(typeof(InjectionUtils).Assembly.Location);
            var type = x.GetType();
            var tr = a.MainModule.ImportReference(type);
            var td = tr.Resolve();

            if (opcode != null){
                var method = InjectionUtils.GetMethodDefinition(className, methodName);
                var field = InjectionUtils.GetFieldDefinition(className, fieldName);
                var il = Tools.GetIL(method);

                // RID: 10554
                Instruction first = method.Body.Instructions[0];

                il.InsertBefore( // Load arg 0
                    first, Instruction.Create(
                        OpCodes.Ldarg_0
                    )
                );    

                il.InsertBefore( // Push x to stack
                    first, Instruction.Create(
                        opcode, td // x
                    )
                );    

                il.InsertBefore( // Apply x to field
                    first, Instruction.Create(
                        OpCodes.Stfld, field
                    )
                );    
            } else {
                System.Console.WriteLine("FIELD COULDN'T BE CHANGED! " + className + @"\" + methodName + @"\" + fieldName);
            }
        }
    }

    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Library ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ \\

    public static class Player {
        public static void canWallClimb(bool x){
            // RID: 10554
            Tools.ChangeField<bool>("TestVanDammeAnim", "Awake", "canWallClimb", x);
        }
    }
}