using System.Collections;
using UnityEngine;

public class Trigger2 : MonoBehaviour
{
    public Animator Door; // 문 애니메이터
    public GameObject Trigger1; // 반대쪽 트리거 오브젝트
    private bool isProcessing = false; // 코루틴 중복 방지 플래그

    void Start()
    {
        if (Door == null)
        {
            Debug.LogError("Door Animator is not assigned in Trigger2.");
        }
        if (Trigger1 == null)
        {
            Debug.LogError("Trigger1 is not assigned in Trigger2.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !isProcessing) // 중복 실행 방지
        {
            Debug.Log("Enemy entered Trigger2.");
            StartCoroutine(Open());
        }
    }

    IEnumerator Open()
    {
        isProcessing = true;
        yield return new WaitForSeconds(3f);
        Door.SetTrigger("Open");
        yield return new WaitForSeconds(7f); // 3초 대기

        Door.SetTrigger("Close");
        yield return new WaitForSeconds(8f); // 문 닫히는 시간 대기

        Trigger1.SetActive(true); // 반대쪽 트리거 활성화
        this.gameObject.SetActive(false); // 현재 트리거 비활성화

        isProcessing = false; // 처리 완료
    }
}
