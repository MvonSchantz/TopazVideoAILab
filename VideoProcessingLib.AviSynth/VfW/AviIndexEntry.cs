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
    /// Flags for The default AVI file handler supports writing only at the end of a stream.
    /// </summary>
    [Flags]
    public enum AviIndexEntry
    {
        /// <summary>
        /// 
        /// </summary>
        // TODO : Put documentation
        AVIIF_LIST = 0x00000001,
        /// <summary>
        /// 
        /// </summary>
        // TODO : Put documentation
        AVIIF_TWOCC = 0x00000002,
        /// <summary>
        /// Indicates this data does not rely on preceding data in the file
        /// </summary>
        AVIIF_KEYFRAME = 0x00000010
    }
}