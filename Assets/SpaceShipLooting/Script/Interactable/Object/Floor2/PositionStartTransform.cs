using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class PositionStartTransform : MonoBehaviour
{
    public SceneFader fader;
    private Transform player;

    private GameObject leftController;
    private GameObject rightController;
    private GameObject move;
    private PlayerInputHandler playerInputHandler;

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

        FindObjects();

        fader.FromFade(1f);

        player = PlayerStateManager.PlayerTransform;
        if (player != null)
        {
            player.transform.position = transform.position;
            player.transform.rotation = transform.rotation;

            StartCoroutine(EnableObjectsWithDelay(3f)); // 딜레이를 적용
        }
    }

    private void FindObjects()
    {
        var allObjects = FindObjectsOfType<Transform>(true);

        foreach (var obj in allObjects)
        {
            if (obj.name == "Left Controller")
            {
                leftController = obj.gameObject;
                Debug.Log("왼쪽 컨트롤러 찾음.");
            }
            else if (obj.name == "Right Controller")
            {
                rightController = obj.gameObject;
                Debug.Log("오른쪽 컨트롤러 찾음.");
            }
        }

        // DynamicMoveProvider가 붙은 오브젝트 검색
        var dynamicMoveProvider = FindObjectOfType<DynamicMoveProvider>(true);
        if (dynamicMoveProvider != null)
        {
            move = dynamicMoveProvider.gameObject;
            Debug.Log("DynamicMoveProvider 오브젝트 찾음.");
        }
        else
        {
            Debug.LogError("DynamicMoveProvider 오브젝트를 찾을 수 없습니다.");
        }
        // PlayerInputHandler 스크립트를 검색
        playerInputHandler = FindObjectOfType<PlayerInputHandler>();
        if (playerInputHandler != null)
        {
            Debug.Log("PlayerInputHandler 스크립트 찾음.");
        }
        else
        {
            Debug.LogError("PlayerInputHandler 스크립트를 찾을 수 없습니다.");
        }
    }

    private IEnumerator EnableObjectsWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 딜레이 추가

        if (leftController != null)
        {
            leftController.SetActive(true);
        }
        else
        {
            Debug.LogError("Left Controller not found.");
        }

        if (rightController != null)
        {
            rightController.SetActive(true);
        }
        else
        {
            Debug.LogError("Right Controller not found.");
        }

        if (move != null)
        {
            move.SetActive(true);
        }
        else
        {
            Debug.LogError("Locomotion object not found.");
        }
        if (playerInputHandler != null)
        {
            playerInputHandler.enabled = true;
            Debug.Log("PlayerInputHandler 비활성화됨.");
        }
    }
}
