using System;
using System.Collections.Generic;
using System.Windows;

namespace HuntnPeck.Engine.Hints
{
    /// <summary>
    /// Represents a hint that has 1 or more capabilities
    /// </summary>
    public class Hint
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="owningWindow">The owning window</param>
        /// <param name="boundingRectangle">The bounding rectangle of the hint in owner window coordinates</param>
        /// <param name="capabilities">The capabilities of the hint</param>
        protected Hint(IntPtr owningWindow, Rect boundingRectangle, IEnumerable<HintCapabilityBase> capabilities)
        {
            OwningWindow = owningWindow;
            BoundingRectangle = boundingRectangle;

            Label = string.Empty;

            Capabilities = capabilities;
        }

        /// <summary>
        /// The label of the hint
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The bounding rectangle for the hint in Window coordinates for the owning window
        /// </summary>
        public Rect BoundingRectangle { get; private set; }

        /// <summary>
        /// The window handle of the owning window
        /// </summary>
        public IntPtr OwningWindow { get; private set; }

        /// <summary>
        /// The hint capabilities
        /// </summary>
        public IEnumerable<HintCapabilityBase> Capabilities { get; private set; }
    }
}
