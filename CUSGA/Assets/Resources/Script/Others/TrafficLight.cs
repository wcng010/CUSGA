using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    private Interaction inter;
    public GameObject stopCar;
    void Start()
    {
        inter = GetComponentInChildren<Interaction>();
    }

    // Update is called once per frame
    void Update()
    {
        if(inter.index == 1)
        {
            stopCar.SetActive(true);
        }
    }
}
