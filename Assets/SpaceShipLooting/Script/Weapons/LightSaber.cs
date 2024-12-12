using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LightSaber : XRGrabInteractableOutline
{
    [SerializeField] private float damage;
    [SerializeField] private GameObject blade;
    [SerializeField] private bool isActive = false;
    [SerializeField] private float cooldownTime = 1f; // 데미지 간 간격 (초)
    private float lastDamageTime = 0f;

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

        blade.SetActive(false);
        
        base.OnSelectExiting(args);

    }

    protected override void OnActivated(ActivateEventArgs args)
    {
        base.OnActivated(args);
        isActive = !isActive;
        blade.SetActive(isActive);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (Time.time - lastDamageTime < cooldownTime) return;
        Debug.Log(other.gameObject.name + "Enter");
        
        if(other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Core") || other.gameObject.CompareTag("Boss"))
        {
            Damageable damageable = other.gameObject.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.InflictDamage(damage);
                lastDamageTime = Time.time;
            }
        }
    }
}
