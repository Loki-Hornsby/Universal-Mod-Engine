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
    public static class Library {
        /// <summary>
        /// Wipes a function clean
        /// </summary>
        public static void ClearFunction(MethodDefinition method){
            method.Body.Instructions.Clear(); 
            method.Body.ExceptionHandlers.Clear(); 
        }

        /// <summary>
        /// Change a defined field (variable)
        /// </summary>
        public static void ChangeField<T>(LoadedAssembly assembly, string className, string methodName, string fieldName, T x){
            // Opcode
            Mono.Cecil.Cil.OpCode op = (Mono.Cecil.Cil.OpCode) Utilities.Determine.Opcode<T>(x);

            // If the opcode isn't empty
            if (op != null){
                // Main module
                var main = assembly.GetMainModule();

                // Method name
                var method = assembly.GetMethodDefinition(className, methodName);

                // Field (variable) name
                var field = assembly.GetFieldDefinition(className, fieldName);

                // IL processor
                var il = assembly.GetIL(method);
                
                // Second to last instruction
                Instruction last = method.Body.Instructions[method.Body.Instructions.Count - 1];

                //// Load 
                Utilities.Create.NewInstruction(
                    Utilities.Create.Modes.Before, last,
                    OpCodes.Ldarg_0, 
                    il, main
                );

                Utilities.Create.NewInstruction(
                    Utilities.Create.Modes.Before, last,
                    OpCodes.Ldarg_0, 
                    il, main
                );

                Utilities.Create.NewInstruction(
                    Utilities.Create.Modes.Before, last,
                    OpCodes.Ldarg_0, 
                    il, main
                );

                //// Push // Wacky error happens here
                
                /*
                // Import and resolve type reference
                var type = assembly.GetType();
                var td = main.ImportReference(type).Resolve();

                // Define Instruction
                Instruction in2 = Instruction.Create(
                    op, 
                    td
                ); */

                /*

                // Insert Instruction
                il.InsertAfter( // Push x to stack
                    last, 
                    Utilities.ImportInstruction(in2, main)
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
                    Utilities.ImportInstruction(in3, main)
                );   
                */
            } else {
                System.Console.WriteLine("FIELD COULDN'T BE CHANGED! " + className + @"\" + methodName + @"\" + fieldName);
            }
        }
    }
}