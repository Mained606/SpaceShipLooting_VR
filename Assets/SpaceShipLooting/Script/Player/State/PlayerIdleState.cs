using UnityEngine;

public class PlayerIdleState : IPlayerState
{
    public float Speed => GameManager.Instance.PlayerStatsData.walkingSpeed;

    public void EnterState(PlayerStateManager manager)
    {
        manager.MoveProvider.moveSpeed = Speed;
    }
    public void UpdateState(PlayerStateManager manager)
    {
        if (manager.IsStealthMode)
        {
            manager.SwitchState(new PlayerStealthState());
        }
        else if (manager.MoveInput.magnitude >= 0.1f && manager.IsRunningMode)
        {
            manager.SwitchState(new PlayerRunningState());
        }
        else if (manager.MoveInput.magnitude >= 0.1f && !manager.IsRunningMode)
        {
            manager.SwitchState(new PlayerWalkingState());
        }
    }
    public void ExitState(PlayerStateManager manager)
    {
        Debug.Log("Exiting Idle State");
    }
}
