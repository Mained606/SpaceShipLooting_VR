using UnityEngine;

public class ExitGame : MonoBehaviour
{
    // VR에서 오브젝트를 클릭했을 때 호출되는 메서드
    public void OnExitGame()
    {
#if UNITY_EDITOR
        // 에디터에서 실행 중일 경우 에디터를 중지
        UnityEditor.EditorApplication.isPlaying = false;
        Debug.Log("Editor Quit");
#else
        // 빌드된 애플리케이션에서는 게임 종료
        SaveLoad.DeleteFile();
        Application.Quit();
#endif
    }
}
