using UnityEngine;
using System.Collections;
using UnityEditor;

public static class XWinGUI
{
    private class PanelInfo
    {
        public bool isDraggingLeft;
        public bool isDraggingRight;
        public bool isDraggingTop;
        public bool isDraggingBottom;

        public void ClearDrag()
        {
            isDraggingBottom = false;
            isDraggingLeft = false;
            isDraggingRight = false;
            isDraggingTop = false;
        }
    }

    private class ControlPreviewInfo
    {
        public bool isDragging;
    }

    private class ControlInfo
    {
        public bool isDraggingLeft;
        public bool isDraggingRight;
        public bool isDraggingTop;
        public bool isDraggingBottom;
        public bool isDragging;

        public void ClearDrag()
        {
            isDraggingBottom = false;
            isDraggingLeft = false;
            isDraggingRight = false;
            isDraggingTop = false;
            isDragging = false;
        }
    }

    public static Rect Panel(Rect rect, bool resizeLeft, bool resizeRight, bool resizeTop, bool resizeBottom,
        GUIContent content, GUIStyle style)
    {
        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        var state = (PanelInfo) GUIUtility.GetStateObject(typeof(PanelInfo), controlID);
        var result = rect;
        var resizeLeftRect = new Rect(rect.x, rect.y, 8, rect.height);
        var resizeRightRect = new Rect(rect.x + rect.width - 8, rect.y, 8, rect.height);
        var resizeTopRect = new Rect(rect.x, rect.y, rect.width, 8);
        var resizeBottomRect = new Rect(rect.x, rect.y + rect.height - 8, rect.width, 8);

        bool resizeL = (resizeLeft && resizeLeftRect.Contains(Event.current.mousePosition));
        bool resizeR = (resizeRight && resizeRightRect.Contains(Event.current.mousePosition));
        bool resizeT = (resizeTop && resizeTopRect.Contains(Event.current.mousePosition));
        bool resizeB = (resizeBottom && resizeBottomRect.Contains(Event.current.mousePosition));

        if (resizeL && resizeT)
        {
            EditorGUIUtility.AddCursorRect(new Rect(resizeLeftRect), MouseCursor.ResizeUpLeft);
            EditorGUIUtility.AddCursorRect(new Rect(resizeTopRect), MouseCursor.ResizeUpLeft);
        }
        else if (resizeL && resizeB)
        {
            EditorGUIUtility.AddCursorRect(new Rect(resizeLeftRect), MouseCursor.ResizeUpRight);
            EditorGUIUtility.AddCursorRect(new Rect(resizeBottomRect), MouseCursor.ResizeUpRight);
        }
        else if (resizeR && resizeT)
        {
            EditorGUIUtility.AddCursorRect(new Rect(resizeRightRect), MouseCursor.ResizeUpRight);
            EditorGUIUtility.AddCursorRect(new Rect(resizeTopRect), MouseCursor.ResizeUpRight);
        }
        else if (resizeR && resizeB)
        {
            EditorGUIUtility.AddCursorRect(new Rect(resizeRightRect), MouseCursor.ResizeUpLeft);
            EditorGUIUtility.AddCursorRect(new Rect(resizeBottomRect), MouseCursor.ResizeUpLeft);
        }
        else if (resizeL)
        {
            EditorGUIUtility.AddCursorRect(new Rect(resizeLeftRect), MouseCursor.ResizeHorizontal);
        }
        else if (resizeR)
        {
            EditorGUIUtility.AddCursorRect(new Rect(resizeRightRect), MouseCursor.ResizeHorizontal);
        }
        else if (resizeT)
        {
            EditorGUIUtility.AddCursorRect(new Rect(resizeTopRect), MouseCursor.ResizeVertical);
        }
        else if (resizeB)
        {
            EditorGUIUtility.AddCursorRect(new Rect(resizeBottomRect), MouseCursor.ResizeVertical);
        }

        switch (Event.current.GetTypeForControl(controlID))
        {
            case EventType.Repaint:
                
                style.Draw(rect, content, false, false, false, false);
                break;
            case EventType.MouseDown:
                bool dragging = false;
                if (resizeLeft && resizeLeftRect.Contains(Event.current.mousePosition) && Event.current.button == 0 &&
                    GUIUtility.hotControl == 0)
                {
                    state.isDraggingLeft = true;
                    dragging = true;
                }
                if (resizeRight && resizeRightRect.Contains(Event.current.mousePosition) && Event.current.button == 0 &&
                    GUIUtility.hotControl == 0)
                {
                    state.isDraggingRight = true;
                    dragging = true;
                }
                if (resizeTop && resizeTopRect.Contains(Event.current.mousePosition) && Event.current.button == 0 &&
                    GUIUtility.hotControl == 0)
                {
                    state.isDraggingTop = true;
                    dragging = true;
                }
                if (resizeBottom && resizeBottomRect.Contains(Event.current.mousePosition) && Event.current.button == 0 &&
                    GUIUtility.hotControl == 0)
                {
                    state.isDraggingBottom = true;
                    dragging = true;
                }
                if (dragging)
                    GUIUtility.hotControl = controlID;
                break;
            case EventType.MouseUp:
                if (GUIUtility.hotControl == controlID)
                {
                    GUIUtility.hotControl = 0;
                    if (state != null) state.ClearDrag();
                }

                break;
            case EventType.MouseDrag:
                if (GUIUtility.hotControl == controlID)
                {
                    bool dragend = false;
                    if (state.isDraggingTop)
                    {
                        result.y = Event.current.mousePosition.y - 4; dragend = true;
                    }
                    if (state.isDraggingBottom)
                    {
                        result.height = Event.current.mousePosition.y - result.y + 4; dragend = true;
                    }
                    if (state.isDraggingLeft)
                    {
                        result.x = Event.current.mousePosition.x - 4; dragend = true;
                    }
                    if (state.isDraggingRight)
                    {
                        result.width = Event.current.mousePosition.x - result.x + 4; dragend = true;
                    }

                    if (dragend)
                        Event.current.Use();
                }
                break;
        }

        return result;
    }


    public static bool ControlPreview(Rect rect, Rect panelRect, GUIContent content, GUIStyle style)
    {
        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        var state = (ControlPreviewInfo)GUIUtility.GetStateObject(typeof(ControlPreviewInfo), controlID);

        switch (Event.current.GetTypeForControl(controlID))
        {
            case EventType.Repaint:
                style.Draw(rect, content, false, false, state.isDragging, false);
                break;
            case EventType.MouseDown:
                if (!state.isDragging && rect.Contains(Event.current.mousePosition) && Event.current.button == 0 &&
                    GUIUtility.hotControl == 0)
                {
                    if (state != null) state.isDragging = true;
                    GUIUtility.hotControl = controlID;
                }
                break;
            case EventType.MouseUp:
                if (GUIUtility.hotControl == controlID)
                {
                    GUIUtility.hotControl = 0;
                    if (state != null && state.isDragging)
                    {
                        state.isDragging = false;
                        if (panelRect.Contains(Event.current.mousePosition))
                        {
                            Event.current.Use();
                            return true;
                        }
                    }
                }
                break;
            case EventType.MouseDrag:
                if (GUIUtility.hotControl == controlID)
                {
                        Event.current.Use();
                }
                break;
        }

        return false;
    }

    public static bool Control(ControlElement element, bool selected, bool align, GUIContent content, GUIStyle style)
    {
        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        var state = (ControlInfo)GUIUtility.GetStateObject(typeof(ControlInfo), controlID);
        var rect = new Rect(element.position.x, element.position.y, element.size.x, element.size.y);
        var result = rect;
        var resizeLeftRect = new Rect(rect.x, rect.y, 8, rect.height);
        var resizeRightRect = new Rect(rect.x + rect.width - 8, rect.y, 8, rect.height);
        var resizeTopRect = new Rect(rect.x, rect.y, rect.width, 8);
        var resizeBottomRect = new Rect(rect.x, rect.y + rect.height - 8, rect.width, 8);

        bool resizeL = (selected && resizeLeftRect.Contains(Event.current.mousePosition));
        bool resizeR = (selected && resizeRightRect.Contains(Event.current.mousePosition));
        bool resizeT = (selected && resizeTopRect.Contains(Event.current.mousePosition));
        bool resizeB = (selected && resizeBottomRect.Contains(Event.current.mousePosition));

        if (resizeL && resizeT)
        {
            EditorGUIUtility.AddCursorRect(new Rect(resizeLeftRect), MouseCursor.ResizeUpLeft);
            EditorGUIUtility.AddCursorRect(new Rect(resizeTopRect), MouseCursor.ResizeUpLeft);
        }
        else if (resizeL && resizeB)
        {
            EditorGUIUtility.AddCursorRect(new Rect(resizeLeftRect), MouseCursor.ResizeUpRight);
            EditorGUIUtility.AddCursorRect(new Rect(resizeBottomRect), MouseCursor.ResizeUpRight);
        }
        else if (resizeR && resizeT)
        {
            EditorGUIUtility.AddCursorRect(new Rect(resizeRightRect), MouseCursor.ResizeUpRight);
            EditorGUIUtility.AddCursorRect(new Rect(resizeTopRect), MouseCursor.ResizeUpRight);
        }
        else if (resizeR && resizeB)
        {
            EditorGUIUtility.AddCursorRect(new Rect(resizeRightRect), MouseCursor.ResizeUpLeft);
            EditorGUIUtility.AddCursorRect(new Rect(resizeBottomRect), MouseCursor.ResizeUpLeft);
        }
        else if (resizeL)
        {
            EditorGUIUtility.AddCursorRect(new Rect(resizeLeftRect), MouseCursor.ResizeHorizontal);
        }
        else if (resizeR)
        {
            EditorGUIUtility.AddCursorRect(new Rect(resizeRightRect), MouseCursor.ResizeHorizontal);
        }
        else if (resizeT)
        {
            EditorGUIUtility.AddCursorRect(new Rect(resizeTopRect), MouseCursor.ResizeVertical);
        }
        else if (resizeB)
        {
            EditorGUIUtility.AddCursorRect(new Rect(resizeBottomRect), MouseCursor.ResizeVertical);
        }

        switch (Event.current.GetTypeForControl(controlID))
        {
            case EventType.Repaint:

                style.Draw(rect, content, false, false, false, false);
                break;
            case EventType.MouseDown:
                bool dragging = false;
                if (selected && resizeLeftRect.Contains(Event.current.mousePosition) && Event.current.button == 0 &&
                    GUIUtility.hotControl == 0)
                {
                    state.isDraggingLeft = true;
                    dragging = true;
                }
                if (selected && resizeRightRect.Contains(Event.current.mousePosition) && Event.current.button == 0 &&
                    GUIUtility.hotControl == 0)
                {
                    state.isDraggingRight = true;
                    dragging = true;
                }
                if (selected && resizeTopRect.Contains(Event.current.mousePosition) && Event.current.button == 0 &&
                    GUIUtility.hotControl == 0)
                {
                    state.isDraggingTop = true;
                    dragging = true;
                }
                if (selected && resizeBottomRect.Contains(Event.current.mousePosition) && Event.current.button == 0 &&
                    GUIUtility.hotControl == 0)
                {
                    state.isDraggingBottom = true;
                    dragging = true;
                }

                if (!dragging && rect.Contains(Event.current.mousePosition) && Event.current.button == 0 &&
                    GUIUtility.hotControl == 0)
                {
                    state.isDragging = true;
                    dragging = true;
                    selected = true;
                }
                if (dragging)
                    GUIUtility.hotControl = controlID;
                break;
            case EventType.MouseUp:
                if (GUIUtility.hotControl == controlID)
                {
                    GUIUtility.hotControl = 0;
                    if (state != null) state.ClearDrag();
                }

                break;
            case EventType.MouseDrag:
                if (GUIUtility.hotControl == controlID)
                {
                    bool dragend = false;
                    if (state.isDraggingTop)
                    {
                        float h = result.y + result.height;
                        result.y = Event.current.mousePosition.y - 4;
                        result.height = h - result.y;
                        dragend = true;
                    }
                    if (state.isDraggingBottom)
                    {
                        result.height = Event.current.mousePosition.y - result.y + 4; dragend = true;
                    }
                    if (state.isDraggingLeft)
                    {
                        float w = result.x + result.width;
                        result.x = Event.current.mousePosition.x - 4;
                        result.width = w - result.x;
                        dragend = true;
                    }
                    if (state.isDraggingRight)
                    {
                        result.width = Event.current.mousePosition.x - result.x + 4; dragend = true;
                    }

                    if (state.isDragging)
                    {
                        result.x += Event.current.delta.x;
                        result.y += Event.current.delta.y;
                        dragend = true;
                        selected = true;
                    }

                    if (dragend)
                    {
                        if(align)
                            result = CheckAlign(element, result);
                        Event.current.Use();
                    }
                }
                break;
        }
        if (selected)
            DrawResizeBoarder(rect);

        element.position.x = result.x;
        element.position.y = result.y;
        element.size.x = result.width;
        element.size.y = result.height;

        return selected;
    }

    private static Rect CheckAlign(ControlElement element, Rect rect)
    {
        if (element.parent != null)
        {
            if (element.parent.hasChildren)
            {
                for (int i = 0; i < element.parent.children.Count; i++)
                {
                    if(element.parent.children[i] == element)
                        continue;
                    var brother = (ControlElement)element.parent.children[i];
                    rect = CheckAlignHorizontal(brother, rect);
                    rect = CheckAlignVertical(brother, rect);
                }
            }
        }

        return rect;
    }

    private static Rect CheckAlignHorizontal(ControlElement brother, Rect rect)
    {
        float deltaLToL = Mathf.Abs(rect.x - brother.position.x);
        float deltaRToR = Mathf.Abs(rect.x + rect.width - (brother.position.x + brother.size.x));
        if (deltaRToR <= deltaLToL && deltaRToR < 2)
        {
            rect.x = brother.position.x + brother.size.x - rect.width;
            return rect;
        }else if (deltaLToL <= deltaRToR && deltaLToL < 2)
        {
            rect.x = brother.position.x;
            return rect;
        }

        float deltaLToR = Mathf.Abs(rect.x - (brother.position.x + brother.size.x));
        if (deltaLToR < 2)
        {
            rect.x = brother.position.x + brother.size.x;
            return rect;
        }

        float deltaRToL = Mathf.Abs(rect.x + rect.width - brother.position.x);
        if (deltaRToL < 2)
        {
            rect.x = brother.position.x - rect.width;
            return rect;
        }

        return rect;
    }

    private static Rect CheckAlignVertical(ControlElement brother, Rect rect)
    {
        float deltaTToT = Mathf.Abs(rect.y - brother.position.y);
        float deltaBToB = Mathf.Abs(rect.y + rect.height - (brother.position.y + brother.size.y));
        if (deltaBToB <= deltaTToT && deltaBToB < 2)
        {
            rect.y = brother.position.y + brother.size.y - rect.height;
            return rect;
        }
        else if (deltaTToT <= deltaBToB && deltaTToT < 2)
        {
            rect.y = brother.position.y;
            return rect;
        }

        float deltaTToB = Mathf.Abs(rect.y - (brother.position.y + brother.size.y));
        if (deltaTToB < 2)
        {
            rect.y = brother.position.y + brother.size.y;
            return rect;
        }

        float deltaBToT = Mathf.Abs(rect.y + rect.height - brother.position.y);
        if (deltaBToT < 2)
        {
            rect.y = brother.position.y - rect.height;
            return rect;
        }

        return rect;
    }

    private static void DrawResizeBoarder(Rect rect)
    {
        Handles.DrawLine(new Vector3(rect.x, rect.y, 0), new Vector3(rect.x + rect.width, rect.y, 0));
        Handles.DrawLine(new Vector3(rect.x, rect.y, 0), new Vector3(rect.x, rect.y + rect.height, 0));
        Handles.DrawLine(new Vector3(rect.x + rect.width, rect.y, 0), new Vector3(rect.x + rect.width, rect.y + rect.height, 0));
        Handles.DrawLine(new Vector3(rect.x, rect.y + rect.height, 0), new Vector3(rect.x + rect.width, rect.y + rect.height, 0));

        DrawPoint(new Vector2(rect.x, rect.y));
        DrawPoint(new Vector2(rect.x + rect.width, rect.y));
        DrawPoint(new Vector2(rect.x + rect.width, rect.y + rect.height));
        DrawPoint(new Vector2(rect.x, rect.y + rect.height));

        DrawPoint(new Vector2(rect.x + rect.width * 0.5f, rect.y));
        DrawPoint(new Vector2(rect.x, rect.y + rect.height * 0.5f));
        DrawPoint(new Vector2(rect.x + rect.width, rect.y + rect.height * 0.5f));
        DrawPoint(new Vector2(rect.x + rect.width * 0.5f, rect.y + rect.height));
    }

    private static void DrawPoint(Vector2 position)
    {
        Handles.DrawLine(new Vector3(position.x - 2, position.y - 2, 0), new Vector3(position.x + 2, position.y - 2, 0));
        Handles.DrawLine(new Vector3(position.x - 2, position.y - 2, 0), new Vector3(position.x - 2, position.y + 2, 0));
        Handles.DrawLine(new Vector3(position.x - 2, position.y + 2, 0), new Vector3(position.x + 2, position.y + 2, 0));
        Handles.DrawLine(new Vector3(position.x + 2, position.y - 2, 0), new Vector3(position.x + 2, position.y + 2, 0));
    }
}

public static class XWinGUILayout
{
    public static bool ControlPreview(Rect panelRect, GUIContent content, GUIStyle style)
    {
        Rect position = GUILayoutUtility.GetRect(GUIContent.none, style);
        return XWinGUI.ControlPreview(position, panelRect, content, style);
    }

    public static bool ControlPreview(Rect panelRect, GUIContent content, GUIStyle style, params GUILayoutOption[] opts)
    {
        Rect position = GUILayoutUtility.GetRect(GUIContent.none, style, opts);
        return XWinGUI.ControlPreview(position, panelRect, content, style);
    }
}