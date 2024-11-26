using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    private IObjectPool<Bullet> managedPool;   // 관리되는 객체 풀
    [SerializeField] private float lifeTime = 5f; // 총알 수명 (5초)

    private void OnEnable()
    {
        // 총알 활성화 시 5초 뒤 ReturnToPool 메서드 호출 예약
        Invoke(nameof(DestroyBullet), lifeTime);
    }

    private void OnDisable()
    {
        // 비활성화될 때 예약된 Invoke 취소
        CancelInvoke(nameof(DestroyBullet));
    }

    public void SetManagePool(IObjectPool<Bullet> pool)
    {
        managedPool = pool;
    }

    public void DestroyBullet()
    {
        // 풀로 반환
        if (managedPool != null)
        {
            managedPool.Release(this);
        }
        else
        {
            Destroy(gameObject); // 풀 관리가 없는 경우 안전하게 파괴
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        // 충돌 시 풀로 반환
        Debug.Log($"Bullet이 {collision.gameObject.name}에 충돌");

        // 적이면 데미지 입히기

        // 풀로 반환
        // DestroyBullet();
    }
}
