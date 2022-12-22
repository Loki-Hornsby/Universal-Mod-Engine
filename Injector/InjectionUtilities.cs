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

using Injection;
        
namespace Injection.Utilities {
    public static class InjectionUtils {
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
        /// Imports an instruction as 1 of 3 types
        /// https://stackoverflow.com/questions/38038517/argumentexception-in-mono-cecil-while-saving-assembly
        /// </summary>
        public static Instruction ImportInstruction(Instruction instruction, ModuleDefinition module){
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

        /// <summary>
        /// Gets a type definition from the new [Game] executable.
        /// Self Explainer: We access the public types available from our loaded assembly and then we return the one matching [typeName]
        /// Linkie: https://github.com/jbevain/cecil/blob/master/Mono.Cecil/TypeDefinition.cs
        /// </summary>
        private static TypeDefinition GetTypeDefinition(Mono.Collections.Generic.Collection<Mono.Cecil.TypeDefinition> types, string typeName){
            return (from TypeDefinition t in types
                    where t.Name == typeName
                    select t).FirstOrDefault();
        }

        /// <summary>
        /// Gets a method definition from the new [Game] executable.
        /// Self Explainer: We look through the methods available in our chosen type and return the one matching [methodName]
        /// Linkie: https://github.com/jbevain/cecil/blob/master/Mono.Cecil/MethodDefinition.cs
        /// </summary>
        public static MethodDefinition GetMethodDefinition(string className, string methodName){
            TypeDefinition t = InjectionUtils.GetTypeDefinition(
                Injector.ChosenAssembly.GetTypes(), 
                className   //"TestVanDammeAnim"
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
        public static FieldDefinition GetFieldDefinition(string className, string fieldName){
            TypeDefinition t = InjectionUtils.GetTypeDefinition(
                Injector.ChosenAssembly.GetTypes(), 
                className   //"TestVanDammeAnim"
            );
            
            return (from FieldDefinition f in t.Fields
                    where f.Name == fieldName
                    select f).FirstOrDefault();
        } 
    }
}