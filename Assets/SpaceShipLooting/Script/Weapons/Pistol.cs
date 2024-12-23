using UnityEngine;
using System.Collections;
using UnityEngine.Pool;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class Pistol : XRGrabInteractableOutline
{
    #region Variables
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    private float bulletSpeed;
    [SerializeField] private ParticleSystem muzzleEffect;
    // 연결 수정 필요
    [SerializeField] private float fireRate = 0.5f;
    private bool isFiring = false;

    [SerializeField] private int ammocount;

    [SerializeField] Canvas ammoCanvas;
    [SerializeField] TextMeshProUGUI ammoText;

    private IObjectPool<Bullet> pool; // 총알 객체 풀

    [SerializeField] private Animator animator;

    private Coroutine hideAmmoUICoroutine; // 코루틴 참조
    [SerializeField] private float ammoUIDisplayTime = 3f; // UI 숨기기 대기 시간
    #endregion

    protected override void Awake()
    {
        base.Awake();
        pool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize: 20);
    }

    protected override void Start()
    {
        base.Start();
        bulletSpeed = GameManager.Instance.PlayerStatsData.pistolBulletSpeed;
        ammocount = GameManager.Instance.PlayerStatsData.currentAmmo;

        UpdateAmmoUI();
    }

    protected override void OnActivated(ActivateEventArgs args)
    {
        base.OnActivated(args);
        if (ammocount > 0 && !isFiring)
        {
            StartCoroutine(Fire(args));
        }

        else if (ammocount == 0 && !isFiring)
        {
            Debug.Log("리로드 실행");
            Reload();
        }
    }

    // 총 발사 메서드
    private IEnumerator Fire(ActivateEventArgs args)
    {
        AudioManager.Instance.Play("GunShot");

        isFiring = true;
        animator.SetTrigger("Shoot");
        ammoCanvas.gameObject.SetActive(true);


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

        muzzleEffect.gameObject.SetActive(true);
        muzzleEffect.Play();

        ammocount--;
        // GameManager.Instance.PlayerStatsData.UseAmmo(1);

        UpdateAmmoUI();


        if (ammocount <= 0)
        {
            animator.SetBool("HaveAmmo", false);
            ammoText.color = Color.red;
        }

        yield return new WaitForSeconds(fireRate);

        muzzleEffect.Stop();
        muzzleEffect.gameObject.SetActive(false);

        isFiring = false;

        // 발사 방향을 디버그 레이로 시각화
        // Debug.DrawRay(firePoint.position, firePoint.forward * 5, Color.red, 2f);
    }

    // 재장전
    private void Reload()
    {
        AudioManager.Instance.Play("GunReady");

        // 보유 총알이 남아있는 경우에만 재장전
        if (GameManager.Instance.PlayerStatsData.maxAmmo > 0)
        {
            int ammoToLoad = Mathf.Min(7 - ammocount, GameManager.Instance.PlayerStatsData.maxAmmo);
            ammocount += ammoToLoad;
            GameManager.Instance.PlayerStatsData.UseAmmo(ammoToLoad);

            // 현재 장전된 탄창 정보 갱신
            GameManager.Instance.PlayerStatsData.currentAmmo = ammocount;

            animator.SetBool("HaveAmmo", true);
            ammoText.color = Color.white;
            UpdateAmmoUI();

            Debug.Log($"재장전 완료: 탄창 {ammocount} / 보유 총알 {GameManager.Instance.PlayerStatsData.maxAmmo}");
        }
        else
        {
            UpdateAmmoUI();
            Debug.Log("총알이 부족합니다");
        }

        // if (GameManager.Instance.PlayerStatsData.maxAmmo > 0)
        // {
        //     ammocount += 7;
        //     GameManager.Instance.PlayerStatsData.UseAmmo(7);

        //     animator.SetBool("HaveAmmo", true);
        //     ammoText.color = Color.white;
        //     UpdateAmmoUI();
        //     Debug.Log($"재장전 완료: 탄창 {ammocount} / 보유 총알 {GameManager.Instance.PlayerStatsData.maxAmmo}");
        // }
        // else
        // {
        //     UpdateAmmoUI();
        //     Debug.Log("총알이 부족합니다");
        // }
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

    private void UpdateAmmoUI()
    {
        if (!ammoCanvas.gameObject.activeSelf) // 이미 활성화되어 있지 않은 경우에만 실행
        {
            ammoCanvas.gameObject.SetActive(true);
        }

        ammoText.text = ammocount.ToString() + " / " + GameManager.Instance.PlayerStatsData.maxAmmo.ToString();
        ammoText.color = Color.white;

        // 기존 코루틴이 실행 중이면 중지
        if (hideAmmoUICoroutine != null)
        {
            StopCoroutine(hideAmmoUICoroutine);
        }

        // 새 코루틴 시작
        hideAmmoUICoroutine = StartCoroutine(HideAmmoCanvasAfterDelay());
    }

    // 일정 시간 후 UI 숨기기
    private IEnumerator HideAmmoCanvasAfterDelay()
    {
        yield return new WaitForSeconds(ammoUIDisplayTime);
        ammoCanvas.gameObject.SetActive(false);
    }

    // 씬 변경 시 탄창 데이터 동기화
    protected override void OnDestroy()
    {
        GameManager.Instance.PlayerStatsData.currentAmmo = ammocount;
        base.OnDestroy();
    }

    public void ResetAmmo()
    {
        ammocount = 0;
    }

}