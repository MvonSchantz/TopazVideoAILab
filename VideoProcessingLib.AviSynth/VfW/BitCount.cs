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
    /// Number of bits-per-pixel 
    /// </summary>
    [Flags]
    public enum BitCount : ushort
    {
        /// <summary>
        /// Windows 98/Me, Windows 2000/XP: The number of bits-per-pixel is specified or is 
        /// implied by the JPEG or PNG format. 
        /// </summary>
        BitPerPixel0BPP = 0,
        /// <summary>
        /// The bitmap is monochrome, and the bmiColors member of BITMAPINFO contains two entries.
        /// Each bit in the bitmap array represents a pixel. If the bit is clear, the pixel is 
        /// displayed with the color of the first entry in the bmiColors table; if the bit is set,
        /// the pixel has the color of the second entry in the table.
        /// </summary>
        BitPerPixel1BPP = 1,
        /// <summary>
        /// The bitmap has a maximum of 16 colors, and the bmiColors member of BITMAPINFO contains
        /// up to 16 entries. Each pixel in the bitmap is represented by a 4-bit index into the 
        /// color table. For example, if the first byte in the bitmap is 0x1F, the byte represents 
        /// two pixels. The first pixel contains the color in the second table entry, and the second 
        /// pixel contains the color in the sixteenth table entry.
        /// </summary>
        BitPerPixel4BPP = 4,
        /// <summary>
        /// The bitmap has a maximum of 256 colors, and the bmiColors member of BITMAPINFO contains up 
        /// to 256 entries. In this case, each byte in the array represents a single pixel.
        /// </summary>
        BitPerPixel8BPP = 8,
        /// <summary>
        /// The bitmap has a maximum of 2^16 colors. If the biCompression member of the BITMAPINFOHEADER is BI_RGB,
        /// the bmiColors member of BITMAPINFO is null. Each WORD in the bitmap array represents a single pixel. 
        /// The relative intensities of red, green, and blue are represented with five bits for each color component.
        /// The value for blue is in the least significant five bits, followed by five bits each for green and red.
        /// The most significant bit is not used. The bmiColors color table is used for optimizing colors used on 
        /// palette-based devices, and must contain the number of entries specified by the biClrUsed member of the 
        /// BITMAPINFOHEADER. 
        /// If the biCompression member of the BITMAPINFOHEADER is BitmapCompression.BI_BITFIELDS, the bmiColors member contains 
        /// three DWORD color masks that specify the red, green, and blue components, respectively, of each pixel.
        /// Each WORD in the bitmap array represents a single pixel.
        /// Windows NT/Windows 2000/XP: When the biCompression member is BitmapCompression.BI_BITFIELDS, bits set in each 
        /// DWORD mask must be contiguous and should not overlap the bits of another mask. All the bits in 
        /// the pixel do not have to be used. 
        /// Windows 95/98/Me: When the biCompression member is BitmapCompression.BI_BITFIELDS, the system supports only the following
        /// 16bpp color masks: A 5-5-5 16-bit image, where the blue mask is 0x001F, the green mask is 0x03E0, and 
        /// the red mask is 0x7C00; and a 5-6-5 16-bit image, where the blue mask is 0x001F, the green mask 
        /// is 0x07E0, and the red mask is 0xF800.
        /// </summary>
        BitPerPixel16BPP = 16,
        /// <summary>
        /// The bitmap has a maximum of 2^24 colors, and the bmiColors member of BITMAPINFO is null. 
        /// Each 3-byte triplet in the bitmap array represents the relative intensities of blue, green, and red, 
        /// respectively, for a pixel. The bmiColors color table is used for optimizing colors used on palette-based
        /// devices, and must contain the number of entries specified by the biClrUsed member of the BITMAPINFOHEADER. 
        /// </summary>
        BitPerPixel24BPP = 24,
        /// <summary>
        /// The bitmap has a maximum of 2^32 colors. If the biCompression member of the BITMAPINFOHEADER is BI_RGB,
        /// the bmiColors member of BITMAPINFO is null. Each DWORD in the bitmap array represents the relative 
        /// intensities of blue, green, and red, respectively, for a pixel. The high byte in each DWORD is not used.
        /// The bmiColors color table is used for optimizing colors used on palette-based devices, and must contain 
        /// the number of entries specified by the biClrUsed member of the BITMAPINFOHEADER. 
        /// If the biCompression member of the BITMAPINFOHEADER is BitmapCompression.BI_BITFIELDS, the bmiColors member contains three
        /// DWORD color masks that specify the red, green, and blue components, respectively, of each pixel. Each 
        /// DWORD in the bitmap array represents a single pixel.
        /// Windows NT/ 2000: When the biCompression member is BitmapCompression.BI_BITFIELDS, bits set in each DWORD mask must be 
        /// contiguous and should not overlap the bits of another mask. All the bits in the pixel do not need to be
        /// used.
        /// Windows 95/98/Me: When the biCompression member is BitmapCompression.BI_BITFIELDS, the system supports only the following
        /// 32-bpp color mask: The blue mask is 0x000000FF, the green mask is 0x0000FF00, and the red mask is 
        /// 0x00FF0000.
        /// </summary>
        BitPerPixel32BPP = 32
    }
}