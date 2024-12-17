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

        // 처음에는 collider1 활성화, collider2 비활성화
        collider1.enabled = true;
        collider2.enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
        // Enemy 태그 확인 및 중복 실행 방지
        if ( other.CompareTag("Enemy") && !isProcessing)
        {
            StartCoroutine(ProcessDoorLogic());
        }
    }

    private IEnumerator ProcessDoorLogic()
    {
        isProcessing = true; // 문 동작 중복 방지

        // 문 열기
        anim.SetTrigger("Open");
        Debug.Log("Door opened");

        yield return new WaitForSeconds(3f); // 3초 동안 대기

        // 문 닫기
        anim.SetTrigger("Close");
        Debug.Log("Door closed");

        yield return new WaitForSeconds(0.5f); // 문 닫히는 시간 고려

        // 콜라이더 전환: collider1 ↔ collider2
        if (collider1.enabled)
        {
            collider1.enabled = false;
            collider2.enabled = true;
            Debug.Log("Switched to Collider2");
        }
        else if (collider2.enabled)
        {
            collider2.enabled = false;
            collider1.enabled = true;
            Debug.Log("Switched to Collider1");
        }

        isProcessing = false; // 문 동작 완료
    }
}
