using UnityEngine;

public class SpaceBossGroggyState : State<BossController>
{
    private SpaceBossController boss;
    private float groggyDuration;
    private float timer;
    private Health health;

    private GameObject groggyEffect;

    public override void OnInitialized()
    {
        // context를 SpaceBossController로 캐스팅
        boss = context as SpaceBossController;
        if (boss == null)
        {
            Debug.LogError("SpaceBossController를 초기화할 수 없습니다.");
        }

        health = boss.GetComponent<Health>();
        groggyEffect = boss.groggyEffect;
    }

    public override void OnEnter()
    {
        Debug.Log("보스 그로기 상태 진입");

        if (boss == null) return;

        health = boss.GetComponent<Health>();

        boss.StopAllSkillCoroutines(); // 이전 상태의 모든 코루틴 종료

        groggyDuration = boss.GroggyDuration;
        timer = 0f;

        TriggerGroggyStartEffects();
    }

    public override void Update(float deltaTime)
    {
        if (boss == null) return;

        timer += deltaTime;
        if (timer >= groggyDuration)
        {
            // 디펜스 상태로 전환
            boss.SpaceBossDefenceState();
        }
    }

    public override void OnExit()
    {
        Debug.Log("보스 그로기 상태 종료");

        if (boss == null) return;

        // 그로기 상태 종료 효과 실행
        TriggerGroggyEndEffects();
    }

    private void TriggerGroggyStartEffects()
    {
        // 그로기 상태 시작 효과 실행
        Debug.Log("그로기 상태 효과 시작");

        health.IsInvincible = false;
        boss.bossShield.SetActive(false);
        groggyEffect.SetActive(true);

        // 추가적인 파티클, 사운드 처리 가능
    }

    private void TriggerGroggyEndEffects()
    {
        // 그로기 상태 종료 효과 실행
        Debug.Log("그로기 상태 효과 종료");
        if (!boss.AllCoresDestroyed)
        {
            health.IsInvincible = true;
            boss.bossShield.SetActive(true);
        }

        groggyEffect.SetActive(false);
        // 추가적인 파티클, 사운드 처리 가능
    }
}