using System;
using System.Windows;
using UIAutomationClient;

namespace HuntAndPeck.Models
{
    /// <summary>
    /// Represents a Windows UI Automation based select hint
    /// </summary>
    internal class UiAutomationSelectHint : Hint
    {
        private readonly IUIAutomationSelectionItemPattern _selectPattern;

        public UiAutomationSelectHint(IntPtr owningWindow, IUIAutomationSelectionItemPattern selectPattern, Rect boundingRectangle)
            : base(owningWindow, boundingRectangle)
        {
            _selectPattern = selectPattern;
        }

        public override void Invoke()
        {
            _selectPattern.Select();
        }
    }
}
