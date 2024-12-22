using UnityEngine;

public class Blade : MonoBehaviour
{
    private float damage;
    [SerializeField] private GameObject impactEffectPrefab;
    [SerializeField] private float cooldownTime = 1f; // 데미지 간 간격 (초)
    private float timer = 0f;
    [SerializeField] private float effectLifetime = 2f; // 이펙트 유지 시간
    private bool attackCoolTime = false;

    private void Start()
    {
        damage = GameManager.Instance.PlayerStatsData.knifeDamage;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= cooldownTime)
        {
            attackCoolTime = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!attackCoolTime) return;

        Debug.Log($"Triggered with: {other.gameObject.name}");

        // 소리 재생
        AudioManager.Instance.Play("BeamSword");

        // 충돌 지점 가져오기
        Vector3 hitPoint = other.ClosestPointOnBounds(transform.position);
        Vector3 hitNormal = (hitPoint - transform.position).normalized;

        // 이펙트 생성
        if (impactEffectPrefab != null)
        {
            GameObject impactEffect = Instantiate(impactEffectPrefab, hitPoint, Quaternion.LookRotation(hitNormal));
            Destroy(impactEffect, effectLifetime); // 일정 시간 후 제거
        }

        // 데미지 적용: 특정 태그에만
        if (other.CompareTag("Enemy") || other.CompareTag("Core") || other.CompareTag("Boss"))
        {
            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.InflictDamage(damage);
            }
        }

        attackCoolTime = false;
        timer = 0;
    }
}