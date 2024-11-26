using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class PlayerStealthState : IPlayerState
{
    [SerializeField] private DynamicMoveProvider moveProvider;
    [SerializeField] float beforePlayerSpeed;
    [SerializeField] float StealthSpeed = 0.5f;

    // 앉기
    private Transform cameraOffset;
    private CharacterController characterController;

    private string cameraOffsetName = "Camera Offset";

    private float beforeColliderCenterY;
    private float beforeCameraY;
    private float beforePlayerHeight;

    private float afterPlayerHeight;
    private float afterCameraY;
    private float afterColliderCenterY;


    public void EnterState(PlayerStateManager manager)
    {
        Debug.Log("Entering Stealth State");

        if (characterController == null)
        {
            characterController = manager.GetComponent<CharacterController>();
            if(characterController == null)
            {
                Debug.Log("캐릭터 컨트롤러 Null");
            }
        }

        if(cameraOffset == null)
        {
            cameraOffset = manager.transform.Find(cameraOffsetName);
            if(cameraOffset == null)
            {
                Debug.Log("카메라 오프셋 Null");
            }
        }

        // 포지션 셋팅
        SetSittingPosition();

        moveProvider = manager.moveProvider;

        beforePlayerSpeed = moveProvider.moveSpeed;
        moveProvider.moveSpeed = StealthSpeed;
    }

    public void UpdateState(PlayerStateManager manager)
    {
        if (manager.IsrunningMode)
        {
            manager.SwitchState(new PlayerRuningState());
        }
        // 스텔스 모드 해제되면 Idle 상태로 전환
        if (!manager.IsStealthMode)
        {
            manager.SwitchState(new PlayerIdleState());
        }
    }

    public void ExitState(PlayerStateManager manager)
    {
        SetStandingPosition();

        manager.IsStealthMode = false;

        moveProvider.moveSpeed = beforePlayerSpeed;
        Debug.Log("Exiting Stealth State");
    }

    // 앉은 포지션
    private void SetSittingPosition()
    {
        beforeColliderCenterY = characterController.center.y;
        beforeCameraY = cameraOffset.localPosition.y;
        beforePlayerHeight = characterController.height;

        // 포지션 초기화
        afterColliderCenterY = beforeColliderCenterY / 2f;
        afterCameraY = beforeCameraY / 2f;
        afterPlayerHeight = beforePlayerHeight / 2f;

        // 포지션 셋팅
        cameraOffset.transform.localPosition = new Vector3(cameraOffset.localPosition.x, afterCameraY, cameraOffset.localPosition.z);
        characterController.height = afterPlayerHeight;
        characterController.center = new Vector3(characterController.center.x, afterColliderCenterY, characterController.center.z);
    }

    // 일어난 포지션
    private void SetStandingPosition()
    {
        cameraOffset.transform.localPosition = new Vector3(cameraOffset.localPosition.x, beforeCameraY, cameraOffset.localPosition.z);
        characterController.height = beforePlayerHeight;
        characterController.center = new Vector3(characterController.center.x, beforeColliderCenterY, characterController.center.z);
    }
}