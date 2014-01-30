using HuntnPeck.Engine.Extensions;
using HuntnPeck.Engine.Hints;
using HuntnPeck.Engine.NativeMethods;
using HuntnPeck.Engine.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Automation;

namespace HuntnPeck.Engine.Services
{
    public class UiAutomationHintProviderService : IHintProviderService, IHintDebugProviderService
    {
        private readonly IScreenshotService _screenshotService;

        /// <summary>
        /// Ctor
        /// </summary>
        public UiAutomationHintProviderService()
        {
            _screenshotService = new ScreenshotService();
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="screenshotService"></param>
        internal UiAutomationHintProviderService(IScreenshotService screenshotService)
        {
            _screenshotService = screenshotService;
        }

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
            RECT rawWindowBounds = new RECT();
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

        private AutomationElementCollection EnumElements(IntPtr hWnd)
        {
            var automationElement = AutomationElement.FromHandle(hWnd);
            var condition = new AndCondition(new PropertyCondition(AutomationElement.IsOffscreenProperty, false),
                                             new PropertyCondition(AutomationElement.IsEnabledProperty, true));

            return automationElement.FindAll(TreeScope.Descendants, condition);
        }

        /// <summary>
        /// Creates the hint and its bounds
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

            // Convert the bounding rect to logical coords then we're done :-)
            var logicalRect = boundingRect.PhysicalToLogicalRect(owningWindow);
            if (!logicalRect.IsEmpty)
            {
                var windowCoords = boundingRect.ScreenToWindowCoordinates(windowBounds);
                return new UiAutomationHint(owningWindow, automationElement, windowCoords);
            }

            return null;
        }

        public Bitmap RenderDebugHints(HintSession session)
        {
            var render = _screenshotService.RenderWindow(session.OwningWindow);

            using (Graphics graphics = Graphics.FromImage(render))
            {
                foreach (var hint in session.Hints)
                {
                    graphics.DrawRectangle(new Pen(Color.Red), hint.BoundingRectangle.ToDrawingRectangle());
                }
            } 

            return render;
        }
    }
}
