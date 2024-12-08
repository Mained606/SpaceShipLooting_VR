using UnityEngine;

public class SpaceBossAttackState : State<BossController>
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
        if (boss == null) return;

        // 공격 패턴 실행
        ExecuteAttackPattern();
    }

    public override void Update(float deltaTime)
    {
        // 현재 상태에서는 업데이트 로직이 필요하지 않음.
        // 상태 지속 시간이나 특정 조건 처리 시 여기에 추가 가능.
    }

    public override void OnExit()
    {

    }

    private void ExecuteAttackPattern()
    {
        if (boss == null) return;

        // SpaceBossController의 공격 패턴 실행 메서드 호출
        boss.ExecuteRandomAttackPattern();
    }
}
