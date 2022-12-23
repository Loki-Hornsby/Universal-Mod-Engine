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
    // This is the class for the assembly we load
    public class CustomAssemblyDefinition {
        Mono.Cecil.AssemblyDefinition definition;

        /// <summary>
        /// Constructor for our assembly class
        /// </summary>
        public CustomAssemblyDefinition(string Input){
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

        /// <summary>
        /// Gets the IL processor
        /// </summary>
        public Mono.Cecil.Cil.ILProcessor GetIL(MethodDefinition method){
            // Get the IL processor (Only needed for methods)
            // CIL and IL are synonymous https://stackoverflow.com/questions/293800/what-is-the-difference-between-cil-and-msil-il 
            // https://en.wikipedia.org/wiki/Common_Intermediate_Language
                // https://en.wikipedia.org/wiki/List_of_CIL_instructions 
            return method.Body.GetILProcessor();
        }

        /// <summary>
        /// Gets a type definition from the new [Game] executable.
        /// Self Explainer: We access the public types available from our loaded assembly and then we return the one matching [typeName]
        /// Linkie: https://github.com/jbevain/cecil/blob/master/Mono.Cecil/TypeDefinition.cs
        /// </summary>
        public TypeDefinition GetTypeDefinition(Mono.Collections.Generic.Collection<Mono.Cecil.TypeDefinition> types, string typeName){
            return (from TypeDefinition t in types
                    where t.Name == typeName
                    select t).FirstOrDefault();
        }

        /// <summary>
        /// Gets a method definition from the new [Game] executable.
        /// Self Explainer: We look through the methods available in our chosen type and return the one matching [methodName]
        /// Linkie: https://github.com/jbevain/cecil/blob/master/Mono.Cecil/MethodDefinition.cs
        /// </summary>
        public MethodDefinition GetMethodDefinition(string className, string methodName){
            TypeDefinition t = this.GetTypeDefinition(
                this.GetTypes(), 
                className
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
        public FieldDefinition GetFieldDefinition(string className, string fieldName){
            TypeDefinition t = this.GetTypeDefinition(
                this.GetTypes(), 
                className 
            );
            
            return (from FieldDefinition f in t.Fields
                    where f.Name == fieldName
                    select f).FirstOrDefault();
        } 
    }
}