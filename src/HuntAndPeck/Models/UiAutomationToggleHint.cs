using System;
using System.Windows;
using UIAutomationClient;

namespace HuntAndPeck.Models
{
    /// <summary>
    /// Represents a Windows UI Automation based toggle hint
    /// </summary>
    internal class UiAutomationToggleHint : Hint
    {
        private readonly IUIAutomationTogglePattern _togglePattern;

        public UiAutomationToggleHint(IntPtr owningWindow, IUIAutomationTogglePattern togglePattern, Rect boundingRectangle)
            : base(owningWindow, boundingRectangle)
        {
            _togglePattern = togglePattern;
        }

        public override void Invoke()
        {
            _togglePattern.Toggle();
        }
    }
}
