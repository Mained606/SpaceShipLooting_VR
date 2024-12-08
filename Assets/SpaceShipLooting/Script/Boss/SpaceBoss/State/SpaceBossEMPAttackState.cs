using UnityEngine;

public class SpaceBossEMPAttackState : State<BossController>
{
    private SpaceBossController boss;
    private float timer;
    private float empAttackDuration;

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
            empAttackDuration = boss.EMPAttackDuration;
        }
    }

    public override void OnEnter()
    {
        Debug.Log("보스 EMP 공격 상태 진입");
        timer = 0f; // 타이머 초기화

        if (boss == null) return;

        // EMP 공격 효과 실행
        TriggerEMPAttackEffects();
    }

    public override void Update(float deltaTime)
    {
        if (boss == null) return;

        timer += deltaTime;
        if (timer >= empAttackDuration)
        {
            // 디펜스 상태로 전환
            boss.SpaceBossDefenceState();
        }
    }

    public override void OnExit()
    {
        Debug.Log("보스 EMP 공격 상태 종료");
        // 필요 시 상태 종료 로직 추가
    }

    private void TriggerEMPAttackEffects()
    {
        // EMP 공격 효과 실행 로직
        Debug.Log("EMP 공격 효과 발동");
        // 예: 범위 데미지, 화면 효과, 사운드 재생 등
    }
}
