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