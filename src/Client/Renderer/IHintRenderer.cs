using hap.Engine.Hints;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hap.Client.Renderer
{
    /// <summary>
    /// Renders hints 
    /// </summary>
    public interface IHintRenderer : IDisposable
    {
        /// <summary>
        /// Renders the hints using the given graphics object
        /// </summary>
        /// <param name="graphics">The graphics object to use for rendering</param>
        /// <param name="matchingHints">The matching hints</param>
        /// <param name="allHints">All of the hints, matching and non-matching</param>
        void RenderHints(Graphics graphics, IEnumerable<Hint> matchingHints, IEnumerable<Hint> allHints);
    }
}
