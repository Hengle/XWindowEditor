using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class XEditorTest : EditorWindow {

    [MenuItem("Test/XEditor")]
    static void Init()
    {
        XEditorTest win = XEditorTest.GetWindow<XEditorTest>();
    }

    void OnGUI()
    {
        DrawTBtn(new Rect(20, 20, 100, 25));
    }

    void DrawTBtn(Rect rect)
    {
        GUI.Button(rect, "xxx");

        Handles.DrawLine(new Vector3(rect.x, rect.y, 0), new Vector3(rect.x + rect.width, rect.y, 0));
        Handles.DrawLine(new Vector3(rect.x, rect.y, 0), new Vector3(rect.x, rect.y + rect.height, 0));
        Handles.DrawLine(new Vector3(rect.x + rect.width, rect.y, 0), new Vector3(rect.x + rect.width, rect.y + rect.height, 0));
        Handles.DrawLine(new Vector3(rect.x, rect.y + rect.height, 0), new Vector3(rect.x + rect.width, rect.y + rect.height, 0));

        DrawPoint(new Vector2(rect.x, rect.y));
        DrawPoint(new Vector2(rect.x + rect.width, rect.y));
        DrawPoint(new Vector2(rect.x + rect.width, rect.y + rect.height));
        DrawPoint(new Vector2(rect.x, rect.y + rect.height));

        DrawPoint(new Vector2(rect.x + rect.width*0.5f, rect.y));
        DrawPoint(new Vector2(rect.x, rect.y + rect.height*0.5f));
        DrawPoint(new Vector2(rect.x + rect.width, rect.y + rect.height*0.5f));
        DrawPoint(new Vector2(rect.x + rect.width*0.5f, rect.y + rect.height));
    }

    void DrawPoint(Vector2 position)
    {
        Handles.DrawLine(new Vector3(position.x - 2, position.y - 2, 0), new Vector3(position.x + 2, position.y - 2, 0));
        Handles.DrawLine(new Vector3(position.x - 2, position.y - 2, 0), new Vector3(position.x - 2, position.y + 2, 0));
        Handles.DrawLine(new Vector3(position.x - 2, position.y + 2, 0), new Vector3(position.x + 2, position.y + 2, 0));
        Handles.DrawLine(new Vector3(position.x + 2, position.y - 2, 0), new Vector3(position.x + 2, position.y + 2, 0));
    }
}
