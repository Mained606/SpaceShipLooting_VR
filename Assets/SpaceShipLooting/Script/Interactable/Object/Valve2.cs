using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ValveHandle : XRGrabInteractable
{
    private float currentRotationX = 0f;           // 현재 X축 회전값
    private bool signalSent = false;              // 신호가 이미 전송되었는지 확인
    private bool isFixedAtMaxRotation = false;    // 최대 회전에서 고정 여부

    public float minRotationX = 0f;               // X축 최소 회전 각도
    public float maxRotationX = 180f;             // X축 최대 회전 각도
    public UnityEvent<string> OnSignal { get; } = new UnityEvent<string>();

    private Vector3 initialInteractorPosition;    // 처음 잡은 컨트롤러의 위치
    private Vector3 valveCenter;                  // 밸브 중심 위치

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // 초기 상태 저장
        initialInteractorPosition = args.interactorObject.transform.position;
        valveCenter = transform.position; // 밸브 중심을 기준으로 계산
    }

    private void Update()
    {
        if (isFixedAtMaxRotation) return; // 최대 회전에서 고정된 상태에서는 업데이트 중단

        if (interactorsSelecting.Count > 0)
        {
            Transform interactorTransform = interactorsSelecting[0].transform;

            // 컨트롤러의 현재 위치와 중심을 기준으로 방향 벡터 계산
            Vector3 currentInteractorDirection = (interactorTransform.position - valveCenter).normalized;

            // 초기 방향 벡터 계산
            Vector3 initialDirection = (initialInteractorPosition - valveCenter).normalized;

            // 초기 벡터와 현재 벡터 사이의 각도 계산 (밸브 중심 기준)
            float angleDelta = Vector3.SignedAngle(initialDirection, currentInteractorDirection, transform.right);

            // 새로운 X축 회전값 계산
            currentRotationX = Mathf.Clamp(currentRotationX + angleDelta, minRotationX, maxRotationX);

            // X축 회전값 적용
            transform.localRotation = Quaternion.Euler(currentRotationX, 0f, 0f);

            // 컨트롤러의 위치 업데이트
            initialInteractorPosition = interactorTransform.position;

            // 180도에 도달하면 신호 전송 및 고정
            if (currentRotationX >= maxRotationX && !signalSent)
            {
                OnSignal.Invoke("Valve Fully Turned");
                signalSent = true;
                isFixedAtMaxRotation = true;
            }
        }
    }
}
