using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class XWindowEditor : EditorWindow {

    class Styles
    {
        public GUIStyle dragTabDropWindow = "dragtabdropwindow";
    }

    private static Styles s_Styles;

    private static Styles styles
    {
        get
        {
            if (s_Styles == null)
                s_Styles = new Styles();
            return s_Styles;
        }
    }

    [MenuItem("Test/Open")]
    static void Open()
    {
        XWindowEditor window = XWindowEditor.GetWindow<XWindowEditor>();
    }

    void OnEnable()
    {
        
    }

    void OnDestroy()
    {
        
    }

    void OnGUI()
    {
        DrawItemListView(new Rect(0, 0, 250, 300));
        DrawControlListView(new Rect(0, 300, 250, position.height-300));
        DrawEditorPanel(new Rect(250, 0, position.width-500, position.height));
        DrawAttributeView(new Rect(position.width - 250, 0, 250, position.height));
    }

    private void DrawItemListView(Rect rect)
    {
        GUI.Box(rect, "Items", styles.dragTabDropWindow);
    }

    private void DrawControlListView(Rect rect)
    {
        GUI.Box(rect, "Controls", styles.dragTabDropWindow);
    }

    private void DrawEditorPanel(Rect rect)
    {
        GUI.Box(rect, "Editor", styles.dragTabDropWindow);
    }

    private void DrawAttributeView(Rect rect)
    {
        GUI.Box(rect, "Attribute", styles.dragTabDropWindow);
    }
}
