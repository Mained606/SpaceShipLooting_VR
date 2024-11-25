public interface IPlayerState
{
    void EnterState(PlayerStateManager manager);
    void UpdateState(PlayerStateManager manager);
    void ExitState(PlayerStateManager manager);
}