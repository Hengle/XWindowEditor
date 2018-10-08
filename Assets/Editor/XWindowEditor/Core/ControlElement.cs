using System;
using UnityEngine;
using System.Collections;
using UnityEditor.IMGUI.Controls;
using UnityEditor.TreeView;

public enum ControlLayoutMode
{
    Undefined = -1,
    Min,
    Middle,
    Max,
    Stretch
}

public enum ControlType
{
    [ControlType("Button","button")]
    Button,
    [ControlType("Label", "label")]
    Label,
    [ControlType("Toggle", "toggle")]
    Toggle,
    [ControlType("TextField", "textfield")]
    TextField,
}

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class ControlTypeAttribute : System.Attribute
{
    public string defaultLabel;
    public string defaultStyle;
    public float defaultWidth;
    public float defaultHeight;

    public ControlTypeAttribute(string defaultLabel, string defaultStyle)
    {
        this.defaultWidth = 100;
        this.defaultHeight = 20;
        this.defaultLabel = defaultLabel;
        this.defaultStyle = defaultStyle;
    }

    public ControlTypeAttribute(string defaultLabel, string defaultStyle, float defaultWidth,
        float defaultHeight)
    {
        this.defaultWidth = defaultWidth;
        this.defaultHeight = defaultHeight;
        this.defaultLabel = defaultLabel;
        this.defaultStyle = defaultStyle;
    }
}

[System.Serializable]
public class ControlElement : TreeElement
{
    public ControlType type;

    public Vector2 position;
    public Vector2 size = new Vector2(100,20);
    //public 
    //public float x;
    //public float y;
    //public float width;
    //public float height;
    //public float left;
    //public float right;
    //public float top;
    //public float down;
    public string style;
    public Vector2 pivot;

    public ControlLayoutMode layoutModeH = ControlLayoutMode.Middle;
    public ControlLayoutMode layoutModeV = ControlLayoutMode.Middle;

    //public ControlElement(ControlType type, string label, int depth, int id) : base(label, depth, id)
    //{
    //    this.type = type;

    //}

    //public abstract string GenerateCode(int indent);
}

[System.Serializable]
public class ButtonElement : ControlElement
{
    //public ButtonElement(ControlType type, string label, int depth, int id) : base(type, label, depth, id)
    //{
    //}

    //public override string GenerateCode(int indent)
    //{
    //    return "";
    //    //if(GUI.Button())
    //}
}