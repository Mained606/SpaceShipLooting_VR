using UnityEngine;

public class PlayerWalkingState : IPlayerState
{
    public void EnterState(PlayerStateManager manager)
    {
        Debug.Log("Entering Walking State");
    }

    public void UpdateState(PlayerStateManager manager)
    {
        // 스텔스 모드 활성화 시 바로 스텔스 상태로 전환
        if (manager.IsStealthMode)
        {
            manager.SwitchState(new PlayerStealthState());
            return;
        }

        if (manager.MoveInput.magnitude <= 0.1f)
        {
            manager.SwitchState(new PlayerIdleState());
        }
        else if (manager.IsrunningMode)
        {
            manager.SwitchState(new PlayerRuningState());
        }

    }

    public void ExitState(PlayerStateManager manager)
    {
        Debug.Log("Exiting Walking State");
    }
}