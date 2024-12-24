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
        SceneManager.sceneLoaded += OnSceneLoaded;

        LoadClearedSceneData();
        // 씬 전환 이벤트 등록
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
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            // 저장된 씬 데이터가 null이 아니고, 현재 씬 인덱스가 이전 씬 인덱스보다 작으면 저장하지 않음
        if (playerStatsData.lastClearedScene >= currentSceneIndex)
        {
            Debug.Log($"현재 씬({currentSceneIndex})의 클리어 데이터가 이미 저장된 데이터({playerStatsData.lastClearedScene})보다 작거나 같습니다. 저장하지 않습니다.");
            return;
        }

        // 엔딩씬에 들어오면 세이브 초기화
        if (currentSceneIndex == 5)
        {
            SaveLoad.DeleteFile();
            return;
        }

        PlayerStatsData.lastClearedScene = SceneManager.GetActiveScene().buildIndex;
        SaveLoad.SaveData(playerStatsData);
        Debug.Log(PlayerStatsData.ToString());
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