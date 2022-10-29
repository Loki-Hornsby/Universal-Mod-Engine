using System;
using System.IO;

namespace BroforceModEngine
{
    public static class Loader
    {
        public static string EngineDirectoryPath
        {
            get 
            {
                return Path.Combine(Directory.GetCurrentDirectory(), "BroforceModEngine");
            }
        }

        public static void Main()
        {
            if(Directory.Exists(EngineDirectoryPath))
            {
                //
            }
            else
            {
                Directory.CreateDirectory(EngineDirectoryPath);
            }
        }
    }
}