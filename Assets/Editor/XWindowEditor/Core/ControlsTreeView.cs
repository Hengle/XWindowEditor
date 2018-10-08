using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.TreeView;

public class ControlsTreeView : TreeViewWithTreeModel<ControlElement>
{

    public ControlsTreeView(TreeViewState state, TreeModel<ControlElement> model) : base(state, model)
    {
        Reload();
    }

    public void DrawWorkPanelPreview(Rect rect, bool align)
    {
        int selectionId = -1;
        if (HasSelection())
        {
            var selection = GetSelection();
            if (selection != null && selection.Count > 0)
                selectionId = selection[0];
        }

        OnDrawWorkPanelPreview(rect, selectionId, align);
    }

    private void OnDrawWorkPanelPreview(Rect rect, int selectionId, bool align)
    {
        if (treeModel != null)
        {
            
            ControlElement element = treeModel.root;
            DrawElementPreview(element, selectionId, align);
        }
    }

    private void DrawElementPreview(ControlElement element, int selectionId, bool align)
    {
        if(element == null)
            return;

        if (element.depth != -1)
        {
            bool selected = element.id == selectionId;
            bool selectresult = selected;
            selectresult = XWinGUI.Control(element, selectresult, align, new GUIContent("Button"), GUI.skin.FindStyle("Button"));

            if (selected != selectresult)
                SetSelection(new List<int>() {element.id});
        }

        if (element.hasChildren)
        {
            for (int i = 0; i < element.children.Count; i++)
            {
                DrawElementPreview(((ControlElement) element.children[i]), selectionId, align);
            }
        }
    }
}
