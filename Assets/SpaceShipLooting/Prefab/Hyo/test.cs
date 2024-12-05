using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class test : XRSimpleInteractable
{
    [SerializeField] private Animator animator;
    
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);

        animator.SetBool("IsSit", true);
    }
}
