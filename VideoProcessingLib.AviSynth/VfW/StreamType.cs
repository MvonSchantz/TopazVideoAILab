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
    /// Stream types for use in .Net (translated from C macros)
    /// </summary>
    [Flags]
    public enum StreamType : uint
    {
        /// <summary>
        /// Indicates an audio stream.
        /// </summary>
        AUDIO = 1935963489,
        /// <summary>
        /// Indicates a MIDI stream.
        /// </summary>
        MIDI = 1935960429,
        /// <summary>
        /// Indicates a text stream.
        /// </summary>
        TEXT = 1937012852,
        /// <summary>
        /// Indicates a video stream.
        /// </summary>
        VIDEO = 1935960438
    }
}