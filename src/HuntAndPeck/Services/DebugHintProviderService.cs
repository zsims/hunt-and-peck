using HuntAndPeck.Models;
using HuntAndPeck.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UIAutomationClient;

namespace HuntAndPeck.Services
{
    /// <summary>
    /// Provide debug hint
    /// </summary>
    class DebugHintProviderService : IHintProviderService<DebugHint>
    {
        public DebugHint CreateHint(IntPtr owningWindow, Rect hintBounds, IUIAutomationElement automationElement)
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
