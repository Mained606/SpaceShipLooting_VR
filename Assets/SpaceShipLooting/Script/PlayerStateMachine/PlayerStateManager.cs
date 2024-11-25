using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class PlayerStateManager : MonoBehaviour
{
    public Vector2 MoveInput => moveProvider.leftHandMoveInput.ReadValue();
    public bool IsStealthMode { get; private set; } = false;

    private IPlayerState currentState;
    [SerializeField] private DynamicMoveProvider moveProvider;

    private void Start()
    {
        // 초기 상태를 Idle로 설정
        currentState = new PlayerIdleState();
        currentState.EnterState(this);
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(IPlayerState newState)
    {
        currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    public void ToggleStealthMode()
    {
        IsStealthMode = !IsStealthMode;
    }
}