using UnityEngine;

public class BossController : MonoBehaviour
{
    protected StateMachine<BossController> stateMachine;

    protected virtual void Start()
    {
        stateMachine = new StateMachine<BossController>(this, new SpaceBossIdleState());
    }

    protected virtual void Update()
    {
        // 현재 상태의 업데이트를 StateMachine의 업데이트를 통해 실행
        stateMachine.Update(Time.deltaTime);
    }

    public R ChangeState<R>() where R : State<BossController>
    {
        return stateMachine.ChangeState<R>();
    }
}
