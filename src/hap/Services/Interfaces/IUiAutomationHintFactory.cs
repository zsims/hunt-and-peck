using System;
using System.Windows;
using System.Windows.Automation;
using hap.Models;

namespace hap.Services.Interfaces
{
    internal interface IUiAutomationHintFactory
    {
        /// <summary>
        /// Creates a UI Automation element from the given automation element
        /// </summary>
        /// <param name="owningWindow">The owning window</param>
        /// <param name="windowBounds">The window bounds</param>
        /// <param name="automationElement">The associated automation element</param>
        /// <returns>The created hint, else null if the hint could not be created</returns>
        UiAutomationHint CreateHint(IntPtr owningWindow, Rect windowBounds, AutomationElement automationElement);
    }
}
