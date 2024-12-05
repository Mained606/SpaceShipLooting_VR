using UnityEngine;

public class SpaceBossAttackState : State<BossController>
{
    public override void OnEnter()
    {
        Debug.Log("보스 어택 스테이트");
    }

    public override void Update(float deltaTime)
    {
        
    }
}
