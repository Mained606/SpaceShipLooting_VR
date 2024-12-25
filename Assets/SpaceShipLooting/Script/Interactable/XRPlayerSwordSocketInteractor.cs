using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;

public class XRPlayerSwordSocketInteractor : XRSocketInteractor
{
    [Header("Auto Bind Settings")]
    [SerializeField] private string targetObjectName = "LightSaber01"; // 자동으로 찾을 오브젝트 이름
    private bool isAutoBinding = false; // 자동 바인딩 중 여부 플래그

    protected override void Awake()
    {
        base.Awake();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬 로드 후 약간의 지연 후 바인딩 실행
        StartCoroutine(DelayedAutoBind());
    }

    private IEnumerator DelayedAutoBind()
    {
        yield return new WaitForSeconds(0.1f); // 약간의 딜레이 추가 (0.1초)
        isAutoBinding = true; // 자동 바인딩 시작
        AutoBindStartingInteractable();
        yield return new WaitForEndOfFrame(); // 한 프레임 대기 후 플래그 해제
        isAutoBinding = false; // 자동 바인딩 종료
    }

    private void AutoBindStartingInteractable()
    {
        GameObject targetObject = GameObject.Find(targetObjectName);

        if (targetObject != null)
        {
            XRGrabInteractable interactable = targetObject.GetComponent<XRGrabInteractable>();

            if (interactable != null)
            {
                // StartingSelectedInteractable에 자동 할당
                startingSelectedInteractable = interactable;

                // 강제로 소켓에 바인딩되도록 설정 (명시적 캐스팅)
                interactionManager.SelectEnter((IXRSelectInteractor)this, (IXRSelectInteractable)interactable);
            }
        }
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        // 특정 태그 검사 후 선택 가능 여부 결정
        if (interactable.transform.CompareTag("Weapons"))
        {
            return base.CanSelect(interactable);
        }
        return false;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // 자동 바인딩 중에는 사운드 재생을 막음
        if (!isAutoBinding)
        {
            AudioManager.Instance.Play("SocketIn", false);
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        // 자동 바인딩 중에는 사운드 재생을 막음
        if (!isAutoBinding)
        {
            AudioManager.Instance.Play("SocketOut", false);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        // 씬 로드 이벤트 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
