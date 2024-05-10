#define USE_LOCK

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.PInvoke.Windows;
using System.PInvoke.Windows.Enums;
using System.PInvoke.Windows.Structs;
using System.Runtime.InteropServices;
using Directory = Pri.LongPath.Directory;
using File = Pri.LongPath.File;
using Path = Pri.LongPath.Path;

namespace VideoProcessingLib.AviSynth
{
    public struct AviReaderError
    {
        public static readonly AviReaderError Ok = new AviReaderError(AviError.AVIERR_OK, null);

        public AviError ErrorCode;
        public string ErrorFunction;

        public string ErrorCodeStr
        {
            get
            {
                if (Enum.IsDefined(typeof(AviError), ErrorCode))
                    return Enum.GetName(typeof(AviError), ErrorCode);
                return ((int)ErrorCode).ToString("0xX8");
            }
        }

        public AviReaderError(AviError error, string function)
        {
            ErrorCode = error;
            ErrorFunction = function;
        }
    }

    public class AviReader : IDisposable
    {
        private bool disposed;

        public static object Lock = new object();

        private readonly bool infoOnly;
        private readonly ColorSpace colorSpace;

        private IntPtr aviFile;
        private IntPtr stream;
        private IntPtr getFrameHandle;

        public readonly string TemporaryAvsScriptFile;
        private string avsScript;
        private string source;
        public string Source
        {
            get => source;
            set
            {
                if (source == value)
                    return;

                source = value;
                if (Path.GetDirectoryName(source) == string.Empty && !File.Exists(source))
                {
                    source = Path.Combine(Directory.GetCurrentDirectory(), source);
                    if (!File.Exists(source))
                    {
                        throw new ApplicationException($"Error locating AVI file.");
                    }
                }
                source = Path.GetFullPath(source);

                var error = Reload();
                if (error.ErrorCode != AviError.AVIERR_OK)
                    throw new ApplicationException($"Error opening AVI file: {error.ErrorCodeStr}, {error.ErrorFunction}");
            }
        }

        public bool UseLwLibav { get; }

        public Size Resize { get; set; }

        public enum ResizeFilter
        {
            [Description("PointResize")]
            Point,
            
            [Description("BilinearResize")]
            Bilinear,
            
            [Description("BicubicResize")]
            Bicubic,
            
            [Description("LanczosResize")]
            Lanczos,

            [Description("Spline36Resize")]
            Spline36,

            [Description("Spline64Resize")]
            Spline64,
        }
        public ResizeFilter ResizeMode { get; set; }

        public Size FrameSize { get; private set; }

        public int Length { get; private set; }

        public float FrameRate { get; private set; }

        private AVIStreamInfo header;
        public AVIStreamInfo Header => header;

        private int position;
        public int Position
        {
            get => position;
            set
            {
                int oldPosition = position;
                if (value < 0)
                    position = 0;
                else if (value >= Length && Length > 0)
                    position = Length - 1;
                else
                    position = value;
                if (oldPosition != position)
                {
                    if (colorSpace == ColorSpace.RGB)
                    {
                        if (!RereadDecompressedFrame())
                            throw new ApplicationException("Failed to retrieve frame from AVI stream.");
                    }
                    else
                    {
                        if (!RereadRawFrame())
                            throw new ApplicationException("Failed to retrieve frame from AVI stream.");
                    }
                }
            }
        }

        public byte[] ImageData { get; private set; }
        public BitmapInfo BitmapInfo { get; private set; }

        private Image image;
        public unsafe Image Image
        {
            get
            {
                if (ImageData == null)
                    return null;
                if (image == null)
                {
                    fixed (byte* p = ImageData)
                    {
                        var ptr = (IntPtr)p;
                        image = new Bitmap(BitmapInfo.bmiHeader.biWidth, BitmapInfo.bmiHeader.biHeight,
                                           BitmapInfo.bmiHeader.biWidth * 4,
                                           PixelFormat.Format32bppArgb, ptr);
                        image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    }
                }
                return image;
            }
            private set { image = value; }
        }

        public AviReader(bool infoOnly = false, ColorSpace colorSpace = ColorSpace.RGB)
        {
            this.infoOnly = infoOnly;
            this.colorSpace = colorSpace;

            TemporaryAvsScriptFile = Path.Combine(Path.GetTempPath(), "AviReader_" + Guid.NewGuid() + ".avs");
            ResizeMode = ResizeFilter.Bicubic;
            header = new AVIStreamInfo();

#if USE_LOCK
            lock (Lock)
#endif
            {
                AVI.AVIFileInit();
            }
        }

        public AviReader(string source, bool infoOnly = false, ColorSpace colorSpace = ColorSpace.RGB, bool useLwLibav = false) : this(infoOnly, colorSpace)
        {
            UseLwLibav = useLwLibav;
            Source = source;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AviReader()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // free other managed objects that implement
                // IDisposable only
              /*  if (Image != null)
                    Image.Dispose();*/
            }

            // release any unmanaged objects
            // set the object references to null

#if USE_LOCK
            lock (Lock)
#endif
            {
                if (getFrameHandle != IntPtr.Zero)
                {
                    AVI.AVIStreamGetFrameClose(getFrameHandle);
                    getFrameHandle = IntPtr.Zero;
                }
                if (stream != IntPtr.Zero)
                {
                    AVI.AVIStreamRelease(stream);
                    stream = (IntPtr)0;
                }
                if (aviFile != IntPtr.Zero)
                {
                    AVI.AVIFileRelease(aviFile);
                    aviFile = IntPtr.Zero;
                }

                AVI.AVIFileExit();

                if (File.Exists(TemporaryAvsScriptFile))
                    File.Delete(TemporaryAvsScriptFile);
            }


            disposed = true;
        }

        public AviReaderError Reload()
        {
            if (source != null)
            {
                CreatePreviewScript();
            }

#if USE_LOCK
            lock (Lock)
#endif
            {
                if (getFrameHandle != IntPtr.Zero)
                {
                    AVI.AVIStreamGetFrameClose(getFrameHandle);
                    getFrameHandle = IntPtr.Zero;
                }
                if (stream != IntPtr.Zero)
                {
                    AVI.AVIStreamRelease(stream);
                    stream = IntPtr.Zero;
                }
                if (aviFile != IntPtr.Zero)
                {
                    AVI.AVIFileRelease(aviFile);
                    aviFile = IntPtr.Zero;
                }

                if (source == null)
                    return AviReaderError.Ok;

                var error = AVI.AVIFileOpenW(ref aviFile, avsScript, AccessMode.OF_READ | AccessMode.OF_SHARE_DENY_NONE, IntPtr.Zero);
                if (error != AviError.AVIERR_OK)
                    return new AviReaderError(error, "AVIFileOpenW");
                // ReSharper disable once UnusedVariable
                error = AVI.AVIFileGetStream(aviFile, out stream, StreamType.VIDEO, 0);
                if (stream == IntPtr.Zero)
                {
                    AVI.AVIFileRelease(aviFile);
                    aviFile = IntPtr.Zero;
                    return error != AviError.AVIERR_OK
                        ? new AviReaderError(error, "AVIFileGetStream")
                        : new AviReaderError(AviError.AVIERR_ERROR, "AVIFileGetStream (NULL stream returned)");
                }
                error = AVI.AVIStreamInfo(stream, ref header, Marshal.SizeOf(header));
                if (error != AviError.AVIERR_OK)
                {
                    AVI.AVIStreamRelease(stream);
                    stream = IntPtr.Zero;
                    AVI.AVIFileRelease(aviFile);
                    aviFile = IntPtr.Zero;
                    return new AviReaderError(error, "AVIStreamInfo");
                }
                Length = Header.dwLength;
                FrameSize = new Size(Header.rcFrame.Width, Header.rcFrame.Height);
                FrameRate = Header.dwRate / (float)Header.dwScale;

                if (Header.fccHandler == 0)
                {
                    BitmapInfoHeader infoHeader = new BitmapInfoHeader();
                    int cbFormat = Marshal.SizeOf(infoHeader);
                    error = AVI.AVIStreamReadFormat(stream, 0, out infoHeader, out cbFormat);
                    if (error != AviError.AVIERR_OK)
                    {
                        AVI.AVIStreamRelease(stream);
                        stream = IntPtr.Zero;
                        AVI.AVIFileRelease(aviFile);
                        aviFile = IntPtr.Zero;
                        return new AviReaderError(error, "AVIStreamReadFormat");
                    }

                    header.fccHandler = (uint)infoHeader.biCompression;
                }

                if (infoOnly)
                {
                    position = 0;
                    return AviReaderError.Ok;
                }
                if (position > Length - 1)
                    position = Length - 1;

                if (colorSpace == ColorSpace.RGB)
                {
                    var bmih = new BitmapInfoHeader
                    {
                        biSize = (uint)Marshal.SizeOf(typeof(BitmapInfoHeader)),
                        biWidth = 0,
                        biHeight = 0,
                        biPlanes = 1,
                        biBitCount = BitCount.BitPerPixel32BPP,
                        biCompression = BitmapCompression.BI_RGB,
                        biSizeImage = 0,
                        biXPelsPerMeter = 0,
                        biYPelsPerMeter = 0,
                        biClrUsed = 0,
                        biClrImportant = 0
                    };

                    getFrameHandle = AVI.AVIStreamGetFrameOpen(stream, ref bmih);

                    if (getFrameHandle == IntPtr.Zero)
                    {
                        AVI.AVIStreamRelease(stream);
                        stream = IntPtr.Zero;
                        AVI.AVIFileRelease(aviFile);
                        aviFile = IntPtr.Zero;
                        return new AviReaderError(AviError.AVIERR_ERROR, "AVIStreamGetFrameOpen (NULL frame handle returned)");
                    }

                    if (!RereadDecompressedFrame())
                        return new AviReaderError(AviError.AVIERR_ERROR, "AVIStreamGetFrame (NULL frame returned)");

                }
                else
                {
                    if (!RereadRawFrame())
                        return new AviReaderError(AviError.AVIERR_ERROR, "AVIStreamGetFrame (NULL frame returned)");

                }
            }
            return AviReaderError.Ok;
        }

        // ReSharper disable once InconsistentNaming
        private static BitmapCompression ToFourCC(string fourCC)
        {
            uint[] c = fourCC.ToCharArray().Select(v => (uint)v).ToArray();
            return (BitmapCompression)((c[3] << 24) | (c[2] << 16) | (c[1] << 8) | c[0]);
        }

        private void CreatePreviewScript()
        {
            if (Source == null)
                return;

            bool sourceIsAvi = Source.ToLowerInvariant().EndsWith(".avi");
            bool sourceIsAvs = Source.ToLowerInvariant().EndsWith(".avs");
            if (UseLwLibav)
            {
                sourceIsAvi = false;
            }
            if (infoOnly && sourceIsAvi)
            {
                avsScript = Source;
                return;
            }
            if (Resize == null && sourceIsAvs && colorSpace == ColorSpace.RGB)
            {
                avsScript = Source;
                return;
            }

            avsScript = TemporaryAvsScriptFile;
            var avsScriptPath = Path.GetDirectoryName(avsScript);
            if (avsScriptPath != null)
            {
                Directory.CreateDirectory(avsScriptPath);
            }

            using (var writer = new StreamWriter(avsScript))
            {
                writer.WriteLine(@"SetMemoryMax(512)");
                if (sourceIsAvi)
                {
                    writer.WriteLine($@"OpenDMLSource(""{Source}"", audio=false)");
                } else if (sourceIsAvs)
                {
                    writer.WriteLine($@"Import(""{Source}"").KillAudio()");
                }
                else
                {
                    writer.WriteLine($@"LWLibavVideoSource(""{Source}"").KillAudio()");
                }

                switch (colorSpace)
                {
                    case ColorSpace.YUY2:
                        writer.WriteLine(@"ConvertToYUY2()");
                        break;
                    case ColorSpace.YV12:
                        writer.WriteLine(@"ConvertToYV12()");
                        break;
                    case ColorSpace.YV24:
                        writer.WriteLine(@"ConvertToYV24()");
                        break;
                    default:
                        writer.WriteLine(@"ConvertToRGB32()");
                        break;
                }

                if (Resize != null)
                {
                    var type = typeof(ResizeFilter);
                    var memInfo = type.GetMember(ResizeMode.ToString());
                    var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute),
                        false);
                    var description = ((DescriptionAttribute)attributes[0]).Description;

                    writer.WriteLine($@"{description}({Resize.Width.ToInvariant()},{Resize.Height.ToInvariant()})");
                }
            }
        }

        private bool RereadDecompressedFrame()
        {
#if USE_LOCK
            lock (Lock)
#endif
            {
                if (getFrameHandle != IntPtr.Zero && Position >= 0 && Position < Length)
                {
                    var pDib = AVI.AVIStreamGetFrame(getFrameHandle, Position);
                    if (pDib == IntPtr.Zero)
                        return false;
                    var bih = (BitmapInfoHeader)Marshal.PtrToStructure(pDib, typeof(BitmapInfoHeader));
                    BitmapInfo = new BitmapInfo
                    {
                        bmiHeader = bih,
                    };
                    var imgOffset = bih.biSize + bih.biClrUsed * Marshal.SizeOf(typeof(RGBQuad));
                    var imgBytes = pDib + (int)imgOffset;
                    ImageData = new byte[bih.biSizeImage];
                    Marshal.Copy(imgBytes, ImageData, 0, ImageData.Length);
                    Image = null;
                }
            }
            return true;
        }

        private bool RereadRawFrame()
        {
#if USE_LOCK
            lock (Lock)
#endif
            {
                if (Position >= 0 && Position < Length)
                {
                    AVI.AVIStreamRead(stream, Position, 1, out int plBytes, out int plSamples);
                    var frame = Marshal.AllocHGlobal(plBytes);
                    AVI.AVIStreamRead(stream, Position, 1, frame, plBytes);
                    ImageData = new byte[plBytes];
                    Marshal.Copy(frame, ImageData, 0, ImageData.Length);
                    Marshal.FreeHGlobal(frame);
                    Image = null;
                }
            }
            return true;
        }
    }
}
