using System.Collections;
using UnityEngine;

public class Trigger1 : MonoBehaviour
{
    public Animator Door; // 문 애니메이터
    public GameObject Trigger2; // 반대쪽 트리거 오브젝트

    void Start()
    {
        if (Door == null)
        {
            Debug.LogError("Door Animator is not assigned in Trigger1.");
        }
        if (Trigger2 == null)
        {
            Debug.LogError("Trigger2 is not assigned in Trigger1.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            StartCoroutine(Open());
          
        }
       
    }

    IEnumerator Open()
    {
        Door.SetTrigger("Open");
        yield return new WaitForSeconds(3f); // 3초 대기
        Door.SetTrigger("Close");
        Trigger2.SetActive(true); // 반대쪽 트리거 활성화
        this.gameObject.SetActive(false); // 현재 트리거 비활성화
    }
}
