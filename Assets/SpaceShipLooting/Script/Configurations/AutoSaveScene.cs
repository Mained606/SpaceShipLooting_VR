using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSaveScene : MonoBehaviour 
{
    private void Start() 
    {
        SaveClearedScene();
    }

    private void SaveClearedScene() 
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        if (currentSceneIndex > GameManager.Instance.PlayerStatsData.lastClearedScene) 
        {
            GameManager.Instance.SaveClearedSceneData();
        }
    }
}