using UnityEngine;

public class SpaceBossCoreExplosionState : State<BossController>
{
    private SpaceBossController boss;
    private float timer;

    public override void OnInitialized()
    {
        // context를 SpaceBossController로 캐스팅
        boss = context as SpaceBossController;
        if (boss == null)
        {
            Debug.LogError("SpaceBossController를 초기화할 수 없습니다.");
        }
    }

    public override void OnEnter()
    {
        Debug.Log("보스 코어 폭발 상태 진입");
        timer = 0f; // 타이머 초기화

        if (boss == null) return;
    }

    public override void Update(float deltaTime)
    {
        if (boss == null) return;

        timer += deltaTime;
        if (timer >= boss.CoreExplosionDuration)
        {

        }
        // 디펜스 상태로 전환
        boss.SpaceBossDefenceState();
    }

    public override void OnExit()
    {
        Debug.Log("보스 코어 폭발 상태 종료");
        // 필요 시 상태 종료 로직 추가
    }
}

