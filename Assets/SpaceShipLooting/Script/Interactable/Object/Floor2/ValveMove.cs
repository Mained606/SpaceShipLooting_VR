using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class ValveMove : XRSimpleInteractableOutline, ISignal
{
    private Collider col;
    private Animator anim;
 //   public AudioSource girik;
    public static UnityEvent<bool> OnValve { get; } = new UnityEvent<bool>();

    protected override void Start()
    {
        base.Start();
        // Animator를 현재 오브젝트 또는 부모에서 검색
        anim = GetComponent<Animator>() ?? GetComponentInParent<Animator>();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (anim != null)
        {
            anim.SetTrigger("Open");
            col.enabled = false;
            AudioManager.Instance.Play("Valve", false);
        }
        Sender(true);
    }

    public void Sender(bool state) => OnValve?.Invoke(state);

    public void Receiver(bool state) { }

    public void Clear(UnityEvent<bool> signal) => OnValve.RemoveAllListeners();

}
