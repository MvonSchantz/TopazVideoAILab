using File = Pri.LongPath.File;
using FileInfo = Pri.LongPath.FileInfo;

namespace Cybercraft.Common
{
    public class SafeFile : SafeBase
    {
        public static bool Exists(string path)
        {
            return SafeExecute(() => Pri.LongPath.File.Exists(path));
        }

        public static void Move(string sourcePath, string destinationPath)
        {
            SafeExecute(() => Pri.LongPath.File.Move(sourcePath, destinationPath));
        }
    }
}
