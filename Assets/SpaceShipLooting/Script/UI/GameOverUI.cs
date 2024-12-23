using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private SceneFader fader;
    [SerializeField] private string loadToScene = "SceneName";

    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    public void ReTry()
    {
        // 마지막 씬 정보 불러오기
        int lastScene = GameManager.Instance.PlayerStatsData.lastClearedScene;

        if (lastScene > 0 && lastScene < SceneManager.sceneCountInBuildSettings) 
        {
            Health health = GetComponentInParent<Health>();
            health.maxHealth = 10f;
            fader.FadeTo(lastScene); // 마지막 저장된 씬으로 이동
        } 
        else 
        {
            Debug.LogWarning("저장된 데이터가 없습니다.");
        }
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void Quit()
    {
        // Time.timeScale = 1;
        // fader.FadeTo(loadToScene);
        // gameObject.SetActive(false);
        Application.Quit();
    }

}
