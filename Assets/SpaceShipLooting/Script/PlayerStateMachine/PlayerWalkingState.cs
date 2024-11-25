using UnityEngine;

public class PlayerWalkingState : IPlayerState
{
    public void EnterState(PlayerStateManager manager)
    {
        Debug.Log("Entering Walking State");
    }

    public void UpdateState(PlayerStateManager manager)
    {
        if (manager.MoveInput.magnitude <= 0.1f)
        {
            manager.SwitchState(new PlayerIdleState());
        }
        else if (manager.IsStealthMode)
        {
            manager.SwitchState(new PlayerStealthState());
        }
    }

    public void ExitState(PlayerStateManager manager)
    {
        Debug.Log("Exiting Walking State");
    }
}