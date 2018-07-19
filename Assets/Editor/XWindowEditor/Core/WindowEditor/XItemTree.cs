using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class XItemTree
{
    public XItemNode Root
    {
        get
        {
            if (m_nodes == null)
                m_nodes = new List<XItemNode>();
            if (m_nodes.Count == 0)
                m_nodes.Add(new XItemNode());
            return m_nodes[0];
        }
    }

    [SerializeField] private List<XItemNode> m_nodes;

    public void AddNode(XItemNode node)
    {
        //if (m_nodes == null)
        //    m_nodes = new List<XItemNode>();
        //if(m_nodes.Count == 0)
        //    m_nodes.Add(new XItemNode())
    }
}
