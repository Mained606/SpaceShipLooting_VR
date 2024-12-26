using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JsonTextManager : MonoBehaviour
{
    public static JsonTextManager instance;

    [SerializeField] private TMP_Text text; // 텍스트 UI
    private Coroutine onDialogueCor; // 현재 실행 중인 대사 코루틴
    private Queue<string> dialogueList = new Queue<string>(); // 대사 대기열

    private void Awake()
    {
        instance = this;

        while (text == null)
        {
            var Dialogue = GameObject.Find("Dialogue"); // "Dialogue" 오브젝트 찾기
            text = Dialogue?.GetComponent<TMP_Text>();
            if (text == null)
            {
                Debug.LogWarning("텍스트UI가 빠졌잖아..");
            }
        }
    }

    public void OnDialogue(string stringKey)
    {
        TextManagerJsonData.GetInstance().LoadDatas();
        var dicString = TextManagerJsonData.GetInstance().dicString_Table;

        if (dicString.ContainsKey(stringKey))
        {
            foreach (var desc in dicString[stringKey].desc)
            {
                dialogueList.Enqueue(desc); // 대기열에 추가
            }

            if (onDialogueCor == null) // 코루틴이 실행 중이 아니면 처리 시작
            {
                onDialogueCor = StartCoroutine(ProcessDialogueQueue());
            }
        }
        else
        {
            Debug.LogError($"Key Value Error: {stringKey}");
        }
    }

    private IEnumerator ProcessDialogueQueue()
    {
        while (dialogueList.Count > 0)
        {
            var currentDialogue = dialogueList.Dequeue(); // 대기열에서 대사 가져오기
            text.text = currentDialogue; // 텍스트 출력
            yield return new WaitForSeconds(2f); // 출력 대기 시간
        }

        text.text = ""; // 대화 종료 후 초기화
        onDialogueCor = null; // 코루틴 초기화
    }
}
