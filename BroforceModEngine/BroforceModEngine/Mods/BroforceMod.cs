using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// A template for Broforce Mods
/// </summary>

namespace BroforceModEngine.Mods
{
    /// <summary>
    /// 
    /// </summary>
    public class BroforceMod
    {
        public ModInfo info;

        internal void Init()
        {
            info = new ModInfo();
        }
    }
}
