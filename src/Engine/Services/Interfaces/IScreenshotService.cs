using System;
using System.Drawing;

namespace Engine.Services.Interfaces
{
    /// <summary>
    /// Provides screenshots for given windows
    /// </summary>
    internal interface IScreenshotService
    {
        Bitmap RenderWindow(IntPtr hWnd);
    }
}
