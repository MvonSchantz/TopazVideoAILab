using System;
using VideoProcessingLib.AviSynth;
using Microsoft.Win32;

namespace VideoProcessingLib.VirtualDub
{
    public static class Lagarith
    {
        private static readonly object LagarithRegistryLock = new object();
        public static void SetColorMode(ColorSpace colorSpace)
        {
            if (colorSpace != ColorSpace.RGB && colorSpace != ColorSpace.YUY2 && colorSpace != ColorSpace.YV12)
                throw new ArgumentException("Unsupported color mode.", nameof(colorSpace));

            lock (LagarithRegistryLock)
            {
                using (var key = Registry.CurrentUser.CreateSubKey("Software\\Lagarith"))
                {
                    if (key != null)
                    {
                        switch (colorSpace)
                        {
                            case ColorSpace.RGB:
                                key.SetValue("Mode", "RGB");
                                return;
                            case ColorSpace.YUY2:
                                key.SetValue("Mode", "YUY2");
                                return;
                            case ColorSpace.YV12:
                                key.SetValue("Mode", "YV12");
                                return;
                        }
                    }
                }
            }
            throw new Exception("Failed to set Lagarith color mode.");
        }

        public static ColorSpace GetColorMode()
        {
            using (var key = Registry.CurrentUser.OpenSubKey("Software\\Lagarith"))
            {
                if (key != null)
                {
                    var mode = key.GetValue("Mode") as string;
                    switch (mode)
                    {
                        case "RGB":
                            return ColorSpace.RGB;
                        case "YUY2":
                            return ColorSpace.YUY2;
                        case "YV12":
                            return ColorSpace.YV12;
                    }
                }
            }

            return ColorSpace.Unknown;
        }
    }
}
