using System;
using System.IO;

namespace VideoProcessingLib.AviSynth
{
    public enum LimitFft
    {
        Cpu = 1,
        Gpu = 2,
    }

    public class ScriptFunctions
    {
        private StreamWriter Writer { get; }

        public ScriptFunctions(StreamWriter writer)
        {
            Writer = writer;
        }

        public void Source(string file, bool killAudio = true, bool lineBreak = true)
        {
            if (lineBreak)
                Writer.WriteLine(Script.Source(file, killAudio));
            else
                Writer.Write(Script.Source(file, killAudio));
        }

        #region Common script functions

        private void WriteFunction(string function, bool lineBreak)
        {
            if (lineBreak)
                Writer.WriteLine(function);
            else
                Writer.Write(function);
        }

        public void Trim(int start, int end, bool lineBreak = true)
        {
            WriteFunction($"Trim({start},{end})", lineBreak);
        }

        public void ConvertToRgb(Matrix matrix = Matrix.Undefined, bool lineBreak = true)
        {
            switch (matrix)
            {
                case Matrix.Undefined:
                    WriteFunction("ConvertToRGB()", lineBreak);
                    break;
                case Matrix.Rec601:
                    WriteFunction("ConvertToRGB(matrix=\"Rec601\")", lineBreak);
                    break;
                case Matrix.Rec709:
                    WriteFunction("ConvertToRGB(matrix=\"Rec709\")", lineBreak);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(matrix), matrix, null);
            }
        }

        public void ConvertToRgb24(Matrix matrix = Matrix.Undefined, bool lineBreak = true)
        {
            switch (matrix)
            {
                case Matrix.Undefined:
                    WriteFunction("ConvertToRGB24()", lineBreak);
                    break;
                case Matrix.Rec601:
                    WriteFunction("ConvertToRGB24(matrix=\"Rec601\")", lineBreak);
                    break;
                case Matrix.Rec709:
                    WriteFunction("ConvertToRGB24(matrix=\"Rec709\")", lineBreak);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(matrix), matrix, null);
            }
        }

        public void ConvertToYv12(Matrix matrix = Matrix.Undefined, bool lineBreak = true)
        {
            switch (matrix)
            {
                case Matrix.Undefined:
                    WriteFunction("ConvertToYV12()", lineBreak);
                    break;
                case Matrix.Rec601:
                    WriteFunction("ConvertToYV12(matrix=\"Rec601\")", lineBreak);
                    break;
                case Matrix.Rec709:
                    WriteFunction("ConvertToYV12(matrix=\"Rec709\")", lineBreak);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(matrix), matrix, null);
            }
        }

        public void ConvertToYv24(Matrix matrix = Matrix.Undefined, bool lineBreak = true)
        {
            switch (matrix)
            {
                case Matrix.Undefined:
                    WriteFunction("ConvertToYV24()", lineBreak);
                    break;
                case Matrix.Rec601:
                    WriteFunction("ConvertToYV24(matrix=\"Rec601\")", lineBreak);
                    break;
                case Matrix.Rec709:
                    WriteFunction("ConvertToYV24(matrix=\"Rec709\")", lineBreak);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(matrix), matrix, null);
            }
        }

        public void ConvertToYuv(Matrix matrix = Matrix.Undefined, bool lineBreak = true)
        {
            switch (matrix)
            {
                case Matrix.Undefined:
                    WriteFunction("ConvertToYUV()", lineBreak);
                    break;
                case Matrix.Rec601:
                    WriteFunction("ConvertToYUV(matrix=\"Rec601\")", lineBreak);
                    break;
                case Matrix.Rec709:
                    WriteFunction("ConvertToYUV(matrix=\"Rec709\")", lineBreak);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(matrix), matrix, null);
            }
        }
        public void Spline64Resize(int width, int height, bool lineBreak = true)
        {
            WriteFunction($"Spline64Resize({width.ToInvariant()},{height.ToInvariant()})", lineBreak);
        }
        public void LanczosResize(int width, int height, bool lineBreak = true)
        {
            WriteFunction($"LanczosResize({width.ToInvariant()},{height.ToInvariant()})", lineBreak);
        }
        public void BicubicResize(int width, int height, bool lineBreak = true)
        {
            WriteFunction($"BicubicResize({width.ToInvariant()},{height.ToInvariant()})", lineBreak);
        }
        public void BilinearResize(int width, int height, bool lineBreak = true)
        {
            WriteFunction($"BilinearResize({width.ToInvariant()},{height.ToInvariant()})", lineBreak);
        }
        public void PointResize(int width, int height, bool lineBreak = true)
        {
            WriteFunction($"PointResize({width.ToInvariant()},{height.ToInvariant()})", lineBreak);
        }

        public void Subtitle(string text, bool lineBreak = true)
        {
            WriteFunction($"Subtitle(\"{text}\")", lineBreak);
        }

        #endregion

        #region Plugin script functions
        
        public void TemporalDegrain2(LimitFft limitFft = LimitFft.Gpu, int degrainTr = 1, bool lineBreak = true)
        {
            WriteFunction($"TemporalDegrain2(limitFFT={((int)limitFft).ToInvariant()},degrainTR={degrainTr.ToInvariant()})", lineBreak);
        }

        #endregion
    }
}
