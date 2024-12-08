using UnityEngine;

public class SpaceBossGroggyState : State<BossController>
{
    private SpaceBossController boss;
    private float groggyDuration;
    private float timer;

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
            groggyDuration = boss.GroggyDuration;
        }
    }

    public override void OnEnter()
    {
        Debug.Log("보스 그로기 상태 진입");
        timer = 0f; // 타이머 초기화

        if (boss == null) return;

        // 그로기 상태 초기화 효과 실행
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
        // 추가적인 파티클, 사운드 처리 가능
    }

    private void TriggerGroggyEndEffects()
    {
        // 그로기 상태 종료 효과 실행
        Debug.Log("그로기 상태 효과 종료");
        // 추가적인 파티클, 사운드 처리 가능
    }
}