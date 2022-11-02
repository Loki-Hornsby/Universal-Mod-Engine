using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// A template for defining info about a Broforce mod.
/// </summary>

namespace BroforceModEngine.Mods
{
    /// <summary>
    /// 
    /// </summary>
    public class ModInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> Dependencies { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> OptionalDependencies { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Version { get; set; }

    }
}
