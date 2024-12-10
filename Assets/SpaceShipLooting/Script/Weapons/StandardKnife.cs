using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StandardKnife : XRGrabInteractableOutline
{
    [SerializeField] private float damage;

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        Rigidbody rb = GetComponent<Rigidbody>();
        damage = GameManager.Instance.PlayerStatsData.knifeDamage;
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

        if(other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Core") || other.gameObject.CompareTag("Boss"))
        {
            Damageable damageable = other.gameObject.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.InflictDamage(damage);
            }
        }
    }
}