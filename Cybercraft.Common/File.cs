using System;
using System.Threading.Tasks;

namespace Cybercraft.Common
{
    public static class File
    {
        public static void Move(string source, string destination, Action<int> progressCallback)
        {
            Copy(source, destination, progressCallback);
            Pri.LongPath.File.Delete(source);
        }

        public static void Copy(string source, string destination, Action<int> progressCallback)
        {
            const int bufferSize = 1024 * 1024;  //1MB
            byte[] buffer = new byte[bufferSize];
            byte[] buffer2 = new byte[bufferSize];
            bool swap = false;
            int reportedProgress = 0;

            var sourceInfo = new Pri.LongPath.FileInfo(source);
            var destinationInfo = new Pri.LongPath.FileInfo(destination);

            long len = sourceInfo.Length;
            float flen = len;
            Task writer = null;

            using (var sourceStream = sourceInfo.OpenRead())
            using (var destinationStream = destinationInfo.OpenWrite())
            {
                destinationStream.SetLength(sourceStream.Length);
                var read = 0;
                for (long size = 0; size < len; size += read)
                {
                    var progress = 0;
                    if ((progress = ((int)((size / flen) * 100))) != reportedProgress)
                        progressCallback(reportedProgress = progress);
                    read = sourceStream.Read(swap ? buffer : buffer2, 0, bufferSize);
                    writer?.Wait();
                    writer = destinationStream.WriteAsync(swap ? buffer : buffer2, 0, read);
                    swap = !swap;
                }
                writer?.Wait();
            }
        }

    }
}
