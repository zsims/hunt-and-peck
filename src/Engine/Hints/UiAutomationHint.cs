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
        private readonly InvokePattern _invokePattern;

        public UiAutomationHint(IntPtr owningWindow, AutomationElement automationElement, InvokePattern invokePattern, Rect boundingRectangle)
            : base(owningWindow, boundingRectangle)
        {
            AutomationElement = automationElement;
            _invokePattern = invokePattern;
        }

        /// <summary>
        /// The underlying automation element
        /// </summary>
        public AutomationElement AutomationElement { get; private set; }

        public override void Invoke()
        {
            _invokePattern.Invoke();
        }
    }
}
