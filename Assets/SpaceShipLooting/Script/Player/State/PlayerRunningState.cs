using UnityEngine;

public class PlayerRunningState : IPlayerState
{
    public float Speed => GameManager.PlayerStats.runningSpeed;

    public void EnterState(PlayerStateManager manager)
    {
        Debug.Log("Entering Runing State");
        manager.MoveProvider.moveSpeed = Speed;
    }

    public void UpdateState(PlayerStateManager manager)
    {
        // 스텔스 모드 활성화 시 바로 스텔스 상태로 전환
        if (manager.IsStealthMode)
        {
            manager.SwitchState(new PlayerStealthState());
        }

        if (!manager.IsRunningMode)
        {
            manager.SwitchState(new PlayerIdleState());
        }
    }

    public void ExitState(PlayerStateManager manager)
    {
        manager.IsRunningMode = false;
        Debug.Log("Exiting Runing State");
    }
}
