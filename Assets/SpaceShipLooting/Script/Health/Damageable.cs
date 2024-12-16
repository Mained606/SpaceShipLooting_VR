using UnityEngine;

public class Damageable : MonoBehaviour
{
    private Health health;
    private EnemyBehaviour enemy;

    private void Awake()
    {
        health = GetComponent<Health>();
        if(GetComponent<EnemyBehaviour>() != null)
        {
            enemy = GetComponent<EnemyBehaviour>();
        }
    }

    public void InflictDamage(float damage)
    {
        // Health 컴포넌트가 없는 경우 종료
        if (health == null) return;

        if (enemy != null && enemy.isAssassiable)
        {
            damage = enemy.enemyData.health;    // isAssassiable true면 enemy 최대체력만큼 데미지
        }
        // 최종 데미지를 Health 컴포넌트에 전달
        health.TakeDamage(damage);
    }
}
