using System.Collections.Generic;
using UnityEngine;

public class EnemyDoor : MonoBehaviour
{
    private Animator anim; // Animator 컴포넌트
    private HashSet<Collider> enemiesInTrigger = new HashSet<Collider>(); // 트리거 안에 있는 Enemy를 저장
    [SerializeField] private Collider triggerCollider;


    private void Start()
    {
        // Animator 컴포넌트 할당
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator component is missing on this object.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // "Enemy" 태그를 가진 오브젝트만 처리
        if (other == triggerCollider && other.CompareTag("Enemy"))
        {
            enemiesInTrigger.Add(other); // Enemy 추가
            anim.SetTrigger("Open"); // 문 열림 애니메이션 실행
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // "Enemy" 태그를 가진 오브젝트가 나갈 때
        if (other.CompareTag("Enemy"))
        {
            enemiesInTrigger.Remove(other); // 트리거에서 제거

            // 트리거 안에 남은 Enemy가 없으면 문 닫기
            if (enemiesInTrigger.Count == 0)
            {
                anim.SetTrigger("Close");
            }
        }
    }
}
