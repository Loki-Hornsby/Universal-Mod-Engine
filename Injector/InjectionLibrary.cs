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

/// <summary>
/// https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes?view=net-7.0
/// 
/// Assembly
///     Class
///         Method
///             Field
/// </summary>

namespace Injection {
    public static class Library {
        // *************************************** TYPE *************************************** \\
        public class Type {
            
            // *************************************** DEFINE *************************************** \\

            // Parent (Assembly)
            CustomAssemblyDefinition parent;

            // Self (Class)
            string self;

            // Definition (Class)
            TypeDefinition definition;

            // *************************************** GET *************************************** \\

            /// <summary>
            /// Get the name of the class this belongs to
            /// </summary>
            public CustomAssemblyDefinition GetParent() {
                return parent;
            }

            /// <summary>
            /// Get the name of this type
            /// </summary>
            public string GetSelf() {
                return self;
            }

            /// <summary>
            /// Get the type definition
            /// </summary>
            public TypeDefinition GetDefinition() {
                return definition;
            }

            // *************************************** SET *************************************** \\

            /// <summary>
            /// Add the method
            /// </summary>
            public Type(CustomAssemblyDefinition _parent, string _self){
                // Parent
                parent = _parent;

                // Self
                self = _self;

                // Definition
                definition = parent.GetTypeDefinition(parent.GetTypes(), self);
            }

            // *************************************** MODIFY *************************************** \\

            /// <summary>
            /// Change the method
            /// </summary>
            public void Change(){
                
            }

            /// <summary>
            /// Remove the method
            /// </summary>
            public void Remove(){
                
            }

            /// <summary>
            /// Clear the method of all its contents
            /// </summary>
            public void Clear(){
                
            }
        }

        // *************************************** METHOD *************************************** \\
        public class Method {
            
            // *************************************** DEFINE *************************************** \\

            // Parent (Class)
            Library.Type parent;

            // Self (Method)
            string self;

            // Definition (Method)
            MethodDefinition definition;

            // *************************************** GET *************************************** \\

            /// <summary>
            /// Get the name of the class this belongs to
            /// </summary>
            public Library.Type GetParent() {
                return parent;
            }

            /// <summary>
            /// Get the name of this method
            /// </summary>
            public string GetSelf() {
                return self;
            }

            /// <summary>
            /// Get the method definition
            /// </summary>
            public MethodDefinition GetDefinition() {
                return definition;
            }

            // *************************************** SET *************************************** \\

            /// <summary>
            /// Add the method
            /// </summary>
            public Method(Library.Type _parent, string _self){
                // Parent
                parent = _parent;

                // Self
                self = _self;

                // Definition
                definition = parent.GetParent().GetMethodDefinition(parent.GetSelf(), self);
            }

            // *************************************** MODIFY *************************************** \\

            /// <summary>
            /// Change the method
            /// </summary>
            public void Change(){
                
            }

            /// <summary>
            /// Remove the method
            /// </summary>
            public void Remove(){
                
            }

            /// <summary>
            /// Clear the method of all its contents
            /// </summary>
            public void Clear(){
                //definition.Body.Instructions.Clear(); 
                //definition.Body.ExceptionHandlers.Clear(); 
            }
        }

        // *************************************** FIELD *************************************** \\
        public class Field {
            
            // *************************************** DEFINE *************************************** \\

            // Parent (Method)
            Library.Type parent;

            // Self (Field)
            string self;

            // Definition (Field)
            FieldDefinition definition;

            // *************************************** GET *************************************** \\

            /// <summary>
            /// Get the name of the class this belongs to
            /// </summary>
            public Library.Type GetParent() {
                return parent;
            }

            /// <summary>
            /// Get the name of this method
            /// </summary>
            public string GetSelf() {
                return self;
            }

            /// <summary>
            /// Get the method definition
            /// </summary>
            public FieldDefinition GetDefinition() {
                return definition;
            }

            // *************************************** SET *************************************** \\

            /// <summary>
            /// Add the field
            /// </summary>
            public Field(Library.Type _parent, string _self){
                // Parent
                parent = _parent;

                // Self
                self = _self;

                // Definition
                definition = parent.GetParent().GetFieldDefinition(parent.GetSelf(), self);
            }

            // *************************************** MODIFY *************************************** \\

            /// <summary>
            /// Set a field
            /// </summary>
            public void Set<T>(Library.Method method, T x, int position = 0){
                // Opcode
                Mono.Cecil.Cil.OpCode op = Utilities.Determine.Opcode<T>() ?? default(Mono.Cecil.Cil.OpCode);
                
                // If the opcode isn't empty
                if (op != null){
                    //// Definitions
                    
                    // Main module
                    var main = this.GetParent().GetParent().GetMainModule();
                    
                    // Method Definition
                    var method_definition = method.GetDefinition();

                    // Field Definition
                    var field_definition = this.GetDefinition();

                    // IL processor
                    var il = this.GetParent().GetParent().GetIL(method_definition);

                    //// Resolves
                    
                    // Resolve X
                    dynamic value = (dynamic) x;

                    if (typeof(T) == typeof(bool)){
                        if (value == true){
                            value = 1;
                        } else {
                            value = 0;
                        }
                    }

                    // Resolve position
                    if (position < 0){
                        // Starts from the end of the instruction instead of the start
                        position = method_definition.Body.Instructions.Count + position;
                    }

                    // Invalid position
                    if (position > method_definition.Body.Instructions.Count - 1 || position < 0){
                        Logger.Log(
                            "Invalid Index present when setting a field - defaulted to 0", 
                            Logger.LogType.Error, 
                            Logger.VerboseType.Low
                        );

                        position = 0;
                    }

                    //// Instructions
                    
                    // Push
                    Utilities.Insert.NewInstruction(
                        position,
                        Instruction.Create(OpCodes.Ldarg_0),
                        method_definition
                    );

                    // Add
                    Utilities.Insert.NewInstruction(
                        position + 1,
                        Instruction.Create(op, value), 
                        method_definition
                    );

                    // Commit
                    Utilities.Insert.NewInstruction(
                        position + 2,
                        Instruction.Create(OpCodes.Stfld, field_definition),
                        method_definition
                    );

                    // Next challenge is to 
                        // Fix insert before and after (i think the instructions inserted are overwriting others)
                        // Make new instruction insertion dynamic
                        // try and call TestVanDammeAnim::SetAirdashAvailable() using a dynamic function
                }
            }
        }
    }
}