using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

// CREDIT GOES TO https://forum.exetools.com/showthread.php?t=16470 
    // Thankyou ;)

namespace Injection {
    public static class InjectionUtils {
        /// <summary>
        /// Gets a method definition from the new [Game] executable.
        /// Self Explainer: We look through the methods available in our chosen type and return the one matching [methodName]
        /// </summary>
        /// <param name="t"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static MethodDefinition GetMethodDefinition(TypeDefinition t, string methodName){
            return (from MethodDefinition m in t.Methods
                    where m.Name == methodName
                    select m).FirstOrDefault();
        }

        /// <summary>
        /// Gets a type definition from the new [Game] executable.
        /// Self Explainer: We access the public types (classes) available from our loaded assembly and then we return the one matching [typeName]
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static TypeDefinition GetTypeDefinition(Mono.Collections.Generic.Collection<Mono.Cecil.TypeDefinition> types, string typeName){
            return (from TypeDefinition t in types
                    where t.Name == typeName
                    select t).FirstOrDefault();
        }
    }
}