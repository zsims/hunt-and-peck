using Engine.Exceptions;
using Engine.Extensions;
using Engine.NativeMethods;
using Engine.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Automation;

namespace Engine.Services
{
    public class UiAutomationHintProviderService : IHintProvider, IHintDebugProvider
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
        /// <returns>The collection of available hints</returns>
        public IEnumerable<Hint> EnumHints()
        {
            var desktopHandle = User32.GetForegroundWindow();
            return EnumHints(desktopHandle);
        }

        /// <summary>
        /// Enumerate the available hints for the given window
        /// </summary>
        /// <param name="hWnd">The window handle of window to enumerate hints in</param>
        /// <returns>The collection of available hints</returns>
        public IEnumerable<Hint> EnumHints(IntPtr hWnd)
        {
            var result = new List<Hint>();
            var elements = EnumElements(hWnd);

            foreach (AutomationElement element in elements)
            {
                var hint = CreateHint(hWnd, element);

                if (hint != null)
                {
                    result.Add(hint);
                }
            }

            return result;
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
        /// <param name="automationElement">The associated automation element</param>
        /// <returns>The created hint, else null if the hint could not be created</returns>
        private UiAutomationHint CreateHint(IntPtr owningWindow, AutomationElement automationElement)
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
                return new UiAutomationHint(owningWindow, automationElement, boundingRect);
            }

            return null;
        }

        public Bitmap RenderDebugHints(IEnumerable<Hint> hints)
        {
            var owningWindows = hints.Select(hint => hint.OwningWindow)
                                     .Distinct();

            // ensure all the hints come from the same window
            if(owningWindows.Count() > 1)
            {
                throw new NondistinctHintOwnersException();
            }
            else if (owningWindows.Count() < 1)
            {
                return null;
            }

            var owner = owningWindows.First();
            var render = _screenshotService.RenderWindow(owner);

            using (Graphics graphics = Graphics.FromImage(render))
            {
                foreach (var hint in hints)
                {
                    graphics.DrawRectangle(new Pen(Color.Red), hint.BoundingRectangle.ToDrawingRectangle());
                }
            } 

            return render;
        }
    }
}
