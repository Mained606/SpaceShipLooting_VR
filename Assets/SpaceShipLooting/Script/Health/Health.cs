using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    public float CurrentHealth { get; private set; }    // 현재 체력
    private bool isDeath = false;                       //죽음 체크

    public UnityAction<float> OnDamaged;   // 데미지 입었을때 호출되는 함수
    public UnityAction OnDie;              // 죽었을때 호출되는 함수


    private void Start()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        float beforeHealth = CurrentHealth;

        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, maxHealth);

        // 실제로 받은 데미지가 있을 때 이벤트 호출
        float realDamage = beforeHealth - CurrentHealth;
        if (realDamage > 0f)
        {
            Debug.Log(gameObject.name + "가" + realDamage +"의 피해를 입어" + CurrentHealth +"의 체력이 남았습니다.");
            OnDamaged?.Invoke(realDamage);
        }

        // 죽음 처리
        HandleDeath();
    }

    private void HandleDeath()
    {
        // 이미 죽었으면 처리 안함
        if (isDeath) return;

        // 체력이 0 이하면 죽음 처리
        if (CurrentHealth <= 0)
        {
            isDeath = true;
            Debug.Log(gameObject.name + " is dead");

            OnDie?.Invoke();
        }
    }
}
