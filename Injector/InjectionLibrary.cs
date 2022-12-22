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
        /// Change a defined field (variable)
        /// </summary>
        public static void ChangeField<T>(LoadedAssembly assembly, string className, string methodName, string fieldName, T x){
            // Opcode
            Mono.Cecil.Cil.OpCode op = (Mono.Cecil.Cil.OpCode) InjectionUtils.DetermineOpcode<T>(x);

            // If the opcode isn't empty
            if (op != null){
                // Main
                var main = assembly.GetMainModule();

                // Define inputs
                var method = InjectionUtils.GetMethodDefinition(className, methodName);
                var field = InjectionUtils.GetFieldDefinition(className, fieldName);
                var il = Tools.GetIL(method);

                // last instruction
                Instruction last = method.Body.Instructions[method.Body.Instructions.Count - 1];

                //// Load 
                
                // Define Instruction
                Instruction in1 = Instruction.Create(
                    OpCodes.Ldarg_0
                );

                // Insert Instruction
                il.InsertAfter( // Load arg 0
                    last, 
                    InjectionUtils.ImportInstruction(in1, main)
                );    

                //// Push
                
                // Import and resolve type reference
                var type = assembly.GetType();
                var td = main.ImportReference(type).Resolve();

                // Define Instruction
                Instruction in2 = Instruction.Create(
                    op, 
                    td
                );

                // Insert Instruction
                il.InsertAfter( // Push x to stack
                    last, 
                    InjectionUtils.ImportInstruction(in2, main)
                );    
                
                //// Finish
                
                // Define Instruction
                Instruction in3 = Instruction.Create(
                    OpCodes.Stfld, 
                    field
                );

                // Insert Instruction
                il.InsertAfter( // Apply x to field
                    last, 
                    InjectionUtils.ImportInstruction(in3, main)
                );    
            } else {
                System.Console.WriteLine("FIELD COULDN'T BE CHANGED! " + className + @"\" + methodName + @"\" + fieldName);
            }
        }
    }
}