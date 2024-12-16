using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class Intercom : XRSimpleInteractableOutline
{
    private GameObject canvasUI; // Keypad UI 오브젝트
    private Transform displayPosition; // UI가 나타날 위치


    protected override void Awake()
    {
        base.Awake();

        // KeyPadUI 동적 검색
        var keyPadUI = FindObjectOfType<KeyPadUI>(true)?.gameObject;// 비활성화된 오브젝트도 검색
        if (keyPadUI == null)
        {
            Debug.LogError("KeyPadUI not found in the scene!");
        }
        canvasUI = keyPadUI.transform.Find("Canvas")?.gameObject;

        // DisplayPosition을 KeyPadUI 하위에서 찾음
        if (canvasUI != null)
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
        if (canvasUI != null && !canvasUI.activeSelf)
        {
            canvasUI.SetActive(true);
        }
    }

}
