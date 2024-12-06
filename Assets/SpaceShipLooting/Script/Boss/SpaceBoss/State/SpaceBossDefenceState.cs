using UnityEngine;

public class SpaceBossDefenceState : State<BossController>
{
    private SpaceBossController boss;
    private Health health;

    // 디펜스에서 어택으로 전환되는 시간 전용
    private float defenceDuration;
    private float timer = 0f;

    public override void OnInitialized()
    {
        boss = context as SpaceBossController;
        defenceDuration = boss.defenceDuration;
    }

    public override void OnEnter()
    {
        Debug.Log("보스 디펜스 상태 시작");
        //타이머 초기화
        timer = 0f;
        
        // 무적 설정
        CoresInvincibleDisable();
        EntityInvincible();
    }
    
    // 시간 체크해서 보스 어택 상태로 진입
    public override void Update(float deltaTime)
    {
        timer += deltaTime;
        if (timer >= defenceDuration)
        {
            boss.SpaceBossAttackState();
        }
    }

    // 코어 무적 해제
    private void CoresInvincibleDisable()
    {
        foreach (var core in boss.cores)
        {
            core.GetComponent<Health>().isInvincible = false;
        }
    }

    // 본체 무적 설정
    private void EntityInvincible()
    {
        boss.GetComponent<Health>().isInvincible = true;
    }
}
