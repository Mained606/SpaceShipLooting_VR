using UnityEngine;

public class SpaceBossEyeLaserState : State<BossController>
{
    private SpaceBossController boss;
    private float timer;
    private float laserAttackDuration;

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
            laserAttackDuration = boss.LaserAttackDuration;
        }
    }

    public override void OnEnter()
    {
        Debug.Log("보스 레이저 공격 상태 진입");
        timer = 0f; // 타이머 초기화

        if (boss == null) return;

        // 레이저 공격 시작 이벤트 실행
        boss.OnLaserStateStarted?.Invoke();
        TriggerLaserStartEffects();
    }

    public override void Update(float deltaTime)
    {
        if (boss == null) return;

        timer += deltaTime;
        if (timer >= laserAttackDuration)
        {
            // 디펜스 상태로 전환
            boss.SpaceBossDefenceState();
        }
    }

    public override void OnExit()
    {
        Debug.Log("보스 레이저 공격 상태 종료");

        if (boss == null) return;

        // 레이저 공격 종료 이벤트 실행
        boss.OnLaserStateEnded?.Invoke();
        TriggerLaserEndEffects();
    }

    private void TriggerLaserStartEffects()
    {
        // 레이저 공격 시작 시 효과 실행
        Debug.Log("레이저 공격 효과 시작");
        boss.AdjustEyePosition(true);
        // 추가적인 파티클, 사운드 처리 가능
    }

    private void TriggerLaserEndEffects()
    {
        // 레이저 공격 종료 시 효과 실행
        Debug.Log("레이저 공격 효과 종료");
        if (!boss.AllCoresDestroyed)
        {
            boss.AdjustEyePosition(false);
        }
        // 추가적인 파티클, 사운드 처리 가능
    }
}
