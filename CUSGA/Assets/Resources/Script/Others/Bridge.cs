using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : Ohters<Bridge>
{
    private Collider2D coll;
    private SpriteRenderer spr;

    public override void Start()
    {
        base.Start();
        coll = GetComponent<Collider2D>();
        spr = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        FindneedObject();

        inter.InteractionChat();

        if(inter.index == 1 && Input.GetKeyDown(KeyCode.F))
        {
            ShowBridge();
            inter.index++;
        }
           
    }

    public void ShowBridge()
    {
        coll.isTrigger = true;
        spr.enabled = true;
    }
}