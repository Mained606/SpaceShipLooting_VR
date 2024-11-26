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

        moveProvider.GetComponent<DynamicMoveProvider>();

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
            return;
        }

       /* if (manager.MoveInput.magnitude <= 0.1f)
        {
            manager.SwitchState(new PlayerIdleState());
        }*/
        else if (!manager.IsrunningMode)
        {
            manager.SwitchState(new PlayerIdleState());
        }
    }

    public void ExitState(PlayerStateManager manager)
    {
        // isrunning 해제
        Debug.Log("Exiting Runing State");
        moveProvider.moveSpeed = beforePlayerSpeed;
    }

   
}
