using System.Collections;
using UnityEngine;
using UnityEngine.Analytics;


public class BossLaser : MonoBehaviour
{
    private SpaceBossController boss;
    ParticleSystem healEffect;

    private void Awake()
    {
        boss = GameObject.Find("Boss").GetComponent<SpaceBossController>();
    }

    // 보스 코어 힐 이펙트 & 사운드 재생
    private IEnumerator CoreHeal()
    {
        healEffect.gameObject.SetActive(true);
        healEffect.Play();

        yield return new WaitForSeconds(1f);

        healEffect.Stop();
        healEffect.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("레이저 충돌" + other.gameObject.name);
        Debug.Log("레이저 충돌 태그" + other.gameObject.tag);
        Debug.Log("레이저 충돌 태그 비교" + boss.LaserDamage);

        if (other.gameObject.CompareTag("Player"))
        {
            Damageable damageable = other.gameObject.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.InflictDamage(boss.LaserDamage);
            }
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Core"))
        {
            Health health = other.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.Heal(boss.LaserHealAmount);
            }
            healEffect = other.gameObject.transform.Find("HealEffect").GetComponent<ParticleSystem>();
            if (healEffect != null)
            {
                StartCoroutine(CoreHeal());
            }

            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Shield") || other.gameObject.CompareTag("Weapons"))
        {
            
        }

        else
        {
            Destroy(gameObject, 5f);
        }
    }
}
