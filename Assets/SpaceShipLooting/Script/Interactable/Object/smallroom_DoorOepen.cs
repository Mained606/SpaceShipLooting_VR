using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class smallroom_DoorOepen : SelectObject
{
    private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(anim != null)
        {
            anim = GetComponentInChildren<Animator>();
        }
        else
        {
            anim = GetComponent<Animator>();
        }
    }
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {


        base.OnSelectEntered(args);

        anim.SetTrigger("Open");
    }

}
