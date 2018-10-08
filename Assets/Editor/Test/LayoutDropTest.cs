using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public class LayoutDropTest : EditorWindow {

    private class Styles
    {
        public Color tableHeaderColor;

        public Color tableLineColor;

        public Color parentColor;

        public Color selfColor;

        public Color simpleAnchorColor;

        public Color stretchAnchorColor;

        public Color anchorCornerColor;

        public Color pivotColor;

        public GUIStyle frame;

        public GUIStyle label = new GUIStyle(EditorStyles.miniLabel);

        public Styles()
        {
            this.frame = new GUIStyle();
            Texture2D texture2D = new Texture2D(4, 4);
            texture2D.SetPixels(new Color[]
            {
                    Color.white,
                    Color.white,
                    Color.white,
                    Color.white,
                    Color.white,
                    Color.clear,
                    Color.clear,
                    Color.white,
                    Color.white,
                    Color.clear,
                    Color.clear,
                    Color.white,
                    Color.white,
                    Color.white,
                    Color.white,
                    Color.white
            });
            texture2D.filterMode = FilterMode.Point;
            texture2D.Apply();
            texture2D.hideFlags = HideFlags.HideAndDontSave;
            this.frame.normal.background = texture2D;
            this.frame.border = new RectOffset(2, 2, 2, 2);
            this.label.alignment = TextAnchor.LowerCenter;
            if (EditorGUIUtility.isProSkin)
            {
                this.tableHeaderColor = new Color(0.18f, 0.18f, 0.18f, 1f);
                this.tableLineColor = new Color(1f, 1f, 1f, 0.3f);
                this.parentColor = new Color(0.4f, 0.4f, 0.4f, 1f);
                this.selfColor = new Color(0.6f, 0.6f, 0.6f, 1f);
                this.simpleAnchorColor = new Color(0.7f, 0.3f, 0.3f, 1f);
                this.stretchAnchorColor = new Color(0f, 0.6f, 0.8f, 1f);
                this.anchorCornerColor = new Color(0.8f, 0.6f, 0f, 1f);
                this.pivotColor = new Color(0f, 0.6f, 0.8f, 1f);
            }
            else
            {
                this.tableHeaderColor = new Color(0.8f, 0.8f, 0.8f, 1f);
                this.tableLineColor = new Color(0f, 0f, 0f, 0.5f);
                this.parentColor = new Color(0.55f, 0.55f, 0.55f, 1f);
                this.selfColor = new Color(0.2f, 0.2f, 0.2f, 1f);
                this.simpleAnchorColor = new Color(0.8f, 0.3f, 0.3f, 1f);
                this.stretchAnchorColor = new Color(0.2f, 0.5f, 0.9f, 1f);
                this.anchorCornerColor = new Color(0.6f, 0.4f, 0f, 1f);
                this.pivotColor = new Color(0.2f, 0.5f, 0.9f, 1f);
            }
        }
    }

    public enum LayoutMode
    {
        Undefined = -1,
        Min,
        Middle,
        Max,
        Stretch
    }

    private static string[] kVLabels = new string[]
    {
        "custom",
        "top",
        "middle",
        "bottom",
        "stretch",
        "%"
    };

    private static string[] kHLabels = new string[]
    {
        "custom",
        "left",
        "center",
        "right",
        "stretch",
        "%"
    };

    private static float[] kPivotsForModes = new float[]
    {
        0f,
        0.5f,
        1f,
        0.5f,
        0.5f
    };

    private static Styles sStyles;

    private static Styles s_Styles
    {
        get
        {
            if(sStyles == null)
                sStyles = new Styles();
            return sStyles;
        }
    }

    //private Vector2 m_AnchorMin = new Vector2(0.5f, 0.5f);
    //private Vector2 m_AnchorMax = new Vector2(0.5f, 0.5f);

    private LayoutMode m_LayoutMode = LayoutMode.Middle;
    private LayoutMode m_LayoutModeForAxis = LayoutMode.Middle;

    //private MethodInfo m_DrawLayoutMode;
    //private MethodInfo m_DrawLayoutModeHeaderOutsideRect;

    private MyLayoutData m_Data;

    [MenuItem("Test/LayoutDrop")]
    static void Init()
    {
        LayoutDropTest.GetWindow<LayoutDropTest>();
    }

    void OnEnable()
    {
        m_Data = new MyLayoutData();
    }

    void OnGUI()
    {
        Rect rect = new Rect(20, 17, 49, 49);
        if (EditorGUI.DropdownButton(rect, GUIContent.none, FocusType.Passive, "box"))
        {
            GUIUtility.keyboardControl = 0;
            //this.m_DropdownWindow = new LayoutDropdownWindow(base.serializedObject);
            var dropWindow = new MyLayoutDropdownWindow(m_Data);
            PopupWindow.Show(rect, dropWindow);
        }
        MyLayoutDropdownWindow.DrawLayoutMode(new RectOffset(7, 7, 7, 7).Remove(rect), this.m_Data.layoutModeForAxis, this.m_Data.layoutMode);
        MyLayoutDropdownWindow.DrawLayoutModeHeadersOutsideRect(rect, this.m_Data.layoutModeForAxis, this.m_Data.layoutMode);
        //MyLayoutDropdownWindow.DrawLayoutModeHeadersOutsideRect(rect, this.m_AnchorMin, this.m_AnchorMax, this.m_AnchoredPosition, this.m_SizeDelta);
        //DropGUI(new Rect(0, 0, 700, 700));
        //if (GUI.Button(new Rect(0, 0, 100, 20), "Drop"))
        //{
        //    Drop();
        //}
    }

    private void Drop()
    {
        Assembly assembly = typeof(Editor).Assembly;
        
        System.Type tp = assembly.GetType("UnityEditor.LayoutDropdownWindow");
        System.Type mode = tp.GetNestedType("LayoutMode");
        Debug.Log(mode);
        MethodInfo method = tp.GetMethod("DrawLayoutMode", BindingFlags.NonPublic | BindingFlags.Static, null,
            new Type[] {typeof(Rect), mode, mode}, null);
        Debug.Log(method);

        method.Invoke(null,
            new object[] {new Rect(0, 0, 500, 500), 0, 0});
    }

    private void DropGUI(Rect rect)
    {
        GUI.Label(new Rect(rect.x + 5f, rect.y + 3f, rect.width - 10f, 16f), new GUIContent("Anchor Presets"), EditorStyles.boldLabel);
        //GUI.Label(new Rect(rect.x + 5f, rect.y + 3f + 16f, rect.width - 10f, 16f), new GUIContent("Shift: Also set pivot     Alt: Also set position"), EditorStyles.label);
        Color color = GUI.color;
        GUI.color = new Color(1f, 1f, 1f, 0.3f) * color;
        GUI.DrawTexture(new Rect(0f, 21f, 400f, 1f), EditorGUIUtility.whiteTexture);
        GUI.color = color;
        GUI.BeginGroup(new Rect(rect.x, rect.y + 22f, rect.width, rect.height - 22f));
        this.TableGUI(rect);
        GUI.EndGroup();
    }

    private void TableGUI(Rect rect)
    {
        int num = 6;
        int num2 = 31 + num * 2;
        int num3 = 0;
        int[] array = new int[]
        {
        15,
        30,
        30,
        30,
        45,
        45
        };
        Color color = GUI.color;
        int num4 = 62;
        GUI.color = new Color(0.18f, 0.18f, 0.18f, 1f) * color;
        GUI.DrawTexture(new Rect(0f, 0f, 400f, (float)num4), EditorGUIUtility.whiteTexture);
        GUI.DrawTexture(new Rect(0f, 0f, (float)num4, 400f), EditorGUIUtility.whiteTexture);
        GUI.color = new Color(1f, 1f, 1f, 0.3f) * color;
        GUI.DrawTexture(new Rect(0f, (float)num4, 400f, 1f), EditorGUIUtility.whiteTexture);
        GUI.DrawTexture(new Rect((float)num4, 0f, 1f, 400f), EditorGUIUtility.whiteTexture);
        GUI.color = color;
        //LayoutMode layoutModeForAxis = GetLayoutModeForAxis(this.m_AnchorMin, this.m_AnchorMax, 0);
        //LayoutMode layoutMode = GetLayoutModeForAxis(this.m_AnchorMin, this.m_AnchorMax, 1);
        //layoutMode = SwappedVMode(layoutMode);
        //Debug.Log(layoutModeForAxis+","+layoutMode);
        LayoutMode layoutModeForAxis = m_LayoutModeForAxis;
        LayoutMode layoutMode = m_LayoutMode;
        //bool shift = Event.current.shift;
        //bool alt = Event.current.alt;
        int num5 = 5;
        for (int i = 0; i < num5; i++)
        {
            LayoutMode layoutMode2 = (LayoutMode)(i - 1);
            for (int j = 0; j < num5; j++)
            {
                LayoutMode layoutMode3 = (LayoutMode)(j - 1);
                if (i != 0 || j != 0 || layoutMode < LayoutMode.Min || layoutModeForAxis < LayoutMode.Min)
                {
                    Rect position = new Rect((float)(i * (num2 + num3) + array[i]), (float)(j * (num2 + num3) + array[j]), (float)num2, (float)num2);
                    if (j == 0 && (i != 0 || layoutModeForAxis == LayoutMode.Undefined))
                    {
                        DrawLayoutModeHeaderOutsideRect(position, 0, layoutMode2);
                    }
                    if (i == 0 && (j != 0 || layoutMode == LayoutMode.Undefined))
                    {
                        DrawLayoutModeHeaderOutsideRect(position, 1, layoutMode3);
                    }
                    bool flag = layoutMode2 == layoutModeForAxis && layoutMode3 == layoutMode;
                    bool flag2 = false;
                    if (i == 0 && layoutMode3 == layoutMode)
                    {
                        flag2 = true;
                    }
                    if (j == 0 && layoutMode2 == layoutModeForAxis)
                    {
                        flag2 = true;
                    }
                    if (Event.current.type == EventType.Repaint)
                    {
                        if (flag)
                        {
                            GUI.color = Color.white * color;
                            s_Styles.frame.Draw(position, false, false, false, false);
                        }
                        else if (flag2)
                        {
                            GUI.color = new Color(1f, 1f, 1f, 0.7f) * color;
                            s_Styles.frame.Draw(position, false, false, false, false);
                        }
                    }
                    DrawLayoutMode(new Rect(position.x + (float)num, position.y + (float)num, position.width - (float)(num * 2), position.height - (float)(num * 2)), layoutMode2, layoutMode3);
                    //int clickCount = Event.current.clickCount;
                    if (GUI.Button(position, GUIContent.none, GUIStyle.none))
                    {
                        m_LayoutModeForAxis = layoutMode2;
                        m_LayoutMode = layoutMode3;
                        //Debug.Log(layoutMode2);
                        //Debug.Log(layoutMode3);
                        //SetLayoutModeForAxis(ref this.m_AnchorMin, 0, layoutMode2, shift, alt, this.m_InitValues);
                        //SetLayoutModeForAxis(ref this.m_AnchorMin, 1, SwappedVMode(layoutMode3), shift, alt, this.m_InitValues);
                        //if (clickCount == 2)
                        //{
                        //    base.editorWindow.Close();
                        //}
                        //else
                        //{
                        //    base.editorWindow.Repaint();
                        //}
                    }
                }
            }
        }
        GUI.color = color;
    }

    //private void DrawLayoutMode(Rect rect, LayoutMode hMode, LayoutMode vMode, bool doPivot, bool doPosition)
    //{
    //    if (m_DrawLayoutMode == null)
    //    {
    //        Assembly assembly = typeof(Editor).Assembly;

    //        System.Type tp = assembly.GetType("UnityEditor.LayoutDropdownWindow");
    //        System.Type mode = tp.GetNestedType("LayoutMode");
    //        m_DrawLayoutMode = tp.GetMethod("DrawLayoutMode", BindingFlags.NonPublic | BindingFlags.Static, null,
    //            new Type[] {typeof(Rect), mode, mode, typeof(bool), typeof(bool)}, null);

    //        int hModeV = (int) hMode;
    //        int vModeV = (int) vMode;
    //        if (m_DrawLayoutMode != null)
    //            m_DrawLayoutMode.Invoke(null,
    //                new object[] {rect, hModeV, vModeV, doPivot, doPosition});
    //    }
    //}

    private void DrawLayoutModeHeaderOutsideRect(Rect rect, int axis, LayoutMode mode)
    {
        Rect position2 = new Rect(rect.x, rect.y - 16f, rect.width, 16f);
        Matrix4x4 matrix = GUI.matrix;
        if (axis == 1)
        {
            GUIUtility.RotateAroundPivot(-90f, rect.center);
        }
        int num = (int)(mode + 1);
        GUI.Label(position2, (axis != 0) ? kVLabels[num] : kHLabels[num], s_Styles.label);
        GUI.matrix = matrix;

        //if (m_DrawLayoutModeHeaderOutsideRect == null)
        //{
        //    Assembly assembly = typeof(Editor).Assembly;

        //    System.Type tp = assembly.GetType("UnityEditor.LayoutDropdownWindow");
        //    System.Type modeType = tp.GetNestedType("LayoutMode");
        //    m_DrawLayoutModeHeaderOutsideRect = tp.GetMethod("DrawLayoutModeHeaderOutsideRect",
        //        BindingFlags.NonPublic | BindingFlags.Static, null,
        //        new Type[] {typeof(Rect), typeof(int), modeType}, null);

        //    int modeV = (int)mode;
        //    if (m_DrawLayoutModeHeaderOutsideRect != null)
        //        m_DrawLayoutModeHeaderOutsideRect.Invoke(null,
        //            new object[] { rect, axis, modeV});
        //}
    }

    private static void DrawLayoutMode(Rect position, LayoutMode hMode, LayoutMode vMode)
    {
        //if (sStyles == null)
        //{
        //    sStyles = new Styles();
        //}
        Color color = GUI.color;
        int num = (int)Mathf.Min(position.width, position.height);
        if (num % 2 == 0)
        {
            num--;
        }
        int num2 = num / 2;
        if (num2 % 2 == 0)
        {
            num2++;
        }
        Vector2 b = (float)num * Vector2.one;
        Vector2 b2 = (float)num2 * Vector2.one;
        Vector2 vector = (position.size - b) / 2f;
        vector.x = Mathf.Floor(vector.x);
        vector.y = Mathf.Floor(vector.y);
        Vector2 vector2 = (position.size - b2) / 2f;
        vector2.x = Mathf.Floor(vector2.x);
        vector2.y = Mathf.Floor(vector2.y);
        Rect position2 = new Rect(position.x + vector.x, position.y + vector.y, b.x, b.y);
        Rect position3 = new Rect(position.x + vector2.x, position.y + vector2.y, b2.x, b2.y);
        //if (doPosition)
        //{
        //    for (int i = 0; i < 2; i++)
        //    {
        //        LayoutMode layoutMode = (i != 0) ? vMode : hMode;
        //        if (layoutMode == LayoutMode.Min)
        //        {
        //            Vector2 center = position3.center;
        //            int index = default(int);
        //            center[index = i] = center[index] + (position2.min[i] - position3.min[i]);
        //            position3.center = center;
        //        }
        //        if (layoutMode == LayoutMode.Middle)
        //        {
        //        }
        //        if (layoutMode == LayoutMode.Max)
        //        {
        //            Vector2 center2 = position3.center;
        //            int index2 = default(int);
        //            center2[index2 = i] = center2[index2] + (position2.max[i] - position3.max[i]);
        //            position3.center = center2;
        //        }
        //        if (layoutMode == LayoutMode.Stretch)
        //        {
        //            Vector2 min = position3.min;
        //            Vector2 max = position3.max;
        //            min[i] = position2.min[i];
        //            max[i] = position2.max[i];
        //            position3.min = min;
        //            position3.max = max;
        //        }
        //    }
        //}
        Rect rect = default(Rect);
        Vector2 zero = Vector2.zero;
        Vector2 zero2 = Vector2.zero;
        for (int j = 0; j < 2; j++)
        {
            LayoutMode layoutMode2 = (j != 0) ? vMode : hMode;
            if (layoutMode2 == LayoutMode.Min)
            {
                zero[j] = position2.min[j] + 0.5f;
                zero2[j] = position2.min[j] + 0.5f;
            }
            if (layoutMode2 == LayoutMode.Middle)
            {
                zero[j] = position2.center[j];
                zero2[j] = position2.center[j];
            }
            if (layoutMode2 == LayoutMode.Max)
            {
                zero[j] = position2.max[j] - 0.5f;
                zero2[j] = position2.max[j] - 0.5f;
            }
            if (layoutMode2 == LayoutMode.Stretch)
            {
                zero[j] = position2.min[j] + 0.5f;
                zero2[j] = position2.max[j] - 0.5f;
            }
        }
        rect.min = zero;
        rect.max = zero2;
        if (Event.current.type == EventType.Repaint)
        {
            GUI.color = s_Styles.parentColor * color;
            s_Styles.frame.Draw(position2, false, false, false, false);
        }
        if (hMode != LayoutMode.Undefined && hMode != LayoutMode.Stretch)
        {
            GUI.color = s_Styles.simpleAnchorColor * color;
            GUI.DrawTexture(new Rect(rect.xMin - 0.5f, position2.y + 1f, 1f, position2.height - 2f), EditorGUIUtility.whiteTexture);
            GUI.DrawTexture(new Rect(rect.xMax - 0.5f, position2.y + 1f, 1f, position2.height - 2f), EditorGUIUtility.whiteTexture);
        }
        if (vMode != LayoutMode.Undefined && vMode != LayoutMode.Stretch)
        {
            GUI.color = s_Styles.simpleAnchorColor * color;
            GUI.DrawTexture(new Rect(position2.x + 1f, rect.yMin - 0.5f, position2.width - 2f, 1f), EditorGUIUtility.whiteTexture);
            GUI.DrawTexture(new Rect(position2.x + 1f, rect.yMax - 0.5f, position2.width - 2f, 1f), EditorGUIUtility.whiteTexture);
        }
        if (hMode == LayoutMode.Stretch)
        {
            GUI.color = s_Styles.stretchAnchorColor * color;
            DrawArrow(new Rect(position3.x + 1f, position3.center.y - 0.5f, position3.width - 2f, 1f));
        }
        if (vMode == LayoutMode.Stretch)
        {
            GUI.color = s_Styles.stretchAnchorColor * color;
            DrawArrow(new Rect(position3.center.x - 0.5f, position3.y + 1f, 1f, position3.height - 2f));
        }
        if (Event.current.type == EventType.Repaint)
        {
            GUI.color = s_Styles.selfColor * color;
            s_Styles.frame.Draw(position3, false, false, false, false);
        }
        //if (doPivot && hMode != LayoutMode.Undefined && vMode != LayoutMode.Undefined)
        //{
        //    Vector2 vector3 = new Vector2(Mathf.Lerp(position3.xMin + 0.5f, position3.xMax - 0.5f, kPivotsForModes[(int)hMode]), Mathf.Lerp(position3.yMin + 0.5f, position3.yMax - 0.5f, kPivotsForModes[(int)vMode]));
        //    GUI.color = s_Styles.pivotColor * color;
        //    GUI.DrawTexture(new Rect(vector3.x - 2.5f, vector3.y - 1.5f, 5f, 3f), EditorGUIUtility.whiteTexture);
        //    GUI.DrawTexture(new Rect(vector3.x - 1.5f, vector3.y - 2.5f, 3f, 5f), EditorGUIUtility.whiteTexture);
        //}
        if (hMode != LayoutMode.Undefined && vMode != LayoutMode.Undefined)
        {
            GUI.color = s_Styles.anchorCornerColor * color;
            GUI.DrawTexture(new Rect(rect.xMin - 1.5f, rect.yMin - 1.5f, 2f, 2f), EditorGUIUtility.whiteTexture);
            GUI.DrawTexture(new Rect(rect.xMax - 0.5f, rect.yMin - 1.5f, 2f, 2f), EditorGUIUtility.whiteTexture);
            GUI.DrawTexture(new Rect(rect.xMin - 1.5f, rect.yMax - 0.5f, 2f, 2f), EditorGUIUtility.whiteTexture);
            GUI.DrawTexture(new Rect(rect.xMax - 0.5f, rect.yMax - 0.5f, 2f, 2f), EditorGUIUtility.whiteTexture);
        }
        GUI.color = color;
    }

    private static void DrawArrow(Rect lineRect)
    {
        GUI.DrawTexture(lineRect, EditorGUIUtility.whiteTexture);
        if (lineRect.width == 1f)
        {
            GUI.DrawTexture(new Rect(lineRect.x - 1f, lineRect.y + 1f, 3f, 1f), EditorGUIUtility.whiteTexture);
            GUI.DrawTexture(new Rect(lineRect.x - 2f, lineRect.y + 2f, 5f, 1f), EditorGUIUtility.whiteTexture);
            GUI.DrawTexture(new Rect(lineRect.x - 1f, lineRect.yMax - 2f, 3f, 1f), EditorGUIUtility.whiteTexture);
            GUI.DrawTexture(new Rect(lineRect.x - 2f, lineRect.yMax - 3f, 5f, 1f), EditorGUIUtility.whiteTexture);
        }
        else
        {
            GUI.DrawTexture(new Rect(lineRect.x + 1f, lineRect.y - 1f, 1f, 3f), EditorGUIUtility.whiteTexture);
            GUI.DrawTexture(new Rect(lineRect.x + 2f, lineRect.y - 2f, 1f, 5f), EditorGUIUtility.whiteTexture);
            GUI.DrawTexture(new Rect(lineRect.xMax - 2f, lineRect.y - 1f, 1f, 3f), EditorGUIUtility.whiteTexture);
            GUI.DrawTexture(new Rect(lineRect.xMax - 3f, lineRect.y - 2f, 1f, 5f), EditorGUIUtility.whiteTexture);
        }
    }

    private static LayoutMode SwappedVMode(LayoutMode vMode)
    {
        LayoutMode result;
        if (vMode == LayoutMode.Min)
        {
            result = LayoutMode.Max;
        }
        else if (vMode == LayoutMode.Max)
        {
            result = LayoutMode.Min;
        }
        else
        {
            result = vMode;
        }
        return result;
    }

    private static LayoutMode GetLayoutModeForAxis(Vector2 anchorMin, Vector2 anchorMax, int axis)
    {
        LayoutMode result;
        if (anchorMin[axis] == 0f && anchorMax[axis] == 0f)
        {
            result = LayoutMode.Min;
        }
        else if (anchorMin[axis] == 0.5f && anchorMax[axis] == 0.5f)
        {
            result = LayoutMode.Middle;
        }
        else if (anchorMin[axis] == 1f && anchorMax[axis] == 1f)
        {
            result = LayoutMode.Max;
        }
        else if (anchorMin[axis] == 0f && anchorMax[axis] == 1f)
        {
            result = LayoutMode.Stretch;
        }
        else
        {
            result = LayoutMode.Undefined;
        }
        return result;
    }

    //private static void SetLayoutModeForAxis(ref Vector2 anchorMin, int axis, LayoutMode layoutMode, bool doPivot, bool doPosition, Vector2[,] defaultValues)
    //{
    //    anchorMin.serializedObject.ApplyModifiedProperties();
    //    for (int i = 0; i < anchorMin.serializedObject.targetObjects.Length; i++)
    //    {
    //        RectTransform rectTransform = anchorMin.serializedObject.targetObjects[i] as RectTransform;
    //        Undo.RecordObject(rectTransform, "Change Rectangle Anchors");
    //        if (doPosition)
    //        {
    //            if (defaultValues != null && defaultValues.Length > i)
    //            {
    //                Vector2 vector = rectTransform.anchorMin;
    //                vector[axis] = defaultValues[i, 0][axis];
    //                rectTransform.anchorMin = vector;
    //                vector = rectTransform.anchorMax;
    //                vector[axis] = defaultValues[i, 1][axis];
    //                rectTransform.anchorMax = vector;
    //                vector = rectTransform.anchoredPosition;
    //                vector[axis] = defaultValues[i, 2][axis];
    //                rectTransform.anchoredPosition = vector;
    //                vector = rectTransform.sizeDelta;
    //                vector[axis] = defaultValues[i, 3][axis];
    //                rectTransform.sizeDelta = vector;
    //            }
    //        }
    //        if (doPivot && layoutMode != LayoutDropdownWindow.LayoutMode.Undefined)
    //        {
    //            RectTransformEditor.SetPivotSmart(rectTransform, LayoutDropdownWindow.kPivotsForModes[(int)layoutMode], axis, true, true);
    //        }
    //        Vector2 vector2 = Vector2.zero;
    //        switch (layoutMode)
    //        {
    //            case LayoutDropdownWindow.LayoutMode.Min:
    //                RectTransformEditor.SetAnchorSmart(rectTransform, 0f, axis, false, true, true);
    //                RectTransformEditor.SetAnchorSmart(rectTransform, 0f, axis, true, true, true);
    //                vector2 = rectTransform.offsetMin;
    //                EditorUtility.SetDirty(rectTransform);
    //                break;
    //            case LayoutDropdownWindow.LayoutMode.Middle:
    //                RectTransformEditor.SetAnchorSmart(rectTransform, 0.5f, axis, false, true, true);
    //                RectTransformEditor.SetAnchorSmart(rectTransform, 0.5f, axis, true, true, true);
    //                vector2 = (rectTransform.offsetMin + rectTransform.offsetMax) * 0.5f;
    //                EditorUtility.SetDirty(rectTransform);
    //                break;
    //            case LayoutDropdownWindow.LayoutMode.Max:
    //                RectTransformEditor.SetAnchorSmart(rectTransform, 1f, axis, false, true, true);
    //                RectTransformEditor.SetAnchorSmart(rectTransform, 1f, axis, true, true, true);
    //                vector2 = rectTransform.offsetMax;
    //                EditorUtility.SetDirty(rectTransform);
    //                break;
    //            case LayoutDropdownWindow.LayoutMode.Stretch:
    //                RectTransformEditor.SetAnchorSmart(rectTransform, 0f, axis, false, true, true);
    //                RectTransformEditor.SetAnchorSmart(rectTransform, 1f, axis, true, true, true);
    //                vector2 = (rectTransform.offsetMin + rectTransform.offsetMax) * 0.5f;
    //                EditorUtility.SetDirty(rectTransform);
    //                break;
    //        }
    //        if (doPosition)
    //        {
    //            Vector2 anchoredPosition = rectTransform.anchoredPosition;
    //            anchoredPosition[axis] -= vector2[axis];
    //            rectTransform.anchoredPosition = anchoredPosition;
    //            if (layoutMode == LayoutDropdownWindow.LayoutMode.Stretch)
    //            {
    //                Vector2 sizeDelta = rectTransform.sizeDelta;
    //                sizeDelta[axis] = 0f;
    //                rectTransform.sizeDelta = sizeDelta;
    //            }
    //        }
    //    }
    //    anchorMin.serializedObject.Update();
    //}
}
