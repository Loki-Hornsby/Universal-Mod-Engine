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

        public static class Create {
            public enum Modes {
                Before,
                After
            }

            public static void NewInstruction(
                    Utilities.Create.Modes mode, Instruction target,
                    Mono.Cecil.Cil.OpCode opcode, Mono.Cecil.Cil.ILProcessor il, Mono.Cecil.ModuleDefinition main) {

                // Define Instruction
                Instruction instruction = Instruction.Create(
                    opcode
                ); 

                switch (mode) {
                    case Utilities.Create.Modes.Before:
                        il.InsertBefore(
                            target, 
                            Utilities.Import.DefinedInstruction(instruction, main)
                        );  

                        break;
                    case Utilities.Create.Modes.After:
                        il.InsertAfter(
                            target, 
                            Utilities.Import.DefinedInstruction(instruction, main)
                        );    

                        break;
                } 
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

        /// <summary>
        /// Import Values
        /// </summary>
        public static class Import {
            /// <summary>
            /// Imports an instruction as 1 of 3 types
            /// https://stackoverflow.com/questions/38038517/argumentexception-in-mono-cecil-while-saving-assembly
            /// </summary>
            public static Instruction DefinedInstruction(Instruction instruction, ModuleDefinition module){
                object operand = instruction.Operand;

                var fieldOperand = operand as FieldReference;
                if (fieldOperand != null)
                    return Instruction.Create(instruction.OpCode, module.ImportReference(fieldOperand));

                var methodOperand = operand as MethodReference;
                if (methodOperand != null)
                    return Instruction.Create(instruction.OpCode, module.ImportReference(methodOperand));

                var typeOperand = operand as TypeReference;
                if (typeOperand != null)
                    return Instruction.Create(instruction.OpCode, module.ImportReference(typeOperand));

                return instruction;
            }
        }
    }
}