using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Intercom : XRSimpleInteractableOutline, ISignalReceiver
{
    private GameObject keyPadUI; // Keypad UI 오브젝트
    private Transform displayPosition; // UI가 나타날 위치

    private bool isUIActive = false; // UI 활성화 상태 체크

    protected override void Awake()
    {
        base.Awake();

        // KeyPadUI 동적 검색
        keyPadUI = FindObjectOfType<KeyPadUI>(true)?.gameObject;// 비활성화된 오브젝트도 검색
        if (keyPadUI == null)
        {
            Debug.LogError("KeyPadUI not found in the scene!");
        }

        // DisplayPosition을 KeyPadUI 하위에서 찾음
        if (keyPadUI != null)
        {
            displayPosition = keyPadUI.transform.Find("Canvas/Display");
            if (displayPosition == null)
            {
                Debug.LogError("DisplayPosition not found under KeyPadUI!");
            }
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // UI 활성화
        if (!isUIActive && keyPadUI != null)
        {
            keyPadUI.SetActive(true);
            keyPadUI.transform.position = displayPosition.position;

            // Y축 180도 회전 추가
            keyPadUI.transform.rotation = displayPosition.rotation * Quaternion.Euler(0, 180, 0);

            isUIActive = true;
        }
    }

    public void ReceiveSignal(string signal)
    {
        if (signal == "CodeMatched")
        {
            Debug.Log("Correct code entered. Triggering animation.");
            // 여기에 애니메이션 트리거 추가
        }

        // UI 비활성화
        if (keyPadUI != null)
        {
            keyPadUI.SetActive(false);
            isUIActive = false;
        }
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
