using System.Globalization;

namespace VideoProcessingLib.AviSynth
{
    public static class ExtensionMethods
    {
        public static string ToInvariant(this short s, string format = null)
        {
            if (format != null)
            {
                return s.ToString(format, NumberFormatInfo.InvariantInfo);
            }
            return s.ToString(NumberFormatInfo.InvariantInfo);
        }

        public static string ToInvariant(this int i, string format = null)
        {
            if (format != null)
            {
                return i.ToString(format, NumberFormatInfo.InvariantInfo);
            }
            return i.ToString(NumberFormatInfo.InvariantInfo);
        }

        public static string ToInvariant(this long l, string format = null)
        {
            if (format != null)
            {
                return l.ToString(format, NumberFormatInfo.InvariantInfo);
            }
            return l.ToString(NumberFormatInfo.InvariantInfo);
        }

        public static string ToInvariant(this float f, string format = null)
        {
            if (format != null)
            {
                return f.ToString(format, NumberFormatInfo.InvariantInfo);
            }
            return f.ToString(NumberFormatInfo.InvariantInfo);
        }

        public static string ToInvariant(this double d, string format = null)
        {
            if (format != null)
            {
                return d.ToString(format, NumberFormatInfo.InvariantInfo);
            }
            return d.ToString(NumberFormatInfo.InvariantInfo);
        }
    }
}
