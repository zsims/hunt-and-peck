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
    /// Provide General left click simulation hint
    /// </summary>
    class GeneralHintProviderService : IHintProviderService<Hint>
    {
        public Hint CreateHint(IntPtr owningWindow, Rect hintBounds, IUIAutomationElement automationElement)
        {
             try
            {
                var invokePattern = (IUIAutomationInvokePattern)automationElement.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);
                if (invokePattern != null)
                {
                    return new UiAutomationInvokeHint(owningWindow, invokePattern, hintBounds);
                }

                var togglePattern = (IUIAutomationTogglePattern)automationElement.GetCurrentPattern(UIA_PatternIds.UIA_TogglePatternId);
                if (togglePattern != null)
                {
                    return new UiAutomationToggleHint(owningWindow, togglePattern, hintBounds);
                }
                
                var selectPattern = (IUIAutomationSelectionItemPattern) automationElement.GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);
                if (selectPattern != null)
                {
                    return new UiAutomationSelectHint(owningWindow, selectPattern, hintBounds);
                }

                var expandCollapsePattern = (IUIAutomationExpandCollapsePattern) automationElement.GetCurrentPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId);
                if (expandCollapsePattern != null)
                {
                    return new UiAutomationExpandCollapseHint(owningWindow, expandCollapsePattern, hintBounds);
                }

                var valuePattern = (IUIAutomationValuePattern)automationElement.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
                if (valuePattern != null && valuePattern.CurrentIsReadOnly == 0)
                {
                    return new UiAutomationFocusHint(owningWindow, automationElement, hintBounds);
                }

                var rangeValuePattern = (IUIAutomationRangeValuePattern) automationElement.GetCurrentPattern(UIA_PatternIds.UIA_RangeValuePatternId);
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
    }
}
