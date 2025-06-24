using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class MenuButton : MonoBehaviour
{

    public MenuButton m_NextRight;
    public MenuButton m_PrevLeft;
    public UnityAction<GameObject> m_ButtonAction;
    public int m_RowIndex;
    public int m_ListIndex;

    public void Highlight() => GetComponent<SpriteRenderer>().color = Color.red;
    public void Unhighlight() => GetComponent<SpriteRenderer>().color = Color.white;
    
    private void Awake()
    {
       // m_ButtonAction += TestMethods.test;
    }
    public void ButtonSelected()
    {
        onSelected(this.gameObject);
    }
    private void onSelected(GameObject obj)
    {
        m_ButtonAction?.Invoke(obj);
    }
}
