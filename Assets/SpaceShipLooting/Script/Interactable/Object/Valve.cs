using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Valve : XRGrabInteractableOutline
{
    private Transform interactorTransform;
    private Vector3 initialGrabPosition;  // 잡은 시점에서의 컨트롤러 위치
    private Vector3 initialValveRotation; // 잡은 시점에서의 밸브 회전

    public float rotationSensitivity = 100f; // 회전 민감도 조절 변수
    public float maxRotationAngle = 180f;    // 최대 회전 각도
    private bool hasReachedMaxRotation = false; // 최대 회전에 도달했는지 확인하는 변수

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        interactorTransform = args.interactorObject.transform;

        // 그랩 시점의 컨트롤러 위치와 밸브의 초기 로컬 회전 값 저장
        initialGrabPosition = interactorTransform.position;
        initialValveRotation = transform.localEulerAngles;

        // 초기화
        hasReachedMaxRotation = false; // 밸브를 다시 잡으면 각도 제한 초기화
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        interactorTransform = null;
    }

    private void Update()
    {
        if (interactorTransform != null && !hasReachedMaxRotation)
        {
            // 현재 컨트롤러의 위치와 처음 잡았을 때의 위치 차이 계산
            Vector3 currentGrabPosition = interactorTransform.position;
            Vector3 positionDelta = currentGrabPosition - initialGrabPosition;

            // X축 방향으로의 이동을 회전 각도로 변환 (부호 반전 제거)
            float direction = Vector3.Dot(positionDelta, transform.right) < 0 ? 1f : -1f; // 이동 방향 확인 및 부호 반전
            float rotationAmount = direction * positionDelta.magnitude * rotationSensitivity;

            // 총 회전 각도 제한 적용
            float currentRotation = transform.localEulerAngles.x;
            if (Mathf.Abs(currentRotation - initialValveRotation.x) >= maxRotationAngle)
            {
                hasReachedMaxRotation = true;

                // 회전 멈춤 처리
                transform.localEulerAngles = new Vector3(initialValveRotation.x + maxRotationAngle, initialValveRotation.y, initialValveRotation.z);
            }
            else
            {
                // 밸브의 X축 회전 업데이트
                transform.localEulerAngles = new Vector3(initialValveRotation.x + rotationAmount, initialValveRotation.y, initialValveRotation.z);
            }
        }
    }
}
