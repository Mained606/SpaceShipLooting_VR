using UnityEngine;

public class PlayerIdleState : IPlayerState
{
    public float Speed => GameManager.PlayerStats.walkingSpeed;

    public void EnterState(PlayerStateManager manager)
    {
        Debug.Log("Entering Idle State" + Speed);
        manager.MoveProvider.moveSpeed = Speed;
    }
    public void UpdateState(PlayerStateManager manager)
    {
        if (manager.IsStealthMode)
        {
            manager.SwitchState(new PlayerStealthState());
        }
        else if (manager.MoveInput.magnitude >= 0.1f)
        {
            manager.SwitchState(new PlayerWalkingState());
        }
        else if (manager.IsRunningMode)
        {
            manager.SwitchState(new PlayerRunningState());
        }
    }
    public void ExitState(PlayerStateManager manager)
    {
        Debug.Log("Exiting Idle State");
    }
}
