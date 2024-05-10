using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.PInvoke.Windows.Structs;

namespace VideoProcessingLib.AviSynth
{
    public enum Matrix
    {
        Undefined,
        Rec601,
        Rec709,
    }

    public class Script : IDisposable
    {
        public StreamWriter Writer { get; private set; }

        public ScriptFunctions Output { get; private set; }

        private AviReader aviReader;

        public AviReader AviReader
        {
            get
            {
                if (aviReader != null)
                    return aviReader;
                aviReader = new AviReader(ScriptFile);
                return aviReader;
            }
        }

        public string ScriptFile { get; }
        private bool IsTemporary { get; }

        public int MaxMemory { get; }
        public int Concurrency { get; set; }

        public Script(string scriptFile, int maxMemory = 1024, int concurrency = -1)
        {
            ScriptFile = scriptFile;
            Writer = new StreamWriter(ScriptFile);
            Output = new ScriptFunctions(Writer);
            MaxMemory = maxMemory;
            Concurrency = concurrency;
            Writer.WriteLine($@"SetMemoryMax({maxMemory.ToInvariant()})");
        }

        public Script(int maxMemory = 1024, int concurrency = -1) : this(Path.Combine(Path.GetTempPath(), "Script_" + Guid.NewGuid() + ".avs"), maxMemory, concurrency)
        {
            IsTemporary = true;
        }

        /*public void Dispose()
        {
            if (Writer != null)
                Close();

            aviReader?.Dispose();
            aviReader = null;

            if (IsTemporary)
            {
                if (File.Exists(ScriptFile))
                {
                    File.Delete(ScriptFile);
                }
            }
        }*/

        private void ReleaseUnmanagedResources()
        {
            // TODO release unmanaged resources here
        }

        private void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            if (disposing)
            {
                aviReader?.Dispose();
                Close();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Script()
        {
            Dispose(false);
        }


        public void Close()
        {
            if (Writer != null)
            {
                if (Concurrency > 0)
                {
                    Writer.WriteLine($@"Prefetch({Concurrency.ToInvariant()})");
                }
                else if (Concurrency == 0)
                {
                    //Writer.WriteLine($@"Prefetch({Environment.ProcessorCount})");
                    Writer.WriteLine($@"Prefetch()");
                }

                Writer.Dispose();
                Writer = null;
            }
        }

        public BitmapInfo GetBitmapInfo()
        {
            return AviReader.BitmapInfo;
        }

        public Image GetImage(int position)
        {
            AviReader.Position = position;
            return AviReader.Image;
        }

        public byte[] GetImageData(int position)
        {
            AviReader.Position = position;
            return AviReader.ImageData;
        }

        public void Write(string str)
        {
            Writer.Write(str);
        }

        public void WriteLine(string str)
        {
            Writer.WriteLine(str);
        }

        public void WriteLine()
        {
            Writer.WriteLine();
        }

        public void Write(string str, params object[] values)
        {
            Writer.Write(str, values.Select(v => v is int i ? i.ToInvariant() : v));
        }

        public void WriteLine(string str, params object[] values)
        {
            Write(str, values);
            Writer.WriteLine();
        }

        public static string Source(string file, bool killAudio = true)
        {
            if (file.ToLowerInvariant().EndsWith(".avi"))
            {
                return $@"OpenDMLSource(""{file}"", audio={(killAudio ? "false" : "true")})";
            }

            if (file.ToLowerInvariant().EndsWith(".avs"))
            {
                return $@"Import(""{file}"")" + (killAudio ? ".KillAudio()" : "");
            }

            if (file.EndsWith("\\"))
            {
                var files = Directory.GetFiles(file, "*.png");
                var fileNumbers = files.Select(f => int.Parse(Path.GetFileNameWithoutExtension(f))).OrderBy(n => n).ToArray();

                //return $@"ImageSource(""{file}%06d.png"", {fileNumbers.First()}, {fileNumbers.Last()}, 25, true, false, ""RGB32"")";
                return $@"PNGSource(""{file}%06d.png"", {fileNumbers.First()}, {fileNumbers.Last()}, 25)";
            }

            return $@"LWLibavVideoSource(""{file}"")" + (killAudio ? ".KillAudio()" : "");
        }

    }
}
