using UnityEngine;
using UnityEditor;

[System.Serializable]
public abstract class XWinPanel
{
    public bool resizeLeft;
    public bool resizeRight;
    public bool resizeTop;
    public bool resizeBottom;

    protected XWinEditorWindow m_Window;

    public XWinPanel(XWinEditorWindow window, bool resizeLeft, bool resizeRight, bool resizeTop, bool resizeBottom)
    {
        m_Window = window;
        this.resizeLeft = resizeLeft;
        this.resizeRight = resizeRight;
        this.resizeTop = resizeTop;
        this.resizeBottom = resizeBottom;
    }

    public virtual Rect DoGUI(Rect rect, GUIContent title)
    {
        rect = XWinGUI.Panel(rect, resizeLeft, resizeRight, resizeTop, resizeBottom,
            title, XWinEditorWindow.styles.dragtabdropwindow);

        return rect;
    }
}
