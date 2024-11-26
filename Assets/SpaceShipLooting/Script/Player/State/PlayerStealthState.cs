using UnityEngine;

public class PlayerStealthState : IPlayerState
{
    public void EnterState(PlayerStateManager manager)
    {
        Debug.Log("Entering Stealth State");
    }

    public void UpdateState(PlayerStateManager manager)
    {
        // if (manager.MoveInput.magnitude <= 0.1f)
        // {
        //     manager.SwitchState(new PlayerIdleState());
        // }
        // else if (!manager.IsStealthMode)
        // {
        //     manager.SwitchState(new PlayerWalkingState());
        // }
    }

    public void ExitState(PlayerStateManager manager)
    {
        Debug.Log("Exiting Stealth State");
    }
}