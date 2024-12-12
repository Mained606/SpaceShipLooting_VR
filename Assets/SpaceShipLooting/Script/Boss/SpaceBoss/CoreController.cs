using UnityEngine;
using UnityEngine.Events;

public class CoreController : MonoBehaviour
{
    [SerializeField] private bool isDestroyed = false;
    [SerializeField] private GameObject shieldEffects;
    [SerializeField] private GameObject lightEffects;

    public UnityEvent OnCoreRecovered { get; private set; } = new UnityEvent();


    private void Awake()
    {
        // 자식 오브젝트 중에서 실드 오브젝트 참조
        Transform shieldTransform = transform.Find("Shield01");
        if (shieldTransform != null)
        {
            shieldEffects = shieldTransform.gameObject;
        }
        else
        {
            Debug.LogError($"Shield 오브젝트를 찾을 수 없습니다: {gameObject.name}");
        }
        // 자식 오브젝트 중에서 라이트 오브젝트 참조
        Transform lightTransform = transform.Find("CoreLight");
        if (lightTransform != null)
        {
            lightEffects = lightTransform.gameObject;
            lightEffects.SetActive(true); // 시작할 때 라이트 켜기
        }
        else
        {
            Debug.LogError($"Light 오브젝트를 찾을 수 없습니다: {gameObject.name}");
        }
    }

    public void SetDestroyed(bool destroyed)
    {
        isDestroyed = destroyed;
        if (isDestroyed)
        {
            // 코어가 파괴되면 모든 이펙트를 비활성화
            shieldEffects.SetActive(false);
            lightEffects.SetActive(false);
        }
        else
        {
            // 코어가 부활하면 라이트와 실드 다시 켜기
            lightEffects.SetActive(true);
            shieldEffects.SetActive(true);
        }
    }

    // 쉴드 이펙트를 활성화하는 새로운 메서드 추가
    public void SetShieldEffect(bool active)
    {
        // 파괴된 상태가 아닐 때만 쉴드 이펙트 활성화 가능
        if (!isDestroyed && shieldEffects != null)
        {
            shieldEffects.SetActive(active);
        }
    }

    public bool IsDestroyed()
    {
        return isDestroyed;
    }
}
