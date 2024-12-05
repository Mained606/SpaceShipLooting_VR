using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class ValveMove : SelectObject, ISignal
{
    private Animator anim;

    public UnityEvent<string> OnSignal { get; } = new UnityEvent<string>();

    private void Start()
    {
        // Animator를 현재 오브젝트 또는 부모에서 검색
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            anim = GetComponentInParent<Animator>();
        }

        if (anim == null)
        {
            Debug.LogError($"[{gameObject.name}] Animator component is missing!");
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (anim != null)
        {
            anim.SetTrigger("Open");
            Debug.Log($"[{gameObject.name}] Valve turned, sending signal...");
            OnSignal.Invoke("OpenGas");
        }
        else
        {
            Debug.LogError($"[{gameObject.name}] Animator is null. Cannot trigger animation.");
        }
    }

    public GameObject GetGameObject()
    {
        return gameObject; // 현재 오브젝트 반환
    }

    public void ClearListeners()
    {
        OnSignal.RemoveAllListeners();
    }
}
