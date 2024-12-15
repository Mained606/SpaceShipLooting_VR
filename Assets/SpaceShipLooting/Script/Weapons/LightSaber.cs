using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LightSaber : XRGrabInteractableOutline
{
    [SerializeField] private float damage;
    [SerializeField] private GameObject blade;
    [SerializeField] private Collider bladeCollider;
    [SerializeField] private bool isActive = false;
    [SerializeField] private float cooldownTime = 1f; // 데미지 간 간격 (초)
    private float lastDamageTime = 0f;

    [SerializeField] GameObject impactEffectPrefab;
    [SerializeField] private float effectLifetime = 2f;

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

        if (other.contacts.Length > 0)
        {
            Collider thisCollider = other.contacts[0].thisCollider; // 충돌을 발생시킨 현재의 콜라이더
            if (thisCollider != bladeCollider) return; // 블레이드가 아니면 처리 중단
        }

        ContactPoint contact = other.contacts[0];
        Vector3 hitPoint = contact.point;
        Vector3 hitNormal = contact.normal;

        // 이펙트 생성
        if (impactEffectPrefab != null)
        {
            GameObject impactEffect = Instantiate(impactEffectPrefab, hitPoint, Quaternion.LookRotation(hitNormal));
            Destroy(impactEffect, effectLifetime); // 일정 시간 후 이펙트 제거
        }

        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Core") || other.gameObject.CompareTag("Boss"))
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
