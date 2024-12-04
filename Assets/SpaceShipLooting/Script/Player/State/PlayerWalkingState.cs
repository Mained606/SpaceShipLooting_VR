using UnityEngine;

public class PlayerWalkingState : IPlayerState
{
    public float Speed => GameManager.PlayerStats.walkingSpeed;

    public void EnterState(PlayerStateManager manager)
    {
        Debug.Log("Entering Walking State");
        manager.MoveProvider.moveSpeed = Speed;
    }

    public void UpdateState(PlayerStateManager manager)
    {
        // 스텔스 모드 활성화 시 바로 스텔스 상태로 전환
        if (manager.IsStealthMode)
        {
            manager.SwitchState(new PlayerStealthState());
        }
        
        if (manager.IsRunningMode)
        {
            manager.SwitchState(new PlayerRunningState());
        }

        if (manager.MoveInput.magnitude <= 0.1f)
        {
            manager.SwitchState(new PlayerIdleState());
        }
    }

    public void ExitState(PlayerStateManager manager)
    {
        Debug.Log("Exiting Walking State");
    }
}