using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;

public class PauseUI : MonoBehaviour
{

    public GameObject gameMenu;
    public InputActionProperty showButton;

    //머리 따라가게
    public Transform head;
    [SerializeField] private float distance = 1.5f;

    private bool isPaused = false;

    private void Update()
    {
        if (showButton.action.WasPressedThisFrame())
        {
            Toggle();
        }
    }

    void Toggle()
    {
        isPaused = !isPaused;
        gameMenu.SetActive(isPaused);

        //show 설정
        if (isPaused)
        {
            //머리따라서
            gameMenu.transform.position = head.position + new Vector3(head.forward.x, 0f, head.forward.z).normalized * distance;
            gameMenu.transform.LookAt(new Vector3(head.position.x, gameMenu.transform.position.y, head.position.z));
            gameMenu.transform.forward *= -1;

            //게임 멈춤
            Time.timeScale = 0f;
        }
        else
        {
            //게임 재개
            Time.timeScale = 1f;
        }
    }

    //나가기
    public void Quit()
    {
        Time.timeScale = 1f;

#if UNITY_EDITOR
        // 에디터에서 실행 중일 경우 에디터를 중지
        UnityEditor.EditorApplication.isPlaying = false;
        Debug.Log("Editor Quit");
#else
        // 빌드된 애플리케이션에서는 게임 종료
        Application.Quit();
#endif
    }
}

