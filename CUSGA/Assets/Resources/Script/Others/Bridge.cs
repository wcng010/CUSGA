using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : Ohters<Bridge>
{
    void Update()
    {
        FindneedObject();

        inter.InteractionChat();

        if(inter.index == 1 && Input.GetKeyDown(KeyCode.F))
        {
            ShowObject();
        }
           
    }
}
