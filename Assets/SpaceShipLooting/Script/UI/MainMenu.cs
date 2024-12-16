using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public SceneFader fader;
    [SerializeField] private string loadToScene = "SceneName";

   

    private void Start()
    {
        
    }

    public void StartGame(Button button)
    {
        SceneManager.LoadScene(loadToScene);
    }

    public void Option(Button button)
    {
        Debug.Log("Option Game");
    }

    public void QuitGame(Button button)
    {
        Debug.Log("Quit Game");
    }
}
