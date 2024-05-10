#region Copyright Notice
// 
// 
// Copyright (c) 2009-2010 Anindya Chatterjee
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// 
// 
#endregion
using System.PInvoke.Windows.Enums;
using System.Runtime.InteropServices;

namespace System.PInvoke.Windows.Structs
{
    /// <summary>
    /// The AVISTREAMINFO structure contains information for a single stream. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AVIStreamInfo
    {
        /// <summary>
        /// Type of stream
        /// </summary>
        public StreamType fccType;

        /// <summary>
        /// Four-character code of the compressor handler that will compress
        /// this video stream when it is saved (for example, 
        /// mmioFOURCC('M','S','V','C')
        /// mmioFOURCC('X','V','I','D')
        /// mmioFOURCC('D','I','V','X')
        /// mmioFOURCC('M','P','G','4')
        /// mmioFOURCC('D','X','5','0')
        /// mmioFOURCC('W','M','V','1')).
        /// This member is not used for audio streams.
        /// </summary>
        public UInt32 fccHandler;

        public string FourCc => FourCcString(fccHandler);

        /// <summary>
        /// Applicable flags for the stream. The bits in the high-order 
        /// word of these flags are specific to the type of data contained in the stream.
        /// </summary>
        public AviStreamInfo dwFlags;

        /// <summary>
        /// Capability flags; currently unused.
        /// </summary>
        public Int32 dwCaps;

        /// <summary>
        /// Priority of the stream.
        /// </summary>
        public Int16 wPriority;

        /// <summary>
        /// Language of the stream.
        /// </summary>
        public Int16 wLanguage;

        /// <summary>
        /// Time scale applicable for the stream. Dividing dwRate by dwScale gives 
        /// the playback rate in number of samples per second.
        /// For video streams, this rate should be the frame rate. For audio streams,
        /// this rate should correspond to the audio block size (the nBlockAlign member 
        /// of the WAVEFORMAT or PCMWAVEFORMAT structure), which for PCM (Pulse Code Modulation)
        /// audio reduces to the sample rate.
        /// </summary>
        public Int32 dwScale;

        /// <summary>
        /// Rate in an integer format. To obtain the rate in samples per second, 
        /// divide this value by the value in dwScale.
        /// </summary>
        public Int32 dwRate;

        /// <summary>
        /// Sample number of the first frame of the AVI file. The units are defined by 
        /// dwRate and dwScale. Normally, this is zero, but it can specify a delay time 
        /// for a stream that does not start concurrently with the file. 
        /// The 1.0 release of the AVI tools does not support a nonzero starting time.
        /// </summary>
        public Int32 dwStart;

        /// <summary>
        /// Length of this stream. The units are defined by dwRate and dwScale.
        /// </summary>
        public Int32 dwLength;

        /// <summary>
        /// Audio skew. This member specifies how much to skew the audio data ahead of 
        /// the video frames in interleaved files. Typically, this is about 0.75 seconds.
        /// </summary>
        public Int32 dwInitialFrames;

        /// <summary>
        /// Recommended buffer size, in bytes, for the stream. Typically, this member 
        /// contains a value corresponding to the largest chunk in the stream. Using the 
        /// correct buffer size makes playback more efficient. Use zero if you do not know
        /// the correct buffer size.
        /// </summary>
        public Int32 dwSuggestedBufferSize;

        /// <summary>
        /// Quality indicator of the video data in the stream. Quality is represented
        /// as a number between 0 and 10,000. For compressed data, this typically 
        /// represents the value of the quality parameter passed to the compression software.
        /// If set to  - 1, drivers use the default quality value.
        /// </summary>
        public Int32 dwQuality;

        /// <summary>
        /// Size, in bytes, of a single data sample. If the value of this member is zero,
        /// the samples can vary in size and each data sample (such as a video frame) must
        /// be in a separate chunk. A nonzero value indicates that multiple samples of data
        /// can be grouped into a single chunk within the file.
        /// For video streams, this number is typically zero, although it can be nonzero 
        /// if all video frames are the same size. For audio streams, this number should 
        /// be the same as the nBlockAlign member of the WAVEFORMAT or WAVEFORMATEX 
        /// structure describing the audio.
        /// /// </summary>
        public Int32 dwSampleSize;

        /// <summary>
        /// Dimensions of the video destination rectangle. The values represent the 
        /// coordinates of upper left corner, the height, and the width of the rectangle.
        /// </summary>
        public Rect rcFrame;

        /// <summary>
        /// Number of times the stream has been edited. The stream handler maintains this count.
        /// </summary>
        public Int32 dwEditCount;

        /// <summary>
        /// Number of times the stream format has changed. The stream handler maintains this count.
        /// </summary>
        public Int32 dwFormatChangeCount;

        /// <summary>
        /// null-terminated string containing a description of the stream.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public UInt16[] szName;


        /// <summary>
        /// Use this method to get the four character code for various purpose
        /// </summary>
        /// <param name="fcc">String value, constructed from four characters</param>
        /// <returns>An integer value corresponding that code</returns>
        public static uint MMIOFOURCC(string fcc)
        {
            var str = new char[4];
            for (var i = 0; i < 4; i++)
            {
                if (i < fcc.Length)
                {
                    str[i] = fcc[i];
                }
                else
                {
                    str[i] = ' ';
                }
            }
            return MMIOFOURCC(str[0], str[1], str[2], str[3]);
        }


        /// <summary>
        /// Use this method to get the four character code for various purpose
        /// </summary>
        /// <param name="ch0">First character code</param>
        /// <param name="ch1">Second character code</param>
        /// <param name="ch2">Third character code</param>
        /// <param name="ch3">Fourth character code</param>
        /// <returns>An integer value corresponding that code</returns>
        public static uint MMIOFOURCC(char ch0, char ch1, char ch2, char ch3)
        {
            return (uint) ((byte) (ch0) | ((byte) (ch1) << 8) | ((byte) (ch2) << 16) | ((byte) (ch3) << 24));
        }

        public static string FourCcString(uint fourCc)
        {
            char ch0 = (char)(fourCc & 0xFF);
            char ch1 = (char)((fourCc & 0xFF00) >> 8);
            char ch2 = (char)((fourCc & 0xFF0000) >> 16);
            char ch3 = (char)((fourCc & 0xFF000000) >> 24);
            return string.Format("{0}{1}{2}{3}", ch0, ch1, ch2, ch3);
        }
    }
}