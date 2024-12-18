using UnityEngine;
using UnityEngine.SceneManagement;

public class Position1Transform : MonoBehaviour
{
    private GameObject player;

    private void OnEnable()
    {
        // 씬 로드 이벤트에 메서드 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // 씬 로드 이벤트에서 메서드 제거 (리소스 정리)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 씬 로드가 완료되었을 때 호출
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬에서 플레이어 찾기
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            MovePlayerToSpawnPoint();
        }
        else
        {
            Debug.LogWarning("Player not found in the scene.");
        }
    }

    private void MovePlayerToSpawnPoint()
    {
        player.transform.position = transform.position;
        player.transform.rotation = transform.rotation;
        Debug.Log("Player moved to spawn point.");
    }
}
