using System.Collections;
using NUnit.Framework;
using UnityEngine;
using TMPro;

public class TextManagerByYong : MonoBehaviour
{
    public static TextManagerByYong instance;

    [SerializeField] private TMP_Text text;
    Coroutine onDialogueCor;
    private void Awake() 
    {
        instance = this;
    }

    public void OnDialogue(string stringKey)
    {
        TextManagerJsonData.GetInstance().LoadDatas();
        var dicString = TextManagerJsonData.GetInstance().dicString_Table;
        
        if(dicString.ContainsKey(stringKey))
        {
            if(onDialogueCor == null)
            {
                onDialogueCor = StartCoroutine(PlayDialogue(dicString[stringKey].desc));
            }
            else
            {
                Debug.Log("대사 실행 중 좀 이따 신호주셈");
            }
            
        }
        else
        {
            Debug.Log("키 입력 잘못함");
        }
    }

    IEnumerator PlayDialogue(string[] stringDesc)
    {
        foreach(var playDesc in stringDesc)
        {
            text.text = playDesc;
            yield return new WaitForSeconds(1f);
        }

        text.text = "";
        onDialogueCor = null;
        yield return null;
    }
}
