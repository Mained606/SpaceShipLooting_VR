using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ResizableSocket : XRSocketInteractor
{
    [Header("스케일 설정")]
    public Vector3 scaledSize = new Vector3(0.5f, 0.5f, 0.5f); // 소켓에 객체가 들어갔을 때의 크기
    public float scaleDuration = 0.2f; // 부드럽게 스케일링하는 시간
    private Vector3 originalScale; // 객체의 원래 스케일 저장

    // 객체가 소켓에 들어갈 때 호출되는 함수
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);

        // 상호작용 가능한 객체의 Transform 가져오기
        Transform interactableTransform = args.interactableObject.transform;

        // 객체의 원래 스케일 저장
        originalScale = interactableTransform.localScale;

        // 부드럽게 원하는 크기로 스케일 조정
        StartCoroutine(ScaleObject(interactableTransform, scaledSize, scaleDuration));
    }

    // 객체가 소켓에서 나올 때 호출되는 함수
    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);

        // 상호작용 가능한 객체의 Transform 가져오기
        Transform interactableTransform = args.interactableObject.transform;

        // 부드럽게 원래 크기로 스케일 복원
        StartCoroutine(ScaleObject(interactableTransform, originalScale, scaleDuration));
    }

    // 객체를 부드럽게 스케일 조정하는 코루틴
    private IEnumerator ScaleObject(Transform target, Vector3 targetScale, float duration)
    {
        Vector3 initialScale = target.localScale; // 초기 크기 저장
        float elapsedTime = 0f; // 경과 시간 초기화

        // 지정된 시간 동안 스케일 점진적으로 변경
        while (elapsedTime < duration)
        {
            target.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 최종 크기를 정확히 설정
        target.localScale = targetScale;
    }
}