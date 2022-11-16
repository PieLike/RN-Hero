using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFrameSizeFilter : MonoBehaviour
{
    //TMPro.TextMeshProUGUI[] children;
    [SerializeField] float m_TextMeshProPreferredHeight;
    public float TextMeshProPreferredHeight
    {        
        get 
        {
            //Debug.Log("math");
            TMPro.TextMeshProUGUI[] children = transform.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
            if (m_TextMeshProPreferredHeight == 0)
            {
                //float margin = 0;
                foreach (var child in children)
                {
                    if (child)
                    {
                        m_TextMeshProPreferredHeight += child.preferredHeight - child.margin.y;
                        //TMProRecrTransform = m_TextMeshPro.rectTransform;
                        //margin += child.margin.y;
                    }
                }
            }
            return m_TextMeshProPreferredHeight;   
        } 
    }

    //public RectTransform TMProRecrTransform;
    private RectTransform m_RecrTransform;
    public RectTransform Rect
    {
        get
        {
            if (m_RecrTransform == null)
            {
                m_RecrTransform = GetComponent<RectTransform>();
            }
            return m_RecrTransform;
        }
    }

    public float PreferredHeight;

    private void SetHeight()
    {
        if(TextMeshProPreferredHeight != 0)
        {
            PreferredHeight = TextMeshProPreferredHeight;
            Rect.sizeDelta = new Vector2 (Rect.sizeDelta.x, PreferredHeight);
        }
    }

    private void OnEnable() 
    {
        SetHeight();
    }
    private void Start() 
    {
        SetHeight(); 
    }
    /*private void Update() 
    {
        //Debug.Log("PreferredHeight = " + PreferredHeight);
        //Debug.Log("TextMeshProPreferredHeight = " + TextMeshProPreferredHeight);
        if (PreferredHeight != TextMeshProPreferredHeight)
        {
            SetHeight();
        }
    }*/
    private void OnDisable() 
    {
        m_TextMeshProPreferredHeight = 0;
    }
}
