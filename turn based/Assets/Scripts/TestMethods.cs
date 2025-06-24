using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMethods : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void test(GameObject i_Invoker)
    {
        Debug.Log("WORKS");
        Color newColor = i_Invoker.GetComponent<SpriteRenderer>().color;
        newColor.a = 0.2f;
        i_Invoker.GetComponent<SpriteRenderer>().color = newColor;
    }
}
