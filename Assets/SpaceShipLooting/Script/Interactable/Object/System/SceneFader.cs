using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class SceneFader : MonoBehaviour
{
    #region Variable

    // Fader 이미지
    public Image image;
    public AnimationCurve curve;

    private GameObject leftController;
    private GameObject rightController;
    private GameObject move;
    private PlayerInputHandler playerInputHandler;

    #endregion

    private void Start()
    {
        FindControllers();

        // 초기화: 시작시 화면을 검정색으로 시작
        image.color = new Color(0f, 0f, 0f, 1f);
        FromFade();
    }

    private void FindControllers()
    {
        // 전체 게임 오브젝트에서 컨트롤러 검색
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
        var dynamicMoveProvider = FindObjectOfType<DynamicMoveProvider>();
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

    public void FromFade(float delayTime = 0f)
    {
        // 씬 시작시 페이드 인 효과
        StartCoroutine(FadeIn(delayTime));
    }

    IEnumerator FadeIn(float delayTime)
    {
        if (delayTime > 0f)
        {
            yield return new WaitForSeconds(delayTime);
        }

        float t = 1;

        while (t > 0)
        {
            t -= Time.deltaTime;
            float a = curve.Evaluate(t);
            image.color = new Color(0f, 0f, 0f, a); // 알파 값 변경
            yield return null;
        }
    }

    public void FadeTo(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    public void FadeTo(int sceneNumber)
    {
        StartCoroutine(FadeOut(sceneNumber));
    }

    IEnumerator FadeOut(int sceneNumber)
    {
        // 페이드 아웃 시작 시 컨트롤러 및 로코모션 비활성화
        leftController?.SetActive(false);
        rightController?.SetActive(false);
        move?.SetActive(false);
        if (playerInputHandler != null)
        {
            playerInputHandler.enabled = false;
            Debug.Log("PlayerInputHandler 비활성화됨.");
        }

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime;
            float a = curve.Evaluate(t);
            image.color = new Color(0f, 0f, 0f, a); // 알파 값 변경
            yield return null;
        }

        // 다음 씬 로드
        SceneManager.LoadScene(sceneNumber);
    }

    IEnumerator FadeOut(string sceneName)
    {
        // 페이드 아웃 시작 시 컨트롤러 및 로코모션 비활성화
        leftController?.SetActive(false);
        rightController?.SetActive(false);
        move?.SetActive(false);

        if (playerInputHandler != null)
        {
            playerInputHandler.enabled = false;
            Debug.Log("PlayerInputHandler 비활성화됨.");
        }

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime;
            float a = curve.Evaluate(t);
            image.color = new Color(0f, 0f, 0f, a); // 알파 값 변경
            yield return null;
        }

        // 다음 씬 로드
        SceneManager.LoadScene(sceneName);
    }
}
