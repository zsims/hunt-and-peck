using System;
using System.Windows;

namespace HuntnPeck.Engine.Hints
{
    public abstract class Hint
    {
        public Hint(IntPtr owningWindow, Rect boundingRectangle)
        {
            OwningWindow = owningWindow;
            BoundingRectangle = boundingRectangle;

            Label = string.Empty;
        }

        /// <summary>
        /// The label of the hint
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The bounding rectangle for the hint in Window coordinates for the owning window
        /// </summary>
        public Rect BoundingRectangle { get; private set; }

        public IntPtr OwningWindow { get; private set; }

        /// <summary>
        /// Activates the hint
        /// </summary>
        public abstract void Activate();
    }
}
