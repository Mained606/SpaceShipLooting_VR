using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private SceneFader fader;
    [SerializeField] private string loadToScene = "SceneName";
    private Health health;
    private GameOverEvent gameOverEvent;
    private void OnEnable()
    {
        health = GetComponentInParent<Health>();
        gameOverEvent = GetComponentInParent<GameOverEvent>();
        Time.timeScale = 0;
    }

    public void ReTry()
    {
        Debug.Log("플레이어 재시작 버튼 클릭");

        // 현재 도전 중인 씬 가져오기
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex >= 0 && currentSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            gameOverEvent.PlayerDataInit?.Invoke();
            fader.FadeTo(currentSceneIndex); // 현재 씬으로 이동
        }
        else
        {
            Debug.LogWarning("현재 씬 정보를 가져올 수 없습니다.");
        }

        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void Quit()
    {
        gameOverEvent.PlayerDataInit?.Invoke();
        fader.FadeTo(1);
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
