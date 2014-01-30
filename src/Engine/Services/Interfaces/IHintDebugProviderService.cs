using HuntnPeck.Engine.Hints;
using System.Collections.Generic;
using System.Drawing;

namespace HuntnPeck.Engine.Services.Interfaces
{
    /// <summary>
    /// Provides debugging information for hints
    /// </summary>
    public interface IHintDebugProviderService
    {
        Bitmap RenderDebugHints(HintSession session);
    }
}
