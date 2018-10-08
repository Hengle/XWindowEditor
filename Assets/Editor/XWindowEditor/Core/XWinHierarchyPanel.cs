using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEditor.TreeView;

[System.Serializable]
public class XWinHierarchyPanel : XWinPanel
{
    public ControlsTreeView treeView
    {
        get { return m_TreeView; }
    }

    [SerializeField] TreeViewState m_TreeViewState;

    private ControlsTreeView m_TreeView;

    

    public XWinHierarchyPanel(XWinEditorWindow window, bool resizeLeft, bool resizeRight, bool resizeTop, bool resizeBottom) : base(window, resizeLeft, resizeRight, resizeTop, resizeBottom)
    {
        if (m_TreeViewState == null)
            m_TreeViewState = new TreeViewState();

        m_TreeView = new ControlsTreeView(m_TreeViewState, window.model); 
    }

    public override Rect DoGUI(Rect rect, GUIContent title)
    {
        rect = base.DoGUI(rect, title);

        var mainRect = new Rect(rect.x + 1, rect.y + 1, rect.width - 2, rect.height - 2);
        GUI.BeginGroup(mainRect);

        if(m_TreeView == null)
            m_TreeView = new ControlsTreeView(m_TreeViewState, m_Window.model); 
        m_TreeView.OnGUI(mainRect);

        GUI.EndGroup();

        return rect;
    }

    
}
