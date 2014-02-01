using System;
using System.Windows;
using System.Windows.Automation;

namespace HuntnPeck.Engine.Hints
{
    /// <summary>
    /// Represents a Windows UI Automation based hint
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
