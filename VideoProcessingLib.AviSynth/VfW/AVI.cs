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
using System.PInvoke.Windows.Structs;
using System.Runtime.InteropServices;

namespace System.PInvoke.Windows
{
    /// <summary>
    /// Contains PInvoke signature of handling AVI files
    /// </summary>
    public class AVI
    {
        /// <summary>
        /// Use the best format for the display
        /// </summary>
        public const int AVIGETFRAMEF_BESTDISPLAYFMT = 1;


        /// <summary>
        /// The AVIFileInit function initializes the AVIFile library. 
        ///
        /// The AVIFile library maintains a count of the number of times
        /// it is initialized, but not the number of times it was released. 
        /// Use the AVIFileExit function to release the AVIFile library and decrement 
        /// the reference count. Call AVIFileInit before using any other AVIFile functions.
        ///
        /// This function supersedes the obsolete AVIStreamInit function.
        /// </summary>
        public static void AVIFileInit()
        {
            Native.AVIFileInit();
        }

        /// <summary>
        /// The AVIFileExit function exits the AVIFile library and decrements the reference count for the library.
        /// This function supersedes the obsolete AVIStreamExit function.
        /// </summary>
        public static void AVIFileExit()
        {
            Native.AVIFileExit();
        }

        /// <summary>
        /// The AVIFileCreateStream function creates a new stream in an existing
        /// file and creates an interface to the new stream. 
        /// </summary>
        /// <param name="pfile">Handle to an open AVI file.</param>
        /// <param name="ppavi">Pointer to the new stream interface.</param>
        /// <param name="streaminfo">Pointer to a structure containing information
        /// about the new stream, including the stream type and its sample rate.</param>
        /// <returns>Returns zero if successful or an error otherwise.
        /// Unless the file has been opened with write permission, this function returns AVIERR_READONLY.</returns>
        /// <remarks>
        /// This function starts a reference count for the new stream.
        /// </remarks>
        public static AviError AVIFileCreateStream(int pfile, out IntPtr ppavi, ref AVIStreamInfo streaminfo)
        {
            return Native.AVIFileCreateStream(pfile, out ppavi, ref streaminfo);
        }

        /// <summary>
        /// The AVIFileOpen function opens an AVI file and returns the address of a file interface
        /// used to access it. The AVIFile library maintains a count of the number of times a file
        /// is opened, but not the number of times it was released. Use the AVIFileRelease function
        /// to release the file and decrement the count.
        /// </summary>
        /// <param name="ppfile">Pointer to a buffer that receives the new IAVIFile interface pointer.</param>
        /// <param name="fileName">null-terminated string containing the name of the file to open.</param>
        /// <param name="mode">Access mode to use when opening the file. The default access mode is OF_READ</param>
        /// <param name="pclsidHandler">Pointer to a class identifier of the standard or custom handler you want to use.
        /// If the value is null, the system chooses a handler from the registry based on the file extension or 
        /// the RIFF type specified in the file. 
        ///</param>
        /// <returns>Returns zero if successful or an error otherwise.</returns>
        public static AviError AVIFileOpenW(ref IntPtr ppfile, string fileName, AccessMode mode, IntPtr pclsidHandler)
        {
            return Native.AVIFileOpenW(ref ppfile, fileName, mode, pclsidHandler);
        }

        /// <summary>
        /// The AVIStreamOpenFromFile function opens a single stream from a file.
        /// </summary>
        /// <param name="ppavi">Pointer to a buffer that receives the new stream handle.</param>
        /// <param name="szfile">null-terminated string containing the name of the file to open.</param>
        /// <param name="fccType">Type of stream to be opened. 
        /// Zero indicates that any stream can be opened. The following definitions apply to the data 
        /// commonly found in AVI streams:
        /// 
        /// StreamType.AUDIO
        /// StreamType.MIDI
        /// StreamType.TEXT
        /// StreamType.VIDEO
        /// </param>
        /// <param name="lParam">Stream of the type specified in fccType to access. This parameter is zero-based;
        /// use zero to specify the first occurrence.
        /// </param>
        /// <param name="mode">Access mode to use when opening the file. This function can open only existing
        /// streams, so the OF_CREATE mode flag cannot be used. For more information about the available flags
        /// for the mode parameter, see the OpenFile function.</param>
        /// <param name="dummy">Pointer to a class identifier of the handler you want to use. 
        /// If the value is null, the system chooses one from the registry based on the file 
        /// extension or the file RIFF type.</param>
        /// <returns>Returns zero if successful or an error otherwise. </returns>
        public static AviError AVIStreamOpenFromFile(out IntPtr ppavi, [MarshalAs(UnmanagedType.LPWStr)] string szfile,
                                                     StreamType fccType, int lParam, AccessMode mode, int dummy)
        {
            if (Environment.OSVersion.Platform >= PlatformID.Win32NT)
                return Native.AVIStreamOpenFromFileW(out ppavi, szfile, fccType, lParam, mode, dummy);
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// The AVIMakeCompressedStream function creates a compressed stream from an uncompressed stream
        /// and a compression filter, and returns the address of a pointer to the compressed stream. 
        /// This function supports audio and video compression.
        /// </summary>
        /// <param name="ppsCompressed">Pointer to a buffer that receives the compressed stream pointer.</param>
        /// <param name="aviStream">Pointer to the stream to be compressed.</param>
        /// <param name="ao">Pointer to a structure that identifies the type of compression to use and the 
        /// options to apply. You can specify video compression by identifying an appropriate handler in 
        /// the AVICOMPRESSOPTIONS structure. For audio compression, specify the compressed data format.</param>
        /// <param name="dummy">Pointer to a class identifier used to create the stream.</param>
        /// <returns>Returns OK if successful or an error otherwise. Possible error values include the following.
        /// NOCOMPRESSOR
        /// MEMORY
        /// UNSUPPORTED
        /// </returns>
        public static AviError AVIMakeCompressedStream(out IntPtr ppsCompressed, IntPtr aviStream,
                                                       ref AVICompressOptions ao, int dummy)
        {
            return Native.AVIMakeCompressedStream(out ppsCompressed, aviStream, ref ao, dummy);
        }

        /// <summary>
        /// The AVIStreamSetFormat function sets the format of a stream at the specified position.
        /// </summary>
        /// <param name="aviStream">Handle to an open stream.</param>
        /// <param name="lPos">Position in the stream to receive the format.</param>
        /// <param name="lpFormat">Pointer to a structure containing the new format.</param>
        /// <param name="cbFormat">Size, in bytes, of the block of memory referenced by lpFormat.
        /// </param>
        /// <returns>Returns zero if successful or an error otherwise. </returns>
        public static AviError AVIStreamSetFormat(IntPtr aviStream, Int32 lPos, ref BitmapInfoHeader lpFormat,
                                                  Int32 cbFormat)
        {
            return Native.AVIStreamSetFormat(aviStream, lPos, ref lpFormat, cbFormat);
        }

        public static AviError AVIStreamReadFormat(IntPtr aviStream, Int32 lPos, out BitmapInfoHeader lpFormat, out Int32 cbFormat)
        {
            return Native.AVIStreamReadFormat(aviStream, lPos, out lpFormat, out cbFormat);
        }

        /// <summary>
        /// The AVISaveOptions function retrieves the save options for a file and returns them in a buffer.
        /// </summary>
        /// <param name="hwnd">Handle to the parent window for the Compression Options dialog box.</param>
        /// <param name="flags">Flags for displaying the Compression Options dialog box.</param>
        /// <param name="nStreams">Number of streams that have their options set by the dialog box.</param>
        /// <param name="ppavi">Pointer to an array of stream interface pointers. The nStreams parameter indicates the number of pointers in the array.></param>
        /// <param name="ao">Pointer to an array of pointers to AVICOMPRESSOPTIONS structures. These structures hold the compression options set by the dialog box. The nStreams parameter indicates the number of pointers in the array.</param>
        /// <returns>Returns true if the user pressed OK, false for CANCEL, or an error otherwise.</returns>
        public static unsafe bool AVISaveOptions(IntPtr hwnd, CompressionDialog flags, int nStreams, IntPtr* ppavi,
                                                 AVICompressOptions** ao)
        {
            return Native.AVISaveOptions(hwnd, flags, nStreams, ppavi, ao);
        }

        public static int AVIStreamRead(IntPtr aviStream, int lStart, int lSamples, out Int32 plBytes, out Int32 plSamples)
        {
            return Native.AVIStreamRead(aviStream, lStart, lSamples, IntPtr.Zero, 0, out plBytes, out plSamples);
        }

        public static int AVIStreamRead(IntPtr aviStream, int lStart, int lSamples, IntPtr lpBuffer, Int32 cbBuffer)
        {
            return Native.AVIStreamRead(aviStream, lStart, lSamples, lpBuffer, cbBuffer, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// The AVIStreamWrite function writes data to a stream.
        /// </summary>
        /// <param name="aviStream">Handle to an open stream.</param>
        /// <param name="lStart">First sample to write.</param>
        /// <param name="lSamples">Number of samples to write.</param>
        /// <param name="lpBuffer">Pointer to a buffer containing the data to write.</param>
        /// <param name="cbBuffer">Size of the buffer referenced by lpBuffer.</param>
        /// <param name="dwFlags"></param>
        /// <param name="dummy1">Pointer to a buffer that receives the number of samples written. This can be set to null.</param>
        /// <param name="dummy2">Pointer to a buffer that receives the number of bytes written. This can be set to null.</param>
        /// <returns>Returns zero if successful or an error otherwise. </returns>
        /// <remarks>The default AVI file handler supports writing only at the end of a stream. 
        /// The "WAVE" file handler supports writing anywhere. This function overwrites existing data, 
        /// rather than inserting new data. The argument pavi is a pointer to an IAVIStream interface.</remarks>
        public static int AVIStreamWrite(IntPtr aviStream, Int32 lStart, Int32 lSamples, IntPtr lpBuffer, Int32 cbBuffer,
                                         AviIndexEntry dwFlags, Int32 dummy1, Int32 dummy2)
        {
            return Native.AVIStreamWrite(aviStream, lStart, lSamples, lpBuffer, cbBuffer, dwFlags, dummy1, dummy2);
        }

        /// <summary>
        /// The AVIStreamRelease function decrements the reference count of an AVI stream interface handle, 
        /// and closes the stream if the count reaches zero. This function supersedes the obsolete AVIStreamClose function.
        /// </summary>
        /// <param name="aviStream">Handle to an open stream.</param>
        /// <returns>Returns the current reference count of the stream. This value should be used only for debugging purposes.
        /// The argument aviStream is a pointer to an IAVIStream interface.</returns>
        public static int AVIStreamRelease(IntPtr aviStream)
        {
            return Native.AVIStreamRelease(aviStream);
        }

        /// <summary>
        /// The AVIFileRelease function decrements the reference count of an AVI file interface handle 
        /// and closes the file if the count reaches zero. This function supersedes the obsolete AVIFileClose function.
        /// </summary>
        /// <param name="pfile">Handle to an open AVI file (pointer to an IAVIFile interface).</param>
        /// <returns>Returns the reference count of the file. This return value should be used only for debugging purposes.</returns>
        public static int AVIFileRelease(IntPtr pfile)
        {
            return Native.AVIFileRelease(pfile);
        }

        /// <summary>
        /// The AVIStreamGetFrameOpen function prepares to decompress video frames from the specified video stream.
        /// </summary>
        /// <param name="pAviStream">Pointer to the video stream used as the video source (pointer to an IAVIStream interface).</param>
        /// <param name="bih">Pointer to a structure that defines the desired video format. Specify null to use a 
        /// default format. You can also specify AVIGETFRAMEF_BESTDISPLAYFMT to decode the frames to the best format for your display.</param>
        /// <returns>Returns a GetFrame object that can be used with the AVIStreamGetFrame function. If the system cannot find
        /// a decompressor that can decompress the stream to the given format, or to any RGB format, the function returns null.</returns>
        public static IntPtr AVIStreamGetFrameOpen(IntPtr pAviStream, ref BitmapInfoHeader bih)
        {
            return Native.AVIStreamGetFrameOpen(pAviStream, ref bih);
        }

        public static IntPtr AVIStreamGetFrameOpen(IntPtr pAviStream)
        {
            return Native.AVIStreamGetFrameOpen(pAviStream, IntPtr.Zero);
        }

        /// <summary>
        /// The AVIFileGetStream function returns the address of a stream interface that is associated with a specified AVI file.
        /// </summary>
        /// <param name="pfile">Handle to an open AVI file.</param>
        /// <param name="ppavi">Pointer to the new stream interface.</param>
        /// <param name="fccType">Type of stream to open. Zero indicates any stream can be opened.</param>
        /// <param name="lParam">Count of the stream type. Identifies which occurrence of the specified stream type to access. </param>
        /// <returns>Returns zero if successful or an error otherwise. Possible error values include the following.
        /// AVIERR_NODATA
        /// AVIERR_MEMORY
        /// </returns>
        public static AviError AVIFileGetStream(IntPtr pfile, out IntPtr ppavi, StreamType fccType, int lParam)
        {
            return Native.AVIFileGetStream(pfile, out ppavi, fccType, lParam);
        }

        /// <summary>
        /// The AVIStreamStart function returns the starting sample number for the stream.
        /// </summary>
        /// <param name="pavi">Handle to an open stream.</param>
        /// <returns>Returns the number if successful or - 1 otherwise.</returns>
        public static int AVIStreamStart(int pavi)
        {
            return Native.AVIStreamStart(pavi);
        }

        /// <summary>
        /// The AVIStreamLength function returns the length of the stream.
        /// </summary>
        /// <param name="pavi">Handle to an open stream (pointer to an IAVIStream interface).</param>
        /// <returns>Returns the stream's length, in samples, if successful or -1 otherwise.</returns>
        public static int AVIStreamLength(int pavi)
        {
            return Native.AVIStreamLength(pavi);
        }

        /// <summary>
        /// The AVIStreamInfo function obtains stream header information.
        /// </summary>
        /// <param name="pAVIStream">Handle to an open stream (pointer to an IAVIStream interface).</param>
        /// <param name="psi">Pointer to a structure to contain the stream information</param>
        /// <param name="lSize">Size, in bytes, of the structure used for psi</param>
        /// <returns>Returns zero if successful or an error otherwise</returns>
        public static AviError AVIStreamInfo(IntPtr pAVIStream, ref AVIStreamInfo psi, int lSize)
        {
            return (AviError)Native.AVIStreamInfo(pAVIStream, ref psi, lSize);
        }

        /// <summary>
        /// The AVIStreamGetFrame function returns the address of a decompressed video frame
        /// </summary>
        /// <param name="pGetFrameObj">Pointer to a GetFrame object</param>
        /// <param name="lPos">Position, in samples, within the stream of the desired frame</param>
        /// <returns>Returns a pointer to the frame data if successful or null otherwise. The frame data is returned as a packed DIB</returns>
        /// <remarks>The returned frame is valid only until the next call to this function or the AVIStreamGetFrameClose function</remarks>
        public static IntPtr AVIStreamGetFrame(IntPtr pGetFrameObj, int lPos)
        {
            return Native.AVIStreamGetFrame(pGetFrameObj, lPos);
        }

        /// <summary>
        /// The AVIStreamGetFrameClose function releases resources used to decompress video frames
        /// </summary>
        /// <param name="pGetFrameObj">Handle returned from the AVIStreamGetFrameOpen function. After calling this function, the handle is invalid</param>
        /// <returns>Returns zero if successful or an error otherwise</returns>
        public static int AVIStreamGetFrameClose(IntPtr pGetFrameObj)
        {
            return Native.AVIStreamGetFrameClose(pGetFrameObj);
        }

        #region Nested type: Native

        internal static class Native
        {
            [DllImport("avifil32.dll", SetLastError = true)]
            internal static extern void AVIFileInit();

            [DllImport("avifil32.dll", SetLastError = true)]
            internal static extern void AVIFileExit();

            [DllImport("avifil32.dll", SetLastError = true)]
            internal static extern AviError AVIFileOpenW(ref IntPtr pfile,
                                                         [MarshalAs(UnmanagedType.LPWStr)] string fileName,
                                                         AccessMode mode, IntPtr pclsidHandler);

            [DllImport("avifil32.dll", SetLastError = true)]
            internal static extern AviError AVIStreamSetFormat(IntPtr aviStream, Int32 lPos,
                                                               ref BitmapInfoHeader lpFormat, Int32 cbFormat);

            [DllImport("avifil32.dll", SetLastError = true)]
            internal static extern AviError AVIStreamReadFormat(IntPtr aviStream, int lPos, out BitmapInfoHeader lpFormat, out int lpcbFormat);

            [DllImport("avifil32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern unsafe bool AVISaveOptions(IntPtr hwnd, CompressionDialog flags, int nStreams,
                                                              IntPtr* ppavi, AVICompressOptions** ao);


            [DllImport("avifil32.dll", SetLastError = true)]
            internal static extern int AVIStreamRead(IntPtr aviStream, int lStart, int lSamples, IntPtr lpBuffer, Int32 cbBuffer, out Int32 plBytes, out Int32 plSamples);

            [DllImport("avifil32.dll", SetLastError = true)]
            internal static extern int AVIStreamRead(IntPtr aviStream, int lStart, int lSamples, IntPtr lpBuffer, Int32 cbBuffer, IntPtr plBytes, IntPtr plSamples);

            [DllImport("avifil32.dll", SetLastError = true)]
            internal static extern int AVIStreamWrite(IntPtr aviStream, Int32 lStart, Int32 lSamples, IntPtr lpBuffer,
                                                      Int32 cbBuffer, AviIndexEntry dwFlags, Int32 dummy1, Int32 dummy2);

            [DllImport("avifil32.dll", SetLastError = true)]
            internal static extern int AVIStreamRelease(IntPtr aviStream);

            [DllImport("avifil32.dll", SetLastError = true)]
            internal static extern int AVIFileRelease(IntPtr pfile);

            [DllImport("avifil32.dll", SetLastError = true)]
            internal static extern IntPtr AVIStreamGetFrameOpen(IntPtr pAVIStream, ref BitmapInfoHeader bih);

            [DllImport("avifil32.dll", SetLastError = true)]
            internal static extern IntPtr AVIStreamGetFrameOpen(IntPtr pAVIStream, IntPtr bih);

            [DllImport("avifil32.dll", PreserveSig = true, SetLastError = true)]
            internal static extern int AVIStreamStart(int pavi);

            [DllImport("avifil32.dll", PreserveSig = true, SetLastError = true)]
            internal static extern int AVIStreamLength(int pavi);

            [DllImport("avifil32.dll", SetLastError = true)]
            internal static extern int AVIStreamInfo(IntPtr pAVIStream, ref AVIStreamInfo psi, int lSize);

            [DllImport("avifil32.dll", SetLastError = true)]
            internal static extern IntPtr AVIStreamGetFrame(IntPtr pGetFrameObj, int lPos);

            [DllImport("avifil32.dll", SetLastError = true)]
            internal static extern int AVIStreamGetFrameClose(IntPtr pGetFrameObj);

            [DllImport("avifil32.dll", SetLastError = true)]
            internal static extern AviError AVIFileCreateStream(int pfile, out IntPtr ppavi,
                                                                ref AVIStreamInfo streaminfo);

            [DllImport("avifil32.dll", SetLastError = true)]
            internal static extern AviError AVIStreamOpenFromFileW(out IntPtr ppavi,
                                                                   [MarshalAs(UnmanagedType.LPWStr)] string szfile,
                                                                   StreamType fccType, int lParam, AccessMode mode,
                                                                   int dummy);

            [DllImport("avifil32.dll", SetLastError = true)]
            public static extern AviError AVIMakeCompressedStream(out IntPtr ppsCompressed, IntPtr aviStream,
                                                                  ref AVICompressOptions ao, int dummy);

            [DllImport("avifil32.dll", SetLastError = true)]
            public static extern AviError AVIFileGetStream(IntPtr pfile, out IntPtr ppavi, StreamType fccType, int lParam);
        }

        #endregion
    }
}