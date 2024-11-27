using UnityEngine;

public class Damageable : MonoBehaviour
{
    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    public void InflictDamage(float damage)
    {
        // Health 컴포넌트가 없는 경우 종료
        if (health == null) return;

        // 최종 데미지를 Health 컴포넌트에 전달
        health.TakeDamage(damage);
    }
}
