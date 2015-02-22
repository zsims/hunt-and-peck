using hap.Engine.Extensions;
using hap.Engine.Hints;
using hap.Engine.Services.Interfaces;
using hap.NativeMethods;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Automation;

namespace hap.Engine.Services
{
    internal class UiAutomationHintProviderService : IHintProviderService
    {
        private readonly IUiAutomationHintFactory _hintFactory;

        /// <summary>
        /// Ctor
        /// </summary>
        public UiAutomationHintProviderService()
        {
            _hintFactory = new UiAutomationHintFactory();
        }

        /// <summary>
        /// Ctor
        /// </summary>
        internal UiAutomationHintProviderService(IUiAutomationHintFactory hintFactory)
        {
            _hintFactory = hintFactory;
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
                var hint = _hintFactory.CreateHint(hWnd, windowBounds, element);

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

    }
}
