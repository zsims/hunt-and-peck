using System;
using System.Windows;
using HuntAndPeck.Models;
using UIAutomationClient;

namespace HuntAndPeck.Services.Interfaces
{
    /// <summary>
    /// Provide basic hint creation method
    /// </summary>
    /// <typeparam name="THint">Specific Hint class implementation</typeparam>
    public interface IHintProviderService<THint>
        where THint : Hint
    {
        /// <summary>
        /// Creates a UI Automation element from the given automation element
        /// </summary>
        /// <param name="owningWindow">The owning window</param>
        /// <param name="hintBounds">The hint bounds</param>
        /// <param name="automationElement">The associated automation element</param>
        /// <returns>The created hint, else null if the hint could not be cre
        THint CreateHint(IntPtr owningWindow, Rect hintBounds, IUIAutomationElement automationElement);
    }
}
