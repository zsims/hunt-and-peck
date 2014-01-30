using System;
using System.Drawing;

namespace HuntnPeck.Engine.Services.Interfaces
{
    /// <summary>
    /// Provides screenshots for given windows
    /// </summary>
    internal interface IScreenshotService
    {
        Bitmap RenderWindow(IntPtr hWnd);
    }
}
