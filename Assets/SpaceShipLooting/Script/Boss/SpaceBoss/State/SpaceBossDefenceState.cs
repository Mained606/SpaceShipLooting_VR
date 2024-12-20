using UnityEngine;

public class SpaceBossDefenceState : State<BossController>
{
    private SpaceBossController boss;
    private float defenceDuration;
    private float timer = 0f;

    public override void OnInitialized()
    {
        // context를 SpaceBossController로 캐스팅
        boss = context as SpaceBossController;
        if (boss == null)
        {
            Debug.LogError("SpaceBossController를 초기화할 수 없습니다.");
        }
        else
        {
            defenceDuration = boss.DefenceDuration;
        }
    }

    public override void OnEnter()
    {
        Debug.Log("보스 디펜스 상태 진입");

        if (boss == null) return;

        boss.StopAllSkillCoroutines(); // 이전 상태의 모든 코루틴 종료

        timer = 0f; // 타이머 초기화

        if (boss == null) return;

        // 디펜스 상태 초기화 로직
        if (!boss.AllCoresDestroyed)
        {
            boss.bossShield.SetActive(true);
        }
        else
        {
            boss.bossShield.SetActive(false);
        }

        SetCoreShield(true);
        SetCoresVulnerable(true); // 코어 무적 설정

        if (!boss.AllCoresDestroyed)
        {
            SetEntityInvincible(true); // 본체 무적 설정
        }

    }

    // 시간 체크해서 보스 어택 상태로 진입
    public override void Update(float deltaTime)
    {
        if (boss == null) return;

        timer += deltaTime;
        if (timer >= defenceDuration)
        {
            // 시간이 지나면 공격 상태로 전환
            boss.SpaceBossAttackState();
        }
    }

    public override void OnExit()
    {
        Debug.Log("보스 디펜스 상태 종료");
        if (boss == null) return;

        // 디펜스 상태 종료 시 코어를 다시 취약 상태로 설정
        SetCoreShield(false);
        SetCoresVulnerable(false);
    }

    // 코어의 무적 상태를 설정하는 메서드
    private void SetCoresVulnerable(bool isVulnerable)
    {
        foreach (var core in boss.cores)
        {
            var coreHealth = core.GetComponent<Health>();
            if (coreHealth != null)
            {
                coreHealth.IsInvincible = isVulnerable;
            }
        }
    }

    // 본체의 무적 상태를 설정하는 메서드
    private void SetEntityInvincible(bool isInvincible)
    {
        var bossHealth = boss.GetComponent<Health>();
        if (bossHealth != null)
        {
            bossHealth.IsInvincible = isInvincible;
        }
    }

    private void SetCoreShield(bool isShield)
    {
        foreach (var core in boss.cores)
        {
            if (core.TryGetComponent(out CoreController coreController))
            {
                if (!coreController.IsDestroyed())
                {
                    core.SetActive(true);
                    coreController.SetShieldEffect(isShield);
                }
            }
        }
    }
}

