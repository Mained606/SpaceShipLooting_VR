using UnityEngine;
using UnityEngine.Events;

public class BossEye : MonoBehaviour
{
    private SpaceBossController bossController;
    private Animator animator;

    // 데미지를 받을 때 발생하는 이벤트
    public UnityEvent onDamageReceived { get; private set; } = new UnityEvent();

    private bool allCoresDestroyed = false;
    private bool onlaserState;

    [SerializeField] private int maxDamageCount = 5;
    private int damageCount = 0;

    private void Start()
    {
        bossController = GetComponentInParent<SpaceBossController>();
        if (bossController == null)
        {
            Debug.Log("SpaceBossController를 찾을 수 없습니다.");
            return;
        }

        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.Log("animator 찾을 수 없습니다.");
            return;
        }

        // 코어 파괴 및 레이저 상태 이벤트 구독
        SubscribeToBossEvents();
    }

    private void Update()
    {
        // 모든 코어가 파괴되면 눈은 항상 플레이어를 바라봄
        if (allCoresDestroyed || onlaserState)
        {
            this.transform.LookAt(bossController.Target.position);
        }
    }

    private void SubscribeToBossEvents()
    {
        bossController.OnAllCoresDestroyed.AddListener(OnAllCoresDestroyed);
        bossController.OnLaserStateStarted.AddListener(OnLaserStateStarted);
        bossController.OnLaserStateEnded.AddListener(OnLaserStateEnded);
    }

    private void OnAllCoresDestroyed()
    {
        allCoresDestroyed = true;
    }

    public void OnLaserStateStarted()
    {
        onlaserState = true;
    }

    public void OnLaserStateEnded()
    {
        onlaserState = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger 충돌 태그: {other.tag}");
        // Weapons 태그이거나 Bullet 태그가 아니라면 리턴
        if (!(other.CompareTag("Weapons") || other.CompareTag("Bullet"))) return;

        TakeDamage();
    }

    // 5번 공격 당하면 그로기 스테이트로 전환
    public void TakeDamage()
    {
        // 레이저 패턴이 아니고 모든 코어가 파괴된 상태가 아니라면 리턴
        // if (!onlaserState && !allCoresDestroyed) return;

        animator.SetTrigger("damage");

        AudioManager.Instance.Play("EyeDamage");

        damageCount++;
        Debug.Log($"[BossEye] 데미지 횟수: {damageCount}/{maxDamageCount}");

        if (damageCount >= maxDamageCount)
        {
            // 이벤트 신호 발생
            onDamageReceived.Invoke();
            Debug.Log("[BossEye] 최대 데미지 도달 - 그로기 상태로 전환");
            damageCount = 0;    // 데미지 카운트 초기화
        }
    }
}
