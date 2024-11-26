using Unity.XR.CoreUtils;
using UnityEngine;

public class PlayerStealthState : IPlayerState
{
    // [SerializeField] private XROrigin xROrigin;
    // [SerializeField] private CharacterController characterController;

    // [Tooltip("Height when the player is standing.")]
    // [SerializeField] private float standingHeight = 1.3f;

    // [Tooltip("Height when the player is in stealth mode.")]
    // [SerializeField] private float stealthHeight = 0.9f;

    // [Tooltip("Speed of transition between standing and stealth mode.")]
    // [SerializeField] private float transitionSpeed = 5f;

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