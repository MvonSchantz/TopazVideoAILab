using System;
using System.Reflection;

namespace Cybercraft.Common
{
    public static class Version
    {
        public static System.Version GetAssemblyVersion()
        {
            return Assembly.GetEntryAssembly().GetName().Version;
        }

        public static void WriteSoftwareVersion(string name)
        {
            var version = GetAssemblyVersion();
            Console.WriteLine("{0} {1}.{2}.{3}", name, version.Major, version.Minor, version.Build);
        }
    }
}
