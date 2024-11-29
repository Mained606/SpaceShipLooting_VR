using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class ValveObject : XRGrabInteractableOutline, ISignal
{
    [SerializeField] private float targetAngle = 180f; // 목표 각도
    private Transform interactorTransform; // 플레이어 컨트롤러 Transform
    private float currentAngle = 0f; // 현재 회전 각도
    private Vector3 initialInteractorPosition; // 초기 컨트롤러 위치
    public UnityEvent<string> OnSignal { get; } = new UnityEvent<string>();

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        interactorTransform = args.interactorObject.transform; // 컨트롤러 Transform 저장
        initialInteractorPosition = interactorTransform.position; // 초기 컨트롤러 위치 저장
    }

    private void Update()
    {
        if (isSelected && interactorTransform != null)
        {
            // 컨트롤러와 밸브의 중심 간의 상대 위치 계산
            Vector3 currentInteractorPosition = interactorTransform.position;
            Vector3 startDirection = initialInteractorPosition - transform.position; // 초기 방향
            Vector3 currentDirection = currentInteractorPosition - transform.position; // 현재 방향

            // 두 벡터 간의 각도 계산 (X축 기준)
            float deltaAngle = Vector3.SignedAngle(startDirection, currentDirection, transform.right);

            // 회전 각도 누적
            currentAngle = Mathf.Clamp(currentAngle + deltaAngle, 0, targetAngle);

            // 로컬 X축 회전 적용
            transform.localRotation = Quaternion.Euler(currentAngle, 0f, 0f);

            // 기준 컨트롤러 위치 갱신
            initialInteractorPosition = currentInteractorPosition;

            // 목표 각도 도달 시 신호 발사
            if (Mathf.Abs(currentAngle) >= targetAngle)
            {
                OnSignal.Invoke($"Valve Turned: {gameObject.name}");
                currentAngle = 0f; // 초기화
            }
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        interactorTransform = null; // 컨트롤러 Transform 초기화
        currentAngle = 0f; // 각도 초기화
    }

    public void ReceiveSignal()
    {
    }
}
