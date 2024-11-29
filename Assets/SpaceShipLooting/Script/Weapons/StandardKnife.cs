using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StandardKnife : XRGrabInteractableOutline
{
    private float damage = 10f;

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        base.OnSelectExiting(args);

    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name + "Enter");

        if(other.gameObject.tag == "Enemy")
        {
            Damageable damageable = other.gameObject.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.InflictDamage(damage);
            }
        }
    }
}