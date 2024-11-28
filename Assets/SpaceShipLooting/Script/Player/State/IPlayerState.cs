public interface IPlayerState
{
    float Speed { get; } // 각 상태의 기본 속도
    void EnterState(PlayerStateManager manager);
    void UpdateState(PlayerStateManager manager);
    void ExitState(PlayerStateManager manager);
}