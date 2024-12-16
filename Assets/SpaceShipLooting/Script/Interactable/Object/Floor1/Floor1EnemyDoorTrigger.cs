using UnityEngine;

public class Floor1EnemyDoorTrigger : MonoBehaviour
{
    public Collider trigger1;
    public Collider trigger2;
    public string Tag = "Enemy";

    private Animator anim;
    private int activeTriggers = 0; // 트리거를 밟고 있는 오브젝트 수를 추적

    private void Start()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("[DEBUG] Animator가 연결되지 않았습니다.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 특정 태그 확인
        if (other.CompareTag(Tag))
        {
            // 트리거 활성 상태 업데이트
            activeTriggers++;
            Debug.Log($"[DEBUG] Enemy가 트리거를 밟음. 활성 트리거 수: {activeTriggers}");

            // 문 열기 애니메이션 실행
            anim.SetTrigger("Open");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 특정 태그 확인
        if (other.CompareTag(Tag))
        {
            // 트리거 비활성 상태 업데이트
            activeTriggers = Mathf.Max(0, activeTriggers - 1);
            Debug.Log($"[DEBUG] Enemy가 트리거에서 나감. 활성 트리거 수: {activeTriggers}");

            // 트리거가 더 이상 없으면 문 닫기 애니메이션 실행
            if (activeTriggers == 0)
            {
                anim.SetTrigger("Close");
            }
        }
    }
}
