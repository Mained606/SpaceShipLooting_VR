using UnityEngine;

[System.Serializable]
public class EnemyData
{
    [Header("기본 설정")]
    public float health = 40f;
    public float moveSpeed = 3f;
    public float attackDamage = 10f;
    public GameObject item;

    [Header("패트롤 설정")]
    public PatrolType enemyPatrolType;
    public float patrolSpeed = 1f;
    public float circlePatrolRange = 10f;
    public Vector2 rectanglePatrolRange = new Vector2(5f, 10f);
    public float runPerceptionRange = 15f;
    public float stealthPerceptionRange = 3f;
    public bool infinitePatrolMode;

    [Header("추격 설정")]
    public ChaseType enemyChaseType;
    public float deadZone = 1.5f;

    [Header("이벤트 설정")]
    public InteractEventData enemyInteractData;

    [Header("기타 설정")]
    public float attackInterval = 1.5f;
    public float chaseInterval = 1.5f;
    public AudioManager audioManager;
    [HideInInspector] public GameObject targetEncounterUI;

    [Header("효과음")]
    public AudioClip EnemyWalk;
    public AudioClip EnemyFirstEncounter;
    public AudioClip EnemyChase;
    public AudioClip EnemyShot;
    public AudioClip EnemyKill;
    public AudioClip KeyCard;

    public AudioSource audioSource;

    [Header("디버그용")]
    public EnemyState currentState;
    public bool isLookAround = false;
    public bool isInteracting = false;

    public void SetState(EnemyState newState)
    {
        if (newState == currentState) return;

        currentState = newState;
    }

    public void SoundPlay(AudioClip clip)
    {
        audioSource.clip = clip;
        if(audioSource.clip == EnemyWalk)
        {
            audioSource.pitch = 0.9f;
        }
        else if(audioSource.clip == EnemyChase)
        {
            audioSource.pitch = 1.5f;
        }
        else
        {
            audioSource.pitch = 1.0f;
        }
        if(audioSource.clip != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void SoundStop()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}

//public enum EnemyType
//{
//    RandomPatrol,
//    WaypointPatrol,
//    NonePatrol
//}
