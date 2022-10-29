using System;
using System.IO;
using HarmonyLib;

namespace BroforceModEngine
{
    public static class Loader
    {
        public static void Main()
        {
            ModEngine.harmony = new Harmony("BroforceModEngine");
            ModEngine.harmony.PatchAll();
            //ModEngine.Load();
        }
    }
}