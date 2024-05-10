using System;
using System.IO;
using System.Text;

namespace Cybercraft.Common.Hash
{
    public static class Oso
    {
        private static byte[] ComputeMovieHash(string filename)
        {
            byte[] result;
            using (Stream input = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                result = ComputeMovieHash(input);
            }
            return result;
        }

        public static string Calculate(string filename)
        {
            return ToHexadecimal(ComputeMovieHash(filename));
        }

        private static byte[] ComputeMovieHash(Stream input)
        {
            var streamSize = input.Length;
            var hash = streamSize;

            long i = 0;
            byte[] buffer = new byte[sizeof(long)];
            while (i < 65536 / sizeof(long) && (input.Read(buffer, 0, sizeof(long)) > 0))
            {
                i++;
                hash += BitConverter.ToInt64(buffer, 0);
            }

            input.Position = Math.Max(0, streamSize - 65536);
            i = 0;
            while (i < 65536 / sizeof(long) && (input.Read(buffer, 0, sizeof(long)) > 0))
            {
                i++;
                hash += BitConverter.ToInt64(buffer, 0);
            }
            input.Close();
            byte[] result = BitConverter.GetBytes(hash);
            Array.Reverse(result);
            return result;
        }

        public static string Calculate(Stream input)
        {
            return ToHexadecimal(ComputeMovieHash(input));
        }

        private static string ToHexadecimal(byte[] bytes)
        {
            StringBuilder hexBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                hexBuilder.Append(bytes[i].ToString("x2"));
            }
            return hexBuilder.ToString();
        }
    }
}
