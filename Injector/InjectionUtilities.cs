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
        
namespace Injection {
    public static class Utilities {
        /// <summary>
        /// Inserts Values
        /// </summary>
        public static class Insert {
            public enum Modes {
                Before,
                After
            }

            /// <summary>
            /// Inserts a new instruction at the desired location
            /// </summary>
            public static void NewInstruction(
                Mono.Cecil.Cil.ILProcessor il, // The ILProcessor
                Instruction instruction, // inserts X (instruction)
                Utilities.Insert.Modes mode, // AFTER or BEFORE
                Instruction target // Y (target)
                ){
                
                switch (mode) {
                    case Utilities.Insert.Modes.Before:
                        il.InsertBefore(
                            target, 
                            instruction
                        );  

                        break;
                    case Utilities.Insert.Modes.After:
                        il.InsertAfter(
                            target,
                            instruction
                        );    

                        break;
                }
            }
        }

        /// <summary>
        /// Creates Values
        /// </summary>
        public static class Create {
            /// <summary>
            /// Creates a new instruction
            /// </summary>
            public static Instruction? NewInstruction(
                Mono.Cecil.Cil.OpCode opcode, object operand, ModuleDefinition module) {
                
                if (operand != null) {
                    // Define Operand
                    var fieldOperand = operand as FieldReference;
                    if (fieldOperand != null)
                        // Return Instruction
                        return Instruction.Create(opcode, module.ImportReference(fieldOperand));

                    // Define Operand
                    var methodOperand = operand as MethodReference;
                    if (methodOperand != null)
                        // Return Instruction
                        return Instruction.Create(opcode, module.ImportReference(methodOperand));

                    // Define Operand
                    var typeOperand = operand as TypeReference;
                    if (typeOperand != null)
                        // Return Instruction
                        return Instruction.Create(opcode, module.ImportReference(typeOperand));
                } else {
                    // Return opcode only instruction
                    return Instruction.Create(opcode);
                }

                // Return nothing
                return null;
            }
        }

        /// <summary>
        /// Determine Values
        /// </summary>
        public static class Determine {
            /// <summary>
            /// Determine needed opcode for operation
            /// </summary>
            public static Mono.Cecil.Cil.OpCode? Opcode<T>(T x){
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
        }
    }
}