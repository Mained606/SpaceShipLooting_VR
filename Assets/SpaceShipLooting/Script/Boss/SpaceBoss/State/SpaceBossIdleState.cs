using UnityEngine;

public class SpaceBossIdleState : State<BossController>
{
    private SpaceBossController boss;

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
        Debug.Log("보스 아이들 상태 진입");
    }

    public override void Update(float deltaTime)
    {
        if (boss == null)
        {
            Debug.LogWarning("보스 컨트롤러가 초기화 되지 않았습니다.");
            return;
        }

        // 타겟이 범위 안으로 들어오면 무적 상태로 전환
        if (boss.IsTargetInRange())
        {
            AudioManager.Instance.PlayBgm("Boss1");
            boss.SpaceBossDefenceState();
        }
    }
}
