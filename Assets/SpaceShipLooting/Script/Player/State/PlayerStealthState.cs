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

    private RectTransform leftNightVisionPanel;
    private RectTransform rightNightVisionPanel;

    private Vector3 beforeLeftPanelSize;
    private Vector3 beforeRightPanelSize;

    // 속도
    public float Speed => GameManager.PlayerStats.stealthSpeed;

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

        leftNightVisionPanel = manager.transform.Find("NigitVision Left Panel").GetComponent<RectTransform>();
        rightNightVisionPanel = manager.transform.Find("NigitVision Right Panel").GetComponent<RectTransform>();

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

        // 나이트비젼 패널 위치 설정
        if (leftNightVisionPanel != null && rightNightVisionPanel != null)
        {
            beforeLeftPanelSize.y = leftNightVisionPanel.localPosition.y;
            beforeRightPanelSize.y = rightNightVisionPanel.localPosition.y;

            leftNightVisionPanel.localPosition = new Vector3(leftNightVisionPanel.localPosition.x, leftNightVisionPanel.localPosition.y / 2, leftNightVisionPanel.localPosition.z);
            rightNightVisionPanel.localPosition = new Vector3(rightNightVisionPanel.localPosition.x, rightNightVisionPanel.localPosition.y / 2, rightNightVisionPanel.localPosition.z);
        }

    }

    // 일어난 포지션
    private void SetStandingPosition()
    {
        cameraOffset.transform.localPosition = new Vector3(cameraOffset.localPosition.x, beforeCameraY, cameraOffset.localPosition.z);
        characterController.height = beforePlayerHeight;
        characterController.center = new Vector3(characterController.center.x, beforeColliderCenterY, characterController.center.z);

        leftNightVisionPanel.localPosition = new Vector3(leftNightVisionPanel.localPosition.x, beforeLeftPanelSize.y, leftNightVisionPanel.localPosition.z);
        rightNightVisionPanel.localPosition = new Vector3(rightNightVisionPanel.localPosition.x, beforeRightPanelSize.y, rightNightVisionPanel.localPosition.z);
    }
}