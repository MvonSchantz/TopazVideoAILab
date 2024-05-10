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
    /// The AVICOMPRESSOPTIONS structure contains information about a stream
    /// and how it is compressed and saved. This structure passes data to the
    /// AVIMakeCompressedStream function (or the AVISave function, which uses 
    /// AVIMakeCompressedStream).
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AVICompressOptions
    {
        /// <summary>
        /// Type of the stream
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

        /// <summary>
        /// Maximum period between video key frames. This member is used only if 
        /// the AviCompression.KEYFRAMES flag is set; otherwise every video frame is a key frame.
        /// </summary>
        public UInt32 dwKeyFrameEvery;

        /// <summary>
        /// Quality value passed to a video compressor. This member is not used for an audio compressor.
        /// </summary>
        public UInt32 dwQuality;

        /// <summary>
        /// Video compressor data rate. This member is used only if the AviCompression.DATARATE flag is set.
        /// </summary>
        public UInt32 dwBytesPerSecond;

        /// <summary>
        /// Flags used for compression.
        /// </summary>
        public AviCompression dwFlags;

        /// <summary>
        /// Pointer to a structure defining the data format. 
        /// For an audio stream, this is an LPWAVEFORMAT structure.
        /// </summary>
        public IntPtr lpFormat;

        /// <summary>
        /// Size, in bytes, of the data referenced by lpFormat.
        /// </summary>
        public UInt32 cbFormat;

        /// <summary>
        /// Video-compressor-specific data; used internally.
        /// </summary>
        public IntPtr lpParms;

        /// <summary>
        /// Size, in bytes, of the data referenced by lpParms
        /// </summary>
        public UInt32 cbParms;

        /// <summary>
        /// Interleave factor for interspersing stream data with data from the first stream. 
        /// Used only if the AviCompression.INTERLEAVE flag is set.
        /// </summary>
        public UInt32 dwInterleaveEvery;
    }
}