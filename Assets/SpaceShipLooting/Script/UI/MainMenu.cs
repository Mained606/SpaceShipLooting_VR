using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public SceneFader fader;
    [SerializeField] private string loadToScene = "SceneName";
    private bool flag = false;
    private Button button;

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
                    Debug.Log("SceneFader successfully assigned in Start().");
                }
                else
                {
                    Debug.LogError("SceneFader not found as a child of GameManager.");
                }
            }
            else
            {
                Debug.LogError("GameManager not found in the scene.");
            }
        }
        button = GetComponent<Button>();

        int lastScene = GameManager.Instance.PlayerStatsData.lastClearedScene;
        if(lastScene == 1)
        {
            button.interactable = false;
        }

    }

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
        }
    }

    // 플래그 리셋
    private void ResetFlag()
    {
        flag = false;
    }
}
