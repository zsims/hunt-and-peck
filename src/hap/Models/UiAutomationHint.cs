using System;
using System.Windows;
using UIAutomationClient;

namespace hap.Models
{
    /// <summary>
    /// Represents a Windows UI Automation based hint
    /// </summary>
    internal class UiAutomationHint : Hint
    {
        private readonly IUIAutomationInvokePattern _invokePattern;

        public UiAutomationHint(IntPtr owningWindow, IUIAutomationInvokePattern invokePattern, Rect boundingRectangle)
            : base(owningWindow, boundingRectangle)
        {
            _invokePattern = invokePattern;
        }

        public override void Invoke()
        {
            _invokePattern.Invoke();
        }
    }
}
