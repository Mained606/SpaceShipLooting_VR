using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHandOnInput : MonoBehaviour
{
    private Animator handAnimator;

    public InputActionProperty pinchAnimationAction;
    public InputActionProperty gripAnimationAction;

    void Start()
    {
        handAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        float triggerValue = pinchAnimationAction.action.ReadValue<float>();
        float gripValue = gripAnimationAction.action.ReadValue<float>();

        handAnimator.SetFloat("Trigger", triggerValue);
        handAnimator.SetFloat("Grip", gripValue);
    }

}
