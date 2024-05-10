using System;
using System.Runtime.InteropServices;
using System.Text;
using Cybercraft.Common.WinForms.Gdi;
// ReSharper disable IdentifierTypo

namespace Cybercraft.Common.WinForms
{
    public static class Gdi32
    {
        /// <summary>
        ///     Specifies a raster-operation code. These codes define how the color data for the
        ///     source rectangle is to be combined with the color data for the destination
        ///     rectangle to achieve the final color.
        /// </summary>
        public enum TernaryRasterOperations : uint
        {
            /// <summary>dest = source</summary>
            SRCCOPY = 0x00CC0020,
            /// <summary>dest = source OR dest</summary>
            SRCPAINT = 0x00EE0086,
            /// <summary>dest = source AND dest</summary>
            SRCAND = 0x008800C6,
            /// <summary>dest = source XOR dest</summary>
            SRCINVERT = 0x00660046,
            /// <summary>dest = source AND (NOT dest)</summary>
            SRCERASE = 0x00440328,
            /// <summary>dest = (NOT source)</summary>
            NOTSRCCOPY = 0x00330008,
            /// <summary>dest = (NOT src) AND (NOT dest)</summary>
            NOTSRCERASE = 0x001100A6,
            /// <summary>dest = (source AND pattern)</summary>
            MERGECOPY = 0x00C000CA,
            /// <summary>dest = (NOT source) OR dest</summary>
            MERGEPAINT = 0x00BB0226,
            /// <summary>dest = pattern</summary>
            PATCOPY = 0x00F00021,
            /// <summary>dest = DPSnoo</summary>
            PATPAINT = 0x00FB0A09,
            /// <summary>dest = pattern XOR dest</summary>
            PATINVERT = 0x005A0049,
            /// <summary>dest = (NOT dest)</summary>
            DSTINVERT = 0x00550009,
            /// <summary>dest = BLACK</summary>
            BLACKNESS = 0x00000042,
            /// <summary>dest = WHITE</summary>
            WHITENESS = 0x00FF0062,
            /// <summary>
            /// Capture window as seen on screen.  This includes layered windows
            /// such as WPF windows with AllowsTransparency="true"
            /// </summary>
            CAPTUREBLT = 0x40000000
        }

        public enum BackgroundMode : int
        {
            ERROR = 0,
            TRANSPARENT = 1,
            OPAQUE = 2,
        }

        [Flags]
        public enum DrawTextFormat : uint
        {
            DT_TOP = 0x00000000,
            DT_LEFT = 0x00000000,
            DT_CENTER = 0x00000001,
            DT_RIGHT = 0x00000002,
            DT_VCENTER = 0x00000004,
            DT_BOTTOM = 0x00000008,
            DT_WORDBREAK = 0x00000010,
            DT_SINGLELINE = 0x00000020,
            DT_EXPANDTABS = 0x00000040,
            DT_TABSTOP = 0x00000080,
            DT_NOCLIP = 0x00000100,
            DT_EXTERNALLEADING = 0x00000200,
            DT_CALCRECT = 0x00000400,
            DT_NOPREFIX = 0x00000800,
            DT_INTERNAL = 0x00001000,
            DT_EDITCONTROL = 0x00002000,
            DT_PATH_ELLIPSIS = 0x00004000,
            DT_END_ELLIPSIS = 0x00008000,
            DT_MODIFYSTRING = 0x00010000,
            DT_RTLREADING = 0x00020000,
            DT_WORD_ELLIPSIS = 0x00040000,
            DT_NOFULLWIDTHCHARBREAK = 0x00080000,
            DT_HIDEPREFIX = 0x00100000,
            DT_PREFIXONLY = 0x00200000,
        }

        public enum PenStyle : int
        {
            PS_SOLID = 0, //The pen is solid.
            PS_DASH = 1, //The pen is dashed.
            PS_DOT = 2, //The pen is dotted.
            PS_DASHDOT = 3, //The pen has alternating dashes and dots.
            PS_DASHDOTDOT = 4, //The pen has alternating dashes and double dots.
            PS_NULL = 5, //The pen is invisible.
            PS_INSIDEFRAME = 6,// Normally when the edge is drawn, it’s centred on the outer edge meaning that half the width of the pen is drawn
            // outside the shape’s edge, half is inside the shape’s edge. When PS_INSIDEFRAME is specified the edge is drawn
            //completely inside the outer edge of the shape.
            PS_USERSTYLE = 7,
            PS_ALTERNATE = 8,
            PS_STYLE_MASK = 0x0000000F,

            PS_ENDCAP_ROUND = 0x00000000,
            PS_ENDCAP_SQUARE = 0x00000100,
            PS_ENDCAP_FLAT = 0x00000200,
            PS_ENDCAP_MASK = 0x00000F00,

            PS_JOIN_ROUND = 0x00000000,
            PS_JOIN_BEVEL = 0x00001000,
            PS_JOIN_MITER = 0x00002000,
            PS_JOIN_MASK = 0x0000F000,

            PS_COSMETIC = 0x00000000,
            PS_GEOMETRIC = 0x00010000,
            PS_TYPE_MASK = 0x000F0000
        };

        public enum StockObject : int
        {
            ANSI_FIXED_FONT = 11,
            ANSI_VAR_FONT = 12,
            BLACK_BRUSH = 4,
            BLACK_PEN = 7,
            DEFAULT_GUI_FONT = 17,
            DEFAULT_PALETTE = 15,
            DEVICE_DEFAULT_FONT = 14,
            DKGRAY_BRUSH = 3,
            GRAY_BRUSH = 2,
            HOLLOW_BRUSH = 5,
            LTGRAY_BRUSH = 1,
            NULL_BRUSH = 5,
            NULL_PEN = 8,
            OEM_FIXED_FONT = 10,
            SYSTEM_FIXED_FONT = 16,
            SYSTEM_FONT = 13,
            WHITE_BRUSH = 0,
            WHITE_PEN = 6,
        }

        public enum DibColors : uint
        {
            DIB_RGB_COLORS = 0x00,
            DIB_PAL_COLORS = 0x01,
            DIB_PAL_INDICES = 0x02
        };

        public enum StretchBltMode : int
        {
            STRETCH_ANDSCANS = 1,
            STRETCH_ORSCANS = 2,
            STRETCH_DELETESCANS = 3,
            STRETCH_HALFTONE = 4,
        }

        public enum FontWeight : int
        {
            FW_DONTCARE = 0,
            FW_THIN = 100,
            FW_EXTRALIGHT = 200,
            FW_LIGHT = 300,
            FW_NORMAL = 400,
            FW_MEDIUM = 500,
            FW_SEMIBOLD = 600,
            FW_BOLD = 700,
            FW_EXTRABOLD = 800,
            FW_HEAVY = 900,
        }

        public enum FontCharSet : UInt32
        {
            ANSI_CHARSET = 0,
            DEFAULT_CHARSET = 1,
            SYMBOL_CHARSET = 2,
            SHIFTJIS_CHARSET = 128,
            HANGUL_CHARSET = 129,
            GB2312_CHARSET = 134,
            CHINESEBIG5_CHARSET = 136,
            OEM_CHARSET = 255,
            JOHAB_CHARSET = 130,
            HEBREW_CHARSET = 177,
            ARABIC_CHARSET = 178,
            GREEK_CHARSET = 161,
            TURKISH_CHARSET = 162,
            VIETNAMESE_CHARSET = 163,
            THAI_CHARSET = 222,
            EASTEUROPE_CHARSET = 238,
            RUSSIAN_CHARSET = 204,
            MAC_CHARSET = 77,
            BALTIC_CHARSET = 186,
        }

        public enum FontPrecision : UInt32
        {
            OUT_DEFAULT_PRECIS = 0,
            OUT_STRING_PRECIS = 1,
            OUT_CHARACTER_PRECIS = 2,
            OUT_STROKE_PRECIS = 3,
            OUT_TT_PRECIS = 4,
            OUT_DEVICE_PRECIS = 5,
            OUT_RASTER_PRECIS = 6,
            OUT_TT_ONLY_PRECIS = 7,
            OUT_OUTLINE_PRECIS = 8,
            OUT_SCREEN_OUTLINE_PRECIS = 9,
            OUT_PS_ONLY_PRECIS = 10,
        }

        public enum FontClipPrecision : UInt32
        {
            CLIP_DEFAULT_PRECIS = 0,
            CLIP_CHARACTER_PRECIS = 1,
            CLIP_STROKE_PRECIS = 2,
            CLIP_MASK = 0xf,
            CLIP_LH_ANGLES = (1 << 4),
            CLIP_TT_ALWAYS = (2 << 4),
            CLIP_DFA_DISABLE = (4 << 4),
            CLIP_EMBEDDED = (8 << 4),
        }

        public enum FontQuality : UInt32
        {
            DEFAULT_QUALITY = 0,
            DRAFT_QUALITY = 1,
            PROOF_QUALITY = 2,
            NONANTIALIASED_QUALITY = 3,
            ANTIALIASED_QUALITY = 4,
            CLEARTYPE_QUALITY = 5,
            CLEARTYPE_NATURAL_QUALITY = 6,
        }

        [Flags]
        public enum FontPitchAndFamily : UInt32
        {
            DEFAULT_PITCH = 0,
            FIXED_PITCH = 1,
            VARIABLE_PITCH = 2,
            // FF_DONTCARE = (0 << 4),
            FF_ROMAN = (1 << 4),
            FF_SWISS = (2 << 4),
            FF_MODERN = (3 << 4),
            FF_SCRIPT = (4 << 4),
            FF_DECORATIVE = (5 << 4),
        }

        public static uint RGB(int r, int g, int b)
        {
            return ((uint)(((byte)(r) | ((uint)((byte)(g)) << 8)) | (((uint)(byte)(b)) << 16)));
        }


        /// <summary>
        ///        Creates a memory device context (DC) compatible with the specified device.
        /// </summary>
        /// <param name="hdc">A handle to an existing DC. If this handle is NULL,
        ///        the function creates a memory DC compatible with the application's current screen.</param>
        /// <returns>
        ///        If the function succeeds, the return value is the handle to a memory DC.
        ///        If the function fails, the return value is <see cref="System.IntPtr.Zero"/>.
        /// </returns>
        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC", SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC([In] IntPtr hdc);


        /// <summary>
        ///        Creates a bitmap compatible with the device that is associated with the specified device context.
        /// </summary>
        /// <param name="hdc">A handle to a device context.</param>
        /// <param name="nWidth">The bitmap width, in pixels.</param>
        /// <param name="nHeight">The bitmap height, in pixels.</param>
        /// <returns>If the function succeeds, the return value is a handle to the compatible bitmap (DDB). If the function fails, the return value is <see cref="System.IntPtr.Zero"/>.</returns>
        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
        public static extern IntPtr CreateCompatibleBitmap([In] IntPtr hdc, int nWidth, int nHeight);

        /// <summary>Deletes the specified device context (DC).</summary>
        /// <param name="hdc">A handle to the device context.</param>
        /// <returns><para>If the function succeeds, the return value is nonzero.</para><para>If the function fails, the return value is zero.</para></returns>
        /// <remarks>An application must not delete a DC whose handle was obtained by calling the <c>GetDC</c> function. Instead, it must call the <c>ReleaseDC</c> function to free the DC.</remarks>
        [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
        public static extern bool DeleteDC([In] IntPtr hdc);

        /// <summary>Selects an object into the specified device context (DC). The new object replaces the previous object of the same type.</summary>
        /// <param name="hdc">A handle to the DC.</param>
        /// <param name="hgdiobj">A handle to the object to be selected.</param>
        /// <returns>
        ///   <para>If the selected object is not a region and the function succeeds, the return value is a handle to the object being replaced. If the selected object is a region and the function succeeds, the return value is one of the following values.</para>
        ///   <para>SIMPLEREGION - Region consists of a single rectangle.</para>
        ///   <para>COMPLEXREGION - Region consists of more than one rectangle.</para>
        ///   <para>NULLREGION - Region is empty.</para>
        ///   <para>If an error occurs and the selected object is not a region, the return value is <c>NULL</c>. Otherwise, it is <c>HGDI_ERROR</c>.</para>
        /// </returns>
        /// <remarks>
        ///   <para>This function returns the previously selected object of the specified type. An application should always replace a new object with the original, default object after it has finished drawing with the new object.</para>
        ///   <para>An application cannot select a single bitmap into more than one DC at a time.</para>
        ///   <para>ICM: If the object being selected is a brush or a pen, color management is performed.</para>
        /// </remarks>
        [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
        public static extern IntPtr SelectObject([In] IntPtr hdc, [In] IntPtr hgdiobj);


        /// <summary>Deletes a logical pen, brush, font, bitmap, region, or palette, freeing all system resources associated with the object. After the object is deleted, the specified handle is no longer valid.</summary>
        /// <param name="hObject">A handle to a logical pen, brush, font, bitmap, region, or palette.</param>
        /// <returns>
        ///   <para>If the function succeeds, the return value is nonzero.</para>
        ///   <para>If the specified handle is not valid or is currently selected into a DC, the return value is zero.</para>
        /// </returns>
        /// <remarks>
        ///   <para>Do not delete a drawing object (pen or brush) while it is still selected into a DC.</para>
        ///   <para>When a pattern brush is deleted, the bitmap associated with the brush is not deleted. The bitmap must be deleted independently.</para>
        /// </remarks>
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);


        /// <summary>
        ///    Performs a bit-block transfer of the color data corresponding to a
        ///    rectangle of pixels from the specified source device context into
        ///    a destination device context.
        /// </summary>
        /// <param name="hdc">Handle to the destination device context.</param>
        /// <param name="nXDest">The leftmost x-coordinate of the destination rectangle (in pixels).</param>
        /// <param name="nYDest">The topmost y-coordinate of the destination rectangle (in pixels).</param>
        /// <param name="nWidth">The width of the source and destination rectangles (in pixels).</param>
        /// <param name="nHeight">The height of the source and the destination rectangles (in pixels).</param>
        /// <param name="hdcSrc">Handle to the source device context.</param>
        /// <param name="nXSrc">The leftmost x-coordinate of the source rectangle (in pixels).</param>
        /// <param name="nYSrc">The topmost y-coordinate of the source rectangle (in pixels).</param>
        /// <param name="dwRop">A raster-operation code.</param>
        /// <returns>
        ///    <c>true</c> if the operation succeedes, <c>false</c> otherwise. To get extended error information, call <see cref="System.Runtime.InteropServices.Marshal.GetLastWin32Error"/>.
        /// </returns>
        [DllImport("gdi32.dll", EntryPoint = "BitBlt", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BitBlt([In] IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, [In] IntPtr hdcSrc, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

        [DllImport("gdi32.dll")]
        public static extern bool StretchBlt([In] IntPtr hdcDest, int nXDest, int nYDest, int nWidthDest, int nHeightDest, [In] IntPtr hdcSrc, int nXSrc, int nYSrc, int nWidthSrc, int nHeightSrc, TernaryRasterOperations dwRop);

        [DllImport("gdi32.dll")]
        public static extern StretchBltMode SetStretchBltMode(IntPtr hdc, StretchBltMode mode);

        [DllImport("gdi32.dll")]
        public static extern int StretchDIBits([In] IntPtr hdc, int xDest, int yDest, int nDestWidth, int nDestHeight, int xSrc, int ySrc, int nSrcWidth, int nSrcHeight, byte[] lpBits, [In] ref BITMAPINFO lpBitsInfo, DibColors iUsage, TernaryRasterOperations dwRop);

/*        [DllImport("gdi32.dll")]
        public static extern int StretchDIBits(IntPtr hdc, int xDest, int yDest, int nDestWidth, int nDestHeight, int xSrc, int ySrc, int nSrcWidth, int nSrcHeight, IntPtr lpBits, [In] ref BITMAPINFO lpBitsInfo, DibColors iUsage, TernaryRasterOperations dwRop);*/

        [DllImport("gdi32.dll")]
        public static extern IntPtr GetStockObject(StockObject i);

        [DllImport("gdi32.dll")]
        public static extern int GetObject([In] IntPtr hGdiObj, int cbBuffer, out BITMAP lpvObject);


        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateSolidBrush(uint crColor);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreatePen(PenStyle fnPenStyle, int nWidth, uint crColor);


        [DllImport("gdi32.dll")]
        public static extern bool Rectangle([In] IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        [DllImport("gdi32.dll")]
        public static extern bool RoundRect([In] IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int cornerWidth, int cornerHeight);

        [DllImport("user32.dll")]
        public static extern bool FillRect([In] IntPtr hDC, [In] ref RECT lprc, IntPtr hbr);

        [DllImport("gdi32.dll", ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        public static extern bool Ellipse(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);


        [DllImport("gdi32.dll")]
        public static extern bool MoveToEx([In] IntPtr hdc, int x, int y, IntPtr lpPoint);

        [DllImport("gdi32.dll")]
        public static extern bool LineTo([In] IntPtr hdc, int nXEnd, int nYEnd);

        [DllImport("gdi32.dll")]
        public static extern bool PolyBezier([In] IntPtr hdc, [In] POINT[] apt, uint cpt);

        [DllImport("gdi32.dll")]
        public static extern bool Polygon([In] IntPtr hdc, [In] POINT[] apt, uint cpt);

        [DllImport("gdi32.dll")]
        public static extern uint SetTextColor([In] IntPtr hdc, uint crColor);

        [DllImport("gdi32.dll")]
        public static extern int SetBkMode([In] IntPtr hdc, int iBkMode);
        
        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern bool TextOut([In] IntPtr hdc, int nXStart, int nYStart, string lpString, int cbString);

        [DllImport("user32.dll")]
        public static extern int DrawText([In] IntPtr hDC, string lpString, int nCount, ref RECT lpRect, uint uFormat);

        [DllImport("gdi32", EntryPoint = "CreateFontW", CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateFont([In] Int32 nHeight, [In] Int32 nWidth, [In] Int32 nEscapement, [In] Int32 nOrientation, [In] FontWeight fnWeight, [In] UInt32 fdwItalic, [In] UInt32 fdwUnderline, [In] UInt32 fdwStrikeOut, [In] FontCharSet fdwCharSet, [In] FontPrecision fdwOutputPrecision, [In] FontClipPrecision fdwClipPrecision, [In] FontQuality fdwQuality, [In] FontPitchAndFamily fdwPitchAndFamily, [In] string lpszFace);
    }
}
