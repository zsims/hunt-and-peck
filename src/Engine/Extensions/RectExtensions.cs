using Engine.NativeMethods;
using System;
using System.Drawing;
using System.Windows;

namespace Engine.Extensions
{
    public static class RectExtensions
    {
        /// <summary>
        /// Converts physical screen to logical screen coordinates given a rectangle
        /// </summary>
        /// <param name="source">The source rectangle</param>
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

        public static Rectangle ToDrawingRectangle(this Rect source)
        {
            return new Rectangle((int)source.Left, (int)source.Top, (int)source.Width, (int)source.Height);
        }
    }
}
