using UnityEngine;

public class CardKeyOpen : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponentInParent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator not found on parent!");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트 이름이 "Card_Key"인지 확인
        if (collision.gameObject.name == "Card_Key")
        {
            anim.SetTrigger("Open"); // 애니메이션 트리거
        }
    }
}
