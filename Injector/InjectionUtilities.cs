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