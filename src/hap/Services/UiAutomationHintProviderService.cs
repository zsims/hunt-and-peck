using hap.Extensions;
using hap.Models;
using hap.NativeMethods;
using hap.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation;

namespace hap.Services
{
    internal class UiAutomationHintProviderService : IHintProviderService
    {
        /// <summary>
        /// Enumerate the available hints for the current foreground window
        /// </summary>
        /// <returns>The hint session containing the available hints</returns>
        public HintSession EnumHints()
        {
            var desktopHandle = User32.GetForegroundWindow();
            return EnumHints(desktopHandle);
        }

        /// <summary>
        /// Enumerate the available hints for the given window
        /// </summary>
        /// <param name="hWnd">The window handle of window to enumerate hints in</param>
        /// <returns>The hint session containing the available hints</returns>
        public HintSession EnumHints(IntPtr hWnd)
        {
            var result = new List<Hint>();
            var elements = EnumElements(hWnd);

            // Window bounds
            var rawWindowBounds = new RECT();
            User32.GetWindowRect(hWnd, ref rawWindowBounds);
            Rect windowBounds = rawWindowBounds;

            foreach (AutomationElement element in elements)
            {
                var hint = CreateHint(hWnd, windowBounds, element);

                if (hint != null)
                {
                    result.Add(hint);
                }
            }

            return new HintSession
            {
                Hints = result,
                OwningWindow = hWnd,
                OwningWindowBounds = windowBounds,
            };
        }

        /// <summary>
        /// Enumerates the automation elements from the given window
        /// </summary>
        /// <param name="hWnd">The window handle</param>
        /// <returns>All of the automation elements found</returns>
        private AutomationElementCollection EnumElements(IntPtr hWnd)
        {
            var automationElement = AutomationElement.FromHandle(hWnd);
            var condition = new AndCondition(new PropertyCondition(AutomationElement.IsOffscreenProperty, false),
                                             new PropertyCondition(AutomationElement.IsEnabledProperty, true));

            return automationElement.FindAll(TreeScope.Descendants, condition);
        }

        /// <summary>
        /// Creates a UI Automation element from the given automation element
        /// </summary>
        /// <param name="owningWindow">The owning window</param>
        /// <param name="windowBounds">The window bounds</param>
        /// <param name="automationElement">The associated automation element</param>
        /// <returns>The created hint, else null if the hint could not be created</returns>
        private UiAutomationHint CreateHint(IntPtr owningWindow, Rect windowBounds, AutomationElement automationElement)
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

                InvokePattern pattern;
                if (TryGetInvokePattern(automationElement, out pattern))
                {
                    return new UiAutomationHint(owningWindow, automationElement, pattern, windowCoords);
                }
            }

            return null;
        }

        private bool TryGetInvokePattern(AutomationElement automationElement, out InvokePattern pattern)
        {
            object invokePattern;
            if(automationElement.TryGetCurrentPattern(InvokePattern.Pattern, out invokePattern))
            {
                pattern = invokePattern as InvokePattern;
                return pattern != null;
            }

            pattern = null;
            return false;
        }
    }
}
