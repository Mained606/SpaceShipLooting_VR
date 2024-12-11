using UnityEngine;

public class BossLaser : MonoBehaviour
{
    private SpaceBossController boss;
    
    private void Awake()
    {
        boss = GameObject.Find("Boss").GetComponent<SpaceBossController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("레이저 충돌" + collision.gameObject.name);
        Debug.Log("레이저 충돌 태그" + collision.gameObject.tag);
        Debug.Log("레이저 충돌 태그 비교" + boss.LaserDamage);

        if (collision.gameObject.CompareTag("Player"))
        {
            Damageable damageable = collision.gameObject.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.InflictDamage(boss.LaserDamage);
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Core"))
        {
            Health health = collision.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.Heal(boss.LaserHealAmount);
            }
            Destroy(gameObject);
        }
        else if(collision.gameObject.CompareTag("Shield"))
        {
            
        }

        else
        {
            Destroy(gameObject);
        }
    }
}
