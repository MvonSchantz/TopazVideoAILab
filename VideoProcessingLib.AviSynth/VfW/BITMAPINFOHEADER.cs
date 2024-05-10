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
using System.PInvoke.Windows.Interface;
using System.Runtime.InteropServices;

namespace System.PInvoke.Windows.Structs
{
    /// <summary>
    /// The BITMAPINFOHEADER structure contains information about the dimensions and color format of a DIB.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct BitmapInfoHeader : IBitmapInfoHeader
    {
        /// <summary>
        /// Specifies the number of bytes required by the structure. 
        /// </summary>
        public uint biSize;

        /// <summary>
        /// Specifies the width of the bitmap, in pixels. 
        /// </summary>
        /// <remarks> Windows 98/Me, Windows 2000/XP: If biCompression is BI_JPEG or BI_PNG, 
        /// the biWidth member specifies the width of the decompressed JPEG or PNG image file,
        /// respectively. </remarks>
        public int biWidth;

        /// <summary>
        /// Specifies the height of the bitmap, in pixels. If biHeight is positive, the bitmap
        /// is a bottom-up DIB and its origin is the lower-left corner. If biHeight is negative, 
        /// the bitmap is a top-down DIB and its origin is the upper-left corner. 
        /// If biHeight is negative, indicating a top-down DIB, biCompression must be either 
        /// BI_RGB or BI_BITFIELDS. Top-down DIBs cannot be compressed. 
        /// </summary>
        public int biHeight;

        /// <summary>
        /// Specifies the number of planes for the target device. This value must be set to 1.
        /// </summary>
        public ushort biPlanes;

        /// <summary>
        /// Specifies the number of bits-per-pixel. The biBitCount member of the BITMAPINFOHEADER 
        /// structure determines the number of bits that define each pixel and the maximum number 
        /// of colors in the bitmap. This member must be one of the following values. 
        /// </summary>
        public BitCount biBitCount;

        /// <summary>
        /// Specifies the type of compression for a compressed bottom-up bitmap 
        /// (top-down DIBs cannot be compressed). This member can be one of the following values.
        /// </summary>
        public BitmapCompression biCompression;

        /// <summary>
        /// Specifies the size, in bytes, of the image. This may be set to zero for BI_RGB bitmaps. 
        /// Windows 98/Me, Windows 2000/XP: If biCompression is BI_JPEG or BI_PNG, biSizeImage
        /// indicates the size of the JPEG or PNG image buffer, respectively.
        /// </summary>
        public uint biSizeImage;

        /// <summary>
        /// Specifies the horizontal resolution, in pixels-per-meter, of the target device for the bitmap.
        /// An application can use this value to select a bitmap from a resource group that best matches 
        /// the characteristics of the current device.
        /// </summary>
        public int biXPelsPerMeter;

        /// <summary>
        /// Specifies the vertical resolution, in pixels-per-meter, of the target device for the bitmap.
        /// </summary>
        public int biYPelsPerMeter;

        /// <summary>
        /// Specifies the number of color indexes in the color table that are actually used by the bitmap.
        /// If this value is zero, the bitmap uses the maximum number of colors corresponding to the value
        /// of the biBitCount member for the compression mode specified by biCompression. 
        /// If biClrUsed is nonzero and the biBitCount member is less than 16, the biClrUsed member specifies
        /// the actual number of colors the graphics engine or device driver accesses. If biBitCount is 16 or
        /// greater, the biClrUsed member specifies the size of the color table used to optimize performance 
        /// of the system color palettes. If biBitCount equals 16 or 32, the optimal color palette starts 
        /// immediately following the three DWORD masks. 
        /// When the bitmap array immediately follows the BITMAPINFO structure, it is a packed bitmap. 
        /// Packed bitmaps are referenced by a single pointer. Packed bitmaps require that the biClrUsed 
        /// member must be either zero or the actual size of the color table.
        /// </summary>
        public uint biClrUsed;

        /// <summary>
        /// Specifies the number of color indexes that are required for displaying the bitmap. If this value is zero, all colors are required.
        /// </summary>
        public uint biClrImportant;
    }
}