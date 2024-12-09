using UnityEngine;

public class Wire : MonoBehaviour
{
    private SpaceBossCoreExplosionState explosionState; // 현재 상태를 참조하는 변수
    private Animator animator;
    private bool isInitialized = false;
    // 상태를 초기화하는 메서드
    public void Initialize(SpaceBossCoreExplosionState state)
    {
        explosionState = state;
    }

    private void OnEnable()
    {
        if (!isInitialized)
        {
            animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("WireTrigger"); // Trigger the animation
            }
            else
            {
                Debug.LogWarning("Animator component not found on Wire object.");
            }

            isInitialized = true; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어의 무기 또는 총알과 충돌했을 때
        if (other.CompareTag("Weapons") || other.CompareTag("Bullet")) 
        {
            Debug.Log("와이어가 파괴되었습니다.");
            
            // explosionState가 초기화되었는지 확인
            if (explosionState != null)
            {
                // 상태 클래스의 OnWireTriggerEnter 메서드 호출
                explosionState.OnWireTriggerEnter(gameObject);
            }
            else
            {
                Debug.LogError("ExplosionState가 설정되지 않았습니다.");
            }

            // 와이어를 비활성화
            gameObject.SetActive(false);
        }
    }
}
