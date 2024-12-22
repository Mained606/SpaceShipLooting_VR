using UnityEngine;

[System.Serializable]
// 추후 개선 필요
public class PlayerStatsData
{
    // public GameObject player;

    [Header("Scene Settings")]
    public int lastClearedScene = 0; // 마지막 클리어된 씬 번호

    [Header("Speed Settings")]
    public float walkingSpeed = 2.5f;   // 플레이어 기본 이동 속도
    public float runningSpeed = 5f;     // 플레이어 달리기 속도
    public float stealthSpeed = 0.5f;   // 플레이어 앉기 이동 속도

    [Header("Pistol Settings")]
    public float bulletDamage = 10f;  // 플레이어 총기 공격력
    public float bulletlifeTime = 5f;   // 플레이어 총알 수명
    public float pistolBulletSpeed = 60f;   //탄환 스피드
    public int maxAmmo = 7; // 총 보유 총알
    public int currentAmmo = 0; // 현재 장전된 탄창

    [Header("Knife Settings")]
    public float knifeDamage = 30f;     // 칼 데미지

    [Header("Player State Settings")]
    public bool enableStealthMode = false; // 스텔스 모드 활성화 여부
    public bool enableRunningMode = false; // 러닝 모드 활성화 여부

    // public void SetPlayerTransform(GameObject gameObject)
    // {
    //     player = gameObject;
    // } 

    public void AddAmmo(int amount)
    {
        maxAmmo += amount;
    }

    public void UseAmmo(int amount)
    {
        maxAmmo -= amount;
    }

}
