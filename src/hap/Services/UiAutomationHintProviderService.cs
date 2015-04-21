using Accessibility;
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
            var accessible = GetAccessibleObjectFromHandle(hWnd);
            var kids = GetAccessibleChildren(accessible);

            var rawWindowBounds = new RECT();
            User32.GetWindowRect(hWnd, ref rawWindowBounds);
            Rect windowBounds = rawWindowBounds;

            var hints = new List<Hint>();
            foreach (var kid in kids)
            {
                var location = GetLocation(kid);

                var logicalRect = location.PhysicalToLogicalRect(hWnd);
                if (!logicalRect.IsEmpty)
                {
                    location = logicalRect.ScreenToWindowCoordinates(windowBounds);
                }

                hints.Add(new DebugHint(hWnd, location, new []{ "?" }));
            }

            return new HintSession
            {
                Hints = hints,
                OwningWindow = hWnd,
                OwningWindowBounds = windowBounds
            };
        }

        private Rect GetLocation(IAccessible accObject)
        {
            int x1, y1;
            int width;
            int height;

            accObject.accLocation(out x1, out y1, out width, out height, 0);
            if (x1 > 0 && y1 > 0 && width > 0 && height > 0)
            {
                return new Rect(x1, y1, width, height);
            }
            return Rect.Empty;
        }

        public static IAccessible GetAccessibleObjectFromHandle(IntPtr hwnd)
        {
            IAccessible objAccessible = default(IAccessible);
            var guidAccessible = new Guid("{618736E0-3C3D-11CF-810C-00AA00389B71}");
            if (hwnd != IntPtr.Zero)
            {
                objAccessible = (IAccessible)OleAcc.AccessibleObjectFromWindow(hwnd.ToInt32(), 0, ref guidAccessible);
            }
            return objAccessible;
        }

        public static IAccessible[] GetAccessibleChildren(IAccessible objAccessible)
        {
            int childCount = 0;

            try
            {
                childCount = objAccessible.accChildCount;
            }
            catch (Exception ex)
            {
                childCount = 0;
            }

            var accObjects = new IAccessible[childCount];
            int count = 0;

            if (childCount != 0)
            {
                OleAcc.AccessibleChildren(objAccessible, 0, childCount, accObjects, ref count);
            }

            return accObjects;
        }



        /// <summary>
        /// Enumerates all the hints from the given window
        /// </summary>
        /// <param name="hWnd">The window to get hints from</param>
        /// <param name="hintFactory">The factory to use to create each hint in the session</param>
        /// <returns>A hint session</returns>
        private HintSession EnumWindowHints(IntPtr hWnd, Func<IntPtr, Rect, AutomationElement, Hint> hintFactory)
        {
            var result = new List<Hint>();
            var elements = EnumElements(hWnd);

            // Window bounds
            var rawWindowBounds = new RECT();
            User32.GetWindowRect(hWnd, ref rawWindowBounds);
            Rect windowBounds = rawWindowBounds;

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
