using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public SceneFader fader;
    [SerializeField] private string loadToScene = "SceneName";
    private bool flag = false;
    private Button button;

    //지혜 옵션 만들기~
    private AudioManager audioManager;

    public GameObject optionUI;
    public GameObject mainMenuUI;
    public Button loadButton;

    //Audio
    public AudioMixer audioMixer;
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Start()
    {
        if (fader == null)
        {
            GameObject gameManager = GameObject.Find("GameManager");
            if (gameManager != null)
            {
                Transform faderTransform = gameManager.transform.Find("SceneFader");
                if (faderTransform != null)
                {
                    fader = faderTransform.GetComponent<SceneFader>();
                }
            }
        }

        int lastScene = GameManager.Instance.PlayerStatsData.lastClearedScene;
        if (lastScene == 1)
        {
            loadButton.interactable = false;
        }
        //참조
        audioManager = AudioManager.Instance;

        AudioManager.Instance.PlayBGM(0, 0.2f);

    }

    public void StartGame()
    {
        if (!flag)
        {
            flag = true;
            Debug.Log("Starting game...");
            AudioManager.Instance.Play("Button", false);
            // 로드하지 않고 스타트 할 경우 세이브 데이터 초기화
            // 치트 킨 상태로 세이브 됐을 때 치트 해제 상태로 진입하도록 설정
            SaveLoad.DeleteFile();
            GameManager.Instance.PlayerStatsData.ResetStatData();
            Pistol pistol = FindFirstObjectByType<Pistol>();
            if (pistol != null)
            {
                pistol.ResetAmmo();
            }
            fader.FadeTo(loadToScene);
            Invoke(nameof(ResetFlag), 1.5f);
        }
    }

    public void Option()
    {
        if (!flag)
        {
            flag = true;
            AudioManager.Instance.Play("Button", false);

            ShowOptions();
            Debug.Log("Option");
            Invoke(nameof(ResetFlag), 0.5f);
        }
    }

    public void LoadGame()
    {
        if (!flag)
        {
            Debug.Log("Load game...");
            flag = true;
            AudioManager.Instance.Play("Button", false);

            int lastScene = GameManager.Instance.PlayerStatsData.lastClearedScene;
            if (lastScene > 0 && lastScene < SceneManager.sceneCountInBuildSettings)
            {
                // 치트 킨 상태로 세이브 됐을 때 치트 해제 상태로 진입하도록 설정
                GameManager.Instance.PlayerStatsData.ResetStatData();
                fader.FadeTo(lastScene);
                Invoke(nameof(ResetFlag), 1.5f);
            }
        }
    }

    // 플래그 리셋
    private void ResetFlag()
    {
        flag = false;
    }

    //옵션 보이기
    private void ShowOptions()
    {
        AudioManager.Instance.Play("Button", false);

        optionUI.SetActive(true);
    }

    //옵션죽이기
    public void HideOptions()
    {
        //옵션값 저장하기
        SaveOptions();

        optionUI.SetActive(false);
    }

    //AudioMix Bgm -40~0
    public void SetBgmVolume(float value)
    {
        audioMixer.SetFloat("BgmVolume", value);
    }

    //AudioMix Sfx -40~0
    public void SetSfxVolume(float value)
    {
        audioMixer.SetFloat("SfxVolume", value);
    }
    //AudioMix Bgm -40~0
    public void SetBgmVolume()
    {
        audioMixer.SetFloat("BgmVolume", bgmSlider.value);
    }

    //AudioMix Sfx -40~0
    public void SetSfxVolume()
    {
        audioMixer.SetFloat("SfxVolume", sfxSlider.value);
    }

    //옵션값 저장하기
    private void SaveOptions()
    {
        PlayerPrefs.SetFloat("BgmVolume", bgmSlider.value);
        PlayerPrefs.SetFloat("SfxVolume", sfxSlider.value);
    }

    //옵션값 로드하기
    private void LoadOptions()
    {
        //배경음 볼륨 가져오기
        float bgmVolume = PlayerPrefs.GetFloat("BgmVolume", 0);
        SetBgmVolume(bgmVolume);        //사운드 볼륨 조절
        bgmSlider.value = bgmVolume;    //UI셋팅

        //효과음 볼륨 가져오기
        float sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 0);
        SetBgmVolume(sfxVolume);        //사운드 볼륨 조절
        sfxSlider.value = sfxVolume;    //UI셋팅
    }
}

