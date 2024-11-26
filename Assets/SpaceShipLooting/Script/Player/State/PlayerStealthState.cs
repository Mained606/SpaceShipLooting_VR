using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class PlayerStealthState : IPlayerState
{
    [SerializeField] private DynamicMoveProvider moveProvider;
    [SerializeField] float beforePlayerSpeed;
    [SerializeField] float StealthSpeed = 0.5f;

    public void EnterState(PlayerStateManager manager)
    {
        Debug.Log("Entering Stealth State");

        moveProvider = manager.moveProvider;

        beforePlayerSpeed = moveProvider.moveSpeed;
        moveProvider.moveSpeed = StealthSpeed;
    }

    public void UpdateState(PlayerStateManager manager)
    {
        if (manager.IsrunningMode)
        {
            manager.SwitchState(new PlayerRuningState());
            return;
        }
        // 스텔스 모드 해제되면 Idle 상태로 전환
        if (!manager.IsStealthMode)
        {
            manager.SwitchState(new PlayerIdleState());
        }
    }

    public void ExitState(PlayerStateManager manager)
    {
        manager.IsStealthMode = false;

        moveProvider.moveSpeed = beforePlayerSpeed;
        Debug.Log("Exiting Stealth State");
    }
}