using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
///  인풋 전용 스크립트
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    // 버튼 입력 액션 (Unity의 New Input System 사용)
    public InputActionProperty stealthButton;
    public InputActionProperty runningButton;
    public InputActionProperty NightVisionButton;
    public InputActionProperty menuButton;
    public InputActionProperty cheatButton;
    // public InputActionProperty nextSceneButton;

    private bool[] nextScene = new bool[3];


    // 스텔스 모드 토글 이벤트 (UnityEvent를 통해 외부 구독 가능)
    [HideInInspector] public UnityEvent OnStealthToggle = new UnityEvent();
    [HideInInspector] public UnityEvent OnRunningToggle = new UnityEvent();
    [HideInInspector] public UnityEvent OnNightVisionToggle = new UnityEvent();
    [HideInInspector] public UnityEvent OnMenuButtonToggle = new UnityEvent();
    [HideInInspector] public UnityEvent OnCheatButtonToggle = new UnityEvent();
    [HideInInspector] public UnityEvent OnNextSceneButton = new UnityEvent();
    

    private void Update()
    {
        HandleStealthInput();
        HandleRunningInput();
        HandleNightVisionInput();
        HandleMenuButtonInput();
        HandleCheatButtonInput();
        HandleCheatSceneChangeInput();
    }

    private void HandleCheatSceneChangeInput()
    {
        if(PlayerStateManager.Instance.CheatMonde)
        {
            for (int i = 0; i < nextScene.Length; i++)
            {
                // Debug.Log("키"+ i.ToString() + nextScene[i]);
                if(nextScene[i] == false) return;
            }
            

            OnNextSceneButton?.Invoke();
        }
    }

    private void HandleCheatButtonInput()
    {
        if(cheatButton.action.WasPressedThisFrame())
        {
            OnCheatButtonToggle?.Invoke();
            // nextScene[0] = true;
        }
        else
        {
            // nextScene[0] = false;
        }
    }

    private void HandleStealthInput()
    {
        // 키 입력 받아서 토글로 스텔스 모드 변경
        if (stealthButton.action.WasPressedThisFrame())
        {
            // 스텔스 토글 이벤트 호출
            OnStealthToggle?.Invoke();
            nextScene[0] = true;

        }
        if (stealthButton.action.WasReleasedThisFrame())
        {
            nextScene[0] = false;
        }
    }
    private void HandleRunningInput()
    {
        if(runningButton.action.WasPressedThisFrame())
        {
            OnRunningToggle?.Invoke();
            nextScene[1] = true;

        }
        if(runningButton.action.WasReleasedThisFrame())
        {
            nextScene[1] = false;
        }
    }

    private void HandleNightVisionInput()
    {
        if (NightVisionButton.action.WasPressedThisFrame())
        {
            // 스텔스 토글 이벤트 호출
            OnNightVisionToggle?.Invoke();
            nextScene[2] = true;

        }
        if(NightVisionButton.action.WasReleasedThisFrame())
        {
            nextScene[2] = false;
        }
    }

    private void HandleMenuButtonInput()
    {
        if (menuButton.action.WasPressedThisFrame())
        {
            OnMenuButtonToggle?.Invoke();
        }
    }
}
