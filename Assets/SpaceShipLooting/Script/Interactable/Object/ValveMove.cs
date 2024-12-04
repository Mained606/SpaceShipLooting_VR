using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ValveMove : SelectObject
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        anim.SetTrigger("Open");
    }
}
