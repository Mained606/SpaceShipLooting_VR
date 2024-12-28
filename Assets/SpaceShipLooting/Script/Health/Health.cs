using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] public float maxHealth = 100f;    // 최대 체력
    public float CurrentHealth { get; set; }    // 현재 체력

    public bool isDead = false;                       //죽음 체크
    [SerializeField] private bool isInvincible;                           // 무적 상태
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

    [SerializeField] GameObject damageUi;
    [SerializeField] private Color hitColor = Color.red; // 피격 시 빨간색
    [SerializeField] private float colorChangeDuration = 0.2f; // 색상 변경 지속 시간
    private Renderer[] objectRenderers; // 자식 오브젝트의 모든 Renderer
    private Color[] originalColors; // 각 Renderer의 원래 색상 저장

    // 이벤트
    public UnityEvent<bool> OnInvincibilityChanged { get; private set; } = new UnityEvent<bool>();
    public UnityEvent<float> OnDamaged { get; private set; } = new UnityEvent<float>();
    public UnityEvent OnDie { get; private set; } = new UnityEvent();

    private void Awake()
    {
        CurrentHealth = maxHealth; // 초기 체력을 최대 체력으로 설정
        // 자식 오브젝트의 모든 Renderer 가져오기
        objectRenderers = GetComponentsInChildren<Renderer>();

        // 각 Renderer의 원래 색상 저장
        originalColors = new Color[objectRenderers.Length];
        for (int i = 0; i < objectRenderers.Length; i++)
        {
            var material = objectRenderers[i].material;

            // _Color 속성이 있는 경우에만 처리
            if (material.HasProperty("_Color"))
            {
                originalColors[i] = material.color;
            }
            else
            {
                originalColors[i] = Color.white; // 기본 색상 설정
            }
        }
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

            // 태그에 따라 다른 코루틴 실행
            if (gameObject.CompareTag("Core") || gameObject.CompareTag("Enemy"))
            {
                if (objectRenderers != null && objectRenderers.Length > 0)
                {
                    StartCoroutine(ChangeColorsTemporarily(hitColor, colorChangeDuration));
                }
            }
            else if (gameObject.CompareTag("Boss"))
            {
                if (objectRenderers != null && objectRenderers.Length > 0)
                {
                    StartCoroutine(ChangeColorsTemporarilyBoss(hitColor, colorChangeDuration));
                }
            }
        }

        // Sfx
        if (gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.Play("PlayerDamage", false, 1f, 0.7f);
        }
        if (gameObject.CompareTag("Boss") || gameObject.CompareTag("Core"))
        {
            AudioManager.Instance.Play("BossDamage", false);
        }

        // 데미지 UI
        if (damageUi != null)
        {
            StartCoroutine(DamageUI());
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
            if (gameObject.CompareTag("Core"))
            {
                SpaceBossController boss = FindAnyObjectByType<SpaceBossController>();
                if (boss != null)
                {
                    boss.RecoverCore(gameObject);
                    CoreController coreController = gameObject.GetComponent<CoreController>();
                    coreController.SetDestroyed(false);


                    Debug.Log("리커버 실행");
                }

                else
                {
                    Debug.Log("보스 컨트롤러 못 찾음");
                }
            }
        }
    }

    // 죽음을 처리하는 메서드
    private void HandleDeath()
    {
        if (!gameObject.CompareTag("Core"))
        {
            isDead = true;
        }

        Debug.Log($"[Health] {gameObject.name}이(가) 사망했습니다.");
        OnDie?.Invoke(); // 죽음 이벤트 호출
    }

    //데미지 Ui
    private IEnumerator DamageUI()
    {
        damageUi.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        damageUi.SetActive(false);
    }

    // 색상 변경 효과 (자식 오브젝트 포함)
    private IEnumerator ChangeColorsTemporarily(Color newColor, float duration)
    {
        // 모든 자식 오브젝트의 색상을 변경
        foreach (var renderer in objectRenderers)
        {
            renderer.material.color = newColor;
        }

        // 지정된 시간 대기
        yield return new WaitForSeconds(duration);

        // 모든 자식 오브젝트의 색상을 원래 색상으로 복원
        for (int i = 0; i < objectRenderers.Length; i++)
        {
            objectRenderers[i].material.color = originalColors[i];
        }
    }


    private IEnumerator ChangeColorsTemporarilyBoss(Color newColor, float duration)
    {
        foreach (var renderer in objectRenderers)
        {
            // 태그가 "Core"인 경우 무시
            if (renderer.gameObject.CompareTag("Core"))
            {
                continue;
            }

            foreach (var material in renderer.materials)
            {
                // 기존 색상을 저장
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", newColor * 2f); // 발광 색상 설정
            }
        }

        yield return new WaitForSeconds(duration);

        // 원래 색상으로 복원
        for (int i = 0; i < objectRenderers.Length; i++)
        {
            // 태그가 "Core"인 경우 무시
            if (objectRenderers[i].gameObject.CompareTag("Core"))
            {
                continue;
            }

            foreach (var material in objectRenderers[i].materials)
            {
                material.SetColor("_EmissionColor", Color.black); // 발광 끔
            }
        }
    }
}
