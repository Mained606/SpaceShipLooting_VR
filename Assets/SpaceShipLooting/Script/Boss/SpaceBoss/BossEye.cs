using UnityEngine;
using UnityEngine.Events;
using System.Collections;

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

    [SerializeField] private Color hitColor = Color.red; // 피격 시 빨간색
    [SerializeField] private float colorChangeDuration = 0.2f; // 색상 변경 지속 시간
    private Renderer[] objectRenderers; // 자식 오브젝트의 모든 Renderer
    private Color[] originalColors; // 각 Renderer의 원래 색상 저장

    private void Start()
    {
        bossController = GetComponentInParent<SpaceBossController>();
        if (bossController == null)
        {
            Debug.LogError("SpaceBossController를 찾을 수 없습니다.");
            return;
        }

        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator를 찾을 수 없습니다.");
            return;
        }

        // 모든 Renderer 및 SkinnedMeshRenderer 가져오기
        objectRenderers = GetComponentsInChildren<Renderer>();

        // 각 Material의 원래 색상 저장
        var colorList = new System.Collections.Generic.List<Color>();
        foreach (var renderer in objectRenderers)
        {
            foreach (var material in renderer.materials)
            {
                if (material.HasProperty("_Color"))
                {
                    colorList.Add(material.color);
                }
            }
        }

        originalColors = colorList.ToArray(); // 모든 색상을 배열로 저장

        SubscribeToBossEvents(); // 이벤트 구독
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

        AudioManager.Instance.Play("EyeDamage", false);

        damageCount++;
        Debug.Log($"[BossEye] 데미지 횟수: {damageCount}/{maxDamageCount}");

        if (objectRenderers != null && objectRenderers.Length > 0)
        {
            StartCoroutine(ChangeColorsTemporarilyBoss(hitColor, colorChangeDuration));
        }

        if (damageCount >= maxDamageCount)
        {
            // 이벤트 신호 발생
            onDamageReceived.Invoke();
            Debug.Log("[BossEye] 최대 데미지 도달 - 그로기 상태로 전환");
            damageCount = 0;    // 데미지 카운트 초기화
        }
    }


    private IEnumerator ChangeColorsTemporarilyBoss(Color newColor, float duration)
    {
        foreach (var renderer in objectRenderers)
        {
            foreach (var material in renderer.materials)
            {
                if (material.HasProperty("_EmissionColor"))
                {
                    material.SetColor("_EmissionColor", newColor * 2f);
                }
            }
        }

        yield return new WaitForSeconds(duration);

        // 원래 에미션 색상 복원
        foreach (var renderer in objectRenderers)
        {
            foreach (var material in renderer.materials)
            {
                if (material.HasProperty("_EmissionColor"))
                {
                    material.SetColor("_EmissionColor", Color.white);
                }
            }
        }
    }
}
