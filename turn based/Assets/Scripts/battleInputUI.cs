using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.Events;
//using System.Diagnostics;

public class BattleInputUI : MonoBehaviour
{
    //private void setupButtons();
    public UnityEvent buttonAction;
    enum input{up,down,left,right,selected}
    KeyCode[] inputKeys = { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D,KeyCode.Space };
   public ListData currentButton;
    List<ListData> currentList;
        Event currentEvent;
    int currentIndex = 0;
   

  [System.Serializable]
    [Inspectable]
    public class ListWrapper
    {
        public List<ListData> list = new List<ListData>();
    }
 
    
    
   
    public List<ListWrapper> buttons= new List<ListWrapper>(); // Must be assigned in the constructor

    // Constructor (called before Start in Unity)
    void Awake()
    {
       
        
    }
    // Start is called before the first frame update
    void Start()
    {
       
        setupButtons();
        currentIndex = 0;
       currentList = buttons[0].list;
        currentButton = currentList[0];

    }

    // Update is called once per frame
    void Update()
    {
        manageInputs();
        if (Input.GetKeyDown(inputKeys[(int)input.selected])) 
        {
           
           
        }
    }
 void setupButtons()
{
       
       
        for (int i = 0;i<buttons.Count-1;i++)
        {
            
            if (buttons[i].list.Count!=0&& buttons[i+1].list.Count != 0)
            {

                linkButtons(buttons[i].list, buttons[i + 1].list,i);
                
                
            }
        } 
        
}
     void linkButtons(List<ListData> buttonlist1, List<ListData> buttonlist2,int listindex)
    {
        int listindex1 = 0;
        int listindex2 = 0;
        buttonlist1[0].rowIndex = 0;
        buttonlist2[0].rowIndex=0;
        ListData currbuttonToLink = buttonlist1[0];
        ListData LinkTo = buttonlist2[0];
       


        for (int i = 0;i<Mathf.Max(buttonlist1.Count,buttonlist2.Count);i++)
        {
            if(listindex1<buttonlist1.Count)
            {
                currbuttonToLink = buttonlist1[listindex1];
                currbuttonToLink.rowIndex=listindex1++;

            }
           
            if (listindex2<buttonlist2.Count)
            { 
                LinkTo= buttonlist2[listindex2];
                LinkTo.rowIndex=listindex2++;

            }
           
           
 
           
                if(currbuttonToLink.nextRight==null)
                    currbuttonToLink.nextRight = LinkTo;
                if(LinkTo.prevLeft==null)
                    LinkTo.prevLeft = currbuttonToLink;
                 
                 
                  currbuttonToLink.listIndex = listindex;
                  LinkTo.listIndex = listindex+1;

          
            

        }
    }
    void manageInputs()
    {

        if (Input.GetKeyDown(inputKeys[(int)input.up]))
        {
            if(currentIndex>0)
            {
                exitButton();
                currentIndex--;
                currentButton = currentList[currentIndex];
                selectedButton();

            }
            
        }
        if (Input.GetKeyDown(inputKeys[(int)input.down]))
        {
            if (currentIndex < currentList.Count-1)
            {
                exitButton();
                currentIndex++;
                currentButton = currentList[currentIndex];
                selectedButton();

            }
                
        }
        if (Input.GetKeyDown(inputKeys[(int)input.left]))
        {
            if (currentButton.prevLeft!=null)
            {
               exitButton();
                currentButton=currentButton.prevLeft;
                currentList = buttons[currentButton.listIndex].list;
                currentIndex=currentButton.rowIndex;
                selectedButton();
            }    
        }
        if (Input.GetKeyDown(inputKeys[(int)input.right]))
        {
            if (currentButton.nextRight != null)
            {
                exitButton();
                currentButton = currentButton.nextRight;
                currentList = buttons[currentButton.listIndex].list;
                currentIndex = currentButton.rowIndex;
                selectedButton();

            }
        }



    }
    void selectedButton()
    {
        currentButton.GetComponent<SpriteRenderer>().color=Color.red;
    }
    void exitButton()
    {
        currentButton.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
