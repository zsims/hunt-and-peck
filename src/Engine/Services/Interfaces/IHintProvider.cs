using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Services.Interfaces
{
    /// <summary>
    /// Provides hints for the entire desktop or a given window handle
    /// </summary>
    public interface IHintProvider
    {
        /// <summary>
        /// Enumerate the available hints for the current foreground window
        /// </summary>
        /// <returns>The collection of available hints</returns>
        IEnumerable<Hint> EnumHints();

        /// <summary>
        /// Enumerate the available hints for the given window
        /// </summary>
        /// <param name="hWnd">The window handle of window to enumerate hints in</param>
        /// <returns>The collection of available hints</returns>
        IEnumerable<Hint> EnumHints(IntPtr hWnd);
    }
}
