using UnityEngine;

[System.Serializable]
// 추후 개선 필요
public class PlayerStatsData
{
    [Header("Speed Settings")]
    public float walkingSpeed = 2.5f;   // 플레이어 기본 이동 속도
    public float runningSpeed = 5f;     // 플레이어 달리기 속도
    public float stealthSpeed = 0.5f;   // 플레이어 앉기 이동 속도

    [Header("Pistol Settings")]
    public float bulletDamage = 10f;  // 플레이어 총기 공격력
    public float bulletlifeTime = 5f;   // 플레이어 총알 수명
    public float pistolBulletSpeed = 60f;   //탄환 스피드
    public int maxAmmo = 7;

    [Header("Knife Settings")]
    public float knifeDamage = 30f;     // 칼 데미지

    [Header("Player State Settings")]
    public bool enableStealthMode = false; // 스텔스 모드 활성화 여부
    public bool enableRunningMode = false; // 러닝 모드 활성화 여부

    public void AddAmmo(int amount)
    {
        maxAmmo += amount;
    }

    public void UseAmmo(int amount)
    {
        maxAmmo -= amount;
    }
    
}
