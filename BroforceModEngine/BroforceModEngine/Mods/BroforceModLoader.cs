using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

/// <summary>
/// A template for loading Broforce mods?
/// </summary>

namespace BroforceModEngine.Mods
{
    internal static class BroforceModLoader
    {
        public static void InitializeMods()
        {
            foreach (var jsonFile in Directory.GetFiles(ModEngine.ModsDirectoryPath, "Info.json", SearchOption.AllDirectories))
            {

            }

            /*foreach (string assembly in Directory.GetFiles(ModEngine.ModsDirectoryPath, "*.dll", SearchOption.AllDirectories))
            {
                // We'll see this later
            }*/
        }
    }
}
