using System;
using System.Diagnostics;

namespace VideoProcessingLib.AviSynth
{
    public class Range
    {
        public int First;
        public int Last;

        public override string ToString()
        {
            return AvsString;
        }

        public string AvsString => $"{First},{Last}";

        public int Length => Last - First + 1;

        public Range(int first, int last)
        {
            First = first;
            Last = last;
        }

        public Range(Range range)
        {
            if (range != null)
            {
                First = range.First;
                Last = range.Last;
            }
        }

        public bool Contains(int position)
        {
            return position >= First && position <= Last;
        }

        public void ClipTo(Range range)
        {
            if (range.First > Last || range.Last < First)
            {
                First = 0;
                Last = 0;
                return;
            }

            if (range.First <= First && range.Last >= Last)
            {
                return;
            }

            if (range.First <= First && range.Last >= First && range.Last <= Last)
            {
                Last = range.Last;
                return;
            }

            if (range.Last >= Last && range.First <= Last && range.First >= First)
            {
                First = range.First;
                return;
            }

            Debugger.Break();
            throw new NotImplementedException();
        }
    }
}
