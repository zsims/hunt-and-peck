using Engine.NativeMethods;
using Engine.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Engine.Services
{
    internal class ScreenshotService : IScreenshotService
    {
        public Bitmap RenderWindow(IntPtr hWnd)
        {
            var rect = new RECT();
            User32.GetWindowRect(hWnd, ref rect);
            var bounds = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);

            var result = new Bitmap(bounds.Width, bounds.Height);

            using (var graphics = Graphics.FromImage(result))
            {
                graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            }

            return result;
        }
    }
}
