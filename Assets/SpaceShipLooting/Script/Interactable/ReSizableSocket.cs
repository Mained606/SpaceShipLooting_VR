using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ResizableSocket : XRSocketInteractor
{
    [Header("소켓에 들어갈 때 스케일 설정")]
    [Range(0.1f, 1.0f)]
    [SerializeField] private float scaleFactor = 0.2f; // 크기 비율 (0.5 = 원래 크기의 절반)
    private Vector3 originalScale; // 객체의 원래 스케일 저장

    // 객체가 소켓에 들어갈 때 호출되는 함수
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);

        // 상호작용 가능한 객체의 Transform 가져오기
        Transform interactableTransform = args.interactableObject?.transform;
        if (interactableTransform == null)
        {
            return;
        }

        // 객체의 원래 스케일 저장
        originalScale = interactableTransform.localScale;

        // 목표 크기 계산 (비율에 따라 축소)
        Vector3 targetScale = new Vector3(originalScale.x * scaleFactor, originalScale.y * scaleFactor, originalScale.y * scaleFactor);

        interactableTransform.localScale = targetScale;
    }

    // 객체가 소켓에서 나올 때 호출되는 함수
    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);

        // 상호작용 가능한 객체의 Transform 가져오기
        Transform interactableTransform = args.interactableObject?.transform;
        if (interactableTransform == null)
        {
            return;
        }

        interactableTransform.localScale = originalScale;
       
    }
}
