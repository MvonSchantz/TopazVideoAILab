using System;
using Pri.LongPath;

namespace VideoProcessingLib.VirtualDub
{
    public static class VirtualDub
    {
        public static string ToVirtualDubPath(string path) => path.Replace(@"\", @"\\");

        public static void CreateJobsFile(string jobsFile, string sourceFile, string targetFile, Codec codec = Codec.Lagarith, PixelFormat pixelFormat = PixelFormat.Undetermined, int bitDepth = 8)
        {
            string compression = null;
            string compData = null;

            switch (codec)
            {
                case Codec.Lagarith:
                    compression = "0x7367616c,0,10000,0";
                    compData = "1,\"AA==\"";
                    break;
                case Codec.Ffv1:
                    compression = "0x31564646,0,10000,0,\"avlib-1.vdplugin\"";
                    switch (pixelFormat)
                    {
                        case PixelFormat.Yuv444:
                            switch (bitDepth)
                            {
                                case 8:
                                    compData = "32,\"AQAAAAUAAAAIAAAAAwAAAAQAAAABAAAAAQAAAAEAAAA=\"";
                                    break;
                                case 10:
                                    compData = "32,\"AQAAAAUAAAAKAAAAAwAAAAQAAAABAAAAAQAAAAEAAAA=\"";
                                    break;
                                case 16:
                                    compData = "32,\"AQAAAAUAAAAQAAAAAwAAAAQAAAABAAAAAQAAAAEAAAA=\"";
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException(nameof(bitDepth), "Unsupported bit depth");
                            }
                            break;
                        case PixelFormat.Yuv420:
                            switch (bitDepth)
                            {
                                case 8:
                                    compData = "32,\"AQAAAAMAAAAIAAAAAwAAAAQAAAABAAAAAQAAAAEAAAA=\"";
                                    break;
                                case 10:
                                    compData = "32,\"AQAAAAMAAAAKAAAAAwAAAAQAAAABAAAAAQAAAAEAAAA=\"";
                                    break;
                                case 16:
                                    compData = "32,\"AQAAAAMAAAAQAAAAAwAAAAQAAAABAAAAAQAAAAEAAAA=\"";
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException(nameof(bitDepth), "Unsupported bit depth");
                            }
                            break;
                        case PixelFormat.Rgb:
                            switch (bitDepth)
                            {
                                case 8:
                                    compData = "32,\"AQAAAAEAAAAIAAAAAwAAAAQAAAABAAAAAQAAAAEAAAA=\"";
                                    break;
                                case 10:
                                    compData = "32,\"AQAAAAEAAAAKAAAAAwAAAAQAAAABAAAAAQAAAAEAAAA=\"";
                                    break;
                                case 16:
                                    compData = "32,\"AQAAAAEAAAAQAAAAAwAAAAQAAAABAAAAAQAAAAEAAAA=\"";
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException(nameof(bitDepth), "Unsupported bit depth");
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(pixelFormat), pixelFormat, "Unsupported pixel format");
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(codec), codec, null);
            }

            if (compression == null || compData == null)
            {
                throw new ArgumentException("Unsupported codec", nameof(codec));
            }

            const string template = @"
VirtualDub.Open(""{0}"","""",0);
VirtualDub.audio.SetSource(1);
VirtualDub.audio.SetMode(0);
VirtualDub.audio.SetInterleave(1,500,1,0,0);
VirtualDub.audio.SetClipMode(1,1);
VirtualDub.audio.SetConversion(0,0,0,0,0);
VirtualDub.audio.SetVolume();
VirtualDub.audio.SetCompression();
VirtualDub.audio.EnableFilterGraph(0);
VirtualDub.video.SetInputFormat(0);
VirtualDub.video.SetOutputFormat(7);
VirtualDub.video.SetMode(1);
VirtualDub.video.SetSmartRendering(0);
VirtualDub.video.SetPreserveEmptyFrames(0);
VirtualDub.video.SetFrameRate2(0,0,1);
VirtualDub.video.SetIVTC(0, 0, 0, 0);
VirtualDub.video.SetCompression({2});
VirtualDub.video.SetCompData({3});
VirtualDub.video.filters.Clear();
VirtualDub.audio.filters.Clear();
VirtualDub.SaveAVI(""{1}"");
VirtualDub.audio.SetSource(1);
VirtualDub.Close();
";
            var content = template.Replace("{0}", ToVirtualDubPath(sourceFile)).Replace("{1}", ToVirtualDubPath(targetFile)).Replace("{2}", compression).Replace("{3}", compData);

            File.WriteAllText(jobsFile, content);
        }
    }
}
