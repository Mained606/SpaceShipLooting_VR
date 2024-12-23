using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public SceneFader fader;
    [SerializeField] private string loadToScene = "SceneName";
    private bool flag = false;

    public void StartGame()
    {
        if (!flag)
        {
            flag = true;
            Debug.Log("Starting game...");
            AudioManager.Instance.Play("Button");
            fader.FadeTo(loadToScene);
            Invoke(nameof(ResetFlag), 1.5f);
        }
    }

    public void Option()
    {
        if (!flag)
        {
            flag = true;
            AudioManager.Instance.Play("Button");
            Debug.Log("Option Game");
            Invoke(nameof(ResetFlag), 0.5f);
        }
    }

    public void LoadGame()
    {
        if (!flag)
        {
            flag = true;
            AudioManager.Instance.Play("Button");

            int lastScene = GameManager.Instance.PlayerStatsData.lastClearedScene;
            if (lastScene > 0 && lastScene < SceneManager.sceneCountInBuildSettings)
            {
                fader.FadeTo(lastScene);
                Invoke(nameof(ResetFlag), 1.5f);
            }
            else
            {
                Debug.LogWarning("No saved data found.");
                Invoke(nameof(ResetFlag), 0.5f); 
            }
        }
    }

    // 플래그 리셋
    private void ResetFlag()
    {
        flag = false;
    }
}
