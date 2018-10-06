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
}
