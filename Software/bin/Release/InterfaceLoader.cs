using System;
using System.Collections.Generic;
using System.IO;

using Software;

namespace ModInterface {
    public static class InterfaceLoader {
        static List<CustomModInterface> interfaces;

        public static List<CustomModInterface> PollInterfaces(){
            return interfaces;
        }
    }
}