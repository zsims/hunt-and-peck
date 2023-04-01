﻿using HuntAndPeck.Extensions;
using HuntAndPeck.Models;
using HuntAndPeck.NativeMethods;
using HuntAndPeck.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using UIAutomationClient;

namespace HuntAndPeck.Services
{
    internal class UiAutomationHintProviderService : IHintProviderService, IDebugHintProviderService
    {
        private static readonly IUIAutomation _automation = new CUIAutomation();
        private static Dictionary<IntPtr, List<IUIAutomationElement>> cached = new Dictionary<IntPtr, List<IUIAutomationElement>>();

        class CloseEventHandler : IUIAutomationEventHandler
        {
            private IntPtr closeWindowHwnd;

            public CloseEventHandler(IntPtr hwnd)
            {
                this.closeWindowHwnd = hwnd;
            }

            public void HandleAutomationEvent(IUIAutomationElement sender, int eventId)
            {
                if (eventId == UIA_EventIds.UIA_Window_WindowClosedEventId)
                {
                    Debug.WriteLine($"Window hwnd {this.closeWindowHwnd} closed!!!");
                    _automation.RemoveAutomationEventHandler(eventId,sender,this);
                    cached.Remove(this.closeWindowHwnd);
                }
            }
        }

        public HintSession EnumHints()
        {
            var foregroundWindow = User32.GetForegroundWindow();
            if (foregroundWindow == IntPtr.Zero)
            {
                return null;
            }
            return EnumHints(foregroundWindow);
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
            var foregroundWindow = User32.GetForegroundWindow();
            if (foregroundWindow == IntPtr.Zero)
            {
                return null;
            }
            return EnumDebugHints(foregroundWindow);
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
        private HintSession EnumWindowHints(IntPtr hWnd, Func<IntPtr, Rect, IUIAutomationElement, Hint> hintFactory)
        {
            var result = new List<Hint>();
            var elements = EnumElements(hWnd);

            // Window bounds
            var rawWindowBounds = new RECT();
            User32.GetWindowRect(hWnd, ref rawWindowBounds);
            Rect windowBounds = rawWindowBounds;
            
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Parallel.ForEach(elements, element =>
             {
                 var boundingRectObject = element.CachedBoundingRectangle;
                 if ((boundingRectObject.right > boundingRectObject.left) && (boundingRectObject.bottom > boundingRectObject.top))
                 {
                     var niceRect = new Rect(new Point(boundingRectObject.left, boundingRectObject.top), new Point(boundingRectObject.right, boundingRectObject.bottom));
                     // Convert the bounding rect to logical coords
                     var logicalRect = niceRect.PhysicalToLogicalRect(hWnd);
                     if (!logicalRect.IsEmpty)
                     {
                         var windowCoords = niceRect.ScreenToWindowCoordinates(windowBounds);
                         var hint = hintFactory(hWnd, windowCoords, element);
                         if (hint != null)
                         {
                             result.Add(hint);
                         }
                     }
                 }
             });
            stopwatch.Stop();
            Debug.WriteLine($"Execution time of List<Hint> creating: {stopwatch.ElapsedMilliseconds}");

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
        private List<IUIAutomationElement> EnumElements(IntPtr hWnd)
        {
            var result = new List<IUIAutomationElement>();
            var automationElement = _automation.ElementFromHandle(hWnd);

            var conditionControlView = _automation.ControlViewCondition;
            var conditionEnabled = _automation.CreatePropertyCondition(UIA_PropertyIds.UIA_IsEnabledPropertyId, true);
            var enabledControlCondition = _automation.CreateAndCondition(conditionControlView, conditionEnabled);

            var conditionOnScreen = _automation.CreatePropertyCondition(UIA_PropertyIds.UIA_IsOffscreenPropertyId, false);
            var condition = _automation.CreateAndCondition(enabledControlCondition, conditionOnScreen);
            
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var cacheRequest = _automation.CreateCacheRequest();
            cacheRequest.AddProperty(UIA_PropertyIds.UIA_BoundingRectanglePropertyId);
            cacheRequest.AddPattern(UIA_PatternIds.UIA_InvokePatternId);
            cacheRequest.AddPattern(UIA_PatternIds.UIA_TogglePatternId);
            cacheRequest.AddPattern(UIA_PatternIds.UIA_SelectionItemPatternId);
            cacheRequest.AddPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId);
            cacheRequest.AddPattern(UIA_PatternIds.UIA_ValuePatternId);
            cacheRequest.AddPattern(UIA_PatternIds.UIA_RangeValuePatternId);
            IUIAutomationElementArray elementArray = null;   

            if (!cached.ContainsKey(hWnd))
            {
                elementArray = automationElement.FindAllBuildCache(TreeScope.TreeScope_Subtree, condition, cacheRequest);
                _automation.AddAutomationEventHandler(UIA_EventIds.UIA_Window_WindowClosedEventId, automationElement,
                                                      TreeScope.TreeScope_Subtree, null, new CloseEventHandler(hWnd));
                if (elementArray != null)
                {
                    for (var i = 0; i < elementArray.Length; ++i)
                    {
                        result.Add(elementArray.GetElement(i));
                    }
                }
                Debug.WriteLine($"Execution time of FindAll: {stopwatch.ElapsedMilliseconds}");

            }
            else
            {
                cacheRequest.TreeFilter = condition;
                cacheRequest.TreeScope = TreeScope.TreeScope_Subtree;
                elementArray = automationElement.BuildUpdatedCache(cacheRequest).GetCachedChildren();
                GetAllChildren(elementArray);
                Debug.WriteLine($"Execution time of BuildUpdatedCache(): {stopwatch.ElapsedMilliseconds}");
            }

            cached[hWnd] = result;
            stopwatch.Stop();

            return result;

            void GetAllChildren(IUIAutomationElementArray eA)
            {
                for (int i = 0; i < eA.Length; i++)
                {
                    result.Add(eA.GetElement(i));
                    IUIAutomationElementArray tempEa = eA.GetElement(i).GetCachedChildren();
                    if (tempEa != null) GetAllChildren(tempEa);
                }
            }
        }

        /// <summary>
        /// Creates a UI Automation element from the given automation element
        /// </summary>
        /// <param name="owningWindow">The owning window</param>
        /// <param name="hintBounds">The hint bounds</param>
        /// <param name="automationElement">The associated automation element</param>
        /// <returns>The created hint, else null if the hint could not be created</returns>
        private Hint CreateHint(IntPtr owningWindow, Rect hintBounds, IUIAutomationElement automationElement)
        {
            try
            {
                var invokePattern = (IUIAutomationInvokePattern) automationElement.GetCachedPattern(UIA_PatternIds.UIA_InvokePatternId);
                if (invokePattern != null)
                {
                    return new UiAutomationInvokeHint(owningWindow, invokePattern, hintBounds);
                }

                var togglePattern = (IUIAutomationTogglePattern) automationElement.GetCachedPattern(UIA_PatternIds.UIA_TogglePatternId);
                if (togglePattern != null)
                {
                    return new UiAutomationToggleHint(owningWindow, togglePattern, hintBounds);
                }

                var selectPattern = (IUIAutomationSelectionItemPattern) automationElement.GetCachedPattern(UIA_PatternIds.UIA_SelectionItemPatternId);
                if (selectPattern != null)
                {
                    return new UiAutomationSelectHint(owningWindow, selectPattern, hintBounds);
                }

                var expandCollapsePattern = (IUIAutomationExpandCollapsePattern) automationElement.GetCachedPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId);
                if (expandCollapsePattern != null)
                {
                    return new UiAutomationExpandCollapseHint(owningWindow, expandCollapsePattern, hintBounds);
                }

                var valuePattern = (IUIAutomationValuePattern) automationElement.GetCachedPattern(UIA_PatternIds.UIA_ValuePatternId);
                if (valuePattern != null && valuePattern.CurrentIsReadOnly == 0)  // long(expensive) 'System.ArgumentException' when using CachedIsReadOnly
                {
                    return new UiAutomationFocusHint(owningWindow, automationElement, hintBounds);
                }

                var rangeValuePattern = (IUIAutomationRangeValuePattern) automationElement.GetCachedPattern(UIA_PatternIds.UIA_RangeValuePatternId);
                if (rangeValuePattern != null && rangeValuePattern.CurrentIsReadOnly == 0)
                {
                    return new UiAutomationFocusHint(owningWindow, automationElement, hintBounds);
                }

                return null;
            }
            catch (Exception)
            {
                // May have gone
                return null;
            }
        }

        /// <summary>
        /// Creates a debug hint
        /// </summary>
        /// <param name="owningWindow">The window that owns the hint</param>
        /// <param name="hintBounds">The hint bounds</param>
        /// <param name="automationElement">The automation element</param>
        /// <returns>A debug hint</returns>
        private DebugHint CreateDebugHint(IntPtr owningWindow, Rect hintBounds, IUIAutomationElement automationElement)
        {
            // Enumerate all possible patterns. Note that the performance of this is *very* bad -- hence debug only.
            var programmaticNames = new List<string>();

            foreach (var pn in UiAutomationPatternIds.PatternNames)
            {
                try
                {
                    var pattern = automationElement.GetCurrentPattern(pn.Key);
                    if(pattern != null)
                    {
                        programmaticNames.Add(pn.Value);
                    }
                }
                catch (Exception)
                {
                }
            }

            if (programmaticNames.Any())
            {
                return new DebugHint(owningWindow, hintBounds, programmaticNames.ToList());
            }

            return null;
        }
    }
}
