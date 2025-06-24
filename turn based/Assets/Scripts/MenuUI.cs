using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class MenuUI : MonoBehaviour
{
    private enum InputType { Up, Down, Left, Right, Selected }

    private KeyCode[] m_InputKeys = { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D, KeyCode.Space };

    public MenuButton m_CurrentButton;
    private List<MenuButton> m_CurrentList;
    private Event m_CurrentEvent;
    private int m_CurrentIndex = 0;

    [System.Serializable]
    [Inspectable]
    public class MenuRow
    {
        public List<MenuButton> m_ButtonsInRow = new List<MenuButton>();
    }

    public List<MenuRow> m_Buttons = new List<MenuRow>();

    private void Awake()
    {
       
    }
   
    private void Start()
    {
        if (m_Buttons == null || m_Buttons.Count == 0 || m_Buttons[0].m_ButtonsInRow.Count == 0)
        {
            Debug.LogError("MenuUI not initialized: Missing button setup.");
            return;
        }
        setupButtons();
        m_CurrentIndex = 0;
        m_CurrentList = m_Buttons[0].m_ButtonsInRow;
        m_CurrentButton = m_CurrentList[0];
    }
    private void OnValidate()
    {
        if (m_Buttons != null)
        {
            for (int i = 0; i < m_Buttons.Count; i++)
            {
                if (m_Buttons[i].m_ButtonsInRow == null)
                    Debug.LogWarning($"Row {i} is null.");
            }
        }
    }
    private void Update()
    {
        manageInputs();
    }

    private void setupButtons()
    {
        for (int i = 0; i < m_Buttons.Count - 1; i++)
        {
            if (m_Buttons[i].m_ButtonsInRow.Count != 0 && m_Buttons[i + 1].m_ButtonsInRow.Count != 0)
            {
                linkButtons(m_Buttons[i].m_ButtonsInRow, m_Buttons[i + 1].m_ButtonsInRow, i);
            }
        }
    }

    private void linkButtons(List<MenuButton> i_ButtonList1, List<MenuButton> i_ButtonList2, int i_ListIndex)
    {
        if (i_ButtonList1 == null || i_ButtonList2 == null)
            throw new System.ArgumentNullException("Button lists cannot be null.");

        int listIndex1 = 0;
        int listIndex2 = 0;

        i_ButtonList1[0].m_RowIndex = 0;
        i_ButtonList2[0].m_RowIndex = 0;

        MenuButton currButtonToLink = i_ButtonList1[0];
        MenuButton linkTo = i_ButtonList2[0];

        for (int i = 0; i < Mathf.Max(i_ButtonList1.Count, i_ButtonList2.Count); i++)
        {
            if (listIndex1 < i_ButtonList1.Count)
            {
                currButtonToLink = i_ButtonList1[listIndex1];
                currButtonToLink.m_RowIndex = listIndex1++;
            }

            if (listIndex2 < i_ButtonList2.Count)
            {
                linkTo = i_ButtonList2[listIndex2];
                linkTo.m_RowIndex = listIndex2++;
            }

            if (currButtonToLink.m_NextRight == null)
                currButtonToLink.m_NextRight = linkTo;

            if (linkTo.m_PrevLeft == null)
                linkTo.m_PrevLeft = currButtonToLink;

            currButtonToLink.m_ListIndex = i_ListIndex;
            linkTo.m_ListIndex = i_ListIndex + 1;
        }
    }

    private void manageInputs()
    {
        if (Input.GetKeyDown(m_InputKeys[(int)InputType.Selected]))
        {
            m_CurrentButton?.ButtonSelected();
        }

        if (Input.GetKeyDown(m_InputKeys[(int)InputType.Up]))
        {
            if (m_CurrentIndex > 0)
            {
                setCurrentButton(--m_CurrentIndex);
            }
            else
            {
                setCurrentButton(m_CurrentList.Count - 1);
            }
        }

        if (Input.GetKeyDown(m_InputKeys[(int)InputType.Down]))
        {
            if (m_CurrentIndex < m_CurrentList.Count - 1)
            {
                setCurrentButton(++m_CurrentIndex);
            }
            else
            {
                setCurrentButton(0);
            }
        }

        if (Input.GetKeyDown(m_InputKeys[(int)InputType.Left]))
        {
            if (m_CurrentButton.m_PrevLeft != null)
            {
                exitButton();
                m_CurrentButton = m_CurrentButton.m_PrevLeft;
                m_CurrentList = m_Buttons[m_CurrentButton.m_ListIndex].m_ButtonsInRow;
                m_CurrentIndex = m_CurrentButton.m_RowIndex;
                enterButton();
            }
        }

        if (Input.GetKeyDown(m_InputKeys[(int)InputType.Right]))
        {
            if (m_CurrentButton.m_NextRight != null)
            {
                exitButton();
                m_CurrentButton = m_CurrentButton.m_NextRight;
                m_CurrentList = m_Buttons[m_CurrentButton.m_ListIndex].m_ButtonsInRow;
                m_CurrentIndex = m_CurrentButton.m_RowIndex;
                enterButton();
            }
        }
    }

    private void enterButton()
    {
        if (m_CurrentButton == null) throw new System.NullReferenceException("Current button is null.");
        m_CurrentButton.Highlight();
    }

    private void exitButton()
    {
        if (m_CurrentButton == null) throw new System.NullReferenceException("Current button is null.");
        m_CurrentButton.Unhighlight();
    }

    private void setCurrentButton(int i_IndexInList)
    {
        exitButton();
        m_CurrentIndex = i_IndexInList;
        m_CurrentButton = m_CurrentList[m_CurrentIndex];
        enterButton();
    }

    public void SetButtonAction(int i_XPos, int i_YPos, UnityAction<GameObject> i_Action)
    {
        if (i_Action == null) throw new System.ArgumentNullException(nameof(i_Action));
        m_Buttons[i_XPos].m_ButtonsInRow[i_YPos].m_ButtonAction += i_Action;
    }

    public static void Test(GameObject i_Invoker)
    {
        if (i_Invoker == null) throw new System.ArgumentNullException(nameof(i_Invoker));
        Debug.Log("WORKS");

        Color o_NewColor = i_Invoker.GetComponent<SpriteRenderer>().color;
        o_NewColor.a = 0.2f;
        i_Invoker.GetComponent<SpriteRenderer>().color = o_NewColor;
    }
}
