using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // 플레이어의 스텟 값을 조절하기 편하도록 게임 매니저에 등록
    [SerializeField] private PlayerStatsData playerStatsData;
    public PlayerStatsData PlayerStatsData => playerStatsData;

    [Header("ReStart Bullet Count Settings")]
    [SerializeField] private int defaultAmmoCount = 7;

    #region Singleton
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
    #endregion

    private void Start()
    {
        LoadClearedSceneData();
        // 씬 전환 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // 씬 전환 이벤트 해제 (메모리 누수 방지)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 씬 로드 시 호출
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 현재 씬 저장
        SaveClearedSceneData();
    }


    // 클리어된 씬 데이터를 저장
    public void SaveClearedSceneData()
    {
        PlayerStatsData.lastClearedScene = SceneManager.GetActiveScene().buildIndex;
        SaveLoad.SaveData(playerStatsData);
    }

    // 저장된 씬 데이터를 불러오기
    public void LoadClearedSceneData()
    {
        PlayerStatsData loadedData = SaveLoad.LoadData();
        if (loadedData != null)
        {
            playerStatsData = loadedData;

            // 총알 수를 기본값으로 설정
            playerStatsData.maxAmmo = defaultAmmoCount;
        }
    }
}