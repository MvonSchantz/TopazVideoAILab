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
    /// Ternary raster operations
    /// </summary>
    [Flags]
    public enum RasterOperation : uint
    {
        /// <summary>
        /// dest = source
        /// 
        /// Copies the source rectangle directly to the destination rectangle
        /// </summary>
        SRCCOPY = 0x00CC0020,
        /// <summary>
        /// dest = source OR dest
        /// 
        /// Combines the colors of the source and destination rectangles by using the Boolean OR operator.
        /// </summary>
        SRCPAINT = 0x00EE0086,
        /// <summary>
        /// dest = source AND dest
        /// 
        /// Combines the colors of the source and destination rectangles by using the Boolean AND operator
        /// </summary>
        SRCAND = 0x008800C6,
        /// <summary>
        /// dest = source XOR dest
        /// 
        /// Combines the colors of the source and destination rectangles by using the Boolean XOR operator
        /// </summary>
        SRCINVERT = 0x00660046,
        /// <summary>
        /// dest = source AND (NOT dest )
        /// 
        /// Combines the inverted colors of the destination rectangle with the colors of the source 
        /// rectangle by using the Boolean AND operator
        /// </summary>
        SRCERASE = 0x00440328,
        /// <summary>
        /// dest = (NOT source)
        /// 
        /// Copies the inverted source rectangle to the destination
        /// </summary>
        NOTSRCCOPY = 0x00330008,
        /// <summary>
        /// dest = (NOT src) AND (NOT dest)
        /// 
        /// Combines the colors of the source and destination rectangles 
        /// by using the Boolean OR operator and then inverts the resultant color
        /// </summary>
        NOTSRCERASE = 0x001100A6,
        /// <summary>
        /// dest = (source AND pattern)
        /// 
        /// Merges the colors of the source rectangle with the 
        /// brush currently selected in hdcDest, by using the Boolean AND operator
        /// </summary>
        MERGECOPY = 0x00C000CA,
        /// <summary>
        /// dest = (NOT source) OR dest
        /// 
        /// Merges the colors of the inverted source rectangle with the colors of the 
        /// destination rectangle by using the Boolean OR operator
        /// </summary>
        MERGEPAINT = 0x00BB0226,
        /// <summary>
        /// dest = pattern
        /// 
        /// Copies the brush currently selected in hdcDest, into the destination bitmap
        /// </summary>
        PATCOPY = 0x00F00021,
        /// <summary>
        /// dest = DPSnoo
        /// 
        /// Combines the colors of the brush currently selected in hdcDest, with the colors 
        /// of the inverted source rectangle by using the Boolean OR operator. The result of 
        /// this operation is combined with the colors of the destination rectangle by using 
        /// the Boolean OR operator
        /// </summary>
        PATPAINT = 0x00FB0A09,
        /// <summary>
        /// dest = pattern XOR dest 
        /// 
        /// Combines the colors of the brush currently selected in hdcDest, with the colors 
        /// of the destination rectangle by using the Boolean XOR operator
        /// </summary>
        PATINVERT = 0x005A0049,
        /// <summary>
        /// dest = (NOT dest)
        /// 
        /// Inverts the destination rectangle.
        /// </summary>
        DSTINVERT = 0x00550009,
        /// <summary>
        /// dest = BLACK 
        /// Fills the destination rectangle using the color associated with index 
        /// 0 in the physical palette. (This color is black for the default physical palette.)
        /// </summary>
        BLACKNESS = 0x00000042,
        /// <summary>
        /// dest = WHITE 
        /// 
        /// Fills the destination rectangle using the color associated with index 1 in the physical palette.
        /// (This color is white for the default physical palette.)
        /// </summary>
        WHITENESS = 0x00FF0062,
        /// <summary>
        /// Include layered windows
        /// 
        /// Windows 98/Me, Windows 2000/XP: Includes any windows that are layered on top of your 
        /// window in the resulting image. By default, the image only contains your window. Note 
        /// that this generally cannot be used for printing device contexts.
        /// </summary>
        CAPTUREBLT = 0x40000000,
        /// <summary>
        /// Windows 98/Me, Windows 2000/XP: Prevents the bitmap from being mirrored
        /// </summary>
        NOMIRRORBITMAP = 0x80000000
    }
}