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
namespace System.PInvoke.Windows.Enums
{
    /// <summary>
    /// Flags used for AVI Compression
    /// </summary>
    [Flags]
    public enum AviCompression : uint
    {
        /// <summary>
        /// Compresses this video stream using the data rate specified in dwBytesPerSecond.
        /// </summary>
        AVICOMPRESSF_DATARATE = 0x00000002,
        /// <summary>
        /// Interleaves this stream every dwInterleaveEvery frames with respect to the first stream.
        /// </summary>
        AVICOMPRESSF_INTERLEAVE = 0x00000001,
        /// <summary>
        /// Saves this video stream with key frames at least every dwKeyFrameEvery frames.
        /// By default, every frame will be a key frame.
        /// </summary>
        AVICOMPRESSF_KEYFRAMES = 0x00000004,
        /// <summary>
        /// Uses the data in this structure to set the default compression values for 
        /// AVISaveOptions. If an empty structure is passed and this flag is not set, 
        /// some defaults will be chosen.
        /// </summary>
        AVICOMPRESSF_VALID = 0x00000008
    }
}