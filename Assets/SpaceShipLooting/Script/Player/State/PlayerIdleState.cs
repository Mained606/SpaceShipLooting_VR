using UnityEngine;

public class PlayerIdleState : IPlayerState
{
    public void EnterState(PlayerStateManager manager)
    {
        Debug.Log("Entering Idle State");
    }
    public void UpdateState(PlayerStateManager manager)
    {
        if (manager.MoveInput.magnitude >= 0.1f)
        {
            manager.SwitchState(new PlayerWalkingState());
        }
    }
    public void ExitState(PlayerStateManager manager)
    {
        Debug.Log("Exiting Idle State");
    }
}
