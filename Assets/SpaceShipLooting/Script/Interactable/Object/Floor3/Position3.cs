using UnityEngine;
using System.Collections;

public class Position3 : MonoBehaviour
{
    public SceneFader fader;
    private Transform player;

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

        fader.FromFade(1f);
        // 씬 로드 이벤트에 메서드 등록
        player = PlayerStateManager.PlayerTransform;
        if (player != null)
        {
            player.transform.position = transform.position;
            player.transform.rotation = transform.rotation;
            StartCoroutine(Dialogue());
        }
        IEnumerator Dialogue()
        {
            yield return new WaitForSeconds(3f);
            JsonTextManager.instance.OnDialogue("stage3-1");
        }
    }
}
