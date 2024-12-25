using UnityEngine.SceneManagement;
using UnityEngine;

public class TP1 : MonoBehaviour
{
    public SceneFader fader;

    [SerializeField] private string loadToScene = "";

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
    }
    private void OnTriggerEnter(Collider other)
    {
        AudioManager.Instance.Play("Teleport");
        fader.FadeTo(loadToScene);
    }
}

