using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;

public class PauseUI : MonoBehaviour
{

    public GameObject gameMenu;
    public InputActionProperty showButton;

    [SerializeField] private SceneFader fader;



    //Audio
    private AudioManager audioManager;

    public AudioMixer audioMixer;
    public Slider bgmSlider;
    public Slider sfxSlider;

    public GameObject vinetting;

    //게임 멈추게 하는거
    private bool isPaused = false;


    private void Start()
    {
        //참조
        // audioManager = AudioManager.Instance;

        // AudioManager.Instance.PlayBGM(0, 0.2f);
    }

    private void Update()
    {
        if (showButton.action.WasPressedThisFrame())
        {

            Toggle();
        }
    }

    //껐다 켰다
    void Toggle()
    {
        Debug.Log("dd");
        isPaused = !isPaused;
        gameMenu.SetActive(isPaused);

        //show 설정
        if (isPaused)
        {

            //게임 멈춤
            Time.timeScale = 0f;
        }
        else
        {
            //옵션값 저장하기
            //SaveOptions();

            //게임 재개
            Time.timeScale = 1f;
        }
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

    //나가기
    public void Quit()
    {

        fader.FadeTo(1);
        Time.timeScale = 1;
        gameMenu.SetActive(false);
    }

    public void VinettingToggle()
    {
        vinetting.SetActive(!vinetting.activeSelf);
    }

}

