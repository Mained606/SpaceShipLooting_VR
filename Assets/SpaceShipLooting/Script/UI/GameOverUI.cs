using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private SceneFader fader;
    [SerializeField] private string loadToScene = "SceneName";
    private Health health;

    private void OnEnable()
    {
        Time.timeScale = 0;
        health = GetComponentInParent<Health>();
    }

    public void ReTry()
    {
        // 마지막 씬 정보 불러오기
        int lastScene = GameManager.Instance.PlayerStatsData.lastClearedScene;

        if (lastScene > 0 && lastScene < SceneManager.sceneCountInBuildSettings) 
        {
            fader.FadeTo(lastScene); // 마지막 저장된 씬으로 이동
            ResetIsDaed();
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

    private IEnumerator ResetIsDaed()
    {
        yield return new WaitForSeconds(2f);
        
        health.isDead = false;
        health.CurrentHealth = health.maxHealth;
        GameManager.Instance.PlayerStatsData.currentAmmo = 0;
        GameManager.Instance.PlayerStatsData.maxAmmo = 7;
    }

}
