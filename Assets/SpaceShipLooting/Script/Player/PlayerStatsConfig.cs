using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsConfig", menuName = "Configs/PlayerStats")]
public class PlayerStatsConfig : ScriptableObject
{
    public float handAttackPower = 3f;  // 플레이어 기본 공격력
    public float maxHealth = 100f;      // 플레이어 최대 체력

    public float walkingSpeed = 2.5f;   // 플레이어 기본 이동 속도
    public float runningSpeed = 5f;     // 플레이어 기본 이동 속도
    public float stealthSpeed = 0.5f;   // 플레이어 기본 이동 속도

    // New fields to enable or disable modes for testing
    public bool enableStealthMode = false; // 스텔스 모드 활성화 여부
    public bool enableRunningMode = false; // 러닝 모드 활성화 여부

    public float gunAttackPower = 10f;  // 플레이어 총기 공격력
    public float bulletlifeTime = 5f;   // 플레이어 총알 수명
}
