using UnityEngine;

[System.Serializable]
public class PlayerStatsData
{
    [Header("Health Settings")]
    public float maxHealth = 100f;      // 플레이어 최대 체력

    [Header("Speed Settings")]
    public float walkingSpeed = 2.5f;   // 플레이어 기본 이동 속도
    public float runningSpeed = 5f;     // 플레이어 달리기 속도
    public float stealthSpeed = 0.5f;   // 플레이어 앉기 이동 속도

    [Header("Pistol Settings")]
    public float bulletDamage = 10f;  // 플레이어 총기 공격력
    public float bulletlifeTime = 5f;   // 플레이어 총알 수명
    public float pistolBulletSpeed = 60f;   //탄환 스피드

    [Header("Knife Settings")]
    public float knifeDamage = 30f;     // 칼 데미지

    [Header("Player State Settings")]
    public bool enableStealthMode = false; // 스텔스 모드 활성화 여부
    public bool enableRunningMode = false; // 러닝 모드 활성화 여부
}
