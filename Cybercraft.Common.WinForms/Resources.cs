using System.Drawing;
using System.IO;
using System.Reflection;

namespace Cybercraft.Common.WinForms
{
    internal static class Resources
    {
        public static Bitmap scrollbar_arrow_hot = ReadBitmapResource("scrollbar_arrow_hot.png");

        private static Bitmap ReadBitmapResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // ReSharper disable once PossibleNullReferenceException
            var assemblyName = assembly.FullName.Substring(0, assembly.FullName.IndexOf(','));

            resourceName = assemblyName + ".Icons." + resourceName;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                var bitmap = new Bitmap(stream);
                return bitmap;
            }
        }

        private static byte[] ReadResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // ReSharper disable once PossibleNullReferenceException
            var assemblyName = assembly.FullName.Substring(0, assembly.FullName.IndexOf(','));

            resourceName = assemblyName + ".Icons." + resourceName;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                // ReSharper disable once PossibleNullReferenceException
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int)stream.Length);
                return buffer;
            }
        }
    }
}
