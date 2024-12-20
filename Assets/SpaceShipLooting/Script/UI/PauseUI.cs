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

    private void Update()
    {
        if (showButton.action.WasPressedThisFrame())
        {
            Toggle();
        }
    }

    void Toggle()
    {
        gameMenu.SetActive(!gameMenu.activeSelf);

        //show 설정
        if (gameMenu.activeSelf)
        {
            gameMenu.transform.position = head.position + new Vector3(head.forward.x, 0f, head.forward.z).normalized * distance;
            gameMenu.transform.LookAt(new Vector3(head.position.x, gameMenu.transform.position.y, head.position.z));
            gameMenu.transform.forward *= -1;
        }
    }

    //나가기
    public void Quit()
    {
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

