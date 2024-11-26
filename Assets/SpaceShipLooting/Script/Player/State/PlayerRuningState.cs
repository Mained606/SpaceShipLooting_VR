using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class PlayerRuningState : IPlayerState
{
    [SerializeField]private DynamicMoveProvider moveProvider;
    [SerializeField]float beforePlayerSpeed;
    [SerializeField]float runningSpeed = 5f;

    public void EnterState(PlayerStateManager manager)
    {
        moveProvider = manager.moveProvider;

        beforePlayerSpeed = moveProvider.moveSpeed;
        moveProvider.moveSpeed = runningSpeed;

        Debug.Log("Entering Runing State");
    }

    public void UpdateState(PlayerStateManager manager)
    {
        // 스텔스 모드 활성화 시 바로 스텔스 상태로 전환
        if (manager.IsStealthMode)
        {
            manager.SwitchState(new PlayerStealthState());
        }

        if (!manager.IsrunningMode)
        {
            manager.SwitchState(new PlayerIdleState());
        }
    }

    public void ExitState(PlayerStateManager manager)
    {
        manager.IsrunningMode = false;
        moveProvider.moveSpeed = beforePlayerSpeed;
        Debug.Log("Exiting Runing State");
    }
}
