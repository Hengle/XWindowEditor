using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class XItemNode
{

    [SerializeField] private List<int> m_childs;

    [SerializeField] private int m_depth = 0;

    public void AddChild(XItemNode node)
    {
        
    }
}
