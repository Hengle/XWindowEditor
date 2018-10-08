using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.TreeView;
using UnityEngine;

public class XWinEditorWindow : EditorWindow
{
    public class Styles
    {
        public GUIContent title_Hierarchy = new GUIContent("Hierarchy");
        public GUIContent title_Controls = new GUIContent("Controls");
        public GUIContent title_Work = new GUIContent("Work");
        public GUIContent title_Attribute = new GUIContent("Attribute");

        public GUIContent text_Align = new GUIContent("Align");

        public GUIStyle dragtabdropwindow = "dragtabdropwindow";
        public GUIStyle toolbar = "Toolbar";
        public GUIStyle toolbarButton = "toolbarbutton";
    }

    public static Styles styles
    {
        get
        {
            if(sStyles == null)
                sStyles = new Styles();
            return sStyles;
        }
    }

    public XWinHierarchyPanel hierarchyPanel
    {
        get
        {
            if (m_HierarchyPanel == null) m_HierarchyPanel = new XWinHierarchyPanel(this, false, true, false, true);
            return m_HierarchyPanel;
        }
    }
    public XWinControlsPanel controlPanel
    {
        get
        {
            if (m_ControlsPanel == null) m_ControlsPanel = new XWinControlsPanel(this, false, true, false, true);
            return m_ControlsPanel;
        }
    }
    public XWinWorkPanel workPanel
    {
        get
        {
            if (m_WorkPanel == null) m_WorkPanel = new XWinWorkPanel(this, true, true, false, false);
            return m_WorkPanel;
        }
    }
    public XWinAttributePanel attributePanel
    {
        get
        {
            if (m_AttributePanel == null) m_AttributePanel = new XWinAttributePanel(this, true, false, false, false);
            return m_AttributePanel;
        }
    }

    public TreeModel<ControlElement> model
    {
        get
        {
            if (m_Model == null)
                m_Model = new TreeModel<ControlElement>(GetData());
            return m_Model;
        }
    }

    private static Styles sStyles;

    private XWinHierarchyPanel m_HierarchyPanel;
    private XWinControlsPanel m_ControlsPanel;
    private XWinWorkPanel m_WorkPanel;
    private XWinAttributePanel m_AttributePanel;
    

    private float m_HierarchyWidth = 0.25f;
    private float m_HierarchyHeight = 0.67f;
    private float m_WorkPanelWidth = 0.5f;

    private TreeModel<ControlElement> m_Model;

    [MenuItem("Tools/XWinEditor")]
    public static XWinEditorWindow Get()
    {
        var window = GetWindow<XWinEditorWindow>();
        window.titleContent = new GUIContent("XWinEditor");
        window.Focus();
        window.Repaint();
        return window;
    }

    void OnEnable()
    {
    }

    void OnGUI()
    {
        DoToolbar(new Rect(0, 0, position.width, 18));
        DoPanels(new Rect(0, 18, position.width, position.height - 18));
    }

    public void AddControl(ControlType type, Vector2 position)
    {
        var element = new ControlElement() {id = model.GenerateUniqueID(), name = type.ToString(), position = position};
        model.AddElement(element, model.root, 0);
    }

    private void DoToolbar(Rect rect)
    {
        if (Event.current.type == EventType.Repaint)
            styles.toolbar.Draw(rect, false, false, false, false);
    }

    private void DoPanels(Rect panelRect)
    {
        float hwidth = panelRect.width * m_HierarchyWidth;
        float hheight = panelRect.height * m_HierarchyHeight;
        float wwidth = panelRect.width * m_WorkPanelWidth;

        Rect rect = new Rect(panelRect.x, panelRect.y, hwidth, hheight);
        rect = hierarchyPanel.DoGUI(rect, styles.title_Hierarchy);

        m_HierarchyWidth = rect.width / panelRect.width;
        m_HierarchyHeight = rect.height / panelRect.height;

        rect = new Rect(panelRect.x + rect.x, panelRect.y + rect.height, rect.width, panelRect.height - rect.height);
        controlPanel.RefreshWorkPanelRect(new Rect(rect.x + rect.width, panelRect.y, wwidth, panelRect.height));
        rect = controlPanel.DoGUI(rect, styles.title_Controls);

        m_HierarchyWidth = rect.width / panelRect.width;
        m_HierarchyHeight = (rect.y - panelRect.y) / panelRect.height;

        rect = new Rect(panelRect.x + rect.width, panelRect.y, wwidth, panelRect.height);
        rect = workPanel.DoGUI(rect, styles.title_Work);

        m_HierarchyWidth = (rect.x - panelRect.x) / panelRect.width;
        m_WorkPanelWidth = rect.width / panelRect.width;

        rect = new Rect(panelRect.width * (m_HierarchyWidth + m_WorkPanelWidth), panelRect.y,
            panelRect.width * (1.0f - m_HierarchyWidth - m_WorkPanelWidth), panelRect.height);
        rect = attributePanel.DoGUI(rect, styles.title_Attribute);

        m_WorkPanelWidth = (rect.x - m_HierarchyWidth * panelRect.width) / panelRect.width;

        m_HierarchyWidth = Mathf.Clamp(m_HierarchyWidth, 0.2f, 0.3f);
        m_WorkPanelWidth = Mathf.Clamp(m_WorkPanelWidth, 0.4f, 0.6f);
        m_HierarchyHeight = Mathf.Clamp(m_HierarchyHeight, 0.15f, 0.65f);
    }

    private IList<ControlElement> GetData()
    {
        return new List<ControlElement>()
        {
            new ControlElement {id = 1, depth = -1}
        };
    }
}
