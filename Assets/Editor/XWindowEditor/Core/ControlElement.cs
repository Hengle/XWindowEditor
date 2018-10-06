using UnityEngine;
using System.Collections;
using UnityEditor.IMGUI.Controls;
using UnityEditor.TreeView;

public enum ControlType
{
    Button,
    Label,
    Toggle,
    TextField,
}

[System.Serializable]
public class ControlElement : TreeElement
{
    public ControlType type;
}
