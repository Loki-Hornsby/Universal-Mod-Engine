using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

// CREDIT GOES TO https://forum.exetools.com/showthread.php?t=16470 
    // Thankyou ;)

/// EXAMPLES:
    // Get the method we want to inject into
        //var method = InjectionUtils.GetMethodDefinition(InjectionUtils.GetTypeDefinition(ChosenAssembly.GetTypes(), "Menu"), "Awake");

    // Empty method entirely
        //method.Body.Instructions.Clear(); 
        //method.Body.ExceptionHandlers.Clear(); 

namespace Injection {
    public static class InjectionUtils {
        /// <summary>
        /// Gets a method definition from the new [Game] executable.
        /// Self Explainer: We look through the methods available in our chosen type and return the one matching [methodName]
        /// Linkie: https://github.com/jbevain/cecil/blob/master/Mono.Cecil/MethodDefinition.cs
        /// </summary>
        public static MethodDefinition GetMethodDefinition(TypeDefinition t, string methodName){
            return (from MethodDefinition m in t.Methods
                    where m.Name == methodName
                    select m).FirstOrDefault();
        }

        /// <summary>
        /// Gets a field definition from the new [Game] executable.
        /// Self Explainer: Gets a variable of any type
        /// Linkie: https://github.com/jbevain/cecil/blob/master/Mono.Cecil/FieldDefinition.cs 
        /// </summary>
        public static FieldDefinition GetFieldDefinition(TypeDefinition t, string fieldName){
            return (from FieldDefinition f in t.Fields
                    where f.Name == fieldName
                    select f).FirstOrDefault();
        } 

        /// <summary>
        /// Gets a type definition from the new [Game] executable.
        /// Self Explainer: We access the public types available from our loaded assembly and then we return the one matching [typeName]
        /// Linkie: https://github.com/jbevain/cecil/blob/master/Mono.Cecil/TypeDefinition.cs
        /// </summary>
        public static TypeDefinition GetTypeDefinition(Mono.Collections.Generic.Collection<Mono.Cecil.TypeDefinition> types, string typeName){
            return (from TypeDefinition t in types
                    where t.Name == typeName
                    select t).FirstOrDefault();
        }
    }
}