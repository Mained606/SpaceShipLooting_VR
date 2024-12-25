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
        Debug.Log("레이저 충돌: " + other.gameObject.name);
        Debug.Log("레이저 충돌 태그: " + other.gameObject.tag);
        // 레이저 닿았을 때 사운드 추가


        switch (other.gameObject.tag)
        {
            case "Player":
                Damageable damageable = other.gameObject.GetComponent<Damageable>();
                if (damageable != null)
                {
                    damageable.InflictDamage(boss.LaserDamage);
                }
                Destroy(gameObject); // 레이저 삭제
                break;

            case "Core":
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
                Destroy(gameObject); // 레이저 삭제
                break;

            case "Shield":
            case "Weapons":
            case "Boss":
            case "Bullet":
            case "Blade":
                // Do nothing for these tags
                break;

            default:
                Destroy(gameObject); // 5초 후 레이저 삭제
                break;
        }
    }
}


