using UnityEngine;

public class Position1Transform : MonoBehaviour
{
    private GameObject player;

    private void Start()
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
