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
    /// AVI Error Codes
    /// </summary>
    [Flags]
    public enum AviError : uint
    {
        /// <summary>
        /// Compression is not supported for this type of data. 
        /// This error might be returned if you try to compress
        /// data that is not audio or video.
        /// </summary>
        AVIERR_UNSUPPORTED = 0x80044065,
        /// <summary>
        /// The file couldn't be read, indicating a corrupt file or an unrecognized format
        /// </summary>
        AVIERR_BADFORMAT = 0x80044066,
        /// <summary>
        /// There is not enough memory to complete the operation.
        /// </summary>
        AVIERR_MEMORY = 0x80044067,
        /// <summary>
        /// 
        /// </summary>
        // TODO : Put documentation
        AVIERR_INTERNAL = 0x80044068,
        /// <summary>
        /// 
        /// </summary>
        // TODO : Put documentation
        AVIERR_BADFLAGS = 0x80044069,
        /// <summary>
        /// 
        /// </summary>
        // TODO : Put documentation
        AVIERR_BADPARAM = 0x8004406A,
        /// <summary>
        /// 
        /// </summary>
        // TODO : Put documentation
        AVIERR_BADSIZE = 0x8004406B,
        /// <summary>
        /// 
        /// </summary>
        // TODO : Put documentation
        AVIERR_BADHANDLE = 0x8004406C,
        /// <summary>
        /// A disk error occurred while reading the file
        /// </summary>
        AVIERR_FILEREAD = 0x8004406D,
        /// <summary>
        /// 
        /// </summary>
        // TODO : Put documentation
        AVIERR_FILEWRITE = 0x8004406E,
        /// <summary>
        /// A disk error occurred while opening the file
        /// </summary>
        AVIERR_FILEOPEN = 0x8004406F,
        /// <summary>
        /// 
        /// </summary>
        // TODO : Put documentation
        AVIERR_COMPRESSOR = 0x80044070,
        /// <summary>
        /// A suitable compressor cannot be found.
        /// </summary>
        AVIERR_NOCOMPRESSOR = 0x80044071,
        /// <summary>
        /// 
        /// </summary>
        // TODO : Put documentation
        AVIERR_READONLY = 0x80044072,
        /// <summary>
        /// 
        /// </summary>
        // TODO : Put documentation
        AVIERR_NODATA = 0x80044073,
        /// <summary>
        /// 
        /// </summary>
        // TODO : Put documentation
        AVIERR_BUFFERTOOSMALL = 0x80044074,
        /// <summary>
        /// 
        /// </summary>
        // TODO : Put documentation
        AVIERR_CANTCOMPRESS = 0x80044075,
        /// <summary>
        /// 
        /// </summary>
        // TODO : Put documentation
        AVIERR_USERABORT = 0x800440C6,
        /// <summary>
        /// 
        /// </summary>
        // TODO : Put documentation
        AVIERR_ERROR = 0x800440C7,
        /// <summary>
        /// Operation successful
        /// </summary>
        AVIERR_OK = 0x0
    }
}