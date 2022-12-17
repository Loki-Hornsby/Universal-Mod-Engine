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