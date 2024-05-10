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
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.PInvoke.Windows.Structs
{
    /// <summary>
    /// The RECT structure defines the coordinates of the upper-left and lower-right corners of a rectangle. 
    /// </summary>
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        /// <summary>
        /// Specifies the x-coordinate of the upper-left corner of the rectangle. 
        /// </summary>
        public int Left;

        /// <summary>
        /// Specifies the y-coordinate of the upper-left corner of the rectangle.
        /// </summary>
        public int Top;

        /// <summary>
        /// Specifies the x-coordinate of the lower-right corner of the rectangle. 
        /// </summary>
        public int Right;

        /// <summary>
        /// Specifies the y-coordinate of the lower-right corner of the rectangle. 
        /// </summary>
        public int Bottom;

        /// <summary>
        /// Initializes a RECT object
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        public Rect(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        /// <summary>
        /// Gets height of the rectangle
        /// </summary>
        public int Height
        {
            get { return Bottom - Top; }
        }

        /// <summary>
        /// Gets width of the rectangle
        /// </summary>
        public int Width
        {
            get { return Right - Left; }
        }

        /// <summary>
        /// Gets the size of the rectangle
        /// </summary>
        public Drawing.Size Size
        {
            get { return new Drawing.Size(Width, Height); }
        }

        /// <summary>
        /// Gets the location of the rectangle
        /// </summary>
        public Drawing.Point Location
        {
            get { return new Drawing.Point(Left, Top); }
        }

        /// <summary>
        /// Handy method for converting to a System.Drawing.Rectangle 
        /// </summary>
        /// <returns></returns>
        public Rectangle ToRectangle()
        {
            return Rectangle.FromLTRB(Left, Top, Right, Bottom);
        }

        /// <summary>
        /// Method for converting from a System.Drawing.Rectangle
        /// </summary>
        /// <param name="rectangle">System.Drawing.Rectangle object</param>
        /// <returns>RECT object</returns>
        public static Rect FromRectangle(Rectangle rectangle)
        {
            return new Rect(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
        }

        /// <summary>
        /// Gets the specific hash code for RECT object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Left ^ ((Top << 13) | (Top >> 0x13))
                   ^ ((Width << 0x1a) | (Width >> 6))
                   ^ ((Height << 7) | (Height >> 0x19));
        }

        #region Operator overloads

        /// <summary>
        /// Casts a RECT object to a System.Drawing.Rectangle object
        /// </summary>
        /// <param name="rect">RECT object to cast</param>
        /// <returns>System.Drawing.Rectangle object</returns>
        public static implicit operator Rectangle(Rect rect)
        {
            return rect.ToRectangle();
        }

        /// <summary>
        /// Casts a System.Drawing.Rectangle object to a RECT object
        /// </summary>
        /// <param name="rect">System.Drawing.Rectangle object</param>
        /// <returns>Equivalent RECT object</returns>
        public static implicit operator Rect(Rectangle rect)
        {
            return FromRectangle(rect);
        }

        #endregion
    }
}