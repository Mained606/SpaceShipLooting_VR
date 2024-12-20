using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public SceneFader fader;
    [SerializeField] private string loadToScene = "SceneName";

    public void StartGame(Button button)
    {
        AudioManager.Instance.Play("Button");
        fader.FadeTo(loadToScene);
    }

    public void Option(Button button)
    {
        AudioManager.Instance.Play("Button");
        Debug.Log("Option Game");
    }

    public void LoadGame(Button button) 
    {
        AudioManager.Instance.Play("Button");
        // 마지막 씬 정보 불러오기
        int lastScene = GameManager.Instance.PlayerStatsData.lastClearedScene;

        if (lastScene > 0 && lastScene < SceneManager.sceneCountInBuildSettings) 
        {
            fader.FadeTo(lastScene); // 마지막 저장된 씬으로 이동
        } 
        else 
        {
            Debug.LogWarning("저장된 데이터가 없습니다.");
            // fader.FadeTo(1); // 기본적으로 1번 씬으로 이동
        }
    }
}
