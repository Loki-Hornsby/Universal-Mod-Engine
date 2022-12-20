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

namespace Injection.Library {
    
    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ General Helpers ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ \\

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

        /// <summary>
        /// Determine needed opcode for operation
        /// </summary>
        public static Mono.Cecil.Cil.OpCode? DetermineOpcode<T>(T x){
            if (x.GetType() == typeof(int)){
                // Pushes a supplied value of type int32 onto the evaluation stack as an int32
                return OpCodes.Ldc_I4;

            } else if (x.GetType() == typeof(float)){
                // Pushes a supplied value of type float32 onto the evaluation stack as type F (float)
                return OpCodes.Ldc_R4;

            } else if (x.GetType() == typeof(string)){
                // Pushes a new object reference to a string literal stored in the metadata.
                return OpCodes.Ldstr;

            } else if (x.GetType() == typeof(bool)){
                // Pushes a supplied value of type int32 onto the evaluation stack as an int32
                return OpCodes.Ldc_I4;

            } else {
                return null;
            }
        }

        /// <summary>
        /// Change a defined field (variable)
        /// </summary>
        public static void ChangeField<T>(string className, string methodName, string fieldName, T x){
            // Opcode
            Mono.Cecil.Cil.OpCode op = (Mono.Cecil.Cil.OpCode) DetermineOpcode<T>(x);

            // Resolve of type
            var a = AssemblyDefinition.ReadAssembly(typeof(InjectionUtils).Assembly.Location);
            var type = x.GetType();
            var tr = a.MainModule.ImportReference(type);
            Mono.Cecil.TypeReference td = tr.Resolve();

            Logger.Log(td.ToString(), Logger.LogType.Error, Logger.VerboseType.Low);
            Logger.Log(op.ToString(), Logger.LogType.Error, Logger.VerboseType.Low);

            if (op != null){
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

                try {
                    il.InsertBefore( // Push x to stack
                        first, Instruction.Create(
                            op, td // x
                        )
                    );    
                } catch (ArgumentException ex) {
                    Logger.Log(ex.ToString(), Logger.LogType.Error, Logger.VerboseType.Low);
                }

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
}