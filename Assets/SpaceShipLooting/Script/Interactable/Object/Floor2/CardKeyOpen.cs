using UnityEngine;

public class IntercomInteraction : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        // Animator 가져오기
        anim = GetComponentInParent<Animator>();
        if (anim == null)
        {
          anim=GetComponentInParent<Animator>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트의 이름 확인
        if (other.gameObject.CompareTag("Card"))
        {
            Debug.Log("Card tapped on intercom. Triggering animation...");
            anim.SetTrigger("Open");
        }
    }
}
