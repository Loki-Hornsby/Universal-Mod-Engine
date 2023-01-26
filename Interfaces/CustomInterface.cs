using System;

namespace Interfaces {
    public class DLLDefinitions {
        // Ideas on how to go about doing this
        // 1. Have the user define where the dlls are located (unsafe)
        // 2. Find a way to locate which dlls an exe uses and then store the paths
            // This seems like the nicest approach

        public string UnityEngine;
        public string UnityEngine_CoreModule;
        public string UnityEngine_IMGUIModule;
        public string Assembly_CSharp;

        /// <summary>
        /// Constructor
        /// </summary>
        public DLLDefinitions(string EXE){
            // (UnityEngine, UnityEngine_CoreModule, UnityEngine_IMGUIModule, Assembly_CSharp) = DLLDefinitions.FindDefinitions(EXE);

            UnityEngine             = "";
            UnityEngine_CoreModule  = "";
            UnityEngine_IMGUIModule = "";
            Assembly_CSharp         = "";
        }
    }

    public class CustomInterface {
        // Name of interface
        string _N;

        // Path to exe
        string _E;

        // Targeted DLL
        DLLDefinitions _Defs;
        
        /// <summary>
        /// Get name of interface
        /// </summary>
        public string GetName(){
            return _N;
        }
        
        /// <summary>
        /// Get the DLL definitions for our selected EXE
        /// </summary>
        public DLLDefinitions GetDefs(){
            return _Defs;
        }

        /// <summary>
        /// Constructor for your custom interface
        /// </summary>
        /// <param name="Name">Name of your interface</param>
        /// <param name="EXE">Location of the exe you want to mod</param>
        public CustomInterface(string Name, string EXE){
            // Name of interface
            _N = Name;

            // Targeted EXE
            _E = EXE;

            // DLL Definitions
            _Defs = new DLLDefinitions(_E);
        }   
    }
}