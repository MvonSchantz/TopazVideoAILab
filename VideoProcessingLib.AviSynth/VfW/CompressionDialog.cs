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
    /// Flags for displaying the Compression Options dialog box
    /// </summary>
    [Flags]
    public enum CompressionDialog : uint
    {
        /// <summary>
        /// Displays a Key Frame Every dialog box for the video options. 
        /// This is the same flag used in the ICCompressorChoose function.
        /// </summary>
        ICMF_CHOOSE_KEYFRAME = 0x0001,
        /// <summary>
        /// Displays a Data Rate dialog box for the video options. 
        /// This is the same flag used in ICCompressorChoose.
        /// </summary>
        ICMF_CHOOSE_DATARATE = 0x0002,
        /// <summary>
        /// Displays a Preview button for the video options. 
        /// This button previews the compression by using a frame from the stream. 
        /// This is the same flag used in ICCompressorChoose.
        /// </summary>
        ICMF_CHOOSE_PREVIEW = 0x0004
    }
}