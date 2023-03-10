using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : Ohters<TrafficLight>
{
    public GameObject stopCar;

    void Update()
    {
        FindneedObject();

        inter.InteractionChat();

        if(inter.index == 1 && Input.GetKeyDown(KeyCode.F))
        {
            stopCar.layer = LayerMask.NameToLayer("Car");
            coll.isTrigger = true;

            //½áÊø
        }
    }
}
