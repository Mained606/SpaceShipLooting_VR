using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.XR.Interaction.Toolkit;

public class Pistol : XRGrabInteractableOutline
{
    #region Variables
    [Header("Bullet Sttings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    [SerializeField] private float bulletSpeed = 30f;

    private IObjectPool<Bullet> pool; // 총알 객체 풀
    #endregion

    protected override void Awake()
    {
        base.Awake();

        pool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize: 20);

    }

    protected override void OnActivated(ActivateEventArgs args)
    {
        base.OnActivated(args);
        Fire(args);
    }

    // 총 발사 메서드
    void Fire(ActivateEventArgs args)
    {
        // 객체 풀에서 총알 가져오기
        Bullet bullet = pool.Get();

        // 총알 위치와 회전 설정
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;

        // Rigidbody를 사용해 속도 적용
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.forward * bulletSpeed;
        }

        // 발사 방향을 디버그 레이로 시각화
        // Debug.DrawRay(firePoint.position, firePoint.forward * 5, Color.red, 2f);
    }

    // 객체 풀에서 총알을 생성하는 메서드
    private Bullet CreateBullet()
    {
        if (bulletPrefab == null)
        {
            Debug.Log("BulletPrefab이 할당되지 않았습니다!");
            return null;
        }

        // 총알 프리팹 생성 후 Bullet 컴포넌트 가져오기
        Bullet bullet = Instantiate(bulletPrefab).GetComponent<Bullet>();
        if (bullet == null)
        {
            Debug.Log("BulletPrefab에 Bullet 스크립트가 없습니다!");
            return null;
        }

        // 풀 관리 설정
        bullet.SetManagePool(pool);
        return bullet;
    }

    // 객체 풀에서 총알을 가져올 때 호출되는 메서드
    private void OnGetBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    // 객체 풀에 총알을 반환할 때 호출되는 메서드
    private void OnReleaseBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    // 객체가 풀에서 제거될 때 호출되는 메서드
    private void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }
}
