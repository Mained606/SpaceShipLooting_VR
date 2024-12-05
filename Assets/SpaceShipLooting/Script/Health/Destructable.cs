using UnityEngine;

public class Destructable : MonoBehaviour
{
    private Health health;

    private void Start()
    {
        health = GetComponent<Health>();

        health.OnDamaged += OnDamaged;  // 데미지를 입을 때 호출되는 메서드
        health.OnDie += OnDie;          // 죽을 때 호출되는 메서드
    }

    private void OnDestroy()
    {
        if (health != null)
        {
            // 이벤트 구독 해제
            health.OnDamaged -= OnDamaged;
            health.OnDie -= OnDie;
        }
    }

    void OnDamaged(float damage)
    {
        // VFX, Sound 등 데미지 효과 구현
    }

    void OnDie()
    {
        if(gameObject.tag == "Enemy")
        {
            Destroy(gameObject, 2f);
        }
        else if (gameObject.tag == "Player")
        {
            Debug.Log("Player Die");
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
