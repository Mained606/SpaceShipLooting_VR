using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private PlayerStatsConfig playerStatsConfig;

    // 외부 접근용 프로퍼티
    public static PlayerStatsConfig PlayerStats => Instance.playerStatsConfig;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }
}
