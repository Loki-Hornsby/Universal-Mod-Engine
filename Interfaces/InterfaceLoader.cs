/// <summary>
/// Copyright 2022, Loki Alexander Button Hornsby (Loki Hornsby), All rights reserved.
/// Licensed under the BSD 3-Clause "New" or "Revised" License
/// </summary>

using System;
using System.Collections.Generic;
using System.IO;

namespace Interfaces {
    public static class Loader {
        // Has our data been fetched?
        static bool fetched;

        // Our stored interfaces
        static List<CustomInterface> interfaces;

        /// <summary>
        /// Fetch our interfaces
        /// </summary>
        static bool Fetch(){
            try {
                interfaces = new List<CustomInterface>();

                return true;
            } catch (Exception ex) {
                return false;
            }
        }

        /// <summary>
        /// Get the stored list of our interfaces
        /// </summary>
        public static List<CustomInterface> GetInterfaces(){
            // Try fetch
            if (!fetched){
                fetched = Fetch();
            }

            // Return our data
            if (fetched){
                return interfaces;
            } else {
                return null;
            }
        }
    }
}