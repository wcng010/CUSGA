using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gramdma : Ohters<Gramdma>
{
    private Animator anim;

    public override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (anim != null && anim.GetBool("stand") && inter.index == 1 && !inter.chatFrame.activeInHierarchy)
        {
            inter.index++;
        }

        FindneedObject();

        inter.InteractionChat();
    }

    public override void FindneedObject()
    {
        base.FindneedObject();

        if (succeed >= needStrings.Length)
            anim.SetBool("stand", true);
    }
}
