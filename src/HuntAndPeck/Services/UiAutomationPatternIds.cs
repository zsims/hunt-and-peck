using System.Collections.Generic;
using UIAutomationClient;

namespace HuntAndPeck.Services
{
    class UiAutomationPatternIds
    {
        /// <summary>
        /// All possible patterns (excluding custom patterns) -> debug description
        /// </summary>
        private static readonly Dictionary<int, string> s_patternNames = new Dictionary<int, string>()
        {
            {UIA_PatternIds.UIA_InvokePatternId, "Invoke"},
            {UIA_PatternIds.UIA_SelectionPatternId, "Selection"},
            {UIA_PatternIds.UIA_ValuePatternId, "Value"},
            {UIA_PatternIds.UIA_RangeValuePatternId, "RangeValue"},
            {UIA_PatternIds.UIA_ScrollPatternId, "Scroll"},
            {UIA_PatternIds.UIA_ExpandCollapsePatternId, "ExpandCollapse"},
            {UIA_PatternIds.UIA_GridPatternId, "Grid"},
            {UIA_PatternIds.UIA_GridItemPatternId, "GridItem"},
            {UIA_PatternIds.UIA_MultipleViewPatternId, "MultipleView"},
            {UIA_PatternIds.UIA_WindowPatternId, "Window"},
            {UIA_PatternIds.UIA_SelectionItemPatternId, "SelectionItem"},
            {UIA_PatternIds.UIA_DockPatternId, "Dock"},
            {UIA_PatternIds.UIA_TablePatternId, "Table"},
            {UIA_PatternIds.UIA_TableItemPatternId, "TableItem"},
            {UIA_PatternIds.UIA_TextPatternId, "Text"},
            {UIA_PatternIds.UIA_TogglePatternId, "Toggle"},
            {UIA_PatternIds.UIA_TransformPatternId, "Transform"},
            {UIA_PatternIds.UIA_ScrollItemPatternId, "ScrollItem"},
            {UIA_PatternIds.UIA_LegacyIAccessiblePatternId, "LegacyIAccessible"},
            {UIA_PatternIds.UIA_ItemContainerPatternId, "ItemContainer"},
            {UIA_PatternIds.UIA_VirtualizedItemPatternId, "VirtualizedItem"},
            {UIA_PatternIds.UIA_SynchronizedInputPatternId, "SynchronizedInput"},
            {UIA_PatternIds.UIA_ObjectModelPatternId, "ObjectModel"},
            {UIA_PatternIds.UIA_AnnotationPatternId, "Annotation"},
            {UIA_PatternIds.UIA_TextPattern2Id, "Text"},
            {UIA_PatternIds.UIA_StylesPatternId, "Styles"},
            {UIA_PatternIds.UIA_SpreadsheetPatternId, "Spreadsheet"},
            {UIA_PatternIds.UIA_SpreadsheetItemPatternId, "SpreadsheetItem"},
            {UIA_PatternIds.UIA_TransformPattern2Id, "Transform"},
            {UIA_PatternIds.UIA_TextChildPatternId, "TextChild"},
            {UIA_PatternIds.UIA_DragPatternId, "Drag"},
            {UIA_PatternIds.UIA_DropTargetPatternId, "DropTarget"},
            {UIA_PatternIds.UIA_TextEditPatternId, "TextEdit"},
            {UIA_PatternIds.UIA_CustomNavigationPatternId, "CustomNavigation"}
        };

        public static Dictionary<int, string> PatternNames { get { return s_patternNames; } }
    }
}
