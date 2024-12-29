using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class PositionStartTransform : MonoBehaviour
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

        player = PlayerStateManager.PlayerTransform;
        if (player != null)
        {
            player.transform.position = transform.position;
            player.transform.rotation = transform.rotation;
        }
    }

}
