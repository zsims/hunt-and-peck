using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Services.Interfaces
{
    /// <summary>
    /// Provides debugging information for hints
    /// </summary>
    public interface IHintDebugProvider
    {
        Bitmap RenderDebugHints(IEnumerable<Hint> hints);
    }
}
