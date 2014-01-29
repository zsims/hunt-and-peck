using Engine.Services.Interfaces;
using System;
using System.Windows;
using System.Windows.Automation;

namespace Engine
{
    /// <summary>
    /// Represents a Windows 
    /// </summary>
    internal class UiAutomationHint : Hint
    {
        public UiAutomationHint(IntPtr owningWindow, AutomationElement automationElement, Rect boundingRectangle)
            : base(owningWindow, boundingRectangle)
        {
            AutomationElement = automationElement;
        }

        /// <summary>
        /// The underlying automation element
        /// </summary>
        public AutomationElement AutomationElement { get; private set; }
    }
}
