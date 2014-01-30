using HuntnPeck.Engine.Services.Interfaces;
using System;
using System.Windows;
using System.Windows.Automation;

namespace HuntnPeck.Engine.Hints
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

        public override void Activate()
        {
            /*
            var invokePattern = AutomationElement.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
            if (invokePattern != null)
            {
                invokePattern.Invoke();
            }

            var patterns = AutomationElement.GetSupportedPatterns();

            ExpandCollapsePattern pattern;
            try
            {
                pattern = AutomationElement.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
            }
            catch (InvalidOperationException ex)
            {
                // Most likely "Pattern not supported." 
                return;
            }

            pattern.Collapse();
             */
        }
    }
}
