using UnityEngine;

public class Position1Transform : MonoBehaviour
{
    private Transform player;
    private void Start()
    {
        // 씬 로드 이벤트에 메서드 등록
        player = PlayerStateManager.PlayerTransform;
        if(player != null)
        {

            Debug.LogError("플레이어 플러옴");
            
            player.transform.position = transform.position;
            player.transform.rotation = transform.rotation;
        }
    }
}