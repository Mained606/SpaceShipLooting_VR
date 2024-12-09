using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;    // 최대 체력
    public float CurrentHealth { get; set; }    // 현재 체력

    private bool isDead = false;                       //죽음 체크
    private bool isInvincible;                           // 무적 상태
    public bool IsInvincible
    {
        get => isInvincible;
        set
        {
            if (isInvincible == value) return;
            isInvincible = value;
            OnInvincibilityChanged?.Invoke(isInvincible); // 무적 상태 변경 이벤트 호출
        }
    }

    // 이벤트
    public UnityEvent<bool> OnInvincibilityChanged { get; private set; } = new UnityEvent<bool>();
    public UnityEvent<float> OnDamaged { get; private set; } = new UnityEvent<float>();
    public UnityEvent OnDie { get; private set; } = new UnityEvent();

    private void Awake()
    {
        CurrentHealth = maxHealth; // 초기 체력을 최대 체력으로 설정
    }

    // 데미지를 받는 메서드
    public void TakeDamage(float damage)
    {
        if (isDead || IsInvincible) return; // 죽었거나 무적 상태라면 데미지 무효화

        float previousHealth = CurrentHealth;
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0f, maxHealth); // 체력 감소

        if (CurrentHealth < previousHealth)
        {
            Debug.Log($"[Health] {gameObject.name}가 {previousHealth - CurrentHealth}의 피해를 입었습니다. 남은 체력: {CurrentHealth}");
            OnDamaged?.Invoke(previousHealth - CurrentHealth); // 데미지 이벤트 호출
        }

        if (CurrentHealth <= 0 && !isDead)
        {
            HandleDeath(); // 죽음 처리
        }
    }

    // 체력을 회복하는 메서드
    public void Heal(float amount)
    {
        if (isDead) return; // 죽은 상태에서는 회복 불가

        float previousHealth = CurrentHealth;
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0f, maxHealth); // 체력 증가

        if (CurrentHealth > previousHealth)
        {
            Debug.Log($"[Health] {gameObject.name}가 {amount}만큼 회복했습니다. 현재 체력: {CurrentHealth}");
            if(gameObject.CompareTag("Core"))
            {
                SpaceBossController boss = FindAnyObjectByType<SpaceBossController>();
                if (boss != null)
                {
                    boss.RecoverCore(gameObject);
                }
            }
        }
    }

    // 죽음을 처리하는 메서드
    private void HandleDeath()
    {
        isDead = true;
        Debug.Log($"[Health] {gameObject.name}이(가) 사망했습니다.");
        OnDie?.Invoke(); // 죽음 이벤트 호출
    }
}
