using System;
using System.Windows;
using UIAutomationClient;

namespace hap.Models
{
    /// <summary>
    /// Represents a Windows UI Automation based legacy IAccessible hint
    /// </summary>
    internal class UiAutomationLegacyHint : Hint
    {
        private readonly IUIAutomationLegacyIAccessiblePattern _legacyIAccessiblePattern;

        public UiAutomationLegacyHint(IntPtr owningWindow, IUIAutomationLegacyIAccessiblePattern legacyIAccessiblePattern, Rect boundingRectangle)
            : base(owningWindow, boundingRectangle)
        {
            _legacyIAccessiblePattern = legacyIAccessiblePattern;
        }

        public override void Invoke()
        {
            try
            {
                _legacyIAccessiblePattern.DoDefaultAction();
            }
            catch (Exception)
            {
            }
        }
    }
}
