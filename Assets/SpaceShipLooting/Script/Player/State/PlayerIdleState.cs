using UnityEngine;

public class PlayerIdleState : IPlayerState
{
    public void EnterState(PlayerStateManager manager)
    {

        Debug.Log("Entering Idle State");
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
        else if (manager.IsrunningMode)
        {
            manager.SwitchState(new PlayerRuningState());
        }
    }
    public void ExitState(PlayerStateManager manager)
    {
        Debug.Log("Exiting Idle State");
    }
}
