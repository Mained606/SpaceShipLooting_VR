using UnityEngine;

public class SpaceBossEyeLaserState : State<BossController>
{
    private SpaceBossController boss;

    private float timer;
    private float laserAttackDuration;

    public override void OnInitialized()
    {
        boss = context as SpaceBossController;
        laserAttackDuration = boss.laserAttackDuration;
    }

    public override void OnEnter()
    {
        Debug.Log("보스 레이저 스테이트 시작");
        timer = 0f;
        
        // 눈 위치 올리기
        boss.ChangeEyePositionUp();
    }

    public override void Update(float deltaTime)
    {
        timer += deltaTime;
        if(timer >= laserAttackDuration)
        {
            // 레이저 스킬 효과 발동


            // 공격 후 다시 디펜스 스테이트로 변경
            boss.SpaceBossDefenceState();
        }
    }

    public override void OnExit()
    {
        // 만약 모든 코어가 파괴된 상태라면 눈알 위치 내리지 않음
        if(boss.allCoresDestroyed) return;
        boss.ChangeEyePositionDown();
    }
}
