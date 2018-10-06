using UnityEngine;
using System.Collections;

public class XWinControlsPanel : XWinPanel
{
    private Rect m_WorkPanelRect;

    private Vector2 m_ControlsScroller;

    public XWinControlsPanel(XWinEditorWindow window, bool resizeLeft, bool resizeRight, bool resizeTop, bool resizeBottom) : base(window, resizeLeft, resizeRight, resizeTop, resizeBottom)
    {
    }

    public void RefreshWorkPanelRect(Rect workPanelRect)
    {
        m_WorkPanelRect = workPanelRect;
    }

    public override Rect DoGUI(Rect rect, GUIContent title)
    {
        rect = base.DoGUI(rect, title);

        Rect areaRect = new Rect(rect.x + 1, rect.y + 20, rect.width - 2, rect.height - 20);

        var types = System.Enum.GetValues(typeof(ControlType));

        Rect workPanelRect = new Rect(m_WorkPanelRect.x - areaRect.x, m_WorkPanelRect.y - areaRect.y,
            m_WorkPanelRect.width, m_WorkPanelRect.height);

        m_ControlsScroller = GUI.BeginScrollView(new Rect(areaRect), m_ControlsScroller,
            new Rect(0, 0, areaRect.width, types.Length * 18));

        for (int i = 0; i < types.Length; i++)
        {
            if (XWinGUI.ControlPreview(new Rect(0,i*18,areaRect.width,18),  workPanelRect,
                new GUIContent(types.GetValue(i).ToString()), XWinEditorWindow.styles.toolbarButton))
            {
                m_Window.AddControl((ControlType)types.GetValue(i), Event.current.mousePosition);
            }
        }

        GUI.EndScrollView();


        //GUILayout.BeginArea(areaRect);

        //m_ControlsScroller = GUILayout.BeginScrollView(m_ControlsScroller);
        //foreach (ControlType type in System.Enum.GetValues(typeof(ControlType)))
        //{
        //    GUILayout.Button("xx", XWinEditorWindow.styles.tagMenuItem);
        //    //if (XWinGUILayout.ControlPreview(workPanelRect,
        //    //    new GUIContent(type.ToString()), XWinEditorWindow.styles.tagMenuItem))
        //    //{
        //    //    m_Window.AddControl(type, Event.current.mousePosition);
        //    //}
        //}
        //GUILayout.EndScrollView();

        //GUILayout.EndArea();

        return rect;
    }
}
