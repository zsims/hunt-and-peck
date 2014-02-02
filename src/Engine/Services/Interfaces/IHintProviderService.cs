using HuntAndPeck.Engine.Hints;
using System;
using System.Collections.Generic;

namespace HuntAndPeck.Engine.Services.Interfaces
{
    /// <summary>
    /// Provides hints for the entire desktop or a given window handle
    /// </summary>
    public interface IHintProviderService
    {
        /// <summary>
        /// Enumerate the available hints for the current foreground window
        /// </summary>
        /// <returns>The hint session containing the available hints</returns>
        HintSession EnumHints();

        /// <summary>
        /// Enumerate the available hints for the given window
        /// </summary>
        /// <param name="hWnd">The window handle of window to enumerate hints in</param>
        /// <returns>The hint session containing the available hints</returns>
        HintSession EnumHints(IntPtr hWnd);
    }
}
