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

        // 마지막 씬 정보 불러오기
        int lastScene = GameManager.Instance.PlayerStatsData.lastClearedScene;

        if (lastScene > 0 && lastScene < SceneManager.sceneCountInBuildSettings)
        {
            gameOverEvent.PlayerDataInit?.Invoke();
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
        fader.FadeTo(1);
    }
}
