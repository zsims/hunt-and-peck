using System;
using System.Windows;

namespace Engine.Services.Interfaces
{
    public abstract class Hint
    {
        public Hint(IntPtr owningWindow, Rect boundingRectangle)
        {
            OwningWindow = owningWindow;
            BoundingRectangle = boundingRectangle;
        }

        public Rect BoundingRectangle { get; private set; }
        public IntPtr OwningWindow { get; private set; }
    }
}
