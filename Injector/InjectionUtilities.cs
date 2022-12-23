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
        /// Insert Values
        /// </summary>
        public static class Insert {
            public static void NewInstruction(int position, Instruction instruction, Mono.Cecil.MethodDefinition definition){
                definition.Body.Instructions.Insert(position, 
                    instruction
                );
            }
        }

        /// <summary>
        /// Determine Values
        /// </summary>
        public static class Determine {
            /// <summary>
            /// Determine needed opcode for operation
            /// </summary>
            public static Mono.Cecil.Cil.OpCode? Opcode<T>(){
                if (typeof(T) == typeof(int)){
                    // Pushes a supplied value of type int32 onto the evaluation stack as an int32
                    return OpCodes.Ldc_I4;

                } else if (typeof(T) == typeof(float)){
                    // Pushes a supplied value of type float32 onto the evaluation stack as type F (float)
                    return OpCodes.Ldc_R4;

                } else if (typeof(T) == typeof(string)){
                    // Pushes a new object reference to a string literal stored in the metadata.
                    return OpCodes.Ldstr;

                } else if (typeof(T) == typeof(bool)){
                    // Pushes a supplied value of type int32 onto the evaluation stack as an int32
                    return OpCodes.Ldc_I4;

                } else {
                    return null;
                }
            }
        }
    }
}