using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Valve2 : XRGrabInteractableOutline
{
    private Quaternion initialRotation;          // 초기 회전값
    private Vector3 initialInteractorDirection;  // 초기 인터랙터 방향
    private bool signalSent = false;             // 신호 전송 여부

    public float minRotationX = 0f;              // 최소 회전 각도
    public float maxRotationX = 180f;            // 최대 회전 각도
    public UnityEvent<string> OnSignal { get; } = new UnityEvent<string>();

    private float currentRotationX = 0f;         // 현재 회전 각도
    private bool isInteracting => interactorsSelecting.Count > 0; // 상호작용 중인지 확인

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // 초기 상태 저장
        initialRotation = transform.localRotation;
        initialInteractorDirection = GetInteractorDirection(args.interactorObject.transform);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        // 최대 회전 각도에서 고정
        if (currentRotationX >= maxRotationX)
        {
            currentRotationX = maxRotationX;
            SetRotation(currentRotationX);
        }
    }

    private void Update()
    {
        if (!isInteracting || signalSent) return;

        Transform interactorTransform = interactorsSelecting[0].transform;

        // 현재 컨트롤러 방향
        Vector3 currentInteractorDirection = GetInteractorDirection(interactorTransform);

        // 초기 방향과 현재 방향 간의 각도 차이 계산
        float angleDelta = Vector3.SignedAngle(initialInteractorDirection, currentInteractorDirection, transform.right);

        // 새로운 회전 각도 계산
        currentRotationX = Mathf.Clamp(initialRotation.eulerAngles.x + angleDelta, minRotationX, maxRotationX);
        SetRotation(currentRotationX);

        // 신호 전송 및 고정 처리
        if (currentRotationX >= maxRotationX)
        {
            signalSent = true;
            OnSignal.Invoke("Valve Fully Turned");
        }
    }

    private Vector3 GetInteractorDirection(Transform interactorTransform)
    {
        // 밸브 중심으로부터 컨트롤러 방향 벡터 계산
        return (interactorTransform.position - transform.position).normalized;
    }

    private void SetRotation(float rotationX)
    {
        // X축 회전만 변경
        transform.localRotation = Quaternion.Euler(rotationX, initialRotation.eulerAngles.y, initialRotation.eulerAngles.z);
    }
}
