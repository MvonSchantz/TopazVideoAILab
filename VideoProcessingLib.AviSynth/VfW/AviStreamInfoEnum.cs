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
    /// Contains the stream info flags for AVI
    /// </summary>
    [Flags]
    public enum AviStreamInfo
    {
        /// <summary>
        /// Indicates this stream should be rendered when explicitly enabled by the user.
        /// </summary>
        AVISTREAMINFO_DISABLED = 0x00000001,
        /// <summary>
        /// Indicates this video stream contains palette changes. This flag warns the
        /// playback software that it will need to animate the palette.
        /// </summary>
        AVISTREAMINFO_FORMATCHANGES = 0x00010000
    }
}