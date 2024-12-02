using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class ValveObject : XRGrabInteractableOutline, ISignal
{
    private Transform interactorTransform;    // 잡은 컨트롤러의 Transform
    private Quaternion initialRotation;       // 밸브의 초기 회전값
    private Vector3 initialInteractorDirection; // 잡은 순간의 컨트롤러 방향

    public float minRotationX = 0f;           // X축 최소 회전 각도
    public float maxRotationX = 180f;         // X축 최대 회전 각도

    private Rigidbody valveRigidbody;         // 밸브의 Rigidbody
    private bool signalSent = false;          // 신호를 한 번만 발송하기 위한 플래그

    public UnityEvent<string> OnSignal { get; } = new UnityEvent<string>();

    protected override void Awake()
    {
        base.Awake();
        valveRigidbody = GetComponent<Rigidbody>();

        if (valveRigidbody == null)
        {
            Debug.LogError("Rigidbody가 필요합니다. ValveController가 올바르게 작동하지 않을 수 있습니다.", this);
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // 잡은 컨트롤러 Transform 저장
        interactorTransform = args.interactorObject.transform;

        // 초기 상태 저장
        initialRotation = transform.rotation;
        initialInteractorDirection = (interactorTransform.position - transform.position).normalized;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        // 컨트롤러와의 연결 해제
        interactorTransform = null;
    }

    private void FixedUpdate()
    {
        if (interactorTransform == null || signalSent) return;

        // 현재 컨트롤러 방향 계산
        Vector3 currentInteractorDirection = (interactorTransform.position - transform.position).normalized;

        // 초기 방향과 현재 방향 사이의 각도 계산
        float angleDelta = Vector3.SignedAngle(
            initialInteractorDirection,  // 초기 방향
            currentInteractorDirection,  // 현재 방향
            transform.right              // 밸브의 X축 (회전 축)
        );

        // 새로운 X축 회전값 계산 (회전 제한 적용)
        float newRotationX = Mathf.Clamp(initialRotation.eulerAngles.x + angleDelta, minRotationX, maxRotationX);

        // Rigidbody를 이용해 안전하게 회전값 적용
        Quaternion targetRotation = Quaternion.Euler(newRotationX, initialRotation.eulerAngles.y, initialRotation.eulerAngles.z);
        valveRigidbody.MoveRotation(targetRotation);

        // 신호 발송 조건 확인 (180도 도달 시)
        if (Mathf.Approximately(newRotationX, maxRotationX))
        {
            // 신호 발송
            OnSignal.Invoke("Valve fully opened!");
            signalSent = true; // 신호 한 번만 발송
        }
    }

    public void ReceiveSignal()
    {
        // 필요 시 추가 구현 가능
    }
}
