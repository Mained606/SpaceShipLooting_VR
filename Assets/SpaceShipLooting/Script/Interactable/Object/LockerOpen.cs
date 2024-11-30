using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class LockerOpen : SelectObject
{
    private Animator anim;
 

    private void Start()
    {
        anim = GetComponent<Animator>();

        if(anim == null )
        anim = GetComponentInChildren<Animator>();
    }


    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

       
        anim.SetTrigger("Open");


        this.enabled = false;

    }


}
