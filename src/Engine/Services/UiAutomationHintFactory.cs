using hap.Engine.Extensions;
using hap.Engine.Hints;
using hap.Engine.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation;

namespace hap.Engine.Services
{
    internal class UiAutomationHintFactory : IUiAutomationHintFactory
    {

        /// <summary>
        /// Creates a UI Automation element from the given automation element
        /// </summary>
        /// <param name="owningWindow">The owning window</param>
        /// <param name="windowBounds">The window bounds</param>
        /// <param name="automationElement">The associated automation element</param>
        /// <returns>The created hint, else null if the hint could not be created</returns>
        public UiAutomationHint CreateHint(IntPtr owningWindow, Rect windowBounds, AutomationElement automationElement)
        {
            var boundingRectObject = automationElement.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty, true);

            if (boundingRectObject == AutomationElement.NotSupported)
            {
                // Not supported
                return null;
            }

            var boundingRect = (Rect)boundingRectObject;
            if (boundingRect.IsEmpty)
            {
                // Not currently displaying UI
                return null;
            }

            // Convert the bounding rect to logical coords
            var logicalRect = boundingRect.PhysicalToLogicalRect(owningWindow);
            if (!logicalRect.IsEmpty)
            {
                var windowCoords = boundingRect.ScreenToWindowCoordinates(windowBounds);
                var capabilities = CreateHintCapabilities(automationElement);

                return new UiAutomationHint(owningWindow, automationElement, windowCoords, capabilities);
            }

            return null;
        }

        private IEnumerable<HintCapabilityBase> CreateHintCapabilities(AutomationElement automationElement)
        {
            var patterns = automationElement.GetSupportedPatterns();
            var capabilities = new List<HintCapabilityBase>();

            // Invoke
            {
                object invokePattern = null;
                if(automationElement.TryGetCurrentPattern(InvokePattern.Pattern, out invokePattern))
                {
                    capabilities.Add(new UiAutomationInvokeCapability(invokePattern as InvokePattern));
                }

            }

            return capabilities;
        }
    }
}
