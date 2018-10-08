using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MyLayoutData
{
    public MyLayoutDropdownWindow.LayoutMode layoutMode = MyLayoutDropdownWindow.LayoutMode.Middle;
    public MyLayoutDropdownWindow.LayoutMode layoutModeForAxis = MyLayoutDropdownWindow.LayoutMode.Middle;
}

public class MyLayoutDropdownWindow : PopupWindowContent {

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

    private MyLayoutData m_Data;

    private static Styles s_Styles
    {
        get
        {
            if (sStyles == null)
                sStyles = new Styles();
            return sStyles;
        }
    }

    public MyLayoutDropdownWindow(MyLayoutData data)
    {
        m_Data = data;
    }

    public override void OnGUI(Rect rect)
    {
        GUI.Label(new Rect(rect.x + 5f, rect.y + 3f, rect.width - 10f, 16f), new GUIContent("Anchor Presets"), EditorStyles.boldLabel);
        Color color = GUI.color;
        GUI.color = new Color(1f, 1f, 1f, 0.3f) * color;
        GUI.DrawTexture(new Rect(0f, 21f, 400f, 1f), EditorGUIUtility.whiteTexture);
        GUI.color = color;
        GUI.BeginGroup(new Rect(rect.x, rect.y + 22f, rect.width, rect.height - 22f));
        this.TableGUI(rect);
        GUI.EndGroup();
    }

    public override Vector2 GetWindowSize()
    {
        return new Vector2(262f, 300f);
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

        LayoutMode layoutModeForAxis = m_Data.layoutModeForAxis;
        LayoutMode layoutMode = m_Data.layoutMode;
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
                    int clickCount = Event.current.clickCount;
                    if (GUI.Button(position, GUIContent.none, GUIStyle.none))
                    {
                        m_Data.layoutModeForAxis =
                            layoutMode2 != LayoutMode.Undefined ? layoutMode2 : m_Data.layoutModeForAxis;
                        m_Data.layoutMode = layoutMode3!=LayoutMode.Undefined?layoutMode3:m_Data.layoutMode;
                        if (clickCount == 2)
                        {
                            base.editorWindow.Close();
                        }
                        else
                        {
                            base.editorWindow.Repaint();
                        }
                    }
                }
            }
        }
        GUI.color = color;
    }

    private static void DrawLayoutModeHeaderOutsideRect(Rect rect, int axis, LayoutMode mode)
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
    }

    public static void DrawLayoutMode(Rect position, LayoutMode hMode, LayoutMode vMode)
    {
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

    public static void DrawLayoutModeHeadersOutsideRect(Rect rect, LayoutMode layoutModeForAxis, LayoutMode layoutMode)
    {
        DrawLayoutModeHeaderOutsideRect(rect, 0, layoutModeForAxis);
        DrawLayoutModeHeaderOutsideRect(rect, 1, layoutMode);
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
}
