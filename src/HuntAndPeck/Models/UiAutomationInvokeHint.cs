using System;
using System.Windows;
using UIAutomationClient;

namespace HuntAndPeck.Models
{
    /// <summary>
    /// Represents a Windows UI Automation based invoke hint
    /// </summary>
    internal class UiAutomationInvokeHint : Hint
    {
        private readonly IUIAutomationInvokePattern _invokePattern;

        public UiAutomationInvokeHint(IntPtr owningWindow, IUIAutomationInvokePattern invokePattern, Rect boundingRectangle)
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
