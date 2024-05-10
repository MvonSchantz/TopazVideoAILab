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
using System.Runtime.InteropServices;

namespace System.PInvoke.Windows.Structs
{
    /// <summary>
    /// The BITMAPINFO structure defines the dimensions and color information for a DIB
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct BitmapInfo
    {
        /// <summary>
        /// Specifies a BITMAPINFOHEADER structure that contains information about the dimensions of color format
        /// </summary>        
        public BitmapInfoHeader bmiHeader;

        /// <summary>
        /// The bmiColors member contains one of the following: 
        /// An array of RGBQUAD. The elements of the array that make up the color table. 
        /// An array of 16-bit unsigned integers that specifies indexes into the currently realized logical palette. 
        /// This use of bmiColors is allowed for functions that use DIBs. When bmiColors elements contain indexes to 
        /// a realized logical palette, they must also call the following bitmap functions: 
        /// CreateDIBitmap 
        ///
        /// CreateDIBPatternBrush 
        ///
        /// CreateDIBSection 
        ///
        /// The iUsage parameter of CreateDIBSection must be set to DIB_PAL_COLORS.
        /// The number of entries in the array depends on the values of the biBitCount
        /// and biClrUsed members of the BITMAPINFOHEADER structure. 
        /// The colors in the bmiColors table appear in order of importance. For more information, see the Remarks section
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1, ArraySubType = UnmanagedType.Struct)] public RGBQuad[]
            bmiColors;
    }
}