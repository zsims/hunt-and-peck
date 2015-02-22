using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation;

namespace hap.Engine.Hints
{
    /// <summary>
    /// Represents a Windows UI Automation based hint
    /// </summary>
    internal class UiAutomationHint : Hint
    {
        public UiAutomationHint(IntPtr owningWindow, AutomationElement automationElement, Rect boundingRectangle, IEnumerable<HintCapabilityBase> capabilities)
            : base(owningWindow, boundingRectangle, capabilities)
        {
            AutomationElement = automationElement;
        }

        /// <summary>
        /// The underlying automation element
        /// </summary>
        public AutomationElement AutomationElement { get; private set; }
    }
}
