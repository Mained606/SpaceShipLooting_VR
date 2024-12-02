using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class XRPlayerSocketInteractor : XRSocketInteractor
{
    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        if (interactable.transform.CompareTag("Weapons"))
        {
            return base.CanSelect(interactable);
        }

        return false;
    }
}
