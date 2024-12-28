using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LightSaber : XRGrabInteractableOutline
{
    [SerializeField] private GameObject blade;
    [SerializeField] private bool isActive = false;
    private Rigidbody rb;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.Log("라이트 세이버에 리지드 바디가 없습니다.");
        }
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);

        if (rb != null)
        {
            rb.isKinematic = false;
        }

        // 상호작용한 Interactor의 GameObject 가져오기
        GameObject interactorObject = args.interactorObject.transform.gameObject;

        // Interactor 오브젝트의 태그가 "Player"인지 확인
        if (interactorObject.CompareTag("LeftHand") || interactorObject.CompareTag("RightHand"))
        {
            Debug.Log($"{interactorObject.name} 플레이어가 그랩함");
            AudioManager.Instance.Play("SocketOut", false);
        }
        else
        {
            Debug.Log($"{interactorObject.name} 태그가 플레이어가 아님");
        }
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        blade.SetActive(false);
        base.OnSelectExiting(args);

    }

    protected override void OnActivated(ActivateEventArgs args)
    {
        base.OnActivated(args);
        isActive = !isActive;

        //블레이드 셋 엑티브 SFX 추가 
        if (isActive)
        {
            // 소드 On
            AudioManager.Instance.Play("SwordOn", false, 1.4f, 0.7f);
        }
        else
        {
            // 소드 Off
            AudioManager.Instance.Play("SwordOn", false, 3f, 0.7f);
        }

        blade.SetActive(isActive);
    }
}


