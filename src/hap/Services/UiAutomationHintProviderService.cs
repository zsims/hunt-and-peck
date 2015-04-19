using hap.Extensions;
using hap.Models;
using hap.NativeMethods;
using hap.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Automation;

namespace hap.Services
{
    internal class UiAutomationHintProviderService : IHintProviderService, IDebugHintProviderService
    {
        public HintSession EnumHints()
        {
            var desktopHandle = User32.GetForegroundWindow();
            return EnumHints(desktopHandle);
        }

        public HintSession EnumHints(IntPtr hWnd)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var session = EnumWindowHints(hWnd, CreateHint);
            sw.Stop();

            Debug.WriteLine("Enumeration of hints took {0} ms", sw.ElapsedMilliseconds);
            return session;
        }

        public HintSession EnumDebugHints()
        {
            var desktopHandle = User32.GetForegroundWindow();
            return EnumDebugHints(desktopHandle);
        }

        public HintSession EnumDebugHints(IntPtr hWnd)
        {
            return EnumWindowHints(hWnd, CreateDebugHint);
        }

        /// <summary>
        /// Enumerates all the hints from the given window
        /// </summary>
        /// <param name="hWnd">The window to get hints from</param>
        /// <param name="hintFactory">The factory to use to create each hint in the session</param>
        /// <returns>A hint session</returns>
        private HintSession EnumWindowHints(IntPtr hWnd, Func<IntPtr, Rect, AutomationElement, Hint> hintFactory)
        {
            // Set up the request.
            CacheRequest cacheRequest = new CacheRequest();
            cacheRequest.Add(AutomationElement.BoundingRectangleProperty);
            cacheRequest.Add(AutomationElement.IsEnabledProperty);
            cacheRequest.Add(AutomationElement.IsOffscreenProperty);
            cacheRequest.Add(InvokePattern.Pattern);

            // Obtain an element and cache the requested items. 
            AutomationElementCollection elements;
            using (cacheRequest.Activate())
            {
                elements = EnumElements(hWnd);
            }

            // Window bounds
            var rawWindowBounds = new RECT();
            User32.GetWindowRect(hWnd, ref rawWindowBounds);
            Rect windowBounds = rawWindowBounds;

            var result = new List<Hint>();
            foreach (AutomationElement element in elements)
            {
                var boundingRectObject = element.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty, true);

                if (boundingRectObject == AutomationElement.NotSupported)
                {
                    // Not supported
                    continue;
                }

                var boundingRect = (Rect)boundingRectObject;
                if (boundingRect.IsEmpty)
                {
                    // Not currently displaying UI
                    continue;
                }

                // Convert the bounding rect to logical coords
                var logicalRect = boundingRect.PhysicalToLogicalRect(hWnd);
                if (!logicalRect.IsEmpty)
                {
                    var windowCoords = boundingRect.ScreenToWindowCoordinates(windowBounds);

                    var hint = hintFactory(hWnd, windowCoords, element);

                    if (hint != null)
                    {
                        result.Add(hint);
                    }
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
        /// <param name="hintBounds">The hint bounds</param>
        /// <param name="automationElement">The associated automation element</param>
        /// <returns>The created hint, else null if the hint could not be created</returns>
        private UiAutomationHint CreateHint(IntPtr owningWindow, Rect hintBounds, AutomationElement automationElement)
        {
            InvokePattern pattern;
            if (TryGetInvokePattern(automationElement, out pattern))
            {
                return new UiAutomationHint(owningWindow, automationElement, pattern, hintBounds);
            }

            return null;
        }

        /// <summary>
        /// Creates a debug hint
        /// </summary>
        /// <param name="owningWindow">The window that owns the hint</param>
        /// <param name="hintBounds">The hint bounds</param>
        /// <param name="automationElement">The automation element</param>
        /// <returns>A debug hint</returns>
        private DebugHint CreateDebugHint(IntPtr owningWindow, Rect hintBounds, AutomationElement automationElement)
        {
            var supportedPatterns = automationElement.GetSupportedPatterns();
            var programmaticNames = supportedPatterns.Select(x => x.ProgrammaticName);

            if (supportedPatterns.Any())
            {
                return new DebugHint(owningWindow, hintBounds, programmaticNames.ToList());
            }

            return null;
        }

        /// <summary>
        /// Tries to get the invoke pattern (if available)
        /// </summary>
        /// <param name="automationElement">The automation element to get the pattern from</param>
        /// <param name="pattern">The pattern that was retrieved</param>
        /// <returns>True if the pattern was retrieved, false otherwise</returns>
        private bool TryGetInvokePattern(AutomationElement automationElement, out InvokePattern pattern)
        {
            object invokePattern;
            
            if(automationElement.TryGetCachedPattern(InvokePattern.Pattern, out invokePattern))
            {
                pattern = invokePattern as InvokePattern;
                return pattern != null;
            }

            pattern = null;
            return false;
        }
    }
}
