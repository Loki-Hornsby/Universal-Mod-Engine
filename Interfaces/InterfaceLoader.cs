/// <summary>
/// Copyright 2022, Loki Alexander Button Hornsby (Loki Hornsby), All rights reserved.
/// Licensed under the BSD 3-Clause "New" or "Revised" License
/// </summary>

using System;
using System.Collections.Generic;
using System.IO;

namespace ModInterface {
    public static class InterfaceLoader {
        static List<CustomModInterface> interfaces;

        public static void Setup(){
            interfaces = new List<CustomModInterface>();
        }

        public static List<CustomModInterface> PollInterfaces(){
            return interfaces;
        }
    }
}