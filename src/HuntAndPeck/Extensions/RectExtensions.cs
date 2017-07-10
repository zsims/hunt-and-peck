using System;
using System.Windows;
using HuntAndPeck.NativeMethods;

namespace HuntAndPeck.Extensions
{
    public static class RectExtensions
    {
        /// <summary>
        /// Converts physical screen to logical screen coordinates given a rectangle
        /// </summary>
        /// <param name="source">The source rectangle</param>
        /// <param name="hWnd">The window handle to use for conversion</param>
        /// <returns>The rectangle in logical coordinates, else an empty rectangle</returns>
        public static Rect PhysicalToLogicalRect(this Rect source, IntPtr hWnd)
        {
            POINT tl = source.TopLeft;
            POINT br = source.BottomRight;
            if (User32.PhysicalToLogicalPoint(hWnd, out tl) &&
                User32.PhysicalToLogicalPoint(hWnd, out br))
            {
                return new Rect(tl, br);
            }

            return Rect.Empty;
        }

        /// <summary>
        /// Converts logical screen coordinates to window coordinates for a given window
        /// </summary>
        /// <param name="source">The logical screen coordinates</param>
        /// <param name="windowRect">The bounds of the window in which the source rect lies</param>
        /// <returns></returns>
        public static Rect ScreenToWindowCoordinates(this Rect source, Rect windowRect)
        {
            var result = new Rect(source.TopLeft, source.BottomRight);
            result.X -= windowRect.X;
            result.Y -= windowRect.Y;

            return result;
        }
    }
}
