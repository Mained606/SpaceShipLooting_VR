using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FirstDoorOpen :XRSimpleInteractableOutline
{
    private Animator anim;

    protected override void Start()
    {
        // Animator 가져오기
        anim = GetComponentInParent<Animator>();
        if (anim == null)
        {
            anim = GetComponentInParent<Animator>();
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        anim.SetTrigger("Open");
    }

}
