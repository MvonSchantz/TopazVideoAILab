using System.Collections.Generic;
using System.IO;
using Directory = Pri.LongPath.Directory;
using DirectoryInfo = Pri.LongPath.DirectoryInfo;

namespace Cybercraft.Common
{
    public class SafeDirectory : SafeBase
    {
        public static DirectoryInfo CreateDirectory(string path)
        {
            return SafeExecute(() => Directory.CreateDirectory(path));
        }

        public static IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption options)
        {
            return SafeExecute(() => Directory.EnumerateFiles(path, searchPattern, options));
        }

        public static void Move(string sourcePath, string destinationPath)
        {
            SafeExecute(() => Directory.Move(sourcePath, destinationPath));
        }
    }

}
