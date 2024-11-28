using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class PlayerStealthState : IPlayerState
{
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

    // 속도
    public float Speed => PlayerStateManager.Instance.StatsConfig.stealthSpeed;

    public void EnterState(PlayerStateManager manager)
    {
        Debug.Log("Entering Stealth State" + Speed);

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

        manager.MoveProvider.moveSpeed = Speed;
    }

    public void UpdateState(PlayerStateManager manager)
    {
        if (manager.IsRunningMode)
        {
            manager.SwitchState(new PlayerRunningState());
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