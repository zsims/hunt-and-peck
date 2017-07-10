using System;
using HuntAndPeck.Models;

namespace HuntAndPeck.Services.Interfaces
{
    /// <summary>
    /// Provides hints for the entire desktop or a given window handle
    /// </summary>
    public interface IHintProviderService
    {
        /// <summary>
        /// Enumerate the available hints for the current foreground window
        /// </summary>
        /// <returns>The hint session containing the available hints or null if there is no foreground window</returns>
        HintSession EnumHints();
    }
}
