using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class XRBackPackSocketInteractor : XRSocketInteractor
{
    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        if (interactable.transform.CompareTag("Backpack"))
        {
            return base.CanSelect(interactable);
        }

        return false;
    }
}
