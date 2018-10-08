using UnityEngine;
using System.Collections;

public class XWinWorkPanel : XWinPanel
{
    private bool m_Align;

    public XWinWorkPanel(XWinEditorWindow window, bool resizeLeft, bool resizeRight, bool resizeTop, bool resizeBottom) : base(window, resizeLeft, resizeRight, resizeTop, resizeBottom)
    {
    }

    public override Rect DoGUI(Rect rect, GUIContent title)
    {
        rect = base.DoGUI(rect, title);

        DoToolbar(new Rect(rect.x+1, rect.y + 17, rect.width-2, 18));
        DoControlsPanel(new Rect(rect.x+1, rect.y + 35, rect.width-2, rect.height - 35));
        return rect;
    }

    private void DoToolbar(Rect rect)
    {
        if (Event.current.type == EventType.repaint)
            XWinEditorWindow.styles.toolbar.Draw(rect, false, false, false, false);

        m_Align = GUI.Toggle(new Rect(rect.x + rect.width - 100, rect.y, 80, rect.height), m_Align,
            XWinEditorWindow.styles.text_Align, XWinEditorWindow.styles.toolbarButton);
    }

    private void DoControlsPanel(Rect rect)
    {
        GUI.BeginGroup(rect);

        if (m_Window.hierarchyPanel != null && m_Window.hierarchyPanel.treeView != null)
        {
            m_Window.hierarchyPanel.treeView.DrawWorkPanelPreview(new Rect(0, 0, rect.width, rect.height), m_Align);
        }

        GUI.EndGroup();
    }
}
