using System.Collections;
using UnityEngine;

public class EnemyDoor : MonoBehaviour
{
    private Animator anim; // Animator 컴포넌트

    [Header("Colliders")]
    [SerializeField] private Collider collider1; // 첫 번째 트리거 콜라이더
    [SerializeField] private Collider collider2; // 두 번째 트리거 콜라이더

    private bool isProcessing = false; // 문 동작 중복 방지 플래그

    private void Start()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator component is missing on this object.");
        }

        // collider1만 처음에 활성화
        collider1.enabled = true;
        collider2.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Enemy 태그 확인
        if (other.CompareTag("Enemy"))
        {
            if (!isProcessing) // 중복 실행 방지
            {
                StartCoroutine(ProcessDoorLogic());
            }
        }
    }

    private IEnumerator ProcessDoorLogic()
    {
        isProcessing = true; // 동작 시작

        // 문 열기
        anim.SetTrigger("Open");
        yield return new WaitForSeconds(3f); // 3초 대기

        // 문 닫기
        anim.SetTrigger("Close");
        yield return new WaitForSeconds(0.5f); // 문 닫히는 시간 고려

        // 콜라이더 스위칭
        if (collider1.enabled)
        {
            collider1.enabled = false;
            collider2.enabled = true;
        }
        else if (collider2.enabled)
        {
            collider2.enabled = false;
            collider1.enabled = true;
        }

        isProcessing = false; // 동작 완료
    }
}
