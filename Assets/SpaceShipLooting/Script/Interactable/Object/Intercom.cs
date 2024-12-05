using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Intercom : XRSimpleInteractableOutline
{ 

    private GameObject keypadUI; // Keypad UI 오브젝트
    private Transform displayPosition; // UI가 나타날 위치
    private string correctCode = "1945"; // 정답 비밀번호
    private string currentInput = ""; // 현재 입력된 값
    private bool isUIActive = false; // UI 활성화 상태 체크

    protected override void Awake()
    {
        base.Awake();

        // 키패드 UI와 표시 위치 자동 검색
        keypadUI = GameObject.Find("KeypadUI"); // 이름으로 Keypad UI 검색
        displayPosition = transform.Find("DisplayPosition"); // 자식 오브젝트에서 위치 검색

        if (keypadUI == null)
        {
            Debug.LogError("Keypad UI not found!");
        }

        if (displayPosition == null)
        {
            Debug.LogError("Display Position not found!");
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // UI가 이미 활성화되어 있다면 동작 방지
        if (isUIActive)
        {
            Debug.Log("UI is already active. Ignoring further input.");
            return;
        }

        if (keypadUI != null && displayPosition != null)
        {
            // UI 활성화 및 위치 이동
            keypadUI.SetActive(true);
            keypadUI.transform.position = displayPosition.position;
            keypadUI.transform.rotation = displayPosition.rotation;
            isUIActive = true; // UI 활성화 상태 갱신
        }
    }

    public void OnButtonPressed(string number)
    {
        currentInput += number;

        // 입력값 확인
        if (currentInput.Length >= correctCode.Length)
        {
            if (currentInput == correctCode)
            {
                Debug.Log("SUCCESS!");
            }
            else
            {
                Debug.Log("WRONG CODE! UI will hide.");
                if (keypadUI != null)
                {
                    keypadUI.SetActive(false); // UI 비활성화
                }
                isUIActive = false; // UI 상태 갱신
            }

            // 입력 초기화
            currentInput = "";
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        if (keypadUI != null)
        {
            // 선택 종료 시 UI 비활성화
            keypadUI.SetActive(false);
            isUIActive = false; // UI 상태 갱신
        }
    }
}
