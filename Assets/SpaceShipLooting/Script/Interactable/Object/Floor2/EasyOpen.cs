using UnityEngine;

public class EasyOpen : XRSimpleInteractableOutline
{
    private Animator anim;

    void Start()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            anim = GetComponentInChildren<Animator>();
        }
        // Animator를 자식에서 먼저 가져옴
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
